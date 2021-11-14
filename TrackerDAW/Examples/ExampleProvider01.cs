using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using TrackerDAW;

using TrackerDAW.Rev1;

namespace User
{
    [ProviderRegistration(version: ExampleProvider01.Version)]
    public class ExampleProvider01 : BaseProvider
    {
        public const int Version = 1;
        public class MyInterface : EffectProviderInterface
        {
        }

        public override ProviderInterface Interface { get; } = new MyInterface();
        public override float Gain => (float)this.gain;

        private ProviderData providerData;
        private int iStartAt;
        private double gain;

        public ExampleProvider01(PlaybackContext context, ProviderData providerData) :
            base(context)
        {
            this.providerData = providerData;

            if (!this.providerData.TryGetValue<int>(ProviderDataKey.IStartAt, out this.iStartAt))
            {
                Fail("No start-at info");
                return;
            }

            if (!this.providerData.TryGetValue<double>(ProviderDataKey.Gain, out this.gain))
            {
                this.gain = 1d;
            }
        }

        public override int Read(float[] buffer, int offset, int count)
        {
            if (this.Failed)
            {
                return 0;
            }

            var read = 0;

            return read;
        }
    }
}
