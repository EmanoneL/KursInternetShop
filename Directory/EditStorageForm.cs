using DB;
using System.Data;

namespace Directory
{
    public partial class EditStorageForm : InsertStorageForm
    {
        private readonly DB.Storage _storage;
        private readonly DB.HandmadeShopSystemContext _context;

        public EditStorageForm(DB.Storage storage) : base()
        {
            InitializeComponent();
            _storage = storage ?? throw new ArgumentNullException(nameof(storage));
            _context = new();
            InitializeForm();
        }

        private void InitializeForm()
        {
            var addressDisplayItems = _context.Addresses.Select(s => new AddressDisplayItem
            {
                Id = s.IdAddress,
                AddressString = s.CityNavigation != null && s.StreetNavigation != null
                    ? $"{s.CityNavigation.CityName}, {s.StreetNavigation.StreetName}, {s.HomeNumber}"
                    : "Unknown Address"
            }).ToList();

            addressComboBox.DataSource = addressDisplayItems;
            addressComboBox.SelectedItem = addressDisplayItems.FirstOrDefault(c => c.Id == _storage.IdAddress);

            this.Text = "Edit Storage";

            // Кнопки
            addButton.Text = "Save";
            addButton.Click -= addButton_Click; // Удаление обработчика события кнопки добавления
            addButton.Click += EditButton_Click; // Добавление нового обработчика события кнопки сохранения
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            // Обновление данных продукта
            _storage.IdAddress = ((AddressDisplayItem)addressComboBox.SelectedItem).Id;

            _context.Update(_storage);
            _context.SaveChanges();
            MessageBox.Show("Storage updated successfully!");
            this.Close();
        }
    }
}
