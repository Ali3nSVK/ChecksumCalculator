using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Checksum_Calculator
{
    public partial class MainWindow : Window
    {
        private const int HashTypes = 5;
        private static int ElementsCompleted;

        public MainWindow()
        {
            InitializeComponent();
        }

        #region Methods

        private async void InitializeHashComputation(string Filename)
        {
            Init();

            using (var inputStream = File.OpenRead(Filename))
            {
                await Task.Factory.StartNew(() => { GetComputedHash(inputStream, HashType.CRC32, Crc32Box); });
                await Task.Factory.StartNew(() => { GetComputedHash(inputStream, HashType.MD5, Md5Box); });
                await Task.Factory.StartNew(() => { GetComputedHash(inputStream, HashType.SHA1, Sha1Box); });
                await Task.Factory.StartNew(() => { GetComputedHash(inputStream, HashType.SHA256, Sha256Box); });
                await Task.Factory.StartNew(() => { GetComputedHash(inputStream, HashType.SHA512, Sha512Box); });
            }
        }

        private void GetComputedHash(FileStream fstream, HashType type, TextBox dispatchBox)
        {
            Hasher hasher = new Hasher(fstream);
            string hash = hasher.GetChecksumByType(type);

            this.Dispatcher.Invoke((Action)(() =>
            {
                dispatchBox.Text = hash;
            }));
        }

        private void ComputationFinished()
        {
            ElementsCompleted++;

            if (ElementsCompleted == HashTypes)
            {
                InputBrowse.IsEnabled = true;
            }
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
                InputFile.Foreground = Brushes.Black;
                InputFile.Text = dlg.FileName;
            }
            else
            {
                return;
            }

            InitializeHashComputation(InputFile.Text);
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

        private void Crc32Box_TextChanged(object sender, TextChangedEventArgs e)
        {
            ComputationFinished();
            Crc32Progress.Visibility = Visibility.Hidden;
        }

        private void Md5Box_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            ComputationFinished();
            Md5Progress.Visibility = Visibility.Hidden;
        }

        private void Sha1Box_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            ComputationFinished();
            Sha1Progress.Visibility = Visibility.Hidden;
        }

        private void Sha256Box_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            ComputationFinished();
            Sha256Progress.Visibility = Visibility.Hidden;
        }

        private void Sha512Box_TextChanged(object sender, TextChangedEventArgs e)
        {
            ComputationFinished();
            Sha512Progress.Visibility = Visibility.Hidden;
        }

        #endregion
    }
}
