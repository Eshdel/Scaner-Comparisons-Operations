using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FORTRAN_COMMENTS.Utils
{
    internal static class FileWorker
    {
        public static string initialDirectory { get; private set; } = null;
        public static string nameFile { get; private set; } = null;
        public static string defaultExt { get; private set; } = "txt";
        public static string filter { get; private set; } = "TXT|*.txt";
        public static string fileContents { get; private set; } = null;

        public static bool CreateFile()
        {
            SaveFileDialog createFileDialog = new SaveFileDialog();
            createFileDialog.Title = windowTitleCreateFile;
            SetSettingsWindow(createFileDialog);

            if (createFileDialog.ShowDialog() == true)
            {

                Stream myStream;
                if ((myStream = createFileDialog.OpenFile()) != null)
                {
                    myStream.Close();
                    GetSettingsWindow(createFileDialog);
                    SetFileContent("");

                    return true;
                }
            }
            return false;
        }
        public static bool OpenFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = windowTitleOpenFile;
            SetSettingsWindow(openFileDialog);

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    string content = File.ReadAllText(openFileDialog.FileName, Encoding.UTF8);
                    SetFileContent(content);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error");
                    return false;
                }
                GetSettingsWindow(openFileDialog);
                return true;
            }
            return false;
        }
        public static bool SaveFile(string data)
        {
            if (initialDirectory == null)
            {
                bool isSaved = SaveAsFile(data);
                if (!isSaved)
                    return false;
                else
                    return true;
            }


            File.WriteAllText($"{initialDirectory}\\{nameFile}.{defaultExt}", data);
            return true;
        }
        public static bool SaveAsFile(string data)
        {
            SaveFileDialog saveAsFileDialog = new SaveFileDialog();
            saveAsFileDialog.Title = windowTitleSaveAsFile;
            SetSettingsWindow(saveAsFileDialog);

            if (saveAsFileDialog.ShowDialog() == true)
            {
                GetSettingsWindow(saveAsFileDialog);
                File.WriteAllText($"{initialDirectory}\\{nameFile}.{defaultExt}", data);
                return true;
            }
            return false;
        }

        private static void SetFileContent(string content)
        {
            if (content == null)
                content = "";

            if (content == "")
                MessageBox.Show("File is empty.", "Warning");

            fileContents = content;
        }
        private static void SetSettingsWindow(FileDialog window)
        {
            if (initialDirectory == null)
                initialDirectory = Environment.CurrentDirectory;

            window.InitialDirectory = initialDirectory;
            window.FileName = nameFile;
            window.DefaultExt = defaultExt;
            window.Filter = filter;
        }
        private static void GetSettingsWindow(FileDialog window)
        {
            initialDirectory = window.InitialDirectory;

            string[] fullPathToFile = window.FileName.Split('\\');

            int lastIndex = fullPathToFile.Length - 1;
            string lastElement = fullPathToFile[lastIndex];
            string[] infoOnFile = lastElement.Split('.');

            nameFile = infoOnFile[0];
            defaultExt = infoOnFile[1];
        }

        private const string windowTitleCreateFile = "Create file";
        private const string windowTitleOpenFile = "Open file";
        private const string windowTitleSaveAsFile = "Save as file";
    }
}
