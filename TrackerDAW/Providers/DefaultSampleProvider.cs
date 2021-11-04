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

            if (!this.providerData.TryGetValue<string>(ProviderDataKey.SampleName, out this.sampleName))
            {
                Fail("No samplename info");
                return;
            }

            if (!this.providerData.TryGetValue<int>(ProviderDataKey.IStartAt, out this.iStartAt))
            {
                Fail("No start-at info");
                return;
            }

            if (!this.providerData.TryGetValue<double>(ProviderDataKey.Gain, out this.gain))
            {
                this.gain = 1d;
            }

            var waveFileReader = CreateWaveFileReader(this.Song, this.sampleName);
            this.wtsProvider = CreateConverter(waveFileReader);
        }

        private static WaveFileReader CreateWaveFileReader(Song song, string sampleName)
        {
            return new WaveFileReader(System.IO.Path.Combine(song.SamplesPath, sampleName));
        }

        public static double GetFileLength(Song song, string sampleName)
        {
            try
            {
                var wf = CreateWaveFileReader(song, sampleName);
                return wf.TotalTime.TotalSeconds;
            }
            catch (Exception)
            {
                return Env.DefaultPartLength;
            }
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
