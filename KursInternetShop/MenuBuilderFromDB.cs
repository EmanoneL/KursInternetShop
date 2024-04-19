using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace KursInternetShop
{
    class MenuBuilderFromDB
    {
        Form1 form;
        ToolStrip Root = new ToolStrip();//корень дерева в иерархии - полоска меню
        ToolStripItemCollection root = null;
        User curUser = new User("admin", "admin");
        //DB.DB db = new DB.DB();

        //User curUser;

        public MenuBuilderFromDB(Form1 form, User user)
        {
            this.form = form;
            //curUser = user;
            //connectDB();
            getBasicMenuData(root, 0);
            checkUserAccess();

            form.Controls.Add(Root);

            
        }

        private MethodInfo getFunctionByName(string dllName, string functionName)
        {
            // Загрузка DLL
            Assembly assembly = Assembly.LoadFrom(dllName);

            // Получение типа
            Type type = assembly.GetType(dllName+".Form1");

            // Создание экземпляра объекта (если функция не статическая)
            object instance = Activator.CreateInstance(type);

            // Получение метода
            MethodInfo method = type.GetMethod(functionName);
            //method.Invoke(instance, null);

            return method;

        }

        private void addFunctionToButton(ToolStripDropDownItem item, string dllName, string functionName)
        {

            Action action = (Action)Delegate.CreateDelegate(typeof(Action), null, getFunctionByName(dllName, functionName));

            item.Click += (sender, e) => action();
        }
        private void getBasicMenuData(ToolStripItemCollection items, int parentId)
        {

            DB.DB db = new DB.DB();
            SqliteDataReader reader = db.SELECT(FROM:"menu", WHERE: "parent_id = " + parentId + " and orders", ORDERBY:"orders");

                if (reader.HasRows) // если есть данные
                {
                    while (reader.Read())   // построчно считываем данные
                    {   

                        var item_id = Convert.ToInt32(reader.GetValue(0));
                        var item_name = Convert.ToString(reader.GetValue(2));
                        string dllName = Convert.ToString(reader.GetValue(3));
                        string functionName = Convert.ToString(reader.GetValue(4));


                    ToolStripDropDownItem item;

                        if (parentId == 0)
                        {
                            item = new ToolStripDropDownButton(item_name);
                                

                            Root.Items.Add(item);
                        }
                        else
                        {
                            item = new ToolStripMenuItem(item_name);
                            items.Add(item);
                        }

                        if (functionName != "")
                        {
                            addFunctionToButton(item, dllName, functionName);

                        }

                        getBasicMenuData(item.DropDownItems, item_id);
                  
                    }
                

                }
            db.Close();

        }

        private void checkUserAccess()
        {
            foreach (ToolStripItem item in Root.Items)
            {
                if (!hasAccess(curUser, item))
                {
                    item.Visible = false;
                }                

            }
        }

        private bool hasAccess(User curUser, ToolStripItem item)
        {

            //string sqlExpression = @"SELECT menu_item,
            //                            CASE
            //                                WHEN rd = 0 AND write = 0 AND edit = 0 AND del = 0 THEN 0
            //                                ELSE 1
            //                            END AS has_access
            //                        FROM rights
            //                        WHERE user = '"+curUser.Name+"';";


            string sqlExpression = @"SELECT item_name, 
                                        CASE
	                                        WHEN rd = 0 AND write = 0 AND edit = 0 AND del = 0 THEN 0
	                                        ELSE 1
                                        END AS has_access
                                        FROM 
                                        (SELECT login, item_name, rd, write, edit, del
                                        FROM rights 
                                        JOIN users ON user_id = users.id
                                        JOIN menu ON rights.menu_item_id = menu.menu_item_id) as access
                                        WHERE login='" + curUser.Name + "';";
            DB.DB db = new DB.DB();
            SqliteDataReader reader = db.SELECT(sqlExpression);
                if (reader.HasRows) // если есть данные
                {
                    while (reader.Read())   // построчно считываем данные
                    {
                        var item_name = Convert.ToString(reader.GetValue(0));
                        var item_access = Convert.ToBoolean(reader.GetValue(1));

                        if (item_name == item.Text)
                        {
                            db.Close();
                            return item_access;
                        }
                    }
                db.Close();
                } 
                return true;
            
        }

        
    }
}

