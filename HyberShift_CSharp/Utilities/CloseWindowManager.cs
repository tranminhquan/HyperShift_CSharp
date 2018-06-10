using System.Windows;

namespace HyberShift_CSharp.Utilities
{
    public static class CloseWindowManager
    {
        public static void CloseLoginWindow()
        {
            foreach (Window window in Application.Current.Windows)
                if (window.Title == "SignIn")
                    window.Close();
        }

        public static void CloseMainWindow()
        {
            foreach (Window window in Application.Current.Windows)
                if (window.Title == "MainWindow")
                    window.Close();
        }

        public static void CloseCreateRoomWindow()
        {
            foreach (Window window in Application.Current.Windows)
                if (window.Title == "CreateRoom")
                    window.Close();
        }
    }
}