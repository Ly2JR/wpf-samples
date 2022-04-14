using download.Controls;
using download.EventAggregator;
using Prism.Events;
using Prism.Ioc;
using Prism.Regions;
using System;
using System.Collections.Generic;
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

namespace download.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IEventAggregator _ea;

        private SolidColorBrush _normalForeground;
        private SolidColorBrush _normalBackground;
        private SolidColorBrush _activedForeground;
        private SolidColorBrush _activedBackground;

        public MainWindow(IEventAggregator ea)
        {
            InitializeComponent();

            _normalForeground = (SolidColorBrush)FindResource("NormalForeground");
            _normalBackground = (SolidColorBrush)FindResource("NormalBackground");
            _activedForeground = (SolidColorBrush)FindResource("ActivedForeground");
            _activedBackground = (SolidColorBrush)FindResource("ActivedBackground");

            _ea = ea;

            _ea.GetEvent<TaskTypeEventArgss>().Subscribe(TaskTypeSelected);
            _ea.GetEvent<NavigationRailEventArgs>().Subscribe(NavigationRail);
        }

        /// <summary>
        /// 导航
        /// </summary>
        /// <param name="navigation"></param>
        private void NavigationRail(string navigation)
        {
            switch (navigation)
            {
                case "help":
                    break;
            }
        }

        /// <summary>
        /// 任务分类选择
        /// </summary>
        /// <param name="selected"></param>
        private void TaskTypeSelected(object selected)
        {
            var iconButton = selected as IconButton;
            if (iconButton != null)
            {
                var parent = iconButton.Parent as StackPanel;
                if (parent != null)
                {
                    foreach(object item in parent.Children)
                    {
                        var child = item as IconButton;
                        if (child!=null)
                        {
                            if (child.Name != iconButton.Name)
                            {
                                child.Foreground = _normalForeground;
                                child.Background = _normalBackground;
                            }
                        }
                    }
                }
                iconButton.Foreground = _activedForeground;
                iconButton.Background = _activedBackground;
            }
        }

       
        private void WindowStackPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
            e.Handled = false;
        }
    }
}
