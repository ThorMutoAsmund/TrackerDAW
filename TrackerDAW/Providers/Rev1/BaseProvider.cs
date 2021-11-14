using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerDAW.Rev1
{
    public abstract class BaseProvider : IProvider
    {
        public abstract float Gain { get; }
        public WaveFormat WaveFormat => this.Song.WaveFormat;
        public bool Failed { get; set; }
        public string Failure { get; set; }
        public virtual ProviderInterface Interface { get; } = null;

        protected PlaybackContext Context { get; private set; }
        protected Song Song => this.Context.Song;

        public BaseProvider(PlaybackContext context)
        {
            this.Context = context;
        }

        protected void Fail(string failure)
        {
            this.Failed = true;
            this.Failure = failure;
        }

        private int ramp = 0;
        protected int Test(float[] buffer, int offset, int count)
        {
            var r = new Random();
            for (int i = 0; i < count; ++i)
            {
                buffer[offset + i] = (float)(Math.Sin((i + ramp) / 10f) * 2f - 1f);
            }
            ramp += count;

            return count;
        }

        public abstract int Read(float[] buffer, int offset, int count);
    }
}

