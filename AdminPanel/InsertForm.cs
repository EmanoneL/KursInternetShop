using DB;
using Microsoft.Data.Sqlite;
using System;
using System.Linq;
using System.Windows.Forms;

namespace AdminPanel
{
    public partial class InsertForm : Form
    {
        private readonly DB.HandmadeShopSystemContext _context = new();
        string table = "users";
        string[] colomns = { "login", "password" };

        public InsertForm()
        {
            InitializeComponent();
            this.Name = "InsertForm";
        }

        private void insertIntoTable()
        {
            var user = new User
            {
                Login = textBox1.Text,
                Password = DB.DB.CreateMD5(textBox2.Text),
                ChangePassword = checkBox1.Checked.ToString(),
            };

            _context.Users.Add(user);
            _context.SaveChanges();

        }

        private string getUserId(DB.DB db)
        {
            string ueId = _context.Users.FirstOrDefault(p => p.Login == textBox1.Text).Id.ToString();
            return ueId;
        }
        private void giveBasicRights()
        {
            DB.DB db = new DB.DB();
            string ueId = getUserId(db);
            giveRightsToUser(db, ueId);
            db.Close();
        }

        private void giveRightsToUser(DB.DB db, string ueId)
        {
            SqliteDataReader reader = db.SELECT(values: "menu_item_id", FROM: "menu");
            while (reader.Read())
            {
                if (reader.HasRows)
                {
                    string item_id = Convert.ToString(reader.GetInt32(0));
                    string[] colomns = { "user_id", "menu_item_id", "rd", "write", "edit", "del" };
                    string[] values = { ueId, item_id, "0", "0", "0", "0" };
                    db.INSERT(INTO: "rights", colomns, values);
                }
            }

        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                errorProvider1.SetError(textBox1, "Поле должно быть заполнено!");
            }
            else if (string.IsNullOrEmpty(textBox2.Text))
            {
                errorProvider1.SetError(textBox2, "Поле должно быть заполнено!");
            }
            else
            {

                try
                {
                    insertIntoTable();
                    giveBasicRights();
                    Close();
                }
                catch
                {
                    MessageBox.Show("Ошибка при добавлении значения");
                }
            }
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            Form1 form1 = Application.OpenForms.OfType<Form1>().FirstOrDefault();
            if (form1 != null)
            {
                form1.freshDataGrid();
            }
        }
    }
}
