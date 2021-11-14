using TrackerDAW.Rev1;

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
