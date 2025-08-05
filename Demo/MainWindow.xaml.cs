using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace Utilities.DotNet.WPF.Demo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = this;
        }
    }
}