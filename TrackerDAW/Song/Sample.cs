using NAudio.Wave;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerDAW
{
    public class Sample : Part
    {
        public const int ObjTypeValue = 3;

        [JsonProperty]
        public override int ObjType
        {
            get => ObjTypeValue;
            set { }
        }
        [JsonProperty] public ProviderInfo ProviderInfo { get; set; }
        [JsonProperty] public ProviderData ProviderData { get; set; }
        [JsonProperty] public double Gain { get; set; }
        [JsonProperty] public double Length { get; set; }

        [JsonIgnore] public string NameWithDefaultValue => String.IsNullOrEmpty(this.Name) ? "(untitled)" : this.Name;
        
        public Sample()
        {
            this.Offset = 0d;
            this.Gain = 1d;
            this.Length = 0d;
        }

        public Sample(double offset, ProviderInfo providerInfo, ProviderData providerData, double length, double gain = 1d, string name = "")
        {
            this.ProviderInfo = providerInfo;
            this.ProviderData = providerData;
            this.Offset = offset;
            this.Length = length;
            this.Gain = gain;
            this.Name = name;
        }

        public int SampleOffset(Song song)
        {
            return (int)(this.Offset * song.SampleRate);
        }

        public override Part Clone()
        {
            return new Sample(this.Offset, this.ProviderInfo.Clone(), this.ProviderData.Clone(), this.Length, this.Gain, this.Name);
        }

        public override double GetLength()
        {
            return this.Length;
        }
    }
}
