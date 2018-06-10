namespace HyberShift_CSharp.Model
{
    public class UserInfo
    {
        private static UserInfo instance;

        // getter and setter
        public string UserId { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string Phone { get; set; }

        public string FullName { get; set; }

        public string AvatarRef { get; set; }

        public static UserInfo GetInstance()
        {
            if (instance == null)
                instance = new UserInfo();

            return instance;
        }

        public bool isValid()
        {
            return true;
        }
    }
}