using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace ArduinoBoardRenamer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _restoreBoards;
        private bool _renamed;
        private List<Tuple<string, string>> _boards;

        public MainWindow()
        {
            InitializeComponent();
        }

        private IEnumerable<Tuple<string, string>> GetBoards()
        {
            var boards = Regex.Matches(_restoreBoards, "([\\w]+).build.usb_product=\"([^\"]+)\"");
            foreach (Match board in boards)
            {
                yield return new Tuple<string, string>(board.Groups[2].Value, board.Groups[1].Value);
            }
        }

        private void Load(object sender, RoutedEventArgs e)
        {
            if (!File.Exists(txtPath.Text)) return;
            _restoreBoards = File.ReadAllText(txtPath.Text);
            _boards = GetBoards().ToList();
            cboxBoard.ItemsSource = _boards.Select(b => $"{b.Item1} ({b.Item2})");
            cboxBoard.SelectedIndex = 5;
            btnRename.IsEnabled = true;
            btnLoad.IsEnabled = false;
            txtPath.IsEnabled = false;
            cboxBoard.IsEnabled = true;
            txtTempName.IsEnabled = true;
            txtStatus.Text = "Loaded!";
        }

        private void Rename(object sender, RoutedEventArgs e)
        {
            var board = _boards[cboxBoard.SelectedIndex];
            var replace = $"{board.Item2}.build.usb_product=\"{board.Item1}\"";
            if (!_restoreBoards.Contains(replace))
            {
                txtStatus.Text = $"Could not locate '{board}' in '{txtPath.Text}'";
                return;
            }
            var moddedBoards = _restoreBoards.Replace(replace, $"{board.Item2}.build.usb_product=\"{txtTempName.Text}\"");
            try
            {
                File.WriteAllText(txtPath.Text, moddedBoards);
                btnRename.IsEnabled = false;
                btnRestore.IsEnabled = true;
                cboxBoard.IsEnabled = false;
                txtTempName.IsEnabled = false;
                _renamed = true;
                txtStatus.Text = $"'{board.Item1}' renamed to '{txtTempName.Text}'!\nNow flash your program to the board";
            }
            catch (UnauthorizedAccessException)
            {
                txtStatus.Text = $"Admin rights required to rename '{txtPath.Text}'";
            }
        }

        private void Restore(object sender, RoutedEventArgs e)
        {
            File.WriteAllText(txtPath.Text, _restoreBoards);
            btnRename.IsEnabled = true;
            btnRestore.IsEnabled = false;
            txtTempName.IsEnabled = true;
            cboxBoard.IsEnabled = true;
            _renamed = false;
            txtStatus.Text = $"'{_boards[cboxBoard.SelectedIndex].Item1}' restored!";
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (!_renamed) return;
            File.WriteAllText(txtPath.Text, _restoreBoards);
        }
    }
}
