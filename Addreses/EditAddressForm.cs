namespace Addreses
{
    public partial class EditAddressForm : InsertAddressForm
    {
        private readonly DB.Address _address;
        private readonly DB.HandmadeShopSystemContext _context;

        public EditAddressForm(DB.Address address) : base()
        {
            InitializeComponent();
            base.LoadComboBoxes();
            _address = address ?? throw new ArgumentNullException(nameof(address));
            _context = new();
            InitializeForm();
        }

        private void InitializeForm()
        {


            var cities = _context.Cities.ToList();
            cityComboBox.DataSource = cities;
            var streets = _context.Streets.ToList();
            streetComboBox.DataSource = streets;


            // Загрузка данных выбранного продукта на форму

            homenumberTextBox.Text = _address.HomeNumber;
            cityComboBox.SelectedItem = cities.FirstOrDefault(c => c.Id == _address.City);
            streetComboBox.SelectedItem = streets.FirstOrDefault(c => c.Id == _address.Street);


            this.Text = "Edit Address";

            // Кнопки
            addButton.Text = "Save";
            addButton.Click -= addButton_Click; // Удаление обработчика события кнопки добавления
            addButton.Click += EditButton_Click; // Добавление нового обработчика события кнопки сохранения
        }


        private void EditButton_Click(object sender, EventArgs e)
        {
            // Обновление данных продукта
            _address.HomeNumber = homenumberTextBox?.Text;
            _address.Street = ((DB.Street)streetComboBox.SelectedItem).Id;
            _address.City = ((DB.City)cityComboBox.SelectedItem).Id;


            _context.Update(_address);
            _context.SaveChanges();
            MessageBox.Show("Address updated successfully!");
            this.Close();
        }
    }
}
