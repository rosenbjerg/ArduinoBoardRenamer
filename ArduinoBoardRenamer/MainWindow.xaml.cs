using System;
using System.Collections.Generic;
using System.IO;
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

namespace ArduinoBoardRenamer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _restoreBoards;

        public MainWindow()
        {
            InitializeComponent();
        }



        private void Load(object sender, RoutedEventArgs e)
        {
            if (!File.Exists(txtPath.Text)) return;
            _restoreBoards = File.ReadAllText(txtPath.Text);
            btnRename.IsEnabled = true;
            btnLoad.IsEnabled = false;
            txtPath.IsEnabled = false;
            txtStatus.Text = "Loaded!";
        }

        private void Rename(object sender, RoutedEventArgs e)
        {
            var replace = $"micro.build.usb_product=\"{txtBoard.Text}\"";
            if (!_restoreBoards.Contains(replace))
            {
                txtStatus.Text = $"Could not locate '{txtBoard.Text}' in '{txtPath.Text}'";
                return;
            }
            var moddedBoards = _restoreBoards.Replace(replace, $"micro.build.usb_product=\"{txtTempName.Text}\"");
            try
            {
                File.WriteAllText(txtPath.Text, moddedBoards);
                btnRename.IsEnabled = false;
                btnRestore.IsEnabled = true;
                txtBoard.IsEnabled = false;
                txtTempName.IsEnabled = false;
                txtStatus.Text = $"'{txtBoard.Text}' renamed to '{txtTempName.Text}'!\nNow flash your program to the board";
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
            txtBoard.IsEnabled = true;
            txtStatus.Text = $"'{txtBoard.Text}' restored!";
        }
    }
}
