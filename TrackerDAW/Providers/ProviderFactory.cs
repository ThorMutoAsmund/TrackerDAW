using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerDAW
{
    public static class ProviderFactory
    {
        private static Dictionary<object, ISampleProvider> providers = new Dictionary<object, ISampleProvider>();

        public static ISampleProvider Init()
        {
            providers.Clear();
            return Create();
        }

        public static ISampleProvider Init(Pattern pattern)
        {
            providers.Clear();
            return Create(pattern);
        }

        public static ISampleProvider Create()
        {
            var songSampleProvider = new SongSampleProvider(Env.Song);
            providers[Env.Song] = songSampleProvider;
            return songSampleProvider;
        }

        public static ISampleProvider Create(this Pattern pattern)
        {
            if (providers.ContainsKey(pattern))
            {
                return providers[pattern];
            }
            var patternSampleProvider = new PatternSampleProvider(pattern, Env.Song);
            providers[pattern] = patternSampleProvider;
            return patternSampleProvider;
        }

        public static ISampleProvider Create(this Part part)
        {
            if (providers.ContainsKey(part))
            {
                return providers[part];
            }
            var partSampleProvider = part.Provider;
            providers[part] = partSampleProvider;
            return partSampleProvider;
        }
    }
}
