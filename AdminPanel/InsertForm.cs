using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.Sqlite;

namespace AdminPanel
{
    public partial class InsertForm : Form
    {

        string table = "users";
        string[] colomns = { "login", "password" };
        public InsertForm()
        {
            InitializeComponent();
        }

        private void insertIntoTable()
        {
            
            DB.DB db = new DB.DB();
            string[] values = { textBox1.Text,DB.DB.CreateMD5(textBox2.Text)};
            db.INSERT(table, colomns, values);
            db.Close();

        }

        private string getUserId(DB.DB db)
        {
            string ueId="";
            SqliteDataReader readerUser = db.SELECT(values: "id", FROM: "users", WHERE: $"login='{textBox1.Text}'");

            if (readerUser.Read())
            {
                ueId = Convert.ToString(readerUser.GetInt32(0));
            }
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
                insertIntoTable();
                giveBasicRights();
                Close();
                try
                {
                    
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
