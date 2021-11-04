using System;
using System.Windows;
using System.Windows.Controls;

namespace TrackerDAW
{
    public static class DragDropKey
    {
        public const string Sample = "Sample";
        public const string Part = "Part";
    }

    public static class DragDropExtensions
    {
        public static void CheckDragDrop(this Control control, Vector diff, object data, string dataObjectFormat, DragDropEffects effects = DragDropEffects.Move)
        {
            if (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance || Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance)
            {
                // Initialize the drag & drop operation
                DataObject dragData = new DataObject(dataObjectFormat, data);
                DragDrop.DoDragDrop(control, dragData, effects);
            }
        }
        public static void CheckDragDrop(this Control control, Vector diff, Func<object> dataGetter, string dataObjectFormat, DragDropEffects effects = DragDropEffects.Move)
        {
            if (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance || Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance)
            {
                // Initialize the drag & drop operation
                DataObject dragData = new DataObject(dataObjectFormat, dataGetter());
                DragDrop.DoDragDrop(control, dragData, effects);
            }
        }

        public static DragDropEffects CheckEffect(this DragEventArgs e)
        {
            if (e.AllowedEffects.HasFlag(DragDropEffects.Copy) && e.KeyStates.HasFlag(DragDropKeyStates.ControlKey))
            {
                return DragDropEffects.Copy;
            }
            else if (e.AllowedEffects.HasFlag(DragDropEffects.Move) && !e.KeyStates.HasFlag(DragDropKeyStates.ControlKey))
            {
                return DragDropEffects.Move;
            }

            return DragDropEffects.None;
        }
    }
}
