using Ly2JR.iHome.Wpf.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace InkCanvasDemo
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _editingModel;

        public string EditingModel
        {
            get { return _editingModel; }
            set
            {
                _editingModel = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EditingModel)));
            }
        }

        public ICommand OkCammand { get; private set; }
        public MainWindowViewModel()
        {
            OkCammand = new RelayCommand((o) =>
            {
                MessageBox.Show(EditingModel);
            });
        }
    }
}
