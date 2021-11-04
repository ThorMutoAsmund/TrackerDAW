using NAudio.Wave;
using System;
using System.Collections.Generic;

namespace TrackerDAW
{
    public class DefaultSongProvider : BaseProvider
    {
        public const int Version = 1;
        public override float Gain => 1f; 

        private ProviderData providerData;
        private int patternIndex = -1;
        private List<ISampleProvider> patternProviders = new List<ISampleProvider>();

        public DefaultSongProvider(PlaybackContext context, ProviderData providerData) :
            base(context)
        {
            this.providerData = providerData;

            // Get custom offset
            if (!this.providerData.TryGetValue<int>(ProviderDataKey.IStartAt, out var iStartAt))
            {
                iStartAt = 0;
            }

            foreach (var pattern in this.Song.Patterns)
            {
                var provider = this.Context.CreateProvider(pattern, pattern.ProviderInfo, new ProviderData()
                {
                    { ProviderDataKey.Pattern, pattern },
                    { ProviderDataKey.IStartAt, iStartAt }
                });

                patternProviders.Add(provider);
                
                iStartAt += pattern.SampleLength(context.Song);
            }
        }

        public override int Read(float[] buffer, int offset, int count)
        {
            if (this.Failed)
            {
                return 0;
            }

            int bytesRead = 0;

            while (bytesRead < count && this.patternIndex < this.Song.Patterns.Count)
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
            var pattern = this.Song.Patterns[this.patternIndex];
            var bytesRequired = Math.Min(pattern.GetCount(this.Song), count);
            var provider = this.patternProviders[this.patternIndex];
            
            return provider.Read(buffer, offset, bytesRequired);
        }

        private void SelectNextPattern()
        {
            this.patternIndex++;
        }
    }
}
