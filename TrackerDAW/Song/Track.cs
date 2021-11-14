﻿using Newtonsoft.Json;
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
        [JsonProperty] public double Gain { get; set; }

        public Track()
        {
            this.ProviderInfo = DefaultTrackProvider.ProviderInfo;
            this.Parts = new List<Part>();
            this.Gain = 1d;
        }

        public Track Clone()
        {
            var track = new Track()
            {
                ProviderInfo = this.ProviderInfo,
                Gain = this.Gain
            };

            foreach (var part in this.Parts)
            {
                track.Parts.Add(part.Clone());
            }

            return track;
        }
        
        public Part AddPart(Part part)
        {
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
