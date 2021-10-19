using NAudio.Wave;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerDAW
{
    public class Part
    {
        [JsonProperty] public ProviderInfo ProviderInfo { get; set; }
        [JsonProperty] public ProviderData ProviderData { get; set; }
        [JsonProperty] public string Name { get; set; }
        [JsonProperty] public double Offset { get; set; }
        [JsonProperty] public double Gain { get; set; }

        [JsonIgnore] public string NameWithDefaultValue => String.IsNullOrEmpty(this.Name) ? "(untitled)" : this.Name;
        
        public Part()
        {
            this.Offset = 0d;
            this.Gain = 1d;
        }

        public Part(double offset, ProviderInfo providerInfo, ProviderData providerData, double gain = 1d, string name = "")
        {
            this.ProviderInfo = providerInfo;
            this.ProviderData = providerData;
            this.Offset = offset;
            this.Gain = gain;
            this.Name = name;
        }

        public int SampleOffset(Song song)
        {
            return (int)(this.Offset * song.SampleRate);
        }

        public Part Clone()
        {
            return new Part(this.Offset, this.ProviderInfo.Clone(), this.ProviderData.Clone(), this.Gain, this.Name);
        }

        public float GetLength()
        {
            return 3f;
        }
    }
}
