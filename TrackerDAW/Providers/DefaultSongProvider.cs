using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerDAW
{
    public class DefaultSongProvider : BaseProvider
    {
        public const int Version = 1;
        public override double Offset => 0;

        private ProviderData providerData;
        private int patternIndex = -1;
        private List<ISampleProvider> patternProviders = new List<ISampleProvider>();

        public DefaultSongProvider(Song song, ProviderData providerData) :
            base(song)
        {
            this.providerData = providerData;

            var offset = 0d;

            foreach (var pattern in this.song.Patterns)
            {
                var provider = ProviderFactory.Create(song, pattern, pattern.ProviderInfo, new ProviderData()
                {
                    { ProviderData.PatternKey, pattern },
                    { ProviderData.OffsetKey, offset }
                });

                patternProviders.Add(provider);
                
                offset += pattern.Length;
            }
        }

        public override int Read(float[] buffer, int offset, int count)
        {
            if (this.Failed)
            {
                return 0;
            }

            int bytesRead = 0;

            while (bytesRead < count && this.patternIndex < this.song.Patterns.Count)
            {
                if (this.patternIndex < 0)
                {
                    SelectNextPattern();
                }
                var fromThisSegment = ReadFromCurrentPattern(buffer, offset + bytesRead, count - bytesRead);
                if (fromThisSegment == 0)
                {
                    SelectNextPattern();
                }
                bytesRead += fromThisSegment;
            }

            return bytesRead;
        }

        private int ReadFromCurrentPattern(float[] buffer, int offset, int count)
        {
            var pattern = this.song.Patterns[this.patternIndex];
            var bytesRequired = Math.Min(pattern.GetCount(this.song), count);
            var provider = this.patternProviders[this.patternIndex];
            
            return provider.Read(buffer, offset, bytesRequired);
        }

        private void SelectNextPattern()
        {
            this.patternIndex++;
        }
    }
}
