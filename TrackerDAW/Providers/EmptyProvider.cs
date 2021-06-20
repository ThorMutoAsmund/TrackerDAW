
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerDAW
{
    public class EmptyProvider : IProvider
    {
        public static EmptyProvider Instance = new EmptyProvider();
        public string Title => "empty";

        public WaveFormat WaveFormat { get; } = WaveFormat.CreateIeeeFloatWaveFormat(Env.DefaultSampleRate, 2);

        public int Read(float[] buffer, int offset, int count)
        {
            return 0;
        }

        private EmptyProvider()
        {
        }
    }
}
