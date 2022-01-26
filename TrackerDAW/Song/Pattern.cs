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
        [JsonProperty] public ProviderInfo ProviderInfo { get; set; }
        [JsonProperty] public double Length { get; set; }
        [JsonProperty] public List<Track> Tracks { get; set; }
        [JsonProperty] public double BPS { get; set; }
        [JsonProperty] public double Gain { get; set; }

        [JsonIgnore] public string NameWithDefaultValue => String.IsNullOrEmpty(this.Name) ? "(untitled)" : this.Name;
        
        /// <summary>
        /// Must not assign anything due to JSON deserialization
        /// </summary>
        private Pattern()
        {
        }

        public static Pattern CreateNew(string name, double length, double bps)
        {
            return new Pattern()
            {
                ProviderInfo = DefaultPatternProvider.ProviderInfo,
                Tracks = new List<Track>(),
                Gain = 1d,
                Name = name,
                Length = length,
                BPS = bps
            };
        }

        public int SampleLength(Song song)
        {
            return (int)(this.Length * song.SampleRate);
        }

        public Pattern Clone(string name = null)
        {
            var pattern = new Pattern()
            {
                ProviderInfo = this.ProviderInfo,
                Tracks = new List<Track>(),
                Name = name == null ? this.Name : name,
                Length = this.Length,
                BPS = this.BPS,
                Gain = this.Gain
            };

            foreach (var track in this.Tracks)
            {
                pattern.Tracks.Add(track.Clone());
            }

            return pattern;
        }

        public Track AddNewTrack()
        {
            var track = Track.CreateNew();

            this.Tracks.Add(track);

            Song.OnPatternChanged(this);

            return track;
        }
        
        public int GetCount(Song song) => (int)(this.Length * song.SampleRate);
    }
}
