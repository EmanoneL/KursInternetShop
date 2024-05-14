using System;
using System.Windows.Forms;

namespace KursInternetShop
{

    public partial class AutForm : Form
    {
        ErrorProvider errorProvider = new ErrorProvider();

        public AutForm()
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

        private bool IsValidInput()
        {
            bool isValid = true;
            errorProvider.Clear();

            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                errorProvider.SetError(textBox1, "Введите логин");
                return false;
            }

            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                errorProvider.SetError(textBox2, "Введите пароль");
                return false;
            }


            return isValid;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (IsValidInput())
            {
                DB.HandmadeShopSystemContext _context = new();
                var user = new DB.User
                {
                    Login = textBox1.Text,
                    Password = DB.DB.CreateMD5(textBox2.Text)
                };


                Autorization aut = new Autorization();
                var autUser = aut.getUserAutorized(user);

                if (autUser != null)
                {
                    Form1 formm = new Form1();
                    formm.SetUser(autUser);
                    //_context.Users.Find("seller2");
                    formm.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Пользователя с такими данными нет");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
        }
    }
}
