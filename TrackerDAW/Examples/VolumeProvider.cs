using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerDAW
{
    public class VolumeProvider : ISampleProvider
    {
        public WaveFormat WaveFormat { get; private set; }
        public float Volume { get; set; }
        public ISampleProvider InputProvider { get; set; }

        public VolumeProvider(Song song)
        {
            this.WaveFormat = WaveFormat.CreateIeeeFloatWaveFormat(song.SampleRate, song.Channels);
        }

        public int Read(float[] buffer, int offset, int count)
        {
            if (this.InputProvider == null)
            {
                return 0;
            }

            int samplesRead = this.InputProvider.Read(buffer, offset, count);
            if (this.Volume != 1f)
            {
                for (int n = 0; n < count; ++n)
                {
                    buffer[offset + n] *= this.Volume;
                }
            }

            return samplesRead;
        }
    }
}
