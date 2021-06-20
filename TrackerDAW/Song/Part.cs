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
        [JsonProperty] public double Start { get; set; }

        public string Title => this.Provider?.Title;

        public IProvider Provider { get; private set; }

        public Part(IProvider provider, double start)
        {
            this.Provider = provider;
            this.Start = start;
        }

        public float GetLength()
        {
            return 24f;
        }

        public Part Clone()
        {
            return new Part(this.Provider, this.Start);
        }
    }
}
