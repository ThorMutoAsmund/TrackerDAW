using NAudio.Wave;
using System;
using System.Threading;
using System.Windows;

namespace TrackerDAW
{
    public static class Audio
    {
        private static WaveStream fileReader;
        private static ISampleProvider sampleProvider;
        public static WaveOutEvent WaveOut { get; private set; } = new WaveOutEvent();

        private static void EnsureStopped()
        {
            if (WaveOut.PlaybackState != PlaybackState.Stopped)
            {
                WaveOut.Stop();
                if (fileReader != null)
                {
                    fileReader.Dispose();
                    fileReader = null;
                }
                //mp3Reader.Dispose();
                //FileWaveOut.Dispose();
            }
        }

        public static void Stop()
        {
            EnsureStopped();
        }

        public static void PlayFile(string fileName)
        {
            EnsureStopped();

            try
            {
                if (fileName.EndsWith("mp3"))
                {
                    fileReader = new Mp3FileReader(System.IO.Path.Combine(Env.Song.SamplesPath, fileName));
                }
                else
                {
                    fileReader = new WaveFileReader(System.IO.Path.Combine(Env.Song.SamplesPath, fileName));
                }
                WaveOut.Init(fileReader);
                WaveOut.Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error playing file: {ex.Message}");
            }
        }

        public static void Play()
        {
            EnsureStopped();

            sampleProvider = ProviderFactory.InitFromSong(Env.Song);
            WaveOut.Init(sampleProvider);
            WaveOut.Play();
        }

        public static void PlayFromStart()
        {
            EnsureStopped();
                        
            sampleProvider = ProviderFactory.InitFromSong(Env.Song);
            WaveOut.Init(sampleProvider);
            WaveOut.Play();
        }

        public static void PlayFromPatternStart(Pattern pattern)
        {
            EnsureStopped();

            sampleProvider = ProviderFactory.InitFromPattern(Env.Song, pattern);
            WaveOut.Init(sampleProvider);
            WaveOut.Play();
        }
    }
}
