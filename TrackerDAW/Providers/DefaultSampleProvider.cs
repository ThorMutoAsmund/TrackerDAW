using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;

namespace TrackerDAW
{
    public class DefaultSampleProvider : BaseProvider
    {
        public const int Version = 1;

        public override float Gain => (float)this.gain;

        private ISampleProvider wtsProvider;
        private ProviderData providerData;
        private string sampleName;
        private int iStartAt;
        private double gain;

        public DefaultSampleProvider(PlaybackContext context, ProviderData providerData) :
            base(context)
        {
            this.providerData = providerData;

            if (!this.providerData.TryGetValue<string>(ProviderData.SampleNameKey, out this.sampleName))
            {
                Fail("No samplename info");
                return;
            }

            if (!this.providerData.TryGetValue<int>(ProviderData.IStartAtKey, out this.iStartAt))
            {
                Fail("No start-at info");
                return;
            }

            if (!this.providerData.TryGetValue<double>(ProviderData.GainKey, out this.gain))
            {
                this.gain = 1d;
            }

            var waveFileReader = new WaveFileReader(System.IO.Path.Combine(this.Song.SamplesPath, this.sampleName));
            this.wtsProvider = CreateConverter(waveFileReader);
        }

        public override int Read(float[] buffer, int offset, int count)
        {
            if (this.Failed)
            {
                return 0;
            }

            var read = 0;

            if (this.Context.SamplePosition < this.iStartAt)
            {
                read = Math.Min(count, this.iStartAt - this.Context.SamplePosition);
            }

            if (read < count)
            {
                read += this.wtsProvider.Read(buffer, read + offset, count - read);
            }

            return read;
        }

        private ISampleProvider CreateConverter(IWaveProvider waveProvider)
        {
            ISampleProvider sampleProvider;
            if (waveProvider.WaveFormat.Encoding == WaveFormatEncoding.Pcm)
            {
                // go to float
                if (waveProvider.WaveFormat.BitsPerSample == 8)
                {
                    sampleProvider = new Pcm8BitToSampleProvider(waveProvider);
                }
                else if (waveProvider.WaveFormat.BitsPerSample == 16)
                {
                    sampleProvider = new Pcm16BitToSampleProvider(waveProvider);
                }
                else if (waveProvider.WaveFormat.BitsPerSample == 24)
                {
                    sampleProvider = new Pcm24BitToSampleProvider(waveProvider);
                }
                else
                {
                    throw new InvalidOperationException("Unsupported operation");
                }
            }
            else if (waveProvider.WaveFormat.Encoding == WaveFormatEncoding.IeeeFloat)
            {
                sampleProvider = new WaveToSampleProvider(waveProvider);
            }
            else
            {
                throw new ArgumentException("Unsupported source encoding");
            }

            return sampleProvider;
        }
    }
}
