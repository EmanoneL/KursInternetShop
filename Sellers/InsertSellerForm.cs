using DB;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Drawing.Imaging;

namespace Sellers
{
    public partial class InsertSellerForm : Form
    {
        private readonly DB.HandmadeShopSystemContext _context;

        protected ComboBox bankComboBox;
        protected ComboBox addressComboBox;
        protected TextBox nameTextBox;
        protected TextBox emailTextBox;
        protected TextBox loginTextBox;
        protected TextBox passwordTextBox;
        protected TextBox phoneTextBox;
        protected PictureBox pictureBox;
        protected Button selectImageButton;
        protected Button addButton;
        protected ErrorProvider errorProvider;

        public InsertSellerForm()
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
            LoadBank();
            LoadAddress();
        }
        protected void InitializeDynamicControls()
        {
            // Создание и настройка элементов управления
            nameTextBox = new TextBox { Location = new Point(10, 30), Width = 200 };
            emailTextBox = new TextBox { Location = new Point(10, 80), Width = 200 };
            phoneTextBox = new TextBox { Location = new Point(10, 130), Width = 200 };
            loginTextBox = new TextBox { Location = new Point(10, 180), Width = 200 };
            passwordTextBox = new TextBox { Location = new Point(10, 230), Width = 200 };
            bankComboBox = new ComboBox { Location = new Point(10, 280), Width = 200 };
            addressComboBox = new ComboBox { Location = new Point(10, 330), Width = 200 };
            pictureBox = new PictureBox { Location = new Point(10, 380), Width = 200, Height = 80, SizeMode = PictureBoxSizeMode.StretchImage };
            selectImageButton = new Button { Text = "Select Image", Location = new Point(220, 430), Width = 100 };
            addButton = new Button { Text = "Add", Location = new Point(10, 480), Width = 210 };
            errorProvider = new ErrorProvider();

            // Labels
            var nameLabel = new Label { Text = "Name:", Location = new Point(10, 10) };
            var emailLabel = new Label { Text = "Email:", Location = new Point(10, 60) };
            var phoneLabel = new Label { Text = "Phone:", Location = new Point(10, 110) };
            var loginLabel = new Label { Text = "Login:", Location = new Point(10, 160) };
            var passwordLabel = new Label { Text = "Password:", Location = new Point(10, 210) };
            var bankLabel = new Label { Text = "Bank:", Location = new Point(10, 260) };
            var addressLabel = new Label { Text = "Address:", Location = new Point(10, 310) };
            var pictureLabel = new Label { Text = "Picture:", Location = new Point(10, 360) };



            // Добавление элементов управления и меток на форму
            Controls.Add(nameLabel);
            Controls.Add(nameTextBox);
            Controls.Add(emailLabel);
            Controls.Add(emailTextBox);
            Controls.Add(phoneLabel);
            Controls.Add(phoneTextBox);
            Controls.Add(loginLabel);
            Controls.Add(loginTextBox);
            Controls.Add(passwordLabel);
            Controls.Add(passwordTextBox);
            Controls.Add(bankLabel);
            Controls.Add(bankComboBox);
            Controls.Add(addressLabel);
            Controls.Add(addressComboBox);
            Controls.Add(pictureBox);
            Controls.Add(pictureLabel);
            Controls.Add(selectImageButton);
            Controls.Add(addButton);


            // Привязка события для кнопки добавления
            addButton.Click += addButton_Click;
            selectImageButton.Click += selectImageButton_Click;
        }

        private void LoadAddress()
        {
            var addresses = _context.Addresses
                  .Include(a => a.CityNavigation)
                  .Include(a => a.StreetNavigation)
              .ToList();

            var addressDisplayItems = addresses.Select(s => new AddressDisplayItem
            {
                Id = s.IdAddress,
                AddressString = s.CityNavigation != null && s.StreetNavigation != null
                    ? $"{s.CityNavigation.CityName}, {s.StreetNavigation.StreetName}, {s.HomeNumber}"
                    : "Unknown Address"
            }).ToList();

            addressComboBox.DisplayMember = "AddressString";
            addressComboBox.ValueMember = "Id";
            addressComboBox.DataSource = addressDisplayItems;
            addressComboBox.DropDownStyle = ComboBoxStyle.DropDownList;

        }
        private void LoadBank()
        {
            var banks = _context.Banks.ToList();
            bankComboBox.DataSource = banks;
            bankComboBox.DisplayMember = "name";
            bankComboBox.DropDownStyle = ComboBoxStyle.DropDownList;

        }



        protected void addButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsValidInput())
                {
                    var seller = new Seller
                    {
                        Name = nameTextBox.Text,
                        IdBank = ((Bank)bankComboBox.SelectedItem).IdBank,
                        Logo = ConvertImageToByteArray(pictureBox.Image),
                        Email = emailTextBox.Text,
                        PhoneNumber = int.Parse(phoneTextBox.Text),
                        IdAddress = ((AddressDisplayItem)addressComboBox.SelectedItem).Id,
                        Login = loginTextBox.Text,
                        Password = passwordTextBox.Text
                    };

                    _context.Sellers.Add(seller);
                    _context.SaveChanges();
                    MessageBox.Show("Продавец успешно добавлен");
                    ClearForm();
                }
            }
            catch
            {
                MessageBox.Show("Ошибка при добавлении продавца. Проверьте корректность введенных данных");
            }
        }

        private void selectImageButton_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Загрузка выбранного изображения в PictureBox
                    pictureBox.Image = new Bitmap(openFileDialog.FileName);
                }
            }
        }

        private bool IsValidInput()
        {
            bool isValid = true;
            errorProvider.Clear();

            if (string.IsNullOrWhiteSpace(nameTextBox.Text))
            {
                errorProvider.SetError(nameTextBox, "Name is required");
                return false;
            }
            if (bankComboBox.SelectedItem == null)
            {
                errorProvider.SetError(bankComboBox, "Bank is required");
                return false;
            }
            if (string.IsNullOrWhiteSpace(emailTextBox.Text))
            {
                errorProvider.SetError(nameTextBox, "Email is required");
                return false;
            }
            if (string.IsNullOrWhiteSpace(phoneTextBox.Text))
            {
                errorProvider.SetError(nameTextBox, "Phone Number is required");
                return false;
            }
            if (string.IsNullOrWhiteSpace(loginTextBox.Text))
            {
                errorProvider.SetError(nameTextBox, "Login is required");
                return false;
            }
            if (string.IsNullOrWhiteSpace(passwordTextBox.Text))
            {
                errorProvider.SetError(nameTextBox, "Password is required");
                return false;
            }
            if (addressComboBox.SelectedItem == null)
            {
                errorProvider.SetError(bankComboBox, "Address is required");
                return false;
            }




            return isValid;
        }

        protected byte[] ConvertImageToByteArray(Image image)
        {
            if (image == null)
            {
                return null;
                //throw new ArgumentNullException("image");
            }

            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, ImageFormat.Jpeg);
                return ms.ToArray();
            }

        }

        private void ClearForm()
        {
            nameTextBox.Clear();
            emailTextBox.Clear();
            phoneTextBox.Clear();
            loginTextBox.Clear();
            passwordTextBox.Clear();
            bankComboBox.SelectedIndex = -1;
            addressComboBox.SelectedItem = -1;
            pictureBox.Image = null;
        }
    }
}
