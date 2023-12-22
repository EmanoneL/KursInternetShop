using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KursInternetShop
{
    
    public partial class Form2 : Form
    {

        public Form2()
        {
            InitializeComponent();
            KeyPreview = true;
            UpdateFormTitle();
            InputLanguageChanged += Form2_InputLanguageChanged;
            KeyDown += Form2_KeyDown;
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void Form2_InputLanguageChanged(object sender, InputLanguageChangedEventArgs e)
        {
            UpdateFormTitle();
        }

        private void Form2_KeyDown(object sender, KeyEventArgs e)
        {
            //UpdateFormTitle();
            if (e.KeyCode == Keys.CapsLock)
            {
                UpdateFormTitle();
            }
        }

        private void UpdateFormTitle()
        {
            string layout = InputLanguage.CurrentInputLanguage.LayoutName;
            bool capsLock = Control.IsKeyLocked(Keys.CapsLock);
            this.label5.Text = "Раскладка: " + layout;
            this.label6.Text = string.Format("CapsLock: {1}", layout, capsLock ? "On" : "Off");
        }



        private void button1_Click(object sender, EventArgs e)
        {

            string name = textBox1.Text;
            string password = textBox2.Text;
            User user = new User(name, password);  //класс пользователя

            Autorization aut = new Autorization(); 

            if (aut.isUserAutorized(user))   
            {
                Form1 menu = new Form1();
                //menu.FileName = aut.models[user.Role];
                menu.Show();
                //this.Hide();
            } else
            {
                MessageBox.Show("Пользователя с такими данными нет");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
        }
    }
}
