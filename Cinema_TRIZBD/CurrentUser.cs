namespace Cinema_TRIZBD
{
    public static class CurrentUser
    {
        public static Users User { get; private set; }

        public static void Login(Users user)
        {
            User = user;
        }

        public static void Logout()
        {
            User = null;
        }

        public static bool IsLoggedIn => User != null;
    }
}
