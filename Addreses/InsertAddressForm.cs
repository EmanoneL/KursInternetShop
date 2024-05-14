namespace Addreses
{
    public partial class InsertAddressForm : Form
    {
        private readonly DB.HandmadeShopSystemContext _context;

        protected ComboBox cityComboBox;
        protected ComboBox streetComboBox;
        protected TextBox homenumberTextBox;
        protected Button addButton;
        protected ErrorProvider errorProvider;


        public InsertAddressForm()
        {

            _context = new();
            InitializeDynamicControls();
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            LoadComboBoxes();
        }

        protected void LoadComboBoxes()
        {
            // Загрузка данных для ComboBox
            LoadCities();
            LoadStreets();
        }
        protected void InitializeDynamicControls()
        {
            // Создание и настройка элементов управления
            cityComboBox = new ComboBox { Location = new Point(10, 30), Width = 200 };
            streetComboBox = new ComboBox { Location = new Point(10, 80), Width = 200 };
            homenumberTextBox = new TextBox { Location = new Point(10, 130), Width = 200 };
            addButton = new Button { Text = "Add", Location = new Point(10, 180), Width = 210 };
            errorProvider = new ErrorProvider();

            // Labels
            var cityLabel = new Label { Text = "City:", Location = new Point(10, 10) };
            var streetLabel = new Label { Text = "Street:", Location = new Point(10, 60) };
            var humeNumberLabel = new Label { Text = "Home Number:", Location = new Point(10, 110) };



            // Добавление элементов управления и меток на форму
            Controls.Add(cityComboBox);
            Controls.Add(cityLabel);
            Controls.Add(streetComboBox);
            Controls.Add(streetLabel);
            Controls.Add(homenumberTextBox);
            Controls.Add(humeNumberLabel);
            Controls.Add(addButton);


            // Привязка события для кнопки добавления
            addButton.Click += addButton_Click;
        }

        private void LoadCities()
        {
            var cities = _context.Cities.ToList();
            cityComboBox.DataSource = cities;
            cityComboBox.DisplayMember = "City name";
            cityComboBox.DropDownStyle = ComboBoxStyle.DropDownList;

        }
        private void LoadStreets()
        {
            var streets = _context.Streets.ToList();
            cityComboBox.DataSource = streets;
            cityComboBox.DisplayMember = "Street name";
            cityComboBox.DropDownStyle = ComboBoxStyle.DropDownList;

        }



        protected void addButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsValidInput())
                {
                    var address = new DB.Address
                    {
                        City = ((DB.City)cityComboBox.SelectedItem).Id,
                        Street = ((DB.Street)streetComboBox.SelectedItem).Id,
                        HomeNumber = homenumberTextBox.Text
                    };

                    _context.Addresses.Add(address);
                    _context.SaveChanges();
                    MessageBox.Show("Адрес успешно добавлен");


                    ClearForm();
                }
            }
            catch
            {
                MessageBox.Show("Ошибка при добавлении адреса. Проверьте корректность введенных данных");
            }
        }

        private bool IsValidInput()
        {
            bool isValid = true;
            errorProvider.Clear();

            if (streetComboBox.SelectedItem == null)
            {
                errorProvider.SetError(streetComboBox, "Street is required");
                return false;
            }
            if (cityComboBox.SelectedItem == null)
            {
                errorProvider.SetError(cityComboBox, "City is required");
                return false;
            }




            return isValid;
        }



        private void ClearForm()
        {
            streetComboBox.SelectedItem = -1;
            cityComboBox.SelectedItem = -1;
        }
    }
}
