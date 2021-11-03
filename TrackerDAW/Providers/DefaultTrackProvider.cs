using System.Collections.Generic;

namespace TrackerDAW
{
    public class DefaultTrackProvider : MixerBaseProvider
    {
        public const int Version = 1;
        public override float Gain => (float)this.track.Gain;

        private ProviderData providerData;
        private Track track;

        public DefaultTrackProvider(PlaybackContext context, ProviderData providerData) :
            base(context)
        {
            this.providerData = providerData;

            if (!this.providerData.TryGetValue<Track>(ProviderData.TrackKey, out this.track))
            {
                Fail("No track info");
                return;
            }

            if (!this.providerData.TryGetValue<int>(ProviderData.IStartAtKey, out var iStartAt))
            {
                Fail("No start-at info");
                return;
            }

            foreach (var part in this.track.Parts)
            {
                switch (part)
                {
                    case Composition composition:
                        {
                            var provider = this.Context.CreateProvider(composition, composition.ProviderInfo, composition.ProviderData.Extend(new Dictionary<string, object>()
                            {
                                { ProviderData.IStartAtKey, composition.SampleOffset(context.Song) + iStartAt },
                                { ProviderData.GainKey, composition.Gain }
                            }));

                            this.AddInputProvider(provider);
                            break;
                        }
                    case Sample sample:
                        {
                            var provider = this.Context.CreateProvider(sample, sample.ProviderInfo, sample.ProviderData.Extend(new Dictionary<string, object>()
                            {
                                { ProviderData.IStartAtKey, sample.SampleOffset(context.Song) + iStartAt },
                                { ProviderData.GainKey, sample.Gain }
                            }));

                            this.AddInputProvider(provider);
                            break;
                        }
                }
            }
        }
    }
}
