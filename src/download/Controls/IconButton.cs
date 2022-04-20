using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace download.Controls
{
    public  class IconButton:Button
    {

       


        static IconButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(IconButton), new FrameworkPropertyMetadata(typeof(IconButton)));
        }


        public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon", typeof(PackIconKind), typeof(IconButton), new PropertyMetadata());

        /// <summary>
        /// Material Design In Xaml:Icon Pack
        /// </summary>
        public PackIconKind Icon
        {
            get { return (PackIconKind)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        public static readonly DependencyProperty MouseOverBackgroundProperty =
            DependencyProperty.Register("MouseOverBackground", typeof(Brush), typeof(IconButton), new PropertyMetadata());

        /// <summary>
        /// 鼠标移动到按钮上背景色
        /// </summary>
        public Brush MouseOverBackground
        {
            get { return (Brush)GetValue(MouseOverBackgroundProperty); }
            set { SetValue(MouseOverBackgroundProperty, value); }
        }

        public static readonly DependencyProperty MouseOverForegroundProperty =
            DependencyProperty.Register("MouseOverForeground", typeof(Brush), typeof(IconButton), new PropertyMetadata());

        /// <summary>
        /// 鼠标移动在按钮上前景色
        /// </summary>
        public Brush MouseOverForeground
        {
            get { return (Brush)GetValue(MouseOverForegroundProperty); }
            set { SetValue(MouseOverForegroundProperty, value); }
        }

        public static readonly DependencyProperty MousePressedBackgroundProperty=
            DependencyProperty.Register("MousePressedBackground",typeof(Brush),typeof(IconButton),new PropertyMetadata());

        /// <summary>
        /// 按钮按下背景色
        /// </summary>
        public Brush MousePressedBackground
        {
            get { return (Brush)GetValue(MousePressedBackgroundProperty); }
            set { SetValue(MousePressedBackgroundProperty, value); }
        }

        public static readonly DependencyProperty MousePressedForegroundProperty=
            DependencyProperty.Register("MousePressedForeground",typeof(Brush),typeof(IconButton),new PropertyMetadata());

        /// <summary>
        /// 按钮按下前景色
        /// </summary>
        public Brush MousePressedForeground
        {
            get { return (Brush)GetValue(MousePressedForegroundProperty); }
            set { SetValue(MousePressedForegroundProperty, value); }
        }

        public ButtonTypeEnum ButtonType
        {
            get { return (ButtonTypeEnum)GetValue(ButtonTypeProperty); }
            set { SetValue(ButtonTypeProperty, value); }
        }

        public static readonly DependencyProperty ButtonTypeProperty =
            DependencyProperty.Register("ButtonType",typeof(ButtonTypeEnum),typeof(IconButton),new PropertyMetadata(ButtonTypeEnum.ICON_TEXT));


        public enum ButtonTypeEnum
        {
            //图标文本
            ICON_TEXT = 0,
            //只有图标
            ICON = 1,
        }

    }
}
