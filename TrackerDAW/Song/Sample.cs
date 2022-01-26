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
        
        /// <summary>
        /// Must not assign anything due to JSON deserialization
        /// </summary>
        public Sample()
        {
        }

        public static Sample CreateNew(double offset, ProviderInfo providerInfo, ProviderData providerData, double length, double gain = 1d, string name = "")
        {
            return new Sample()
            {
                ProviderInfo = providerInfo,
                ProviderData = providerData,
                Offset = offset,
                Length = length,
                Gain = gain,
                Name = name
            };
        }

        public int SampleOffset(Song song)
        {
            return (int)(this.Offset * song.SampleRate);
        }

        public override Part Clone()
        {
            return Sample.CreateNew(this.Offset, this.ProviderInfo.Clone(), this.ProviderData.Clone(), this.Length, this.Gain, this.Name);
        }

        public override double GetLength()
        {
            return this.Length;
        }
    }
}
