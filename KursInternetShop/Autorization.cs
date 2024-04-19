using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace KursInternetShop
{
    public class User
    {
        public string Name { get; set; }
        public string Password { get; set; }

        public bool shootChangePassword { get; set; } = false;

        public User(string n, string p)
        {
            Name = n;
            Password = p;
        }
    }


    class Autorization
    {
        //DB.DB db = new DB.DB();
        List<User> AutorizedUsers = new List<User> { };

        public bool isUserAutorized(User user)
        {
            foreach (User us in AutorizedUsers)
            {
                bool succefullAut = checkAutorize(user, us);
                if (succefullAut) return succefullAut;
            }
            return false;
        }

        private bool checkAutorize(User CheckUser, User AutorizeUser)
        {
            string hashPassword = DB.DB.CreateMD5(CheckUser.Password).ToLower();
            if ((CheckUser.Name == AutorizeUser.Name) && (hashPassword == AutorizeUser.Password)) return true;
            return false;
        }

        // Считываем зарегестрированных пользователей из БД
        private void readAutorizeDocument()
        {
            DB.DB db = new DB.DB();
            SqliteDataReader reader = db.SELECT(FROM:"users");
            if (reader.HasRows) // если есть данные
            {
                while (reader.Read())   // построчно считываем данные
                {
                    var name = Convert.ToString(reader.GetValue(1));
                    var password = Convert.ToString(reader.GetValue(2));
                    AutorizedUsers.Add(new User(name, password));
                }
            }
            db.Close();
        }

        public Autorization()
        {
            readAutorizeDocument();
        }
    }
}
