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
        [JsonProperty] public double Start { get; set; }

        public string Title => this.provider?.Title;

        private IProvider provider;

        public Part(IProvider provider, double start)
        {
            this.provider = provider;
            this.Start = start;
        }

        public float GetLength()
        {
            return 24f;
        }
    }
}
