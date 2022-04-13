using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ChecksumCalculator
{
    public partial class CCWindow : Window
    {
        private int ElementsCompleted;
        public string Filename { get; set; }

        public CCWindow()
        {
            InitializeComponent();
            Filename = "Select file...";
        }

        #region Methods

        private async void InitializeHashComputation()
        {
            Init();
            List<Task> RunnableTasks = new List<Task>
            {
                GetComputedHash(HashType.Crc32, Crc32Box, Crc32Progress),
                GetComputedHash(HashType.Md5, Md5Box, Md5Progress),
                GetComputedHash(HashType.Sha1, Sha1Box, Sha1Progress),
                GetComputedHash(HashType.Sha256, Sha256Box, Sha256Progress),
                GetComputedHash(HashType.Sha512, Sha512Box, Sha512Progress)
            };

            await Task.WhenAll(RunnableTasks.ToArray());
        }

        private async Task GetComputedHash(HashType type, TextBox resultBox, ProgressBar resultBar)
        {
            await Task.Run(() =>
            {
                Hasher hasher = new Hasher(Filename);
                string hash = hasher.ComputeHash(type);

                this.Dispatcher.Invoke(() =>
                {
                    resultBox.Text = hash;
                    resultBar.Visibility = Visibility.Hidden;

                    ComputationFinished();
                });
            });
        }

        private void ComputationFinished()
        {
            ElementsCompleted++;

            if (ElementsCompleted == Enum.GetNames(typeof(HashType)).Length)
                InputBrowse.IsEnabled = true;
        }

        private void Init()
        {
            ElementsCompleted = 0;

            InputBrowse.IsEnabled = false;

            Crc32Box.Text = Md5Box.Text = Sha1Box.Text = Sha256Box.Text = Sha512Box.Text = String.Empty;
            Crc32Progress.Visibility = Md5Progress.Visibility = Sha1Progress.Visibility = Sha256Progress.Visibility = Sha512Progress.Visibility = Visibility.Visible;
        }

        #endregion

        #region Event Handlers

        private void InputBrowse_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                InputFile.Text = Filename = dlg.FileName;
                InitializeHashComputation();
            } 
            else
                return;
        }

        private void PasteButton_Click(object sender, RoutedEventArgs e)
        {
            HashBox.Text = Clipboard.GetText().Trim().ToLower();
        }

        private void Crc32Button_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(Crc32Box.Text.Trim());
        }

        private void Md5Button_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(Md5Box.Text.Trim());
        }

        private void Sha1Button_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(Sha1Box.Text.Trim());
        }

        private void Sha256Button_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(Sha256Box.Text.Trim());
        }

        private void Sha512Button_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(Sha512Box.Text.Trim());
        }

        private void VerifyButton_Click(object sender, RoutedEventArgs e)
        {
            if (HashBox.Text == String.Empty)
            {
                InfoLabel.Foreground = Brushes.Red;
                InfoLabel.Content = "Paste your hash first!";
                return;
            }

            InfoLabel.Foreground = Brushes.Green;

            if (HashBox.Text.Trim() == Crc32Box.Text.Trim())
            {
                InfoLabel.Content = "Matches with CRC32!";
                return;
            }

            if (HashBox.Text.Trim() == Md5Box.Text.Trim())
            {
                InfoLabel.Content = "Matches with MD5!";
                return;
            }

            if (HashBox.Text.Trim() == Sha1Box.Text.Trim())
            {
                InfoLabel.Content = "Matches with SHA1!";
                return;
            }

            if (HashBox.Text.Trim() == Sha256Box.Text.Trim())
            {
                InfoLabel.Content = "Matches with SHA256!";
                return;
            }

            if (HashBox.Text.Trim() == Sha512Box.Text.Trim())
            {
                InfoLabel.Content = "Matches with SHA512!";
                return;
            }

            InfoLabel.Foreground = Brushes.Red;
            InfoLabel.Content = "Does not match!";
        }

        #endregion
    }
}
