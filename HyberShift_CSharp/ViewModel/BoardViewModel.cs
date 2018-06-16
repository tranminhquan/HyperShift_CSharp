using HyberShift_CSharp.Model;
using HyberShift_CSharp.Model.Interface;
using HyberShift_CSharp.Model.List;
using HyberShift_CSharp.Utilities;
using HyberShift_CSharp.View;
using Newtonsoft.Json.Linq;
using Prism.Commands;
using Quobject.SocketIoClientDotNet.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using HyberShift_CSharp.View.Dialog;
using Microsoft.Win32;
using Xceed.Wpf.Toolkit;
using Image = System.Drawing.Image;
using System.ComponentModel;

namespace HyberShift_CSharp.ViewModel
{
    public class BoardViewModel: BaseViewModel
    {
        private int flagShowPresentation = 0; // đặt cờ để xác định presentation đang được bật hay tắt

        private Socket socket;
        private ListPointModel listPointModel;
        private RoomModel currentRoom;

        private IDialogService dialogService;
        private PresentationAPI presentation;
        private ObservableCollection<string> base64Slide;
        private ObservableCollection<BitmapImage> slideImages;
        private int slideIndex;

        //getter and setter
        public DelegateCommand<object> MouseDownCommand { get; set; }
        public DelegateCommand<object> MouseMoveCommand { get; set; }
        public DelegateCommand<object> MouseUpCommand { get; set; }
        public DelegateCommand<RoomModel> RoomChangeCommand { get; set; }
        public DelegateCommand<object> CanvasChangeCommand { get; set; }

        //presentation
        public string ProgressVisibility { get; set; }
        public DelegateCommand OpenPresentationCommand { get; set; }
        public DelegateCommand<Button> LeftSlideCommand { get; set; }
        public DelegateCommand<Button> RightSlideCommand { get; set; }
        public DelegateCommand<Border> ShowPresenationCommand { get; set; }
        public DelegateCommand SendImagePresentationCommand { get; set; }
        public DelegateCommand<object> SaveImageCommand { get; set; }
        public DelegateCommand ClearBoardCommand { get; set; }
        //Active board
        public BitmapImage CanvasBackground { get; set; }

        public int Thickness { get; set; }

        public Color SelectedColor { get; set; }

        public SolidColorBrush BrushColor
        {
            get { return new SolidColorBrush(SelectedColor); }
        }
        
        public ObservableCollection<EllipseModel> ListPoint
        {
            get { return listPointModel.List; }
            set
            {
                listPointModel.List = value;
                NotifyChanged("ListPoint");
            }
        }

        public BoardViewModel(): base()
        {
            socket = SocketAPI.GetInstance().GetSocket();
            dialogService = new DialogService();
            presentation = new PresentationAPI();
            slideIndex = 0;
            base64Slide = new ObservableCollection<string>();
            slideImages = new ObservableCollection<BitmapImage>();

            listPointModel = new ListPointModel();
            SelectedColor = Color.FromRgb(0, 0, 0);
            Thickness = 5;

            ProgressVisibility = "Collapsed";
            MouseDownCommand = new DelegateCommand<object>(OnMouseDown);
            MouseMoveCommand = new DelegateCommand<object>(OnMouseMove);
            MouseUpCommand = new DelegateCommand<object>(OnMouseUp);
            RoomChangeCommand = new DelegateCommand<RoomModel>(OnRoomChange);
            OpenPresentationCommand = new DelegateCommand(OpenPresentationSlide);
            LeftSlideCommand = new DelegateCommand<Button>(NavigateLeftSlide);
            RightSlideCommand = new DelegateCommand<Button>(NavigateRightSlide);
            ShowPresenationCommand= new DelegateCommand<Border>(ShowPresenation);
            SendImagePresentationCommand = new DelegateCommand(SendImagePresentation);
            SaveImageCommand = new DelegateCommand<object>(SaveImage);
            ClearBoardCommand = new DelegateCommand(ClearBoard);
            HandleSocket();
        }

        private void NavigateLeftSlide(Button btn)
        {
            if (slideIndex == 0)
            {
                btn.IsEnabled = false;
                return;
            }

            // emit slide
            slideIndex--;
            JObject data = new JObject();
            data.Add("room_id", currentRoom.ID);
            data.Add("slide", base64Slide[slideIndex]);
            socket.Emit("new_slide", data);
        }

        private void NavigateRightSlide(Button btn)
        {
            if (slideIndex == base64Slide.Count - 1)
            {
                btn.IsEnabled = false;
                return;
            }

            // emit slide
            slideIndex++;
            JObject data = new JObject();
            data.Add("room_id", currentRoom.ID);
            data.Add("slide", base64Slide[slideIndex]);
            socket.Emit("new_slide", data);
        }

        private void OnRoomChange(RoomModel obj)
        {
            currentRoom = obj;
            listPointModel.List.Clear();
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
            Ellipse ellipse = new Ellipse();
            ellipse.Width = Thickness;
            ellipse.Height = Thickness;
            ellipse.Fill = BrushColor;
            ellipse.Stroke = BrushColor;
            ellipse.StrokeThickness = 1;
            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                ListPoint.Add(new EllipseModel(ellipse, currentPoint));
            });
            
            JObject data = new JObject();
            data.Add("point_x", currentPoint.X);
            data.Add("point_y", currentPoint.Y);
            data.Add("thickness", Thickness);
            data.Add("r", BrushColor.Color.R);
            data.Add("g", BrushColor.Color.G);
            data.Add("b", BrushColor.Color.B);
            socket.Emit("test_drawing", data);
        }

        private void OnMouseUp(object obj)
        {
       
        }

        public void OpenPresentationSlide()
        {
            string path = dialogService.OpenFile("Choose presentation file", "Presentation (.pptx, .ppt)|*.pptx;*.ppt");

            if (path == "")
                return;

            ThreadStart starter = () =>
            {
                ProgressVisibility = "Visible";
                NotifyChanged("ProgressVisibility");
                base64Slide.Clear();
                base64Slide = presentation.LoadAndExportBase64(path);
            };

            // callback when thread done
            starter += () =>
            {
                Debug.LogOutput("Thread done");

                // init first slide
                slideIndex = 0;

                // emit slide
                JObject data = new JObject();
                data.Add("room_id", currentRoom.ID);
                data.Add("slide", base64Slide[slideIndex]);
                socket.Emit("new_slide", data);
                Debug.LogOutput("Emited new slide");
                ProgressVisibility = "Collapsed";
                NotifyChanged("ProgressVisibility");
            };

            Thread thread = new Thread(starter) { IsBackground = true };
            thread.Start();



            //BackgroundWorker worker = new BackgroundWorker();
            //worker.WorkerReportsProgress = true;
            //worker.DoWork += LoadPresentationSlide;
            //worker.ProgressChanged += UpdateProgressBar;

            //worker.RunWorkerAsync();
        }

        //private void LoadPresentationSlide(object sender, DoWorkEventArgs e)
        //{
        //    string path = dialogService.OpenFile("Choose presentation file", "Presentation (.pptx, .ppt)|*.pptx;*.ppt");

        //    if (path == "")
        //        return;

        //    // Show progress bar
        //    ProgressVisibility = "Visible";
        //    base64Slide.Clear();
        //    base64Slide = presentation.LoadAndExportBase64(path);
        //    Debug.LogOutput("Thread done");

        //    // init first slide
        //    slideIndex = 0;

        //    // emit slide
        //    JObject data = new JObject();
        //    data.Add("room_id", currentRoom.ID);
        //    data.Add("slide", base64Slide[slideIndex]);
        //    socket.Emit("new_slide", data);
        //    Debug.LogOutput("Emited new slide");

        //    // Collapsed progress bar
        //    ProgressVisibility = "Collapsed";
        //}

        //private void UpdateProgressBar(object sender, ProgressChangedEventArgs e)
        //{

        //}

        public void ShowPresenation(Border border)//, GridSplitter gridSplitter)
        {
            if (flagShowPresentation == 0)
            {
                border.Visibility = Visibility.Visible;
                //gridSplitter.Visibility = Visibility.Visible;
                flagShowPresentation = 1;
            }
            else if (flagShowPresentation == 1)
            {
                border.Visibility = Visibility.Collapsed;
                //gridSplitter.Visibility = Visibility.Visible;
                flagShowPresentation = 0;
            }
        }

        public void SendImagePresentation()
        {
            string path = dialogService.OpenFile("Choose image file", "Image (.png ,.jpg)|*.png;*.jpg");

            if (path == "")
                return;

            string encodstring = ImageUtils.CopyImageToBase64String(Image.FromFile(path));
            base64Slide.Clear();

            // emit image
            JObject data = new JObject();
            data.Add("room_id", currentRoom.ID);
            data.Add("imgstring", encodstring);
            socket.Emit("new_image", data);

            Debug.LogOutput("Emited new image");
        }

        public void SaveImage(object obj)
        {
            try
            {
                System.Windows.Controls.Canvas canvas = obj as System.Windows.Controls.Canvas;
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Title = "Save Image";
                saveFileDialog.Filter = "JPEG Image|*.jpg|Bitmap Image|*.bmp|Gif Image|*.gif|PNG Image|*.png";

                if (saveFileDialog.ShowDialog() == true)
                {
                    CreateSaveBitmap(canvas, saveFileDialog.FileName);
                    MessageDialog dialog = new MessageDialog("Save Image Complete", "Your image have been saved!");
                    dialog.Show();
                    Debug.LogOutput("Save image success!");
                }

            }
            catch (Exception e)
            {
                MessageDialog dialog = new MessageDialog("Save Image Fail", "Something wrong. Please check again!");
                dialog.Show();
                Debug.LogOutput("Save image error!");
                throw;
            }
        }

        private void CreateSaveBitmap(System.Windows.Controls.Canvas canvas, string filename)
        {
            RenderTargetBitmap rtb = new RenderTargetBitmap((int)canvas.RenderSize.Width,
                (int)canvas.RenderSize.Height, 96d, 96d, System.Windows.Media.PixelFormats.Default);
            rtb.Render(canvas);

            //var crop = new CroppedBitmap(rtb, new Int32Rect(50, 50, 250, 250));

            BitmapEncoder pngEncoder = new PngBitmapEncoder();
            pngEncoder.Frames.Add(BitmapFrame.Create(rtb));

            using (var fs = System.IO.File.OpenWrite(filename))
            {
                pngEncoder.Save(fs);
            }
        }

        public void ClearBoard()
        {
            base64Slide.Clear();

            CanvasBackground = null;
            NotifyChanged("CanvasBackground");

            listPointModel.List.Clear();
        }

        private void HandleSocket()
        {
            socket.On("new_drawing", (arg) =>
            {
               
            });

            socket.On("test_drawing", (arg) =>
            {
                Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    Debug.LogOutput("recieved test_drawing event");
                    JObject obj = (JObject)arg;
                    Point newPoint = new Point((double)obj.GetValue("point_x"), (double)obj.GetValue("point_y"));
                    double thickness = (double)obj.GetValue("thickness");
                    byte r = (byte)obj.GetValue("r");
                    byte g = (byte)obj.GetValue("g");
                    byte b = (byte)obj.GetValue("b");
                    SolidColorBrush color = new SolidColorBrush(Color.FromRgb(r, g, b));
                    Ellipse newEllipse = new Ellipse();
                    newEllipse.Width = thickness;
                    newEllipse.Height = thickness;
                    newEllipse.Fill = color;
                    newEllipse.Stroke = color;
                    newEllipse.StrokeThickness = 1;
                    ListPoint.Add(new EllipseModel(newEllipse, newPoint));
                });
            });

            socket.On("new_slide", (arg) =>
            {
                Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    JObject data = (JObject)arg;
                    string roomId = data.GetValue("room_id").ToString();
                    string slide = data.GetValue("slide").ToString();

                    if (!currentRoom.ID.Equals(roomId))
                        return;

                    // convert to image source
       
                    CanvasBackground = ImageUtils.Base64StringToBitmapSource(slide);
                    NotifyChanged("CanvasBackground");

                    Debug.LogOutput("Updated canvas background");
                });
            });

            socket.On("new_image", (arg) =>
            {
                Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    JObject data = (JObject)arg;
                    string roomId = data.GetValue("room_id").ToString();
                    string image = data.GetValue("imgstring").ToString();

                    if (!currentRoom.ID.Equals(roomId))
                        return;

                    // convert to image source

                    CanvasBackground = ImageUtils.Base64StringToBitmapSource(image);
                    NotifyChanged("CanvasBackground");

                    Debug.LogOutput("Updated canvas background");
                });
            });
        }
    }
}
