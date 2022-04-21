using Ly2JR.iHome.Wpf.Demo.Models;
using Ly2JR.iHome.Wpf.Demo.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ly2JR.iHome.Wpf.Demo.ViewModels
{
    public class MainWindowViewModel : NotifyPropertyObject
    {
        private string? _title;
        public string? Title
        {
            get { return _title; }
            set
            {
                _title = value;
                RaisePropertyChanged("Title");
            }
        }

        private ObservableCollection<MenuItem>? _menuItems;

        public ObservableCollection<MenuItem>? MenuItems
        {
            get { return _menuItems; }
            set
            {
                _menuItems = value;
                RaisePropertyChanged("MenuItems");
            }
        }

        private MenuItem? _currentMenuItem;

        public MenuItem? CurrentMenuItem
        {
            get { return _currentMenuItem; }
            set
            {
                _currentMenuItem = value;
                RaisePropertyChanged("CurrentMenuItem");
            }
        }

        public MainWindowViewModel()
        {
            Title = "iHome UI";
            MenuItems = new ObservableCollection<MenuItem>()
            {
                new MenuItem{Name="按钮-Button",Content=new ButtonView()},
            };

            CurrentMenuItem = MenuItems[0];
        }
    }
}
