
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerDAW
{
    public class EmptyProvider : BaseProvider
    {
        public override string Title => "empty";
        public override double Offset => 0;

        public override int Read(float[] buffer, int offset, int count)
        {
            return 0;
        }

        public EmptyProvider(Song song) :
            base(song)
        {
        }
        public EmptyProvider(Song song, string failure) :
            base(song)
        {
            Fail(failure);
        }
    }
}
