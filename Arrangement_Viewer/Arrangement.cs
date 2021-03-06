﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using System.Windows.Media;


namespace EarthboundArrViewer
{
    class Arrangement {
        const int SNESWidth = 256;
        const int SNESHeight = 256;
        const int SNESTileWidth = 8;
        const int SNESTileHeight = 8;

        const int GBAWidth = 256;
        const int GBAHeight = 256;
        const int GBATileWidth = 8;
        const int GBATileHeight = 8;

        private PixelFormat pf = PixelFormats.Indexed8;
        private BitmapPalette palette;
        private byte[] graphicsData;
        private ushort[] arrangementData;
        private byte bitsPerPixel;
        private BitmapSource[] tiles;
        public String Name;
        public Boolean GBA = false;
        public double hdrift { get; set; }
        public double vdrift { get; set; }
        public double hamplitude { get; set; }
        public double vamplitude { get; set; }
        public double hfrequency { get; set; }
        public double vfrequency { get; set; }
        public double hperiod { get; set; }
        public double vperiod { get; set; }
        public Arrangement(byte[] arrangementData, byte[] graphicsData, byte[] paletteData, byte bitsPerPixel, String name, Boolean GBA)
        {
            if (arrangementData.Length != 2048)
                this.arrangementData = new ushort[1024];
            else
                this.arrangementData = new ushort[arrangementData.Length/2+1];
            Buffer.BlockCopy(arrangementData, 0, this.arrangementData, 0, arrangementData.Length);
            this.graphicsData = graphicsData;
            this.bitsPerPixel = bitsPerPixel;
            this.Name = name;
            this.GBA = GBA;
            ushort[] tempPaletteData = new ushort[paletteData.Length / 2];
            Buffer.BlockCopy(paletteData, 0, tempPaletteData, 0, paletteData.Length);
            List<System.Windows.Media.Color> colors = new List<System.Windows.Media.Color>();
            for (int i = 0; i < tempPaletteData.Length; i++)
                colors.Add(System.Windows.Media.Color.FromArgb((byte)((i == 0) ? 0 : 255), (byte)((tempPaletteData[i] & 31) << 3), (byte)((tempPaletteData[i] & 0x3E0) >> 2), (byte)((tempPaletteData[i] & 0x7C00) >> 7)));
            palette = new BitmapPalette(colors);
        }
        private void CreateTilePalette()
        {
            if (!GBA)
                CreateTilePaletteSNES();
            else
                CreateTilePaletteGBA();
        }

        private void CreateTilePaletteGBA() {
            int rawStride = ((GBATileWidth * pf.BitsPerPixel + 31) & ~31) >> 3;
            byte[] rawImage;
            int offset;
            tiles = new BitmapSource[graphicsData.Length / (8 * 4)];
            for (int i = 0; i < tiles.Length; i++) {
                rawImage = new byte[rawStride * GBATileHeight];
                offset = 0;
                for (byte y = 0; y < GBATileHeight; y++)
                    for (byte x = 0; x < GBATileWidth; x += 2) {
                        rawImage[y * GBATileHeight + x] = (byte)(graphicsData[i * 32 + offset] & 0xF);
                        rawImage[y * GBATileHeight + x + 1] = (byte)((graphicsData[i * 32 + offset++] & 0xF0) >> 4);
                    }
                tiles[i] = BitmapSource.Create(GBATileWidth, GBATileHeight, 96, 96, pf, palette, rawImage, rawStride);
            }
        }

        private void CreateTilePaletteSNES() {
            int rawStride = ((SNESTileWidth * pf.BitsPerPixel + 31) & ~31) >> 3;
            byte[] rawImage;
            tiles = new BitmapSource[graphicsData.Length / (8 * bitsPerPixel)];
            for (int i = 0; i < tiles.Length; i++) {
                rawImage = new byte[rawStride * 8];
                for (byte x = 0; x < 8; x++)
                    for (byte y = 0; y < 8; y++)
                        for (byte bitplane = 0; bitplane < bitsPerPixel; bitplane++)
                            rawImage[y * 8 + x] += (byte)((((int)graphicsData[(i * 8 * bitsPerPixel) + y * 2 + ((bitplane / 2) * 16 + (bitplane & 1))] & (1 << 7 - x)) >> 7 - x) << bitplane);
                tiles[i] = BitmapSource.Create(SNESTileWidth, SNESTileHeight, 96, 96, pf, palette, rawImage, rawStride);
            }
        }
        public BitmapSource getTileDump() {
            int rawStride = ((SNESWidth * pf.BitsPerPixel + 31) & ~31) >> 3;
            byte[] rawImage = new byte[rawStride * SNESHeight];
            for (int i = 0; i < tiles.Length; i++)
                tiles[i].CopyPixels(rawImage, rawStride, (i / 32) * 2048 + (i % 32) * 8);
            BitmapSource output = BitmapSource.Create(SNESWidth, SNESHeight, 96, 96, pf, palette, rawImage, rawStride);
            return output;
        }
        public BitmapSource getGraphic() {
            CreateTilePalette();
            if (GBA)
                return GetGraphicGBA();
            return GetGraphicSNES();
        }
        public BitmapSource GetGraphicSNES() {
            if (tiles.Length == 0)
                return null;
            int rawStride = ((SNESWidth * pf.BitsPerPixel + 31) & ~31) >> 3;
            byte[] rawImage = new byte[rawStride * SNESHeight];
            int tileid;
            for (int tile = 0; tile < 32 * 32; tile++) {
                tileid = arrangementData[tile] & 1023;
                if (tileid >= tiles.Length)
                    tileid = 0;
                if (((arrangementData[tile] >> 8) & 192) == 0)
                    tiles[tileid].CopyPixels(rawImage, rawStride, (tile / 32) * 2048 + (tile % 32) * 8);
                else
                    FlipBitmap(tiles[tileid], ((arrangementData[tile] & 0x4000) == 0x4000), ((arrangementData[tile] & 0x8000) == 0x8000)).CopyPixels(rawImage, rawStride, (tile / 32) * 2048 + (tile % 32) * 8);
            }
            BitmapSource output = BitmapSource.Create(SNESWidth, SNESHeight, 96, 96, pf, palette, rawImage, rawStride);
            return output;
        }
        public BitmapSource GetGraphicGBA() {
            if (tiles.Length == 0)
                return null;
            int rawStride = ((GBAWidth * pf.BitsPerPixel + 31) & ~31) >> 3;
            byte[] rawImage = new byte[rawStride * GBAHeight];
            int tileid;
            for (int tile = 0; tile < 32 * 32; tile++) {
                tileid = arrangementData[tile] & 1023;
                if (tileid >= tiles.Length)
                    tileid = 0;
                if (((arrangementData[tile] >> 8) & 0xC) == 0)
                    tiles[tileid].CopyPixels(rawImage, rawStride, (tile / 32) * 2048 + (tile % 32) * 8);
                else
                    FlipBitmap(tiles[tileid], ((arrangementData[tile] & 0x400) == 0x400), ((arrangementData[tile] & 0x800) == 0x800)).CopyPixels(rawImage, rawStride, (tile / 32) * 2048 + (tile % 32) * 8);
            }
            BitmapSource output = BitmapSource.Create(GBAWidth, GBAHeight, 96, 96, pf, palette, rawImage, rawStride);
            return output;
        }
        private static BitmapSource FlipBitmap(BitmapSource flipped, bool flipX, bool flipY)
        {
            System.Windows.Media.Transform tr = new System.Windows.Media.ScaleTransform((flipX ? -1 : 1), (flipY ? -1 : 1));

            TransformedBitmap transformedBmp = new TransformedBitmap();
            transformedBmp.BeginInit();
            transformedBmp.Source = flipped;
            transformedBmp.Transform = tr;
            transformedBmp.EndInit();
            return transformedBmp;
        }
    }
    class MultilayerArrangement {
        private Arrangement[] layers;
        public int numlayers;
        public String Name;
        public double[] opacity;
        private BitmapSource[] bitmaps;
        public MultilayerArrangement(Arrangement layer1, Arrangement layer2) {
            numlayers = 2;
            layers = new Arrangement[numlayers];
            opacity = new double[numlayers];
            layers[0] = layer1;
            layers[1] = layer2;
            opacity[0] = 1;
            opacity[1] = 1;
            this.Name = layer1.Name + " + " + layer2.Name;
        }
        public MultilayerArrangement(Arrangement layer1) {
            numlayers = 1;
            layers = new Arrangement[numlayers];
            layers[0] = layer1;
            opacity = new double[numlayers];
            opacity[0] = 1;
            this.Name = layer1.Name;
        }
        public BitmapSource GetBitmap(int id) {
            if (id >= numlayers)
                return null;
            if (bitmaps == null) {
                bitmaps = new BitmapSource[numlayers];
                for (int i = 0; i < numlayers; i++) {
                    bitmaps[i] = layers[i].getGraphic();
                }
            }
            return bitmaps[id];
        }
        public Arrangement GetLayer(int id)
        {
            if (id >= numlayers)
                return null;
            return layers[id];
        }
    }
}
