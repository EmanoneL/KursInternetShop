using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MenuItemConstruction
{
    public partial class DynamicForm : Form
    {
        private readonly Type _modelType;
        private readonly Dictionary<string, List<object>> _foreignKeys;

        public DynamicForm(Type modelType)
        {
            //InitializeComponent();
            _modelType = modelType;
            _foreignKeys = new Dictionary<string, List<object>>();

            // Создание элементов управления на основе свойств модели
            foreach (var property in _modelType.GetProperties())
            {
                if (property.Name == "IdProducts") // Исключение поля IdProducts
                {
                    continue;
                }

                var label = new Label
                {
                    Text = property.Name,
                    Width = 150,
                    Top = Controls.Count * 30 + 20
                };

                if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(ICollection<>))
                {
                    // Если свойство является коллекцией, создаем выпадающий список для выбора значений
                    var foreignKeyType = property.PropertyType.GetGenericArguments()[0];
                    var foreignKeyValues = GetForeignKeyValues(foreignKeyType);
                    var comboBox = new ComboBox
                    {
                        Name = property.Name,
                        DataSource = foreignKeyValues,
                        DisplayMember = "Name",
                        ValueMember = "Id",
                        Width = 200,
                        Left = 160,
                        Top = Controls.Count * 30 + 20
                    };

                    Controls.Add(label);
                    Controls.Add(comboBox);
                }
                else
                {
                    var textBox = new TextBox
                    {
                        Name = property.Name,
                        Width = 200,
                        Left = 160,
                        Top = Controls.Count * 30 + 20
                    };

                    Controls.Add(label);
                    Controls.Add(textBox);
                }
            }

            var saveButton = new Button
            {
                Text = "Save",
                Width = 100,
                Top = Controls.Count * 30 + 20
            };

            saveButton.Click += SaveButton_Click;

            Controls.Add(saveButton);
        }

        private List<object> GetForeignKeyValues(Type foreignKeyType)
        {
            // Здесь можно реализовать логику получения значений для выпадающего списка
            // В данном примере возвращается пустой список, его нужно заполнить данными из базы данных
            return new List<object>();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            var instance = Activator.CreateInstance(_modelType);

            foreach (var control in Controls.OfType<TextBox>())
            {
                var property = _modelType.GetProperty(control.Name);
                if (property != null)
                {
                    var value = Convert.ChangeType(control.Text, property.PropertyType);
                    property.SetValue(instance, value);
                }
            }

            foreach (var control in Controls.OfType<ComboBox>())
            {
                var property = _modelType.GetProperty(control.Name);
                if (property != null)
                {
                    var selectedValue = control.SelectedValue;
                    if (selectedValue != null)
                    {
                        var value = Convert.ChangeType(selectedValue, property.PropertyType);
                        property.SetValue(instance, value);
                    }
                }
            }

            // Здесь можно сохранить instance в базу данных
            MessageBox.Show("Data saved successfully!");
        }
    }
}
