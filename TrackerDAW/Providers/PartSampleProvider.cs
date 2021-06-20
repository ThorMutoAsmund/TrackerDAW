using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerDAW
{
    //public class PartSampleProvider : WaveToSampleProvider
    //{
    //    public WaveFormat WaveFormat { get; private set; }

    //    private Part part;
    //    private Song song;
        
    //    public PartSampleProvider(Part part, Song song) :
    //        base(new WaveFileReader(System.IO.Path.Combine(Env.Song.SamplesPath, part. fileName)))
    //    {
    //        this.WaveFormat = new WaveFormat(song.SampleRate, 2);
    //        this.part = part;
    //        this.song = song;
    //    }

    //    public int Read(float[] buffer, int offset, int count)
    //    {
    //        return 0;
    //    }
    //}
}
