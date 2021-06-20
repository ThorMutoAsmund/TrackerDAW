using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerDAW
{
    public class SampleProvider : ISampleProvider, IProvider
    {
        private string sampleName;

        private ISampleProvider wtsProvider;

        public string Title => this.sampleName;

        public WaveFormat WaveFormat => this.wtsProvider.WaveFormat;

        public SampleProvider(string sampleName)
        {
            this.sampleName = sampleName;
            var waveFileReader = new WaveFileReader(System.IO.Path.Combine(Env.Song.SamplesPath, sampleName));
            this.wtsProvider = CreateConverter(waveFileReader);
        }

        public int Read(float[] buffer, int offset, int count)
        {
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
