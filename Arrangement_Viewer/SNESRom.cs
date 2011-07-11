﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace EarthboundArrViewer
{
    class SNESRom:BinaryReader
    {
        public static int Japan = 0;
        public static int America = 1;
        public static int Europe = 2;
        public Boolean HasHeader;
        private Boolean isHiROM = true;
        PixelFormat pf = PixelFormats.Indexed8;

        public static int HexToSnes(int address)
        {

            if (address >= 0 && address < 0x400000)
                return address + 0xC00000;
            else if (address >= 0x400000 && address < 0x600000)
                return address;
            else
                throw new Exception("File offset out of range: " + address);
        }

        public static int SnesToHex(int address)
        {
            if (address >= 0x400000 && address < 0x600000)
                address -= 0x0;
            else if (address >= 0xC00000 && address < 0x1000000)
                address -= 0xC00000;
            else
                throw new Exception("SNES address out of range: " + address);

            return address;
        }

        public SNESRom(string filename):base (File.Open(filename, FileMode.Open))
        {
            HasHeader = false;
            try { detectHeader(); }
            catch (Exception e) {
                Console.WriteLine(e.Message);
            }
        }
        
        public int ReadSNESPointer(int offset) {
            if (offset > this.BaseStream.Length)
            {
                Console.WriteLine("Warning: Attempted read beyond file end!");
                return 0;
            }
            this.BaseStream.Seek(offset+(HasHeader ? 0x200 : 0), SeekOrigin.Begin);
            return SnesToHex(this.ReadInt32());
        }
        public byte[] readCompressedData(int offset)
        {
            byte[] output = new byte[PKHack.Rom.GetDecompressedSize(offset + (HasHeader ? 0x200 : 0), this)];
            PKHack.Rom.Decomp(offset + (HasHeader ? 0x200 : 0), this, output);
            //Console.WriteLine("Compressed data at {0:x} is {1} bytes",offset, output.Length);
            return output;
        }
        public void seekToOffset(int offset)
        {
            BaseStream.Seek(offset + (HasHeader ? 0x200 : 0), SeekOrigin.Begin);
        }
        public void seekToEBOffset(int offset)
        {
            BaseStream.Seek(SnesToHex(offset + (HasHeader ? 0x200 : 0)), SeekOrigin.Begin);
        }
        public void detectHeaderHiROM()
        {
            Boolean HasHeaderOld = HasHeader;
            if (this.BaseStream.Length < 0x10000)
                throw new Exception("File too small");
            if (checkHeader(0xFFDC))
                return;
            HasHeader = true;
            if (checkHeader(0xFFDC))
                return;
            HasHeader = HasHeaderOld;
            throw new Exception("Not a HiROM");
        }
        public void detectHeaderLoROM()
        {
            Boolean HasHeaderOld = HasHeader;
            if (this.BaseStream.Length < 0x8000)
                throw new Exception("File too small");
            if (checkHeader(0x7FDC))
                return;
            HasHeader = true;
            if (checkHeader(0x7FDC))
                return;
            HasHeader = HasHeaderOld;
            throw new Exception("Not a LoROM");
        }
        public Boolean checkHeader(int offset)
        {
            seekToOffset(offset);
            UInt16 complement = ReadUInt16();
            UInt16 checksum   = ReadUInt16();
            return ((UInt16)~complement == checksum);
        }
        public void detectHeader()
        {
            Boolean HiROM = true;
            Boolean LoROM = true;
            try { detectHeaderHiROM(); }
            catch (Exception)
            {
                HiROM = false;
            }
            try { detectHeaderLoROM(); }
            catch (Exception)
            {
                LoROM = false;
            }
            if ((LoROM || HiROM) == false)
                throw new Exception("Bad ROM");
            else if ((LoROM && HiROM) == true)
                throw new Exception("Cannot handle this ROM");
            else if (LoROM == true)
                this.isHiROM = false;
        }

        public string getGameID()
        {
            this.seekToOffset(0xFFB2-(isHiROM ? 0 : 0x8000));
            return new string(this.ReadChars(4));
        }
        public byte getGameDest()
        {
            this.seekToOffset(0xFFD9 - (isHiROM ? 0 : 0x8000));
            return this.ReadByte();
        }
    }
}