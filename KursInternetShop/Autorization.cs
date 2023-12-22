using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace KursInternetShop
{
    class User
    {
        public string Name { get; set; }
        public string Password { get; set; }

        public User(string n, string p)
        {
            Name = n;
            Password = p;
        }
    }


    class Autorization
    {
        string FilePath = "Users.txt";
        List<User> AutorizedUsers = new List<User> { };

        public Dictionary<string, string> models = new Dictionary<string, string>()
        {
            { "admin", "C:\\Users\\Ekaterina\\source\\repos\\ProgLab2\\AdminMenu.txt"},
            { "user", "C:\\Users\\Ekaterina\\source\\repos\\ProgLab2\\MenuBulder.txt"}
        };

        //
        public static string CreateMD5(string input)
        {
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                return Convert.ToHexString(hashBytes);
            }
        }
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
            string hashPassword = CreateMD5(CheckUser.Password).ToLower();
            if ((CheckUser.Name == AutorizeUser.Name) && (hashPassword == AutorizeUser.Password)) return true;
            return false;
        }

        // Считываем зарегестрированных пользователей из БД
        private void readAutorizeDocument()
        {
            string connectionString = "Data Source=handmade shop system.db";
            string sqlExpression = "SELECT * FROM users";

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                SqliteCommand command = new SqliteCommand(sqlExpression, connection);
                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows) // если есть данные
                    {
                        while (reader.Read())   // построчно считываем данные
                        {
                            var name = Convert.ToString(reader.GetValue(1));
                            var password = Convert.ToString(reader.GetValue(2));
                            AutorizedUsers.Add(new User(name, password));
                        }
                    }
                }

            }

        }

        public Autorization()
        {
            readAutorizeDocument();
        }
    }
}
