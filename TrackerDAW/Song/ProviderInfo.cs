﻿using NAudio.Wave;
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
        [JsonProperty] public string Type { get; set; }
        [JsonProperty] public int Version { get; set; }

        /// <summary>
        /// Must not assign anything due to JSON deserialization
        /// </summary>
        private ProviderInfo()
        {
        }

        public static ProviderInfo CreateNew(string type, int version)
        {
            return new ProviderInfo()
            {
                Type = type,
                Version = version
            };
        }

        public static ProviderInfo CreateProvider<T>(int version)
        {
            return new ProviderInfo()
            {
                Type = $"{typeof(T)}",
                Version = version
            };
        }

        public ProviderInfo Clone()
        {
            return new ProviderInfo()
            {
                Type = this.Type,
                Version = this.Version
            };
        }
    }
}
