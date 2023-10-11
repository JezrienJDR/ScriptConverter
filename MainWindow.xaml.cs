using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.IO;

namespace ScriptConverter
{
    
    public partial class MainWindow : Window
    {
 
        private List<string> _characters;
        private static string[] _names;

        public static string[] lines;
        public static string fileAddress;
       
        public MainWindow()
        {
            InitializeComponent();          

            TextBlock myTb = new TextBlock();
            myTb.Text = "Text is written here.";

            _characters = new List<string>();

            fileAddress = "NO ADDRESS";

            ClearErrorText();
        }



        private void RenButton_Click(object sender, RoutedEventArgs e)
        {
            RenButton.Background = Brushes.LightGreen;
            YarnButton.Background = Brushes.LightGray;

            ClearErrorText();
        }

        private void YarnButton_Click(object sender, RoutedEventArgs e)
        {
            RenButton.Background = Brushes.LightGray;
            YarnButton.Background = Brushes.LightGreen;

            ErrorText.Text = "Yarn Spinner conversion not implemented yet.";
        }

        private void BrowseFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                fileAddress = openFileDialog.FileName;
                AddressBox.Text = fileAddress;
            }
        }

        private void ConvertButton_Click(object sender, RoutedEventArgs e)
        {
            if(fileAddress == "NO ADDRESS")
            {
                ErrorText.Text = "No file specified!";
                return;
            }

            lines = File.ReadAllLines(fileAddress);

            if(File.Exists(fileAddress) == false)
            {
                ErrorText.Text = "File does not exist!";
                return;
            }

            ClearErrorText();
            RenPyConvert();
        }

        private void ClearErrorText()
        {
            ErrorText.Text = string.Empty;
        }

        private void CharacterNames_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (CharacterNames.Text.Length == 0)
            {
                ParsedNames.Text = "[No names detected]";
                return;
            }

            _characters = new List<string>();

            _names = CharacterNames.Text.Split(',');

            string nameList = "Names detected: \n";

            foreach (string name in _names)
            {
                if (name.Length == 0)
                {
                    continue;
                }

                string newName = name.Trim(' ');

                if (newName.Length == 0)
                {
                    continue;
                }

                nameList += "    ";
                nameList += newName;
                nameList += "\n";
            }

            ParsedNames.Text = nameList;
        }


        private void RenPyConvert()
        {
            // *Italics*
            // ~ {w} (dialog wait)
            // ^ {p} (paragraph)
            // # comment


            int dialogStartIndex = 0;          

            List<string> newLines = new List<string>();

            for (var i = dialogStartIndex; i < lines.Length; i++)
            {
                string newLine = lines[i];

                if (newLine == "" ||
                    newLine == " " ||
                    newLine == "pause" ||
                    newLine[0] == '#' ||
                    newLine[0] == '$'
                   )
                {
                    newLines.Add(newLine);
                    continue;
                }

                if (newLine == "Pause")
                {
                    newLines.Add("pause");
                    continue;
                }

                bool narrator = true;
                string name = "";

                if (newLine.Contains(" "))
                {
                    name = newLine.Substring(0, newLine.IndexOf(' '));

                    if (_names.Contains(name))
                    {
                        narrator = false;

                        newLine = newLine.Substring(newLine.IndexOf(' ') + 1);
                    }
                }

                newLine = $"\"{newLine}\"";

                if (newLine.Contains("~"))
                {
                    newLine = newLine.Replace("~", "{w}");
                }

                if (newLine.Contains("^"))
                {
                    newLine = newLine.Replace("^", "{p}");
                }

                if (newLine.Contains("*"))
                {
                    while (newLine.Contains("*"))
                    {
                        Console.WriteLine(newLine);

                        int startIndex = newLine.IndexOf('*');

                        newLine = newLine.Remove(newLine.IndexOf('*'), 1);
                        newLine = newLine.Insert(startIndex, "`");

                        int endIndex = newLine.IndexOf('*');
                        startIndex = newLine.IndexOf('`');

                        Console.WriteLine($"StartIndex: {startIndex}, EndIndex: {endIndex}");

                        string substring = newLine.Substring(startIndex, (endIndex - startIndex) + 1);

                        string newSubstring = substring.Trim('`', '*');
                        newSubstring = "{i}" + newSubstring + "{/i}";

                        newLine = newLine.Replace(substring, newSubstring);

                        if (newLine.Contains("`"))
                        {
                            newLine = newLine.Remove(newLine.IndexOf('`', 1));
                        }
                    }
                }

                if (narrator == false)
                {
                    newLine = $"{name} {newLine}";
                }

                newLines.Add(newLine);
                Console.WriteLine(newLine);
            }

            string[] newFile = newLines.ToArray();

            string newFileAddress = fileAddress.Substring(0, fileAddress.Length - 4);

            newFileAddress = newFileAddress.Insert(newFileAddress.Length, "_Converted.txt");

            File.WriteAllLines(newFileAddress, newFile);

            ErrorText.Text = "File created!";
        }
    }
}
