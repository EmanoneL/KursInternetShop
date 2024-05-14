using DB;
using System.Data;

namespace Sellers
{
    public partial class EditSellerForm : InsertSellerForm
    {
        private readonly Seller _seller;
        private readonly DB.HandmadeShopSystemContext _context;

        public EditSellerForm(Seller seller) : base()
        {
            InitializeComponent();
            base.LoadComboBoxes();
            _seller = seller ?? throw new ArgumentNullException(nameof(seller));
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

            var banks = _context.Banks.ToList();
            bankComboBox.DataSource = banks;

            addressComboBox.DataSource = addressDisplayItems;

            // Загрузка данных выбранного продукта на форму

            nameTextBox.Text = _seller.Name;
            emailTextBox.Text = _seller.Email;
            phoneTextBox.Text = _seller.PhoneNumber.ToString();
            loginTextBox.Text = _seller.Login;
            passwordTextBox.Text = _seller.Login;
            bankComboBox.SelectedItem = banks.FirstOrDefault(c => c.IdBank == _seller.IdBank);

            addressComboBox.SelectedItem = addressDisplayItems.FirstOrDefault(c => c.Id == _seller.IdAddress);
            pictureBox.Image = ConvertByteArrayToImage(_seller.Logo);



            this.Text = "Edit Seller";

            // Кнопки
            addButton.Text = "Save";
            addButton.Click -= addButton_Click; // Удаление обработчика события кнопки добавления
            addButton.Click += EditButton_Click; // Добавление нового обработчика события кнопки сохранения
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            // Обновление данных продукта
            _seller.Name = nameTextBox.Text;
            _seller.Email = emailTextBox.Text;
            _seller.PhoneNumber = int.Parse(phoneTextBox.Text);
            _seller.IdBankNavigation = (Bank)bankComboBox.SelectedItem;
            _seller.IdAddress = ((AddressDisplayItem)addressComboBox.SelectedItem).Id;

            _seller.Logo = ConvertImageToByteArray(pictureBox.Image);

            _context.Update(_seller);
            _context.SaveChanges();
            MessageBox.Show("Seller updated successfully!");
            this.Close();
        }

        protected Bitmap ConvertByteArrayToImage(byte[] byteArray)
        {
            if (_seller.Logo == null) return null;
            Bitmap image = new Bitmap(Image.FromStream(new MemoryStream(_seller.Logo)));
            return image;
        }
    }
}
