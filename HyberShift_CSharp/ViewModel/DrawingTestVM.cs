using HyberShift_CSharp.Model.List;
using HyberShift_CSharp.Utilities;
using Newtonsoft.Json.Linq;
using Prism.Commands;
using Quobject.SocketIoClientDotNet.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace HyberShift_CSharp.ViewModel
{
    public class DrawingTestVM: BaseViewModel
    {
        private Socket socket;
        private ObservableCollection<Line> lines;
        private bool isNewLine = false;
        private double lastX, lastY;

        //getter and setter
        public DelegateCommand<object> MouseDownCommand { get; set; }
        public DelegateCommand<object> MouseMoveCommand { get; set; }
        public DelegateCommand<object> MouseUpCommand { get; set; }

        public int Thickness { get; set; }

        public Color SelectedColor { get; set; }

        public SolidColorBrush BrushColor
        {
            get { return new SolidColorBrush(SelectedColor); }
        }


        public ObservableCollection<Line> ListLine
        {
            get { return lines; }
            set
            {
                lines = value;
                NotifyChanged("ListLine");
            }
        }

        public DrawingTestVM() : base()
        {
            socket = SocketAPI.GetInstance().GetSocket();

            lines = new ObservableCollection<Line>();
            SelectedColor = Color.FromRgb(0, 0, 0);
            Thickness = 5;

            MouseDownCommand = new DelegateCommand<object>(OnMouseDown);
            MouseMoveCommand = new DelegateCommand<object>(OnMouseMove);
            MouseUpCommand = new DelegateCommand<object>(OnMouseUp);

            HandleSocket();
        }


        private void OnMouseDown(object obj)
        {
          
        }

        private void OnMouseMove(object obj)
        {
            if (Mouse.LeftButton == MouseButtonState.Released)
                return;



            Canvas canvas = obj as Canvas;
            Point currentPoint = Mouse.GetPosition(canvas);

            Line line = new Line();

            JObject data = new JObject();
            
            

            if (lines.Count == 0 || isNewLine)
            {
                line.X2 = Mouse.GetPosition(canvas).X;
                line.Y2 = Mouse.GetPosition(canvas).Y;
                isNewLine = false;
                data.Add("x2", line.X2);
                data.Add("y2", line.Y2);
                lastX = line.X2;
                lastY = line.Y2;
            }
            else
            {
                line.X2 = lastX;
                line.Y2 = lastY;
                data.Add("x2", line.X2);
                data.Add("y2", line.Y2);
            }

            line.X1 = Mouse.GetPosition(canvas).X;
            line.Y1 = Mouse.GetPosition(canvas).Y;
            line.Stroke = Brushes.Black;
            line.StrokeThickness = 5;
            data.Add("x1", line.X1);
            data.Add("y1", line.Y1);

            socket.Emit("test_drawing_line", data);

            Debug.LogOutput("Emit new drawing to server");

            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                lines.Add(line);
            });
        }

        private void OnMouseUp(object obj)
        {
            isNewLine = true;
        }

        private void HandleSocket()
        {
            socket.On("new_drawing", (arg) =>
            {

            });

            socket.On("test_drawing_line", (arg) =>
            {
                Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    Debug.LogOutput("On test drawing line to server");
                    JObject obj = (JObject)arg;
                    Line newLine = new Line();
                    newLine.Stroke = Brushes.Black;
                    newLine.StrokeThickness = 5;
                    newLine.X1 = (double)obj.GetValue("x1");
                    newLine.Y1 = (double)obj.GetValue("y1");
                    newLine.X2 = (double)obj.GetValue("x2");
                    newLine.Y2 = (double)obj.GetValue("y2");

                    if (newLine.X1 == newLine.X2 && newLine.Y1 == newLine.Y2)
                    {
                        //return;
                    }

                    lines.Add(newLine);
                });
            });
        }
    }
}
