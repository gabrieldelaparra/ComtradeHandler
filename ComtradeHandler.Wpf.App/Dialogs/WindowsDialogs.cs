using System.IO;
using System.Windows;

using Microsoft.Win32;

namespace ComtradeHandler.Wpf.App.Dialogs;

public static class WindowsDialogs
{
    private const string DefaultOpenFile = "Open File...";
    private const string DefaultSaveFile = "Save File...";
    private const string DefaultSelectPath = "Select Path...";

    public static bool? AskYesNoQuestion(string dialogTitle, string question)
    {
        bool? result = false;
        var boxResult = MessageBox.Show(question, dialogTitle, MessageBoxButton.YesNo, MessageBoxImage.Question);

        result = boxResult switch {
            MessageBoxResult.Yes => true,
            MessageBoxResult.No => false,
            _ => result
        };

        return result;
    }

    public static string OpenFile(string fileFilter, string title = DefaultOpenFile)
    {
        var fileName = string.Empty;

        var dialog = new OpenFileDialog {
            Title = title,
            Filter = fileFilter,
            Multiselect = false,
        };

        if (dialog.ShowDialog() == true) {
            fileName = dialog.FileName;
        }

        return fileName;
    }

    public static string SelectPath(string title = DefaultSelectPath)
    {
        var path = string.Empty;

        var dialog = new OpenFileDialog() {
            Title = title,
            ValidateNames = false,
            CheckFileExists = false,
            CheckPathExists = false,
            FileName = "Click 'Open' to confirm folder selection...",
        };

        if (dialog.ShowDialog() == true) {
            var filename = dialog.FileName;
            path = new FileInfo(filename).Directory?.FullName ?? string.Empty;
        }

        return path;
    }

    public static string SaveFile(string fileFilter, string title = DefaultSaveFile)
    {
        var fileName = string.Empty;

        var dialog = new SaveFileDialog {
            Title = title,
            Filter = fileFilter
        };

        if (dialog.ShowDialog() == true) {
            fileName = dialog.FileName;
        }

        return fileName;
    }
}
