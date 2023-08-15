using Ly2JR.iHome.Wpf.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace InkCanvasDemo
{
    public class Canvas8ViewModel
    {
        public ICommand ClearCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public Canvas8ViewModel()
        {
            ClearCommand = new RelayCommand((canvas) =>
            {
                if (canvas is InkCanvas main)
                {
                    main.Strokes.Clear();
                    main.Children.Clear();
                }
            });
            SaveCommand = new RelayCommand((canvas) =>
            {
                if (canvas is InkCanvas main)
                {
                    var saveFileDialog = new Microsoft.Win32.SaveFileDialog()
                    {
                        DefaultExt =".png" ,
                        Filter = "PNG(*.png)|*png",
                    };
                    var dialog= saveFileDialog.ShowDialog();
                    if (dialog != true) return;

                    using (var file = saveFileDialog.OpenFile())
                    {
                        var rtb = new RenderTargetBitmap((int)main.ActualWidth, (int)main.ActualHeight, 96d, 96d, PixelFormats.Pbgra32);
                        rtb.Render(main);
                        var encoder = new PngBitmapEncoder();
                        encoder.Frames.Add(BitmapFrame.Create(rtb));
                        encoder.Save(file);
                    }
                }
            });
        }
        /// <summary>
        /// 起始点
        /// </summary>
        private Point _startPoint;
        /// <summary>
        /// 最后的
        /// </summary>
        private Stroke? _drawerLastStroke = null;
        /// <summary>
        /// 开始绘画标识
        /// </summary>
        private bool _isDrawing = false;

        public void PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is InkCanvas main && e.LeftButton == MouseButtonState.Pressed)
            {
                _startPoint = e.GetPosition(main);
                _isDrawing = true;
                _drawerLastStroke = null;
            }
        }

        public void MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is InkCanvas main)
            {
                main.Strokes.Remove(_drawerLastStroke);
                var endPoint = e.GetPosition(main);
                var textBox = new TextBox()
                {
                    TextWrapping= TextWrapping.Wrap,
                };
                textBox.Width = Math.Abs(endPoint.X - _startPoint.X);
                textBox.Height = Math.Abs(endPoint.Y - _startPoint.Y);
                if (textBox.Width <= 100 || textBox.Height <= 40)
                {
                    textBox.Width = 100;
                    textBox.Height = 40;
                }
                InkCanvas.SetLeft(textBox, Math.Min(_startPoint.X, endPoint.X));
                InkCanvas.SetTop(textBox, Math.Min(_startPoint.Y, endPoint.Y));
                main.Children.Add(textBox);
                textBox.Focus();
            }

            _isDrawing = false;
            _drawerLastStroke = null;
        }

        public void MouseMove(object sender, MouseEventArgs e)
        {
            if (!_isDrawing) return;
            if (sender is InkCanvas main && e.LeftButton == MouseButtonState.Pressed)
            {
                var endPoint = e.GetPosition(main);

                List<Point> pointList = new List<Point>
                {
                    new Point(_startPoint.X, _startPoint.Y),
                    new Point(_startPoint.X, endPoint.Y),
                    new Point(endPoint.X, endPoint.Y),
                    new Point(endPoint.X, _startPoint.Y),
                    new Point(_startPoint.X, _startPoint.Y),
                };

                StylusPointCollection point = new StylusPointCollection(pointList);

                var stroke = new Stroke(point);

                if (_drawerLastStroke != null)
                {
                    main.Strokes.Remove(_drawerLastStroke);
                }

                main.Strokes.Add(stroke);

                _drawerLastStroke = stroke;
            }
        }
    }
}
