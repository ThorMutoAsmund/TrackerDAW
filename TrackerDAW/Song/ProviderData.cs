﻿using NAudio.Wave;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerDAW
{
    public class ProviderData : Dictionary<string, object>
    {
        public const string OffsetKey = "Offset";
        public const string PatternKey = "Pattern";
        public const string TrackKey = "Track";
        public const string PartKey = "Part";
        public const string SampleNameKey = "SampleName";

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
    }
}
