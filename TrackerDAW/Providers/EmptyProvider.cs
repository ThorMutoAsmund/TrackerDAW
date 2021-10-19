
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
        public override float Gain => 0f;
        public override int Read(float[] buffer, int offset, int count)
        {
            return 0;
        }

        public EmptyProvider(PlaybackContext context) :
            base(context)
        {
        }
        public EmptyProvider(PlaybackContext context, string failure) :
            base(context)
        {
            Fail(failure);
        }
    }
}
