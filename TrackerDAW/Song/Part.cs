using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerDAW
{
    public abstract class Part
    {
        [JsonProperty] public string Name { get; set; }
        [JsonProperty] public double Offset { get; set; }

        public abstract Part Clone();
        public abstract float GetLength();
    }
}
