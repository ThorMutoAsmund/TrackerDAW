using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TrackerDAW
{
    public static class CustomCommands
    {
        public static RoutedCommand Play = new RoutedCommand();
        public static RoutedCommand PlayPattern = new RoutedCommand();
        public static RoutedCommand PlayFromStart = new RoutedCommand();
        public static RoutedCommand Stop = new RoutedCommand();
        public static RoutedCommand ExitApp = new RoutedCommand();
        //public static RoutedCommand CloseTab = new RoutedCommand();
        //public static RoutedCommand NewScript = new RoutedCommand();
        //public static RoutedCommand ImportSamples = new RoutedCommand();
        //public static RoutedCommand ImportMP3Files = new RoutedCommand();
        //public static RoutedCommand About = new RoutedCommand();
        //public static RoutedCommand CheckForUpdates = new RoutedCommand();
        //public static RoutedCommand Settings = new RoutedCommand();
        //public static RoutedCommand AddTrack = new RoutedCommand();
        //public static RoutedCommand ZoomIn = new RoutedCommand();
        //public static RoutedCommand ZoomOut = new RoutedCommand();
    }
}
