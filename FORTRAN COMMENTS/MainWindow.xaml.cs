using FORTRAN_COMMENTS.Parsers;
using FORTRAN_COMMENTS.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FORTRAN_COMMENTS
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool isFileSaved = true;
        private CommanderActionsForCode commanderActions = null;
        private Adapter adapter;
        public MainWindow()
        {
            InitializeComponent();
            InitializeTabEdit();
            InitializeKeysInCodeEditor();
            InitializeTabFile();
            InitializeTabHelp();
            InitializeTabRun();
            
            adapter = new Adapter();
            commanderActions = new CommanderActionsForCode(ref TEXTBOX_WindowCodeEditor);
        }

        private void InitializeTabFile()
        {
            void MENUITEM_CreateFile_Click(object sender, RoutedEventArgs e)
            {
                bool isClose = IsCurrentFileSaved();
                if (isClose)
                {
                    bool isOpenNewFile = FileWorker.CreateFile();

                    if (isOpenNewFile)
                    {
                        TEXTBOX_WindowCodeEditor.Text = FileWorker.fileContents;

                        commanderActions = new CommanderActionsForCode(ref TEXTBOX_WindowCodeEditor);
                        isFileSaved = true;
                    }
                }
            }

            MENUITEM_CreateFile.Click += MENUITEM_CreateFile_Click;

            void MENUITEM_OpenFile_Click(object sender, RoutedEventArgs e)
            {
                bool isClose = IsCurrentFileSaved();
                if (isClose)
                {
                    bool isOpenNewFile = FileWorker.OpenFile();

                    if (isOpenNewFile)
                    {
                        TEXTBOX_WindowCodeEditor.Text = FileWorker.fileContents;

                        commanderActions = new CommanderActionsForCode(ref TEXTBOX_WindowCodeEditor);
                        isFileSaved = true;
                    }
                }
            }

            MENUITEM_OpenFile.Click += MENUITEM_OpenFile_Click;

            void MENUITEM_SaveFile_Click(object sender, RoutedEventArgs e)
            {
                FileWorker.SaveFile(TEXTBOX_WindowCodeEditor.Text);
                isFileSaved = true;
            }

            MENUITEM_SaveFile.Click += MENUITEM_SaveFile_Click;

            void MENUITEM_SaveAsFile_Click(object sender, RoutedEventArgs e)
            {
                FileWorker.SaveAsFile(TEXTBOX_WindowCodeEditor.Text);
                isFileSaved = true;
            }

            MENUITEM_SaveAsFile.Click += MENUITEM_SaveAsFile_Click;

            void MENUITEM_ExitFromProgram_Click(object sender, RoutedEventArgs e)
            {
                bool isClose = IsCurrentFileSaved();
                if (isClose)
                {
                    this.Close();
                }
            }

            MENUITEM_ExitFromProgram.Click += MENUITEM_ExitFromProgram_Click;

            void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
            {

                bool isClose = IsCurrentFileSaved();
                if (!isClose)
                {
                    e.Cancel = true;
                }
            }

            this.Closing += MainWindow_Closing;
        }
        private void InitializeTabEdit()
        {
            void MENUITEM_UndoAction_Click(object sender, RoutedEventArgs e)
            {
                commanderActions.UndoActions();
                isFileSaved = false;
            }

            MENUITEM_UndoAction.Click += MENUITEM_UndoAction_Click;

            void MENUITEM_RedoAction_Click(object sender, RoutedEventArgs e)
            {
                commanderActions.RedoActions();
                isFileSaved = false;
            }

            MENUITEM_RedoAction.Click += MENUITEM_RedoAction_Click;

            void MENUITEM_CutText_Click(object sender, RoutedEventArgs e)
            {
                commanderActions.CutAction();
                isFileSaved = false;
            }

            MENUITEM_CutText.Click += MENUITEM_CutText_Click;

            void MENUITEM_CopyText_Click(object sender, RoutedEventArgs e)
            {
                commanderActions.CopyAction();
            }

            MENUITEM_CopyText.Click += MENUITEM_CopyText_Click;

            void MENUITEM_PasteText_Click(object sender, RoutedEventArgs e)
            {
                commanderActions.InsertAction();
                isFileSaved = false;
            }

            MENUITEM_PasteText.Click += MENUITEM_PasteText_Click;

            void MENUITEM_DeleteText_Click(object sender, RoutedEventArgs e)
            {
                commanderActions.DeleteAction();
                isFileSaved = false;
            }

            MENUITEM_DeleteText.Click += MENUITEM_DeleteText_Click;

            void MENUITEM_SelectAllText_Click(object sender, RoutedEventArgs e)
            {
                commanderActions.SelectAllAction();
            }

            MENUITEM_SelectAllText.Click += MENUITEM_SelectAllText_Click;
        }
        private void InitializeTabHelp()
        {
            void MENUITEM_Help_Click(object sender, RoutedEventArgs e)
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "https://github.com/Eshdel/Code-Studio",
                    UseShellExecute = true
                });
            }

            MENUITEM_Help.Click += MENUITEM_Help_Click;

            void MENUITEM_About_Click(object sender, RoutedEventArgs e)
            {
                MessageBox.Show("v 1.3", "Info");
            }

            MENUITEM_About.Click += MENUITEM_About_Click;
        }
        private void InitializeTabRun()
        {
            void MENUITEM_Run_Click(object sender, RoutedEventArgs e)
            {
                this.Run();
            }

            MENUITEM_Run.Click += MENUITEM_Run_Click;
        }

        private void InitializeKeysInCodeEditor()
        {
            void TEXTBOX_WindowCodeEditor_PreviewKeyDown(object sender, KeyEventArgs e)
            {
                bool isEdit = false;
                if (Keyboard.Modifiers != ModifierKeys.Control)
                {
                    switch (e.Key)
                    {
                        case Key.Delete:
                            commanderActions.DeleteAction();
                            isEdit = true;
                            break;
                        case Key.Back:
                            commanderActions.BackspaceAction();
                            isEdit = true;
                            break;
                        default:
                            isFileSaved = false;
                            break;
                    }
                }
                else
                {
                    switch (e.Key)
                    {
                        case Key.X:
                            commanderActions.CutAction();
                            isEdit = true;
                            break;
                        case Key.C:
                            commanderActions.CopyAction();
                            isEdit = true;
                            break;
                        case Key.A:
                            commanderActions.CopyAction();
                            isEdit = true;
                            break;
                        case Key.V:
                            commanderActions.InsertAction();
                            isEdit = true;
                            break;
                        case Key.Z:
                            commanderActions.UndoActions();
                            isEdit = true;
                            break;
                        case Key.Y:
                            commanderActions.RedoActions();
                            isEdit = true;
                            break;
                        default:
                            isFileSaved = false;
                            break;
                    }
                }

                if (isEdit)
                {
                    isFileSaved = false;
                    e.Handled = true;
                }
            }

            TEXTBOX_WindowCodeEditor.PreviewKeyDown += TEXTBOX_WindowCodeEditor_PreviewKeyDown;
        }
        private bool IsCurrentFileSaved()
        {
            bool isClose;
            if (!isFileSaved)
            {
                MessageBoxResult resultClick = MessageBox.Show("There is unsaved data. Save them?", "Warning", MessageBoxButton.YesNoCancel);

                switch (resultClick)
                {
                    case MessageBoxResult.Cancel:
                        isClose = false;
                        break;
                    case MessageBoxResult.Yes:
                        isClose = FileWorker.SaveFile(TEXTBOX_WindowCodeEditor.Text);
                        break;
                    case MessageBoxResult.No:
                        isClose = true;
                        break;
                    default:
                        isClose = false;
                        break;
                }
            }
            else
            {
                isClose = true;
            }

            return isClose;
        }

        public void Run()
        {
            IsCurrentFileSaved();
            TEXTBOX_WindowOutputerInformation.Text = adapter.GetResult(TEXTBOX_WindowCodeEditor.Text);
        }
    }
}

public class Adapter 
{
    public string GetResult(string data) 
    {
        var result = string.Empty;
        var parser = new ComparisonsBinaryOperationsParser(data);
        parser.Parse();

        foreach (Token token in parser.tokens)
        {
            result += ($"Type: {token.Type}, Value: {token.Value} \n");
        }
       
        return result;
    }
}

