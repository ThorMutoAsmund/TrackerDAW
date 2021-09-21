﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TrackerDAW
{
    public static class Dialogs
    {
        public static bool ConfirmChangesMade()
        {
            if (Env.HasChanges)
            {
                if (MessageBox.Show("Changes have been made. Continue without saving?", "Changes Made", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                {
                    return false;
                }
            }

            return true;
        }

        public static bool OpenFile(string description, string initialPath, out string selectedFile, string filter = "Wave files (*.wav)|*.wav|All files (*.*)|*.*")
        {
            using (var dialog = new System.Windows.Forms.OpenFileDialog())
            {
                dialog.Multiselect = false;
                dialog.Title = description;
                dialog.InitialDirectory = initialPath;
                dialog.Filter = filter;
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();

                selectedFile = dialog.FileName;

                return result == System.Windows.Forms.DialogResult.OK;
            }
        }


        //public static bool BrowseFiles(string description, string initialPath, out string[] selectedFiles, string filter = "Wave files (*.wav)|*.wav|All files (*.*)|*.*")
        //{
        //    using (var dialog = new System.Windows.Forms.OpenFileDialog())
        //    {
        //        dialog.Multiselect = true;
        //        dialog.Title = description;
        //        dialog.InitialDirectory = initialPath;
        //        dialog.Filter = filter;
        //        System.Windows.Forms.DialogResult result = dialog.ShowDialog();

        //        selectedFiles = dialog.FileNames;

        //        return result == System.Windows.Forms.DialogResult.OK;
        //    }
        //}
    }
}
