using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerDAW.Rev1
{
    public abstract class ProviderInterface
    {
    }

    public class EffectProviderInterface : ProviderInterface
    {
        public AudioInput AI { get; } = new AudioInput();
        public AudioOutput AO { get; } = new AudioOutput();
    }

    public interface ISocket
    { 
    }

    public class AudioOutput : ISocket
    {
    }
    
    public class AudioInput : ISocket
    {
    }
    
    public class RandomAccessAudioInput : ISocket
    {
    }
}

