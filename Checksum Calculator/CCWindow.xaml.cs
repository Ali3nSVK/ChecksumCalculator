using ChecksumCalculator.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ChecksumCalculator
{
    public partial class CCWindow : Window
    {
        private int ElementsCompleted;
        private Dictionary<HashType, HashUiElement> Elements;
        public string Filename { get; set; }

        public CCWindow()
        {
            InitializeComponent();
            InitializeUI();

            Filename = "Select file...";
        }

        #region Methods

        private async void InitializeHashComputation()
        {
            CompInit();
            var RunnableTasks = Elements.ToList().Select(e => GetComputedHash(e.Value));

            await Task.WhenAll(RunnableTasks.ToArray());
        }

        private async Task GetComputedHash(HashUiElement element)
        {
            await Task.Run(() =>
            {
                string hash = new Hasher(Filename).ComputeHash(element.type);

                this.Dispatcher.Invoke(() =>
                {
                    element.text.Text = hash;
                    element.prog.Visibility = Visibility.Hidden;

                    ComputationFinished();
                });
            });
        }

        private void ComputationFinished()
        {
            if (++ElementsCompleted == Enum.GetNames(typeof(HashType)).Length)
                InputBrowse.IsEnabled = true;
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

        private void HashButton_Click(HashUiElement elem)
        {
            Clipboard.SetText(elem.text.Text.Trim());
        }

        private void PasteButton_Click(object sender, RoutedEventArgs e)
        {
            HashBox.Text = Clipboard.GetText().Trim().ToLower();
        }

        private void VerifyButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(HashBox.Text))
            {
                InfoLabel.Foreground = Brushes.Red;
                InfoLabel.Content = "Paste your hash first!";
                return;
            }

            var match = Elements.Values.FirstOrDefault(x => x.text.Text == HashBox.Text.Trim());
            if (match != null)
            {
                InfoLabel.Foreground = Brushes.Green;
                InfoLabel.Content = string.Format("Matches with {0}!", match.type.ToString());
            }
            else
            {
                InfoLabel.Foreground = Brushes.Red;
                InfoLabel.Content = "Does not match!";
            }
        }

        #endregion

        #region Init

        private void CompInit()
        {
            ElementsCompleted = 0;
            InputBrowse.IsEnabled = false;

            Elements.ToList().ForEach(x =>
            {
                x.Value.text.Text = string.Empty;
                x.Value.prog.Visibility = Visibility.Visible;
            });
        }

        private void InitializeUI()
        {
            Elements = new Dictionary<HashType, HashUiElement>();

            int gridRow = 0;
            foreach (var hashType in Enum.GetValues(typeof(HashType)).Cast<HashType>())
            {
                Checksums.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

                HashUiElement element = new HashUiElement
                {
                    type = hashType,
                    label = new Label
                    {
                        Content = hashType.ToString() + ":",
                        Foreground = Brushes.DarkBlue,
                        HorizontalAlignment = HorizontalAlignment.Right
                    },
                    text = new TextBox
                    {
                        TextWrapping = TextWrapping.NoWrap,
                        IsReadOnly = true,
                        Margin = new Thickness(0, 2, 2, 0)
                    },
                    prog = new ProgressBar
                    {
                        Visibility = Visibility.Hidden,
                        IsIndeterminate = true,
                        Margin = new Thickness(0, 2, 2, 0)
                    },
                    button = new Button
                    {
                        Content = "Copy " + hashType.ToString(),
                        Margin = new Thickness(0, 2, 2, 0),
                        Width = 100
                    }
                };
                element.button.Click += (sender, e) => HashButton_Click(element);

                element.SetColumns();
                element.SetRow(gridRow++);

                Elements[hashType] = element;
                Checksums.Children.Add(element.label);
                Checksums.Children.Add(element.text);
                Checksums.Children.Add(element.prog);
                Checksums.Children.Add(element.button);
            }
        }

        #endregion
    }
}
