using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerDAW
{
    public class SongSampleProvider : ISampleProvider
    {
        public WaveFormat WaveFormat { get; private set; }

        private Song song;
        public SongSampleProvider(Song song)
        {
            this.WaveFormat = WaveFormat.CreateIeeeFloatWaveFormat(song.SampleRate, 2);
            this.song = song;
        }

        public int Read(float[] buffer, int offset, int count)
        {
            return 0;
        }
    }
}
