using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerDAW
{
    public class Track
    {
        [JsonProperty] public List<Part> Parts { get; set; }

        public Track()
        {
            this.Parts = new List<Part>();
        }

        public Part AddEmptyPart(double start)
        {
            var part = new Part(EmptyProvider.Instance, start);

            this.Parts.Add(part);

            Song.OnTrackChanged(this);

            return part;
        }

        public Part AddPart(IProvider provider, double start)
        {
            var part = new Part(provider, start);

            this.Parts.Add(part);

            Song.OnTrackChanged(this);

            return part;
        }

        public Part MovePart(Part part, Track oldTrack, double start)
        {
            oldTrack.Parts.Remove(part);

            part.Start = start;

            this.Parts.Add(part);

            Song.OnTrackChanged(oldTrack);
            Song.OnTrackChanged(this);

            return part;
        }
    }
}
