using System;
using System.Windows.Forms;
using MenuItemConstruction;

namespace AdminPanel
{
    public partial class Form1 : Form, MenuItemSettings
    {
        string selectedUserID;
        string edtColomn;
        string table = "users";
        string[] colomns = { "id", "login", "password" };

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            MenuItemSettings.fillDataGrid(dataGridView1, $"{colomns[0]}, {colomns[1]}", table, ORDERBY:"id");

        }

        public void freshDataGrid()
        {
            dataGridView1.DataSource = null;
            MenuItemSettings.fillDataGrid(dataGridView1, $"{colomns[0]}, {colomns[1]}", table, ORDERBY: "id");
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];
                // Получение данных из выбранной строки
                selectedUserID = selectedRow.Cells[colomns[0]].Value.ToString();
                edtColomn = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].OwningColumn.HeaderText;

            }
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            this.BeginInvoke(new MethodInvoker(() =>
            {
                try
            {
                if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
                {

                    string edtCellValue = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                    DB.DB db = new DB.DB();
                    db.UPDATE(table, $"{edtColomn} = '{edtCellValue}'", $"{colomns[0]} = {selectedUserID}");
                        db.Close();

                }
            }
            catch
            {
                MessageBox.Show("Ошибка при редактировании");
            }
            freshDataGrid();
            }));
        }



        private void button1_Click(object sender, EventArgs e)
        {
            new InsertForm().Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (selectedUserID != null)
                {
                    DB.DB db = new DB.DB();
                    db.DELETE("rights", $"user_id = {selectedUserID}");
                    db.DELETE(table, $"{colomns[0]} = {selectedUserID}");
                    db.Close();
                }
                else MessageBox.Show("Пользователь не выбран");
            }
            catch
            {
                MessageBox.Show("Ошибка при удалении пользователя!");
            }
            freshDataGrid();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            new AccessForm().Show();
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show("При редактировании таблицы использован неверный формат");
            e.ThrowException = false;
            //freshDataGrid();
        }

        public static void ShowForm()
        {
            Form1 form = new Form1();
            form.Show();
        }

    }

}
