using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerDAW
{
    public class Pattern
    {
        [JsonProperty] public string Name { get; set; }
        [JsonProperty] public double Length { get; set; }
        [JsonProperty] public List<Track> Tracks { get; set; }
        [JsonProperty] public double BPS { get; set; }
        public Pattern()
        {
            this.Tracks = new List<Track>();
        }

        public Track AddNewTrack()
        {
            var track = new Track();
            this.Tracks.Add(track);

            Song.OnPatternChanged(this);

            return track;
        }
    }
}
