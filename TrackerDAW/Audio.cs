using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TrackerDAW
{
    public static class Audio
    {
        private static WaveOutEvent FileWaveOut = new WaveOutEvent();


        public static void StopPlayFile()
        {
            if (FileWaveOut.PlaybackState != PlaybackState.Stopped)
            {
                FileWaveOut.Stop();
            }
        }

        public static void PlayFile(string fileName)
        {
            StopPlayFile();

            try
            {
                IWaveProvider reader;
                if (fileName.EndsWith("mp3"))
                {
                    reader = new Mp3FileReader(System.IO.Path.Combine(Env.Song.SamplesPath, fileName));
                }
                else
                {
                    reader = new WaveFileReader(System.IO.Path.Combine(Env.Song.SamplesPath, fileName));
                }
                FileWaveOut.Init(reader);
                FileWaveOut.Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error playing file: {ex.Message}");
            }

        }
    }
}
