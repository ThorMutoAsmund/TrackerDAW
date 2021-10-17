using NAudio.Wave;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TrackerDAW
{
    public partial class Song
    {
        [JsonProperty] public string Name { get; set; }
        [JsonProperty] public ProviderInfo ProviderInfo { get; set; }
        [JsonProperty] public int Version { get; set; }
        [JsonProperty] public int SampleRate { get; set; }
        [JsonProperty] public int Channels { get; set; }
        [JsonProperty] public List<Pattern> Patterns { get; private set; }
        //[JsonProperty] public List<ProviderInfo> Providers { get; set; }
        [JsonProperty] public double BPS { get; set; }

        [JsonIgnore] public string ProjectFilePath => Path.Combine(this.projectPath, $"{Env.ProjectFileName}.json");
        [JsonIgnore] public string ScriptsPath => Path.Combine(this.projectPath, Env.ScriptsFolder);
        [JsonIgnore] public string SamplesPath => Path.Combine(this.projectPath, Env.SamplesFolder);
        [JsonIgnore] public WaveFormat WaveFormat { get; private set; }

        private string projectPath;

        private Song()
        {
            this.ProviderInfo = ProviderFactory.DefaultSongProviderInfo;
            this.Patterns = new List<Pattern>();
            //this.Providers = new List<ProviderInfo>();
            this.WaveFormat = WaveFormat.CreateIeeeFloatWaveFormat(this.SampleRate, this.Channels);
        }

        public static void CreateOrOpenDefault()
        {
            var projectPath = Path.Combine(Env.ApplicationPath, Env.DefaultProjectFileName);
            
            if (Directory.Exists(projectPath))
            {
                try
                {
                    if (Song.Open(projectPath))
                    {
                        return;
                    }
                }
                catch (Exception)
                {
                    // Fall through
                }
            }

            Song.CreateNew(projectPath, Env.DefaultProjectName, Env.DefaultSampleRate, Env.DefaultBPS);
        }

        public static void CreateNew(string projectPath, string songName, int sampleRate, double bps, int channels = 2)
        {
            Env.Song = new Song()
            {
                projectPath = projectPath,
                Name = songName,
                SampleRate = sampleRate,
                BPS = bps,
                Channels = channels
            };

            Song.CreateFolders(projectPath);
            ProviderFactory.AddDefaultProviders(Env.Song);

            for (var p = 0; p < Env.DefaultNumberOfPatterns; ++p)
            {
                var pattern = Env.Song.NewPattern(Env.DefaultPatternLength, Env.Song.BPS);
            }

            Song.Save();

            Env.AddRecentFile(projectPath);
        }

        public static bool Open(string projectPath)
        {
            var songSerializer = SongSerializer.FromFile(new Song() { projectPath = projectPath }.ProjectFilePath);

            Song.CreateFolders(projectPath);
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

        public static void Save()
        {
            var storeCursor = System.Windows.Forms.Cursor.Current;
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
            try
            {
                SongSerializer.ToFile(Env.Song);
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
            int patternNo = 0;
            string patternName;
            do
            {
                patternNo++;
                patternName = $"Pattern {patternNo}";
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
}
