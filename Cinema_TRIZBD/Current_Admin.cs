using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema_TRIZBD
{
    public static class Current_Admin
    {
        public static Administrators Admin { get; private set; }

        public static void Login(Administrators admin)
        {
            Admin = admin;
        }

        public static void Logout()
        {
            Admin = null;
        }

        public static bool IsLoggedIn => Admin != null;
    }
}
