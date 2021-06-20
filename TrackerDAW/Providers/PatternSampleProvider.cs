using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerDAW
{
    public class PatternSampleProvider : MixingSampleProvider
    {
        private Pattern pattern;
        private Song song;
        
        public PatternSampleProvider(Pattern pattern, Song song) :
            base(WaveFormat.CreateIeeeFloatWaveFormat(song.SampleRate, 2))
        {
            this.song = song;
            this.pattern = pattern;

            foreach (var track in this.pattern.Tracks)
            {
                foreach (var part in track.Parts)
                {
                    var partSampleProvider = ProviderFactory.Create(part);
                    this.AddMixerInput(partSampleProvider);
                }
            }
        }
    }
}
