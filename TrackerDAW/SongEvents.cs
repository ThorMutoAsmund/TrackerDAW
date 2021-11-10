using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerDAW
{
    public enum SongChangedAction
    {
        Opened,
        Closed,
        Saved
    }

    public partial class Song
    {
        public static event Action<Song, SongChangedAction> SongChanged;
        public static event Action PatternsChanged;
        public static event Action<Pattern> PatternChanged;
        public static event Action<Track> TrackChanged;
        public static event Action<Part> PartChanged;

        public static void OnSongChanged(Song song, SongChangedAction action)
        {
            SongChanged?.Invoke(song, action);
            Env.OnDirtyChanged(false);
        }

        public static void OnPatternsChanged()
        {
            PatternsChanged?.Invoke();
            Env.OnDirtyChanged(true);
        }
        
        public static void OnPatternChanged(Pattern pattern)
        {
            PatternChanged?.Invoke(pattern);
            Env.OnDirtyChanged(true);
        }

        public static void OnTrackChanged(Track track)
        {
            TrackChanged?.Invoke(track);
            Env.OnDirtyChanged(true);
        }

        public static void OnPartChanged(Part part)
        {
            PartChanged?.Invoke(part);
            Env.OnDirtyChanged(true);
        }
    }
}
