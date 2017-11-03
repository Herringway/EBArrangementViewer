// -----------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by:
//        The WPF ShaderEffect Generator
//        http://wpfshadergenerator.codeplex.com
//
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// -----------------------------------------------------------------------------
namespace EarthboundArrViewer
{
    using System;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Effects;
    using System.Windows.Media.Media3D;


    /// <summary>Earthbound battle background effect.</summary>
    public class WarpEffect : System.Windows.Media.Effects.ShaderEffect
    {

        /// <summary>Time.</summary>
        public static readonly DependencyProperty TimerProperty = DependencyProperty.Register("Timer", typeof(System.Double), typeof(WarpEffect), new PropertyMetadata((double)0, PixelShaderConstantCallback(0)));
        /// <summary>Horizontal Amplitude.</summary>
        public static readonly DependencyProperty HamplitudeProperty = DependencyProperty.Register("Hamplitude", typeof(System.Double), typeof(WarpEffect), new PropertyMetadata((double)0, PixelShaderConstantCallback(1)));
        /// <summary>Horizontal Period.</summary>
        public static readonly DependencyProperty HperiodProperty = DependencyProperty.Register("Hperiod", typeof(System.Double), typeof(WarpEffect), new PropertyMetadata((double)0, PixelShaderConstantCallback(2)));
        /// <summary>Horizontal Frequency.</summary>
        public static readonly DependencyProperty HFrequencyProperty = DependencyProperty.Register("Hfreq", typeof(System.Double), typeof(WarpEffect), new PropertyMetadata((double)1, PixelShaderConstantCallback(3)));
        /// <summary>Vertical Amplitude.</summary>
        public static readonly DependencyProperty VamplitudeProperty = DependencyProperty.Register("Vamplitude", typeof(System.Double), typeof(WarpEffect), new PropertyMetadata((double)0, PixelShaderConstantCallback(4)));
        /// <summary>Vertical Period.</summary>
        public static readonly DependencyProperty VperiodProperty = DependencyProperty.Register("Vperiod", typeof(System.Double), typeof(WarpEffect), new PropertyMetadata((double)0, PixelShaderConstantCallback(5)));
        /// <summary>Vertical Frequency.</summary>
        public static readonly DependencyProperty VFrequencyProperty = DependencyProperty.Register("Vfreq", typeof(System.Double), typeof(WarpEffect), new PropertyMetadata((double)1, PixelShaderConstantCallback(6)));
        /// <summary>Horizontal Drift.</summary>
        public static readonly DependencyProperty HdriftProperty = DependencyProperty.Register("Hdrift", typeof(System.Double), typeof(WarpEffect), new PropertyMetadata((double)0, PixelShaderConstantCallback(7)));
        /// <summary>Vertical Drift.</summary>
        public static readonly DependencyProperty VdriftProperty = DependencyProperty.Register("Vdrift", typeof(System.Double), typeof(WarpEffect), new PropertyMetadata((double)0, PixelShaderConstantCallback(8)));
        /// <summary>The implicit input sampler passed into the pixel shader by WPF.</summary>
        public static readonly DependencyProperty InputProperty = ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(WarpEffect), 0, SamplingMode.Auto);

        public WarpEffect()
        {
            PixelShader pixelShader = new PixelShader();
            pixelShader.UriSource = new Uri("/Earthbound_Arrangement_Viewer;component/WarpEffect.ps", UriKind.Relative);
            this.PixelShader = pixelShader;
            this.UpdateShaderValue(TimerProperty);
            this.UpdateShaderValue(HamplitudeProperty);
            this.UpdateShaderValue(HperiodProperty);
            this.UpdateShaderValue(VamplitudeProperty);
            this.UpdateShaderValue(VperiodProperty);
            this.UpdateShaderValue(VFrequencyProperty);
            this.UpdateShaderValue(HFrequencyProperty);
            this.UpdateShaderValue(HdriftProperty);
            this.UpdateShaderValue(VdriftProperty);
            this.UpdateShaderValue(InputProperty);
            this.DdxUvDdyUvRegisterIndex = -1;
        }

        /// <summary>Time.</summary>
        public virtual double Timer
        {
            get
            {
                return ((double)(this.GetValue(TimerProperty)));
            }
            set
            {
                this.SetValue(TimerProperty, value);
            }
        }

        /// <summary>Horizontal Amplitude.</summary>
        public virtual double Hamplitude
        {
            get
            {
                return ((double)(this.GetValue(HamplitudeProperty)));
            }
            set
            {
                this.SetValue(HamplitudeProperty, value);
            }
        }

        /// <summary>Horizontal Period.</summary>
        public virtual double Hperiod
        {
            get
            {
                return ((double)(this.GetValue(HperiodProperty)));
            }
            set
            {
                this.SetValue(HperiodProperty, value);
            }
        }

        /// <summary>Vertical Amplitude.</summary>
        public virtual double Vamplitude
        {
            get
            {
                return ((double)(this.GetValue(VamplitudeProperty)));
            }
            set
            {
                this.SetValue(VamplitudeProperty, value);
            }
        }

        /// <summary>Vertical Period.</summary>
        public virtual double Vperiod
        {
            get
            {
                return ((double)(this.GetValue(VperiodProperty)));
            }
            set
            {
                this.SetValue(VperiodProperty, value);
            }
        }

        /// <summary>Horizontal Frequency.</summary>
        public virtual double Hfrequency
        {
            get
            {
                return ((double)(this.GetValue(HFrequencyProperty)));
            }
            set
            {
                this.SetValue(HFrequencyProperty, value);
            }
        }

        /// <summary>Vertical Frequency.</summary>
        public virtual double Vfrequency
        {
            get
            {
                return ((double)(this.GetValue(VFrequencyProperty)));
            }
            set
            {
                this.SetValue(VFrequencyProperty, value);
            }
        }

        /// <summary>Horizontal Drift.</summary>
        public virtual double Hdrift
        {
            get
            {
                return ((double)(this.GetValue(HdriftProperty)));
            }
            set
            {
                this.SetValue(HdriftProperty, value);
            }
        }

        /// <summary>Vertical Drift.</summary>
        public virtual double Vdrift
        {
            get
            {
                return ((double)(this.GetValue(VdriftProperty)));
            }
            set
            {
                this.SetValue(VdriftProperty, value);
            }
        }

        /// <summary>The implicit input sampler passed into the pixel shader by WPF.</summary>
        public virtual System.Windows.Media.Brush Input
        {
            get
            {
                return ((System.Windows.Media.Brush)(this.GetValue(InputProperty)));
            }
            set
            {
                this.SetValue(InputProperty, value);
            }
        }
    }
}
