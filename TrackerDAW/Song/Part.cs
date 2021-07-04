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

        public Part()
        {
        }

        public Part(double offset, ProviderInfo providerInfo, ProviderData providerData)
        {
            this.ProviderInfo = providerInfo;
            this.ProviderData = providerData;
            this.Offset = offset;
        }

        public float GetLength()
        {
            return 24f;
        }

        public Part Clone()
        {
            return new Part(this.Offset, this.ProviderInfo.Clone(), this.ProviderData.Clone())
            {
                Name = this.Name
            };
        }
    }
}
