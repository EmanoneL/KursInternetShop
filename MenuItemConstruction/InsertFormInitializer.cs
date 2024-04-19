using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MenuItemConstruction
{
        //ErrorProvider errorProvider1 = new ErrorProvider();
    public class InsertFormInitializer
    {
        string table = "users";
        string[] colomns = { "login", "password" };
        Form form;
        public InsertFormInitializer(Form frm, string tab, string[] col)
        {
            table = tab;
            colomns = col;
            form = frm;
        }

        public void generateForm()
        {
            int a = 1;
            DB.HandmadeShopSystemContext cont = new DB.HandmadeShopSystemContext();

            DataGridView dt = new DataGridView();

            dt.DataSource = cont.Products.ToList();
            dt.Width = 200;

            form.Controls.Add(dt);


        }






        //// Создание элементов на форме на основе входного массива строк
        //for (int i = 0; i < colomns.Length; i++)
        //{
        //    Label label = new Label();
        //    label.Text = char.ToUpper(colomns[i][0]) + colomns[i].Substring(1); ;
        //    label.Top = i * 40+30; // Устанавливаем позицию по высоте
        //    label.Left = 30;

        //    TextBox textBox = new TextBox();
        //    textBox.Width = 200;
        //    textBox.Height = 50;
        //    textBox.Top = i * 40+30; // Устанавливаем позицию по высоте
        //    textBox.Left = 150; // Устанавливаем позицию по горизонтали

        //    form.Controls.Add(label);
        //    form.Controls.Add(textBox);
        //}

        //// Создание кнопки внизу формы
        //Button button = new Button();
        //button.Text = "Add";
        //button.Top = colomns.Length * 40 + 30; // Размещаем кнопку под последним элементом
        //button.Left = 150;
        //button.Click += button_Click;
        //form.Controls.Add(button);

        //// Установка размеров и заголовка формы
        //form.Size = new System.Drawing.Size(400, colomns.Length * 70 + 0);
        //form.Text = "Input";



        private void insertIntoTable(string[] values)
        {

            DB.DB db = new DB.DB();
            db.INSERT(table, colomns, values);
            db.Close();

        }

        private List<string> getValuesFromForm()
        {
            List<string> textBoxData = new List<string>();
            foreach (Control control in form.Controls)
            {
                if (control is TextBox)
                {
                    TextBox textBox = (TextBox)control;
                    textBoxData.Add(textBox.Text);
                }
            }
            return textBoxData;
        }

        private void button_Click(object sender, EventArgs e)
        {
            foreach (Control control in form.Controls)
            {
                if (control is TextBox)
                {
                    if (string.IsNullOrEmpty(control.Text))
                    {
                        //errorProvider1.SetError(control, "Поле должно быть заполнено!");
                    }
                    else
                    {
                        try
                        {
                            string[] values = getValuesFromForm().ToArray();
                            insertIntoTable(values);
                            form.Close();
                        }
                        catch
                        {
                            MessageBox.Show("Ошибка при добавлении значения");
                        }

                    }
                }

            }
        }
    }
}
        

        //private void Form_FormClosed(object sender, FormClosedEventArgs e)
        //{
        //    Form1 form1 = Application.OpenForms.OfType<Form1>().FirstOrDefault();
        //    if (form1 != null)
        //    {
        //        form1.freshDataGrid();
        //    }
        //}

