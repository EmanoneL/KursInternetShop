using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
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
    public delegate void DelegateDLLForm(object inst);
    public partial class Form1 : Form
    {
        User curUser;
        public string FileName;
        public Form1()
        {
            InitializeComponent();
        }

        public void SetUser(User user)
        {
            curUser = user;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //if (openFileDialog1.ShowDialog() != DialogResult.OK)
            //    return;

            MenuBuilderFromDB menuBulder = new MenuBuilderFromDB(this, curUser);
            //Delegate delForm = Delegate.CreateDelegate(typeof(Delegate), null, );

            //Action action = (Action)Delegate.CreateDelegate(typeof(Action), null, FirstMethod());

            //button1.Click += (sender, e) => action();
            //button1.Click +=(EventHandler)delForm;
            //button1.Click += SecondMethod;
        }

        //public MethodInfo FirstMethod()
        //{
            string dllName = "TestForm.dll";
            //string functionName = "ShowForm";

            



            // method.Invoke(instance, null);
            //return method;
        //}
        public void SecondMethod(object sender, EventArgs e)
        {
            string dllName = "TestForm.dll";
            string functionName = "ShowForm";

            // Загрузка DLL
            Assembly assembly = Assembly.LoadFrom(dllName);

            // Получение типа
            Type type = assembly.GetType("TestForm.MyForm");

            // Создание экземпляра объекта (если функция не статическая)
            object instance = Activator.CreateInstance(type);

            // Получение метода
            MethodInfo method = type.GetMethod(functionName);


            // Вызов функции

            method.Invoke(instance, null);
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
