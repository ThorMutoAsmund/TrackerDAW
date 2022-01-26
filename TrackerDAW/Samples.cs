using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace TrackerDAW
{
    public static class Samples
    {
        private static string InitialPath;
        public static void ImportSamples()
        {
            if (string.IsNullOrEmpty(Samples.InitialPath))
            {
                Samples.InitialPath = Env.ApplicationPath;
            }

            string[] selectedFiles;
            if (Dialogs.OpenMultipleFiles("Select samples to import", Samples.InitialPath, out selectedFiles, filter: Dialogs.WaveFilesFilter))
            {
                if (selectedFiles?.Length > 0)
                {
                    Samples.InitialPath = Path.GetDirectoryName(selectedFiles[0]);
                }

                foreach (var sourcePath in selectedFiles)
                {
                    if (!File.Exists(sourcePath))
                    {
                        Env.AddOutput("File not found", sourcePath);
                        continue;
                    }
                    try
                    {
                        var destinationPath = Path.Combine(Env.Song.SamplesPath, Path.GetFileName(sourcePath));
                        File.Copy(sourcePath, destinationPath);
                    }
                    catch (Exception e)
                    {
                        Env.AddOutput("Error copying file", e.Message);
                    }
                }
            }
        }

        public static void DeleteSamples(IEnumerable<string> samples)
        {
            foreach (var sample in samples)
            {
                try
                {
                    var destinationPath = Path.Combine(Env.Song.SamplesPath, sample);
                    File.Delete(destinationPath);
                }
                catch (Exception e)
                {
                    Env.AddOutput("Error deleting file", e.Message);
                }
            }
        }
    }
}
