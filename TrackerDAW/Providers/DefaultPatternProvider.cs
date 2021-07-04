using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerDAW
{
    public class DefaultPatternProvider : MixerBaseProvider
    {
        public const int Version = 1;
        public override double Offset => this.offset; // offset = 50 means this pattern should start returning samples when Read is called with offset 50

        private ProviderData providerData;
        private Pattern pattern;
        private double offset;

        public DefaultPatternProvider(Song song, ProviderData providerData) :
            base(song)
        {
            this.providerData = providerData;

            if (!providerData.TryGetValue<Pattern>(ProviderData.PatternKey, out this.pattern))
            {
                Fail("No pattern info");
                return;
            }

            if (!providerData.TryGetValue<double>(ProviderData.OffsetKey, out this.offset))
            {
                Fail("No offset info");
                return;
            }

            foreach (var track in this.pattern.Tracks)
            {
                var provider = ProviderFactory.Create(song, track, track.ProviderInfo, new ProviderData()
                {
                    { ProviderData.TrackKey, track },
                    { ProviderData.OffsetKey, this.offset }
                });
                this.AddInputProvider(provider);
            }
        }
    }
}
