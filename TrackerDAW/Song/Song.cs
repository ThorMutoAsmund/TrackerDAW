using NAudio.Wave;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace TrackerDAW
{
    public partial class Song
    {
        [JsonProperty] public string Name { get; set; }
        [JsonProperty] public ProviderInfo ProviderInfo { get; set; }
        [JsonProperty] public int Version { get; set; }
        [JsonProperty] public int SampleRate
        {
            get => this.sampleRate;
            set
            {
                this.sampleRate = value;
                this.waveFormat = null;
            }
        }
        [JsonIgnore] public int Channels => 2;
        [JsonProperty] public List<Pattern> Patterns { get; private set; }
        //[JsonProperty] public List<ProviderInfo> Providers { get; set; }
        [JsonProperty] public double BPS { get; set; }

        [JsonIgnore] public string ProjectFilePath => Path.Combine(this.projectPath, $"{Env.ProjectFileName}.json");
        [JsonIgnore] public string ScriptsPath => Path.Combine(this.projectPath, Env.ScriptsFolder);
        [JsonIgnore] public string SamplesPath => Path.Combine(this.projectPath, Env.SamplesFolder);
        
        [JsonIgnore] public WaveFormat WaveFormat
        {
            get
            {
                if (this.waveFormat == null)
                {
                    this.waveFormat = WaveFormat.CreateIeeeFloatWaveFormat(this.SampleRate, 2);
                }

                return this.waveFormat;
            }
        }

        private int sampleRate;
        private string projectPath;
        public WaveFormat waveFormat;

        private Song()
        {
            this.ProviderInfo = DefaultSongProvider.ProviderInfo;
            this.Patterns = new List<Pattern>();
        }

        public static void CreateOrOpenDefault()
        {
            var projectPath = Path.Combine(Env.ApplicationPath, Env.DefaultProjectFileName);
            
            if (Directory.Exists(projectPath))
            {
                if (!Song.Open(projectPath, out var errorMessage))
                {
                    Env.AddOutput(errorMessage);
                    Song.CreateNew(projectPath, Env.DefaultProjectName, Env.DefaultSampleRate, Env.DefaultBPS);
                    return;
                }
            }

        }

        public static void CreateNew(string projectPath, string songName, int sampleRate, double bps)
        {
            Env.Song = new Song()
            {
                projectPath = projectPath,
                Name = songName,
                SampleRate = sampleRate,
                BPS = bps
            };

            Song.CreateFolders(projectPath);

            for (var p = 0; p < Env.DefaultNumberOfPatterns; ++p)
            {
                var pattern = Env.Song.NewPattern(Env.DefaultPatternLength, Env.Song.BPS);
            }

            Song.Save();

            Env.AddRecentFile(projectPath);
        }

        public static bool Open(string projectPath, out string errorMessage)
        {
            errorMessage = String.Empty;
            Song song = null;
            try
            {
                var songSerializer = SongSerializer.FromFile(new Song() { projectPath = projectPath }.ProjectFilePath);
                song = songSerializer.Song;
            }
            catch (Exception ex)
            {
                errorMessage = $"Error creating or opening default: {ex.Message}";

                return false;
            }



            Song.CreateFolders(projectPath);
            Env.Song = song;

            //if (Env.Song == null)
            //{
            //    Env.Song.projectPath = string.Empty;
            //    Song.OnSongChanged(Env.Song, SongChangedAction.Opened);
            //    return false;
            //}
            Env.Song.projectPath = projectPath;

            Song.OnSongChanged(Env.Song, SongChangedAction.Opened);

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
                Song.OnSongChanged(Env.Song, SongChangedAction.Saved);
            }
            finally
            {
                System.Windows.Forms.Cursor.Current = storeCursor;
            }
        }

        private static void CreateFolders(string projectPath)
        {
            // Ensure script directory
            var scriptDirectory = Path.Combine(projectPath, Env.ScriptsFolder);
            if (!Directory.Exists(scriptDirectory))
            {
                Directory.CreateDirectory(scriptDirectory);
            }

            // Ensure sample directory
            var sampleDirectory = Path.Combine(projectPath, Env.SamplesFolder);
            if (!Directory.Exists(sampleDirectory))
            {
                Directory.CreateDirectory(sampleDirectory);
            }

            // Ensure project
            IDEIntegration.CreateBlankProject(projectPath);
        }

        public static void Close()
        {
            if (!Dialogs.ConfirmChangesMade())
            {
                return;
            }

            Env.Song = null;

            Song.OnSongChanged(null, SongChangedAction.Closed);
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
