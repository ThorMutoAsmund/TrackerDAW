using System.Collections.Generic;

namespace TrackerDAW
{

    [ProviderRegistration(version: DefaultCompositionProvider.Version)]
    public class DefaultCompositionProvider : MixerBaseProvider
    {
        public const int Version = 1;
        public static ProviderInfo ProviderInfo = ProviderInfo.CreateProvider<DefaultCompositionProvider>(DefaultCompositionProvider.Version);
        public override float Gain => 1f;

        private ProviderData providerData;
        //private Track track;

        public DefaultCompositionProvider(PlaybackContext context, ProviderData providerData) :
            base(context)
        {
            this.providerData = providerData;

            //if (!this.providerData.TryGetValue<Track>(ProviderDataKey.Track, out this.track))
            //{
            //    Fail("No track info");
            //    return;
            //}

            //if (!this.providerData.TryGetValue<int>(ProviderDataKey.IStartAt, out var iStartAt))
            //{
            //    Fail("No start-at info");
            //    return;
            //}

            //foreach (var part in this.track.Parts)
            //{
            //    switch (part)
            //    {
            //        case Composition composition:
            //            {
            //                var provider = this.Context.CreateProvider(composition, composition.ProviderInfo, composition.ProviderData.Extend(new Dictionary<string, object>()
            //                {
            //                    { ProviderDataKey.IStartAt, composition.SampleOffset(context.Song) + iStartAt },
            //                    { ProviderDataKey.Gain, composition.Gain }
            //                }));

            //                this.AddInputProvider(provider);
            //                break;
            //            }
            //        case Sample sample:
            //            {
            //                var provider = this.Context.CreateProvider(sample, sample.ProviderInfo, sample.ProviderData.Extend(new Dictionary<string, object>()
            //                {
            //                    { ProviderDataKey.IStartAt, sample.SampleOffset(context.Song) + iStartAt },
            //                    { ProviderDataKey.Gain, sample.Gain }
            //                }));

            //                this.AddInputProvider(provider);
            //                break;
            //            }
            //    }
            //}
        }
    }
}
