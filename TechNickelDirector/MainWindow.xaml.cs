using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace TechNickelDirector
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly PresentationWindow _presentationWindow;
        private WindowState _previousWindowState;
        private ResizeMode _resizeMode;
        private readonly List<Cue> _availableFiles;
        private readonly ObservableCollection<Cue> _actualCues = new ObservableCollection<Cue>();
        private Cue _currentCue;
        private readonly CueRepository _cueRepository;

        public MainWindow()
        {
            InitializeComponent();
            _presentationWindow = new PresentationWindow();
            _presentationWindow.Show();
            _availableFiles = LoadFolder(@"Z:\DropBox\Moron Report\Been A While\");           

            AvailableCues.ItemsSource = _availableFiles;
            ActualCues.ItemsSource = _actualCues;
            _cueRepository = new CueRepository();
        }

        private List<Cue> LoadFolder(string path)
        {
            var files = Directory.GetFiles(path);
            var cues = new List<Cue>();
            foreach (var file in files)
            {
                if (file.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }
                cues.Add(new Cue {FullFilename = file});
            }

            return cues;
        }

        private void FullScreenPlayer_Click(object sender, RoutedEventArgs e)
        {
            if (_presentationWindow.WindowStyle != WindowStyle.None)
            {
                _previousWindowState = _presentationWindow.WindowState;
                _resizeMode = _presentationWindow.ResizeMode;
            }

            _presentationWindow.Visibility = Visibility.Collapsed;
            _presentationWindow.ResizeMode = ResizeMode.NoResize;
            _presentationWindow.WindowState = WindowState.Maximized;
            _presentationWindow.WindowStyle = WindowStyle.None;
            _presentationWindow.Visibility = Visibility.Visible;
        }

        private void WindowedPlayer_Click(object sender, RoutedEventArgs e)
        {
            _presentationWindow.WindowState = _previousWindowState;
            _presentationWindow.ResizeMode = _resizeMode;
            _presentationWindow.WindowStyle = WindowStyle.SingleBorderWindow;
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            var media = _presentationWindow.Media;
            _presentationWindow.Media.Play();
        }

        private void PlayNextButton_Click(object sender, RoutedEventArgs e)
        {
            if (_actualCues.Count <= 0)
            {
                return;
            }
                        
            _currentCue = _actualCues[0];            
            _actualCues.RemoveAt(0);

            CurrentCue.Text = _currentCue.FileName;

            var media = _presentationWindow.Media;
            media.Source = new Uri(_currentCue.FullFilename, UriKind.RelativeOrAbsolute);
            media.Play();
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            var media = _presentationWindow.Media;
            _presentationWindow.Media.Pause();
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            var media = _presentationWindow.Media;
            _presentationWindow.Media.Stop();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to close the application", "Close Application?", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.No)
            {
                e.Cancel = true;
                return;
            }

            // show the message box here and collect the result
            _presentationWindow.Close();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (AvailableCues.SelectedItems.Count <= 0)
            {
                return;
            }

            foreach (var item in AvailableCues.SelectedItems)
            {
                _actualCues.Add((Cue)item);
            }

            AvailableCues.SelectedItems.Clear();
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (ActualCues.SelectedItems.Count <= 0)
            {
                return;
            }

            var items = ActualCues.SelectedItems.Cast<Cue>().ToList();
            foreach (var item in items)
            {
                _actualCues.Remove((Cue)item);
            }

            ActualCues.SelectedItems.Clear();
        }

        private void MoveUpButton_Click(object sender, RoutedEventArgs e)
        {
            if (ActualCues.SelectedIndex > 0)
            {
                var index = ActualCues.SelectedIndex;
                _actualCues.Move(index, index-1);                
            }
        }

        private void MoveDownButton_Click(object sender, RoutedEventArgs e)
        {
            if (ActualCues.SelectedIndex == -1)
            {
                return;
            }

            if (ActualCues.SelectedIndex < _actualCues.Count - 1){
                var index = ActualCues.SelectedIndex;
                _actualCues.Move(index, index + 1);
            }
        }

        private void MenuNew_OnClick(object sender, RoutedEventArgs e)
        {
            _actualCues.Clear();
        }

        private async void MenuSave_OnClick(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog
            {
                DefaultExt = "json",
                Filter = "Json Files (*.json)|*.json"
            };

            var result = dialog.ShowDialog();

            if (result.HasValue == false || result.Value == false)
            {
                return;
            }

            // TODO: Add Error Handling
            await _cueRepository.SaveToFile(dialog.FileName, _actualCues);
        }

        private void MenuOpen_OnClick(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                DefaultExt = "json",
                Filter = "Json Files (*.json)|*.json"
            };

            var result = dialog.ShowDialog();

            if (result.HasValue == false || result.Value == false)
            {
                return;
            }

            // TODO: Add Error Handling
            var cues = _cueRepository.LoadFromFile(dialog.FileName);

            // TOOD: Should we verify loaded files still exist?

            _actualCues.Clear();
            _actualCues.AddRange(cues);
        }
        private void MenuExit_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
