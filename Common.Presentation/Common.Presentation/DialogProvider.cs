using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;

namespace Common.Presentation
{
    /// <summary>
    /// Default (basic) Dialog Provider
    /// </summary>
    public class DialogProvider : IDialogProvider
    {
        /// <summary>
        /// Open a browse for folder dialog and return the selected value (null if no selection)
        /// </summary>
        /// <param name="startingFolder">the starting folder to browse from</param>
        /// <returns>the folder path or null if there was no selection</returns>
        public virtual string BrowseForFolder(string startingFolder)
        {
            OpenFileDialog folderBrowser = new OpenFileDialog();
            folderBrowser.ValidateNames = false;
            folderBrowser.CheckFileExists = false;
            folderBrowser.CheckPathExists = true;

            // Always default to Folder Selection.
            folderBrowser.FileName = "Folder Selection.";
            if (folderBrowser.ShowDialog().Value)
            {
                return Path.GetDirectoryName(folderBrowser.FileName);
            }

            return null;
        }

        /// <summary>
        /// Browse for a file name to open
        /// </summary>
        /// <param name="filePattern">file name pattern for display of filenames</param>
        /// <returns>the filename selected or string.empty if there was no selection</returns>
        public virtual string BrowseForOpenFile(string filePattern = "All Files\t*.*|*.*")
        {
            OpenFileDialog ofd = new OpenFileDialog()
            {
                Filter = filePattern
            };
            if (ofd.ShowDialog().Value)
            {
                return ofd.FileName;
            }

            return string.Empty;
        }

        /// <summary>
        /// Browse for filename to save
        /// </summary>
        /// <param name="filePattern">the file pattern for the dialog</param>
        /// <param name="filename">the filename for saving (optional)</param>
        /// <returns>The selected filename or string.empty if not selected</returns>
        public virtual string BrowseForSaveFile(string filePattern = "All Files\t*.*|*.*", string filename = "")
        {
            SaveFileDialog sfd = new SaveFileDialog()
            {
                Filter = filePattern,
                FileName = filename
            };

            if (sfd.ShowDialog().Value)
            {
                return sfd.FileName;
            }

            return string.Empty;
        }

        /// <summary>
        /// Open a custom dialog (virtual method, base class implementation does nothing)
        /// </summary>
        /// <param name="keyName">the key for the dialog</param>
        /// <param name="args">parameters for the dialog</param>
        public virtual void OpenCustomDialog(string keyName, params object[] args)
        {
            
        }

        /// <summary>
        /// Open a custom dialog (virtual method, base class implementation returns default of T)
        /// </summary>
        /// <typeparam name="T">The type of the object to return</typeparam>
        /// <param name="keyName">the key for the dialog</param>
        /// <param name="args">parameters for the dialog</param>
        /// <returns>Object of type T (default of T returned by base class)</returns>
        public virtual T OpenCustomDialog<T>(string keyName, params object[] args)
        {
            return default(T);
        }

        /// <summary>
        /// Show a Message dialog box
        /// </summary>
        /// <param name="title">title for the message box</param>
        /// <param name="message">the message to show in the dialog box</param>
        public virtual void ShowMessageDialog(string title, string message)
        {
            MessageBox.Show(message, title);
        }
    }
}

