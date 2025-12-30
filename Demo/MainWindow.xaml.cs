using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Utilities.DotNet.WPF.Commands;

namespace Utilities.DotNet.WPF.Demo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public ICommand ClickCommand { get; }

        public int ClickCount { get; set; }

        public ICommand PastingCommand { get; }

        public MainWindow()
        {
            InitializeComponent();

            DataContext = this;

            ClickCommand = new DelegateCommand( OnClick );

            PastingCommand = new DelegateCommand<DataObjectPastingEventArgs>( OnPasting );
        }

        private void OnClick()
        {
            //MessageBox.Show( "Button clicked!" );
            ClickCount++;
        }

        private void OnPasting( DataObjectPastingEventArgs args )
        {
            var text = args.DataObject.GetData( DataFormats.Text ) as string;

            Dispatcher.InvokeAsync( () =>
            MessageBox.Show( $"Pasted {text}" ) );
        }
    }
}