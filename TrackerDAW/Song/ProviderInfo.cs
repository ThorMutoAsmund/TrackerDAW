using NAudio.Wave;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerDAW
{
    public class ProviderInfo
    {
        public const string UserNameSpace = "User";
        public const string DefaultNameSpace = "Default";

        [JsonProperty] public string Name { get; set; }
        [JsonProperty] public int Version { get; set; }

        public ProviderInfo()
        {
        }

        public static ProviderInfo CreateUserProvider(string className)
        {
            return new ProviderInfo()
            {
                Name = $"{UserNameSpace}/{className}",
                Version = 0
            };
        }

        public static ProviderInfo CreateUserProvider<T>()
        {
            return new ProviderInfo()
            {
                Name = $"{UserNameSpace}/{nameof(T)}",
                Version = 0
            };
        }

        public static ProviderInfo CreateDefaultProvider<T>(int version)
        {
            return new ProviderInfo()
            {
                Name = $"{DefaultNameSpace}/{nameof(T)}",
                Version = version
            };
        }

        public ProviderInfo Clone()
        {
            return new ProviderInfo()
            {
                Name = this.Name,
                Version = this.Version
            };
        }
    }
}
