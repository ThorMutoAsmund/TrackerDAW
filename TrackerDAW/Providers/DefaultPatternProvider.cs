using System.Linq;

namespace TrackerDAW
{
    public class DefaultPatternProvider : MixerBaseProvider
    {
        public override float Gain => (float)this.pattern.Gain;
        public const int Version = 1;

        private ProviderData providerData;
        private Pattern pattern;

        public DefaultPatternProvider(PlaybackContext context, ProviderData providerData) :
            base(context)
        {
            this.providerData = providerData;

            if (!this.providerData.TryGetValue<Pattern>(ProviderDataKey.Pattern, out this.pattern))
            {
                Fail("No pattern info");
                return;
            }

            if (!this.providerData.TryGetValue<int>(ProviderDataKey.IStartAt, out var iStartAt))
            {
                Fail("No start-at info");
                return;
            }

            foreach (var track in this.pattern.Tracks.Where(t => t.Parts.Count > 0))
            {
                var provider = this.Context.CreateProvider(track, track.ProviderInfo, new ProviderData()
                {
                    { ProviderDataKey.Track, track },
                    { ProviderDataKey.IStartAt, iStartAt }
                });

                this.AddInputProvider(provider);
            }
        }
    }
}
