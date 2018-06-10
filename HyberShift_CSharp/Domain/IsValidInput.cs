using System.Text.RegularExpressions;

namespace HyberShift_CSharp.Domain
{
    public static class IsValidInput
    {
        // Kiểm tra định dạng Email
        public static bool isValidEmail(string value)
        {
            if (value.Trim().Length == 0)
                return false;
            var strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                           @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                           @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            var re = new Regex(strRegex);
            if (re.IsMatch(value))
                return true;
            return false;
        }

        // Kiểm tra định dạng Phone
        public static bool IsValidPhone(string value)
        {
            //var strRegex = @"^-*[0-9,\.?\-?\(?\)?\ ]+$";
            //var re = new Regex(strRegex);
            //if (re.IsMatch(value))
            //    return true;
            if (value.Trim().Length <= 13 && value.Trim().Length >= 0)
                return true;
            return false;
        }

        // Kiểm tra Name
        public static bool IsValidFullName(string value)
        {
            if (value.Trim().Length < 3)
                return false;
            return true;
        }

        // Kiểm tra Password
        public static bool IsValidPassword(string value)
        {
            if (value.Trim().Length < 6)
                return false;
            return true;
        }
    }
}