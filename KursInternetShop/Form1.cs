using AdminPanel;
using System;
using System.Windows.Forms;

namespace KursInternetShop
{
    public partial class Form1 : Form
    {
        DB.User curUser;
        public Form1()
        {
            InitializeComponent();
            this.Closed += (sender, e) => Application.Exit();
            this.Name = "HandMadeShop";
        }

        public void SetUser(DB.User user)
        {
            curUser = user;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            MenuBuilderFromDB menuBulder = new MenuBuilderFromDB(this, curUser);
            NotifyUser();

        }

        private void NotifyUser()
        {
            if (curUser != null && curUser.ChangePassword == "True")
            {
                MessageBox.Show("Пожалуйста поменяйте пароль!");
            }
        }

        }
    }

