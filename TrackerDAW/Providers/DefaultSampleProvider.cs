using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerDAW
{
    public class DefaultSampleProvider : BaseProvider
    {
        public const int Version = 1;
        public override string Title => "";
        public override double Offset => this.part?.Offset ?? 0d;

        private ISampleProvider wtsProvider;

        private ProviderData providerData;
        private Part part;
        private string sampleName;

        public DefaultSampleProvider(Song song, ProviderData providerData) :
            base(song)
        {
            this.providerData = providerData;

            if (!providerData.TryGetValue<Part>(ProviderData.PartKey, out this.part))
            {
                Fail("No part info");
                return;
            }

            if (!providerData.TryGetValue<string>(ProviderData.SampleNameKey, out this.sampleName))
            {
                Fail("No samplename info");
                return;
            }

            var waveFileReader = new WaveFileReader(System.IO.Path.Combine(Env.Song.SamplesPath, this.sampleName));
            this.wtsProvider = CreateConverter(waveFileReader);
        }

        public override int Read(float[] buffer, int offset, int count)
        {
            if (this.Failed)
            {
                return 0;
            }

            return this.wtsProvider.Read(buffer, offset, count);
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
