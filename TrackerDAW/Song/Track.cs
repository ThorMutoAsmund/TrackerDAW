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
        [JsonProperty] public ProviderInfo ProviderInfo { get; set; }
        [JsonProperty] public List<Part> Parts { get; set; }

        public Track()
        {
            this.ProviderInfo = ProviderFactory.DefaultTrackProviderInfo;
            this.Parts = new List<Part>();
        }

        public Part AddPart(Part part)
        {
            //part.ProviderData.Add(ProviderData.PartKey, part); creates self reference!
            this.Parts.Add(part);

            Song.OnTrackChanged(this);

            return part;
        }

        public Part MovePart(Part part, Track oldTrack, double start)
        {
            oldTrack.Parts.Remove(part);

            part.Offset = start;

            this.Parts.Add(part);

            Song.OnTrackChanged(oldTrack);
            Song.OnTrackChanged(this);

            return part;
        }

        public Part CopyPart(Part source, double start)
        {
            var part = source.Clone();
            part.Offset = start;

            this.Parts.Add(part);

            Song.OnTrackChanged(this);

            return part;
        }

        public void DeletePart(Part part)
        {
            this.Parts.Remove(part);

            Song.OnTrackChanged(this);
        }
    }
}
