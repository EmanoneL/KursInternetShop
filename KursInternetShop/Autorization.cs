using System.Collections.Generic;
using System.Linq;

namespace KursInternetShop
{
    //public class User
    //{
    //    public string Name { get; set; }
    //    public string Password { get; set; }

    //    public bool shootChangePassword { get; set; } = false;

    //    public User(string n, string p)
    //    {
    //        Name = n;
    //        Password = p;
    //    }
    //}


    class Autorization
    {
        //DB.DB db = new DB.DB();
        List<DB.User> AutorizedUsers = new List<DB.User> { };

        public DB.User getUserAutorized(DB.User user)
        {
            foreach (DB.User us in AutorizedUsers)
            {
                bool succefullAut = checkAutorize(user, us);
                if (succefullAut) return us;
            }
            return null;
        }

        private bool checkAutorize(DB.User CheckUser, DB.User AutorizeUser)
        {
            string hashPassword = CheckUser.Password;
            if ((CheckUser.Login == AutorizeUser.Login) && (hashPassword == AutorizeUser.Password)) return true;
            return false;
        }

        // Считываем зарегестрированных пользователей из БД
        private void readAutorizeDocument()
        {
            DB.HandmadeShopSystemContext _context = new();
            AutorizedUsers = _context.Users.ToList();

        }

        public Autorization()
        {
            readAutorizeDocument();
        }
    }
}
