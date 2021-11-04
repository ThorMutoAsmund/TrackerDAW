using NAudio.Wave;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerDAW
{
    public static class ProviderDataKey
    {
        public const string IStartAt = "Offset";
        public const string Gain = "Gain";
        public const string Pattern = "Pattern";
        public const string Track = "Track";
        public const string Part = "Part";
        public const string SampleName = "SampleName";
        public const string TrimLeft = "TrimLeft";
        public const string TrimRight = "TrimRight";
    }

    public class ProviderData : Dictionary<string, object>
    {
        public bool TryGetValue<T>(string key, out T value)
        {
            if (base.TryGetValue(key, out var obj) && obj is T typedValue)
            {
                value = typedValue;
                return true;
            }

            value = default;
            return false;
        }

        public ProviderData Clone()
        {
            // TBD
            var clone = new ProviderData();

            foreach (var v in this)
            {
                clone[v.Key] = v.Value;
            }

            return clone;
        }

        public ProviderData Extend(Dictionary<string, object> other)
        {
            var result = this.Clone();
            foreach (var pair in other)
            {
                result[pair.Key] = pair.Value;
            }

            return result;

        }
    }
}
