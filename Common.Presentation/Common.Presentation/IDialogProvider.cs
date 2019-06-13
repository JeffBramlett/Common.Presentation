using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Presentation
{
    /// <summary>
    /// Contract for a Dialog Provider to be used by a View Model
    /// </summary>
    public interface IDialogProvider
    {
        /// <summary>
        /// Browse for a file name to open
        /// </summary>
        /// <param name="filePattern">file name pattern for display of filenames</param>
        /// <returns>the filename selected or string.empty if there was no selection</returns>
        string BrowseForOpenFile(string filePattern = "All Files\t*.*|*.*");

        /// <summary>
        /// Browse for filename to save
        /// </summary>
        /// <param name="filePattern">the file pattern for the dialog</param>
        /// <param name="filename">the filename for saving (optional)</param>
        /// <returns>The selected filename or string.empty if not selected</returns>
        string BrowseForSaveFile(string filePattern = "All Files\t*.*|*.*", string filename = "");

        /// <summary>
        /// Open a browse for folder dialog and return the selected value (null if no selection)
        /// </summary>
        /// <param name="startingFolder">the starting folder to browse from</param>
        /// <returns>the folder path or null if there was no selection</returns>
        string BrowseForFolder(string startingFolder);

        /// <summary>
        /// Show a Message dialog box
        /// </summary>
        /// <param name="title">title for the message box</param>
        /// <param name="message">the message to show in the dialog box</param>
        void ShowMessageDialog(string title, string message);

        /// <summary>
        /// Open a custom dialog (virtual method, base class implementation does nothing)
        /// </summary>
        /// <param name="keyName">the key for the dialog</param>
        /// <param name="args">parameters for the dialog</param>
        void OpenCustomDialog(string keyName, params object[] args);

        /// <summary>
        /// Open a custom dialog (virtual method, base class implementation returns default of T)
        /// </summary>
        /// <typeparam name="T">The type of the object to return</typeparam>
        /// <param name="keyName">the key for the dialog</param>
        /// <param name="args">parameters for the dialog</param>
        /// <returns>Object of type T (default of T returned by base class)</returns>
        T OpenCustomDialog<T>(string keyName, params object[] args);
    }
}
