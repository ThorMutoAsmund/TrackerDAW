using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerDAW
{
    public class DefaultTrackProvider : MixerBaseProvider
    {
        public const int Version = 1;
        public override double Offset => this.offset; // offset = 50 means this track should start returning samples when Read is called with offset 50

        private ProviderData providerData;
        private Track track;
        private double offset;

        public DefaultTrackProvider(Song song, ProviderData providerData) :
            base(song)
        {
            this.providerData = providerData;

            if (!providerData.TryGetValue<Track>(ProviderData.TrackKey, out this.track))
            {
                Fail("No track info");
                return;
            }

            if (!providerData.TryGetValue<double>(ProviderData.OffsetKey, out this.offset))
            {
                Fail("No offset info");
                return;
            }

            foreach (var part in track.Parts)
            {
                var provider = ProviderFactory.Create(song, part, part.ProviderInfo, new ProviderData()
                {
                });
                this.AddInputProvider(provider);
            }
        }
    }
}
