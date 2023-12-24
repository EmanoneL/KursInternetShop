using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestForm;
/*
namespace ProgLab2
{
public partial class Form1 : Form
{
public Form1()
{
InitializeComponent();
}
}
}*/
namespace KursInternetShop
{
    public partial class Form1 : Form
    {

        public string FileName;
        public Form1()
        {
            //this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            InitializeComponent();
            //MyForm test = new TestForm.MyForm();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //if (openFileDialog1.ShowDialog() != DialogResult.OK)
            //    return;

            //MenuBulder menuBulder = new MenuBulder(openFileDialog1.FileName, this);
            MenuBuilderFromDB menuBulder = new MenuBuilderFromDB(this);
        }

        public void FirstMethod(object sender, EventArgs e)
        {
            MessageBox.Show("Вызван метод 'FirstMethod'");
        }
        public void SecondMethod(object sender, EventArgs e)
        {
            MessageBox.Show("Вызван метод 'SecondMethod'");
        }
        public void ThirdMethod(object sender, EventArgs e)
        {
            MessageBox.Show("Вызван метод 'ThirdMethod'");
        }



        private void label1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
