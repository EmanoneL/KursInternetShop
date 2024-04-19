using System;
using System.IO;
using System.Windows.Forms;

namespace KursInternetShop
{
    class MenuBulder
    {
        private string[] Lines;//содержимое файла
        Form1 form;
        ToolStrip Root = new ToolStrip();//корень дерева в иерархии - полоска меню

        public MenuBulder(string FilePath, Form1 form)
        {
            this.form = form;

            // Чтение файла
            using (StreamReader Reader = new StreamReader(FilePath))
                Lines = Reader.ReadToEnd().Split('\n');

            if (Build() == -1)
            {
                MessageBox.Show("Файл Поврежден", "Ошибка");
                return;
            }
 
            form.Controls.Add(Root);
        }
        private int Build(int level = 0, ToolStripDropDownButton root = null, int index = 0)
        {
            //ЕСЛИ ЭЛЕМЕНТ В УЗЛЕ ИЕРАРХИИ - ТО ПОСЛЕ СТАТУСА ПРОБЕЛ И ВМЕСТО МЕТОДА ТОЖЕ ПРОБЕЛ!!!            
            int i = index;
            for (; i < Lines.Length; i++)
            {
                ToolStripDropDownButton control = new ToolStripDropDownButton();

                control.Size = new System.Drawing.Size(80, 22);

                // Разделение строк на слова для извлечения параметров элемента управления
                string[] str = Lines[i].Split(' ');

                control.Text = str[1];

                //задаем видимость и доступность согласно статусу в файле
                try
                {
                    if (Convert.ToInt32(str[2]) == 0)
                    {
                        control.Visible = true;
                        control.Enabled = true;
                    }
                    else if (Convert.ToInt32(str[2]) == 1)
                    {
                        control.Visible = true;
                        control.Enabled = false;
                    }
                    else if (Convert.ToInt32(str[2]) == 2)
                    {
                        control.Visible = false;
                        control.Enabled = false;
                    }
                }
                catch
                {
                    return -1;
                }

                //прикручиваем вызываемый метод
                //if (str[3].Trim() == "FirstMethod")
                    //control.Click += form.FirstMethod;

                if (str[3].Trim() == "SecondMethod")
                    control.Click += form.SecondMethod;

                else if (str[3].Trim() == "ThirdMethod")
                    control.Click += form.ThirdMethod;

                if (level == 0)//добавляем в самый корень если уровень == 0
                    Root.Items.Add(control);

                else if (level == Convert.ToInt32(str[0])) //если мы на нужном уровне добавляем в имеющийся корень
                    root.DropDownItems.Add(control);

                if (i + 1 != Lines.Length)
                {
                    //если уровень следующего элемента больше - рекурсивный вызов с повышением индекса и уровня
                    if (Convert.ToInt32(Lines[i + 1].Split(' ')[0]) > level && level == Convert.ToInt32(str[0]))
                    {
                        i = Build(level + 1, control, i + 1);

                    }
                    if (Convert.ToInt32(Lines[i + 1].Split(' ')[0]) < level && level == Convert.ToInt32(str[0]))
                    {//выходим из рекурсии, уровень понижается так как спускаемся обратно по иерархии
                        level--;
                        return i;
                    }
                }
                else return i;
            }
            return i;
        }

    }
}
