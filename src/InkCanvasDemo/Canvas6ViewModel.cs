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
    public class Canvas6ViewModel
    {
        public ICommand ClearCommand { get; private set; }
        public Canvas6ViewModel()
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
