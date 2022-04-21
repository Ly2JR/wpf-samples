using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Ly2JR.iHome.Wpf
{
    /// <summary>
    /// Button附加属性
    /// </summary>
    public static class ButtonAttached
    {

        #region 圆角半径
        /// <summary>
        /// 圆角半径
        /// </summary>
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.RegisterAttached("CornerRadius", typeof(CornerRadius), typeof(ButtonAttached));

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

        public static double GetCircleDima(DependencyObject obj)
        {
            return (double)obj.GetValue(CircleDimaProperty);
        }

        public static void SetCircleDima(DependencyObject obj, double value)
        {
            obj.SetValue(CircleDimaProperty, value);
        }

        /// <summary>
        /// 圆形按钮直径
        /// </summary>
        public static readonly DependencyProperty CircleDimaProperty =
            DependencyProperty.RegisterAttached("CircleDima", typeof(double), typeof(ButtonAttached));
        #endregion



        #region IsAccent

        public static bool GetIsAccent(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsAccentProperty);
        }

        public static void SetIsAccent(DependencyObject obj, bool value)
        {
            obj.SetValue(IsAccentProperty, value);
        }

        /// <summary>
        /// 是否强调
        /// </summary>
        public static readonly DependencyProperty IsAccentProperty =
            DependencyProperty.RegisterAttached("IsAccent", typeof(bool), typeof(ButtonAttached), new PropertyMetadata(false));

        #endregion




        public static ThemeStyle GetThemeStyle(DependencyObject obj)
        {
            return (ThemeStyle)obj.GetValue(ThemeStyleProperty);
        }

        public static void SetThemeStyle(DependencyObject obj, ThemeStyle value)
        {
            obj.SetValue(ThemeStyleProperty, value);
        }

        // Using a DependencyProperty as the backing store for Theme.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ThemeStyleProperty =
            DependencyProperty.RegisterAttached("ThemeStyle", typeof(ThemeStyle), typeof(ButtonAttached), new PropertyMetadata(ThemeStyle.Light));


    }
}