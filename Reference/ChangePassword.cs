using SQLitePCL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Reference
{
    public partial class ChangePassword : Form
    {
        ErrorProvider errorProvider = new();
        DB.User _user { get; set; }
        private readonly DB.HandmadeShopSystemContext _context;
        public ChangePassword()
        {
            _context = new();
            InitializeComponent();
            KeyPreview = true;
            UpdateFormTitle();
            InputLanguageChanged += ChangePassword_InputLanguageChanged;
            KeyDown += ChangePassword_KeyDown;
        }   
        private void ChangePassword_InputLanguageChanged(object sender, InputLanguageChangedEventArgs e)
        {
            UpdateFormTitle();
        }
        private void ChangePassword_KeyDown(object sender, KeyEventArgs e)
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

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (IsValidInput()) { 
                _user.Password = DB.DB.CreateMD5(textBox2.Text);
                _user.ChangePassword = null;
                _context.Update(_user);
                _context.SaveChanges();

                MessageBox.Show("Пароль успешно изменен!");
                this.Close();
            }
        }

        private bool IsValidInput()
        {
            bool isValid = true;
            errorProvider.Clear();

            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                errorProvider.SetError(textBox1, "Введите старый пароль");
                return false;
            }

            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                errorProvider.SetError(textBox2, "Введите новый пароль");
                return false;
            }

            if (string.IsNullOrWhiteSpace(textBox3.Text))
            {
                errorProvider.SetError(textBox3, "Повторите пароль");
                return false;
            }

            if (textBox3.Text != textBox2.Text)
            {
                errorProvider.SetError(textBox3, "Пароли не совпадают");
                return false;
            }

            string pas = DB.DB.CreateMD5(textBox1.Text);
            if (_user.Password != DB.DB.CreateMD5(textBox1.Text))
            {
                errorProvider.SetError(textBox1, "Неверно введен старый пароль");
                return false;
            }

            return isValid;
        }

        public static void ShowForm(DB.Right rights)
        {
            ChangePassword change = new();
            change._user = change._context.Users.FirstOrDefault(c => c.Id == rights.UserId);
            
            change.ShowDialog();
        }
    }
}
