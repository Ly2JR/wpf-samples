using Ly2JR.iHome.Wpf.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Input;

namespace InkCanvasDemo
{
    public class Canvas7ViewModel
    {

        public ICommand ClearCommand { get; private set; }
        public Canvas7ViewModel()
        {
            ClearCommand = new RelayCommand((canvas) =>
            {
                if (canvas is InkCanvas main)
                {
                    main.Strokes.Clear();
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
            _isDrawing = false;
        }

        public void MouseMove(object sender, MouseEventArgs e)
        {
            if (!_isDrawing) return;
            if (sender is InkCanvas main && e.LeftButton == MouseButtonState.Pressed)
            {
                var endPoint = e.GetPosition(main);

                double a = 0.5 * (endPoint.X - _startPoint.X);
                double b = 0.5 * (endPoint.Y - _startPoint.Y);
                List<Point> pointList = new List<Point>();
                for (double r = 0; r <= 2 * Math.PI; r = r + 0.01)
                {
                    pointList.Add(new Point(0.5 * (_startPoint.X + endPoint.X) + a * Math.Cos(r), 0.5 * (_startPoint.Y + endPoint.Y) + b * Math.Sin(r)));
                }

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
