using Microsoft.Data.Sqlite;
using System;
using System.Data;
using System.Reflection;
using System.Windows.Forms;


namespace MenuItemConstruction
{
    public interface MenuItemSettings
    {

        public static Action getAction(string dllName, string functionName)
        {
            // Загрузка DLL
            Assembly assembly = Assembly.LoadFrom(dllName);

            // Получение типа
            Type type = assembly.GetType("TestForm.MyForm");

            // Создание экземпляра объекта (если функция не статическая)
            object instance = Activator.CreateInstance(type);

            // Получение метода
            MethodInfo method = type.GetMethod(functionName);

            Action action = (Action)Delegate.CreateDelegate(typeof(Action), null, method);

            return action;
        }


        public static DataTable fillDataGrid(DataGridView dataGrid, string COLOMNS = "*", string FROM = "Bank", string WHERE = "", string ORDERBY = "")
        {
            DB.DB db = new DB.DB();
            SqliteDataReader reader = db.SELECT(values: COLOMNS, FROM: FROM, WHERE: WHERE, ORDERBY: ORDERBY);
            DataTable dataTable = new DataTable();
            dataTable.Load(reader);

            dataGrid.DataSource = dataTable;
            db.Close();
            return dataTable;
        }
        public static DataTable fillDataGrid(DataGridView dataGrid, string sqlEx)
        {
            DB.DB db = new DB.DB();
            SqliteDataReader reader = db.SELECT(sqlEx);
            DataTable dataTable = new DataTable();
            dataTable.Load(reader);

            dataGrid.DataSource = dataTable;
            db.Close();
            return dataTable;
        }

        //public static void deleteButtonClick(object sender, EventArgs e, string selectedItem, string table, string colomn)
        //{
        //    try
        //    {
        //        if (selectedItem != null)
        //        {
        //            DB.DB db = new DB.DB();
        //            db.DELETE(table, $"{colomn} = {selectedU}");
        //            db.Close();
        //        }
        //        else MessageBox.Show("Пользователь не выбран");
        //    }
        //    catch
        //    {
        //        MessageBox.Show("Ошибка при удалении пользователя!");
        //    }

        //}

    }
}
