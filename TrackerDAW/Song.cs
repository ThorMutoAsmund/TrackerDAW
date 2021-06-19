using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerDAW
{
    public partial class Song
    {
        [JsonProperty] public string Name { get; set; }
        [JsonProperty] public int Ver { get; set; }
        [JsonProperty] public double SampleRate { get; set; }
        [JsonProperty] public List<Pattern> Patterns { get; private set; }
        [JsonProperty] public double BPS { get; set; }

        [JsonIgnore] public string ProjectFilePath => Path.Combine(this.projectPath, $"{Env.ProjectFileName}.json");
        [JsonIgnore] public string ScriptsPath => Path.Combine(this.projectPath, Env.ScriptsFolder);
        [JsonIgnore] public string SamplesPath => Path.Combine(this.projectPath, Env.SamplesFolder);
        
        private string projectPath;

        private Song()
        {
            this.Patterns = new List<Pattern>();
        }
        public static void CreateEmpty()
        {
            var projectPath = Path.Combine(Env.ApplicationPath, Env.DefaultProjectFileName);
            
            if (Directory.Exists(projectPath))
            {
                try
                {
                    if (Open(projectPath))
                    {
                        return;
                    }
                }
                catch (Exception)
                {
                    // Fall through
                }
            }

            CreateNew(projectPath, Env.DefaultProjectName, Env.DefaultSampleRate, Env.DefaultBPS);
        }

        public static void CreateNew(string projectPath, string songName, double sampleRate, double bps)
        {
            Env.Song = new Song()
            {
                projectPath = projectPath,
                Name = songName,
                SampleRate = sampleRate,
                BPS = bps
            };

            CreateFolders(projectPath);

            for (var p = 0; p < Env.DefaultNumberOfPatterns; ++p)
            {
                var pattern = Env.Song.NewPattern(Env.DefaultPatternLength, Env.Song.BPS);
            }

            Env.Song.Save();

            Env.AddRecentFile(projectPath);
        }

        public static bool Open(string projectPath)
        {
            var songSerializer = SongSerializer.FromFile(new Song() { projectPath = projectPath }.ProjectFilePath);

            Env.Song = songSerializer.Song;

            if (Env.Song == null)
            {
                Env.Song.projectPath = string.Empty;
                Song.OnSongChanged(Env.Song, false);
                return false;
            }

            Env.Song.projectPath = projectPath;
            Song.OnSongChanged(Env.Song, false);

            Env.AddRecentFile(projectPath);
            
            return true;
        }

        public void Save()
        {
            var storeCursor = System.Windows.Forms.Cursor.Current;
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
            try
            {
                SongSerializer.ToFile(this);
                Song.OnSongChanged(Env.Song, false);
            }
            finally
            {
                System.Windows.Forms.Cursor.Current = storeCursor;
            }
        }

        private static void CreateFolders(string path)
        {
            // Ensure script directory
            var scriptDirectory = Path.Combine(path, Env.ScriptsFolder);
            if (!Directory.Exists(scriptDirectory))
            {
                Directory.CreateDirectory(scriptDirectory);
            }

            // Ensure sample directory
            var sampleDirectory = Path.Combine(path, Env.SamplesFolder);
            if (!Directory.Exists(sampleDirectory))
            {
                Directory.CreateDirectory(sampleDirectory);
            }
        }

        public static void Close()
        {
            if (!Dialogs.ConfirmChangesMade())
            {
                return;
            }

            Env.Song = null;

            Song.OnSongChanged(null, false);
            Env.OnSelectedPatternChanged(-1);
        }

        public static void Open()
        {
            if (!Dialogs.ConfirmChangesMade())
            {
                return;
            }
        }

        public Pattern NewPattern(double length, double bps)
        {
            // Get pattern name
            int patternNo = 1;
            string patternName;
            do
            {
                patternNo++;
                patternName = $"P{patternNo}";
            }
            while (this.Patterns.Any(p => p.Name == patternName));

            var pattern = new Pattern()
            {
                Name = patternName,
                Length = length,
                BPS = bps
            };
            for (var t = 0; t < Env.DefaultNumberOfTracks; ++t)
            {
                pattern.AddNewTrack();
            }

            AddPattern(pattern);

            return pattern;
        }

        public void AddPattern(Pattern pattern, int index = -1)
        {
            if (index < 0)
            {
                this.Patterns.Add(pattern);
            }
            else
            {
                this.Patterns.Insert(index, pattern);
            }

            Song.OnPatternsChanged();
        }

        public void MovePattern(int fromIndex, int toIndex)
        {
            var pattern = this.Patterns[fromIndex];
            this.Patterns.RemoveAt(fromIndex);
            this.Patterns.Insert(toIndex > fromIndex ? toIndex - 1 : toIndex, pattern);

            Song.OnPatternsChanged();
        }

        public void RemovePattern(int index)
        {
            this.Patterns.RemoveAt(index);

            Song.OnPatternsChanged();
        }
    }

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

    public class Track
    {
        [JsonProperty] public List<Part> Parts { get; set; }
        
        public Track()
        {
            this.Parts = new List<Part>();
        }

        public Part AddPart(double start)
        {
            var part = new Part(start);
            this.Parts.Add(part);

            Song.OnTrackChanged(this);

            return part;
        }
    }

    public class Part
    {
        [JsonProperty] public double Start { get; set; }
        
        public Part(double start)
        {
            this.Start = start;
        }
    }
}
