using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.PowerPoint;

namespace HyberShift_CSharp.Utilities
{
    public class PresentationAPI
    {
        public PresentationAPI()
        {

        }

        public ObservableCollection<string> LoadAndExportBase64(string path, int width, int height)
        {
            ObservableCollection<string> paths = this.ExportSlide(path, width, height);
            ObservableCollection<Bitmap> bitmaps = this.ImportSlide(paths);
            ObservableCollection<string> base64Arr = this.ConvertToBase64(bitmaps);

            return base64Arr;
        }

        public ObservableCollection<string> LoadAndExportBase64(string path)
        {
            ObservableCollection<string> paths = this.ExportSlide(path);
            ObservableCollection<Bitmap> bitmaps = this.ImportSlide(paths);
            ObservableCollection<string> base64Arr = this.ConvertToBase64(bitmaps);

            return base64Arr;
        }

        public ObservableCollection<string> ExportSlide(string path, int width, int height)
        {
            ObservableCollection<string> paths = new ObservableCollection<string>();

            ApplicationClass pptApplication = new ApplicationClass();
            Presentation pptPresentation = pptApplication.Presentations.Open(path, MsoTriState.msoFalse,
            MsoTriState.msoFalse, MsoTriState.msoFalse);

            int counter = 0;
            foreach (Slide slide in pptPresentation.Slides)
            {
                string destpath = Environment.CurrentDirectory + "\\slide" + counter + ".png";
                slide.Export(destpath, "png", width, height);
                paths.Add(destpath);
                counter++;
            }

            return paths;
        }

        public ObservableCollection<string> ExportSlide(string path)
        {
            ObservableCollection<string> paths = new ObservableCollection<string>();

            ApplicationClass pptApplication = new ApplicationClass();
            Presentation pptPresentation = pptApplication.Presentations.Open(path, MsoTriState.msoFalse,
            MsoTriState.msoFalse, MsoTriState.msoFalse);

            int counter = 0;
            foreach (Slide slide in pptPresentation.Slides)
            {
                string destpath = Environment.CurrentDirectory + "\\slide" + counter + ".png";
                slide.Export(destpath, "png");
                paths.Add(destpath);
                counter++;
            }

            return paths;
        }

        public ObservableCollection<Bitmap> ImportSlide(ObservableCollection<string> paths)
        {
            ObservableCollection<Bitmap> bitmaps = new ObservableCollection<Bitmap>();
            foreach(string path in paths)
            {
                Bitmap tempBitmap = new Bitmap(path);
                bitmaps.Add(tempBitmap); 
            }
            return bitmaps;
        }

        public ObservableCollection<string> ConvertToBase64(ObservableCollection<Bitmap> bitmaps)
        {
            ObservableCollection<string> base64Array = new ObservableCollection<string>();
            foreach(Bitmap bitmap in bitmaps)
            {
                // convert to byte
                System.IO.MemoryStream stream = new System.IO.MemoryStream();
                bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
                byte[] bytes = stream.ToArray();

                // convert to base64
                string base64String = Convert.ToBase64String(bytes);
                base64Array.Add(base64String);
            }

            return base64Array;
        }

        //public BitmapSource ConvertToBitmapSource(string base64Str)
        //{
        //    // Convert Base64 String to byte[]
        //    byte[] imageBytes = Convert.FromBase64String(base64Str);
        //    MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);

        //    // Convert byte[] to Image
        //    ms.Write(imageBytes, 0, imageBytes.Length);
        //    Bitmap bitmap = new Bitmap(ms, true);
            
        //}
    }
}
