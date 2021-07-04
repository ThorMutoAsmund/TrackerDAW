using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerDAW
{
    public abstract class BaseProvider : IProvider
    {
        public virtual string Title => string.Empty;
        public abstract double Offset { get; }
        public WaveFormat WaveFormat => this.song.WaveFormat;
        public bool Failed { get; set; }
        public string Failure { get; set; }
        protected Song song;

        public BaseProvider(Song song)
        {
            this.song = song;
        }

        protected void Fail(string failure)
        {
            this.Failed = true;
            this.Failure = failure;
        }

        public abstract int Read(float[] buffer, int offset, int count);
    }
}

