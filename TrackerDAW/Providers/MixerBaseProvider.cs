using NAudio.Utils;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerDAW
{
    public abstract class MixerBaseProvider : BaseProvider
    {
        private List<ISampleProvider> providers = new List<ISampleProvider>();
        private float[] mixBuffer;

        public MixerBaseProvider(Song song) :
            base(song)
        {
        }

        protected void AddInputProvider(ISampleProvider provider)
        {
            this.providers.Add(provider);
        }

        public override int Read(float[] buffer, int offset, int count)
        {
            if (this.Failed)
            {
                return 0;
            }

            int outputSamples = 0;
            this.mixBuffer = BufferHelpers.Ensure(this.mixBuffer, count);
            lock (this.providers)
            {
                int index = this.providers.Count - 1;
                while (index >= 0)
                {
                    var source = this.providers[index];
                    int samplesRead = source.Read(this.mixBuffer, 0, count);
                    int outIndex = offset;
                    for (int n = 0; n < samplesRead; n++)
                    {
                        if (n >= outputSamples)
                        {
                            buffer[outIndex++] = this.mixBuffer[n];
                        }
                        else
                        {
                            buffer[outIndex++] += this.mixBuffer[n];
                        }
                    }
                    outputSamples = Math.Max(samplesRead, outputSamples);
                    //if (samplesRead == 0)
                    //{
                    //    this.providers.RemoveAt(index);
                    //}
                    index--;
                }
            }
            // optionally ensure we return a full buffer
            if (outputSamples < count)
            {
                int outputIndex = offset + outputSamples;
                while (outputIndex < offset + count)
                {
                    buffer[outputIndex++] = 0;
                }
                outputSamples = count;
            }
            return outputSamples;
        }
    }
}

