using System.Windows;
using System.Windows.Media;
using Ly2JR.iHome.Wpf.Shared;

namespace Ly2JR.iHome.Wpf
{
    /// <summary>
    /// Button附加属性
    /// </summary>
    public static class ButtonAssist
    {


        #region 圆角半径
        /// <summary>
        /// 圆角半径
        /// </summary>
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.RegisterAttached("CornerRadius", typeof(CornerRadius), typeof(ButtonAssist),new PropertyMetadata(ButtonConsts.DefaultCornerRadius));

        public static void SetCornerRadius(DependencyObject obj,CornerRadius value)
        {
            obj.SetValue(CornerRadiusProperty, value);
        }

        public static CornerRadius GetCornerRadius(DependencyObject obj)
        {
            return (CornerRadius)obj.GetValue(CornerRadiusProperty);
        }

        #endregion

        #region 圆形按钮直径

        public static double GetCirclePoint(DependencyObject obj)
        {
            return (double)obj.GetValue(CirclePointProperty);
        }

        public static void SetCirclePoint(DependencyObject obj, double value)
        {
            obj.SetValue(CirclePointProperty, value);
        }

        /// <summary>
        /// 圆形按钮直径
        /// </summary>
        public static readonly DependencyProperty CirclePointProperty =
            DependencyProperty.RegisterAttached("CirclePoint", typeof(double), typeof(ButtonAssist));
        #endregion

        #region 鼠标移动边框颜色
        public static Brush GetMouseOverBorderBrush(DependencyObject obj)
        {
            return (Brush)obj.GetValue(MouseOverBorderBrushProperty);
        }

        public static void SetMouseOverBorderBrush(DependencyObject obj, Brush value)
        {
            obj.SetValue(MouseOverBorderBrushProperty, value);
        }

        // Using a DependencyProperty as the backing store for MouseOverBorderBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MouseOverBorderBrushProperty =
            DependencyProperty.RegisterAttached("MouseOverBorderBrush", typeof(Brush), typeof(ButtonAssist));
        #endregion

        #region 鼠标移动按钮背景色
        public static Brush GetMouseOverBackground(DependencyObject obj)
        {
            return (Brush)obj.GetValue(MouseOverBackgroundProperty);
        }

        public static void SetMouseOverBackground(DependencyObject obj, Brush value)
        {
            obj.SetValue(MouseOverBackgroundProperty, value);
        }

        // Using a DependencyProperty as the backing store for MouseOverBackground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MouseOverBackgroundProperty =
            DependencyProperty.RegisterAttached("MouseOverBackground", typeof(Brush), typeof(ButtonAssist));

        #endregion

        #region 鼠标移动按钮前景色
        public static Brush GetMouseOverForeground(DependencyObject obj)
        {
            return (Brush)obj.GetValue(MouseOverForegroundProperty);
        }

        public static void SetMouseOverForeground(DependencyObject obj, Brush value)
        {
            obj.SetValue(MouseOverForegroundProperty, value);
        }

        // Using a DependencyProperty as the backing store for MouseOverForeground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MouseOverForegroundProperty =
            DependencyProperty.RegisterAttached("MouseOverForeground", typeof(Brush), typeof(ButtonAssist));
        #endregion

        #region 按钮按下边框颜色
        public static Brush GetPressedBorderBrush(DependencyObject obj)
        {
            return (Brush)obj.GetValue(PressedBorderBrushProperty);
        }

        public static void SetPressedBorderBrush(DependencyObject obj, Brush value)
        {
            obj.SetValue(PressedBorderBrushProperty, value);
        }

        // Using a DependencyProperty as the backing store for PressedBorderBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PressedBorderBrushProperty =
            DependencyProperty.RegisterAttached("PressedBorderBrush", typeof(Brush), typeof(ButtonAssist));

        #endregion

        #region 按钮按下背景色
        public static Brush GetPressedBackground(DependencyObject obj)
        {
            return (Brush)obj.GetValue(PressedBackgroundProperty);
        }

        public static void SetPressedBackground(DependencyObject obj, Brush value)
        {
            obj.SetValue(PressedBackgroundProperty, value);
        }

        // Using a DependencyProperty as the backing store for PressedBackground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PressedBackgroundProperty =
            DependencyProperty.RegisterAttached("PressedBackground", typeof(Brush), typeof(ButtonAssist));

        #endregion

        #region 按钮按下前景色
        public static Brush GetPressedForeground(DependencyObject obj)
        {
            return (Brush)obj.GetValue(PressedForegroundProperty);
        }

        public static void SetPressedForeground(DependencyObject obj, Brush value)
        {
            obj.SetValue(PressedForegroundProperty, value);
        }

        // Using a DependencyProperty as the backing store for PressedForeground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PressedForegroundProperty =
            DependencyProperty.RegisterAttached("PressedForeground", typeof(Brush), typeof(ButtonAssist));

        #endregion
    }
}