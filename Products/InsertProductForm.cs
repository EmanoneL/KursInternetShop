using DB;
using MenuItemConstruction;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Products
{

    public partial class InsertProductForm : Form
    {
        private readonly DB.HandmadeShopSystemContext _context;

        protected ComboBox categoryComboBox;
        protected ComboBox sellerComboBox;
        protected ComboBox storageComboBox;
        protected TextBox nameTextBox;
        protected TextBox descriptionTextBox;
        protected TextBox costTextBox;
        protected ComboBox statusComboBox;
        protected PictureBox pictureBox;
        protected Button selectImageButton;
        protected Button addButton;
        protected ErrorProvider errorProvider;

        public InsertProductForm()
        {
            
            _context = new();
            InitializeDynamicControls();
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        }

        protected void LoadComboBoxes()
        {
            // Загрузка данных для ComboBox
            LoadStatus();
            LoadCategories();
            LoadSellers();
            LoadStorages();
        }
        protected void InitializeDynamicControls()
        {
            // Создание и настройка элементов управления
            nameTextBox = new TextBox { Location = new Point(10, 30), Width = 200 };
            descriptionTextBox = new TextBox { Location = new Point(10, 80), Width = 200 };
            costTextBox = new TextBox { Location = new Point(10, 130), Width = 200 };
            statusComboBox = new ComboBox { Location = new Point(10, 180), Width = 200 };
            pictureBox = new PictureBox { Location = new Point(10, 230), Width = 200, Height = 80, SizeMode = PictureBoxSizeMode.StretchImage };
            selectImageButton = new Button { Text = "Select Image", Location = new Point(220, 230), Width = 100 };
            categoryComboBox = new ComboBox { Location = new Point(10, 340), Width = 200 };
            sellerComboBox = new ComboBox { Location = new Point(10, 390), Width = 200 };
            storageComboBox = new ComboBox { Location = new Point(10, 440), Width = 200 };
            addButton = new Button { Text = "Add", Location = new Point(10, 490), Width = 200 };
            errorProvider = new ErrorProvider();

            // Labels
            var nameLabel = new Label { Text = "Name:", Location = new Point(10, 10) };
            var descriptionLabel = new Label { Text = "Description:", Location = new Point(10, 60) };
            var costLabel = new Label { Text = "Cost:", Location = new Point(10, 110) };
            var statusLabel = new Label { Text = "Status:", Location = new Point(10, 160) };
            var pictureLabel = new Label { Text = "Picture:", Location = new Point(10, 210) };
            var categoryLabel = new Label { Text = "Category:", Location = new Point(10, 320) };
            var sellerLabel = new Label { Text = "Seller:", Location = new Point(10, 370) };
            var storageLabel = new Label { Text = "Storage:", Location = new Point(10, 420) };

            

            // Добавление элементов управления и меток на форму
            Controls.Add(selectImageButton);

            // Добавление элементов управления и меток на форму
            Controls.Add(nameLabel);
            Controls.Add(nameTextBox);
            Controls.Add(descriptionLabel);
            Controls.Add(descriptionTextBox);
            Controls.Add(costLabel);
            Controls.Add(costTextBox);
            Controls.Add(statusLabel);
            Controls.Add(statusComboBox);
            Controls.Add(pictureLabel);
            Controls.Add(pictureBox);
            Controls.Add(selectImageButton);
            Controls.Add(categoryLabel);
            Controls.Add(categoryComboBox);
            Controls.Add(sellerLabel);
            Controls.Add(sellerComboBox);
            Controls.Add(storageLabel);
            Controls.Add(storageComboBox);
            Controls.Add(addButton);

            // Привязка события для кнопки добавления
            addButton.Click += addButton_Click;
            selectImageButton.Click += selectImageButton_Click;
        }

        private void LoadStatus()
        {
            var statuses = new List<string> {"in stock", "on the way", "sold" };
            statusComboBox.DataSource = statuses;
            
        }
        private void LoadCategories()
        {
            var categories = _context.Categories.ToList();
            categoryComboBox.DataSource = categories;
            categoryComboBox.DisplayMember = "name"; 
        }

        private void LoadSellers()
        {
            var sellers = _context.Sellers.ToList();
            sellerComboBox.DataSource = sellers;
            sellerComboBox.DisplayMember = "name"; // Замените на имя свойства в вашем классе Seller
        }

        private void LoadStorages()
        {
            var storages = _context.Storages
                          .Include(s => s.IdAddressNavigation)
                              .ThenInclude(a => a.CityNavigation)
                          .Include(s => s.IdAddressNavigation)
                              .ThenInclude(a => a.StreetNavigation)
                          .ToList();

            var storageDisplayItems = storages.Select(s => new StorageDisplayItem
            {
                Id = s.IdStorages,
                AddressString = s.IdAddressNavigation != null && s.IdAddressNavigation.CityNavigation != null && s.IdAddressNavigation.StreetNavigation != null
                    ? $"{s.IdAddressNavigation.CityNavigation.CityName}, {s.IdAddressNavigation.StreetNavigation.StreetName}, {s.IdAddressNavigation.HomeNumber}"
                    : "Unknown Address"
            }).ToList();

            storageComboBox.DisplayMember = "AddressString";
            storageComboBox.ValueMember = "Id";
            storageComboBox.DataSource = storageDisplayItems;
        }

        protected void addButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsValidInput())
                {
                    var selectedStorageId = (int)storageComboBox.SelectedValue;
                    var product = new Product
                    {
                        Name = nameTextBox.Text,
                        Description = descriptionTextBox.Text,
                        Cost = int.Parse(costTextBox.Text),
                        Picture = ConvertImageToByteArray(pictureBox.Image),
                        IdCategory = ((Category)categoryComboBox.SelectedItem).IdCategory,
                        IdSellers = ((Seller)sellerComboBox.SelectedItem).IdSellers,
                        IdStorages = selectedStorageId,
                        Status = statusComboBox.SelectedItem.ToString()
                    };

                    _context.Products.Add(product);
                    _context.SaveChanges();
                    MessageBox.Show("Товар успешно добавлен");
                    ClearForm();
                }
            } catch
            {
                MessageBox.Show("Ошибка при добавлении товара");
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
                isValid = false;
            }

            if (pictureBox.Image == null)
            {
                errorProvider.SetError(pictureBox, "Picture is required");
                isValid = false;
            }

            if (!int.TryParse(costTextBox.Text, out _))
            {
                errorProvider.SetError(costTextBox, "Cost should be a number");
                isValid = false;
            }

            return isValid;
        }

        protected byte[] ConvertImageToByteArray(Image image)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    image.Save(ms, image.RawFormat);
                    return ms.ToArray();
                }
            }
            catch (System.NullReferenceException)
            {
                // Обработка null-значения
                return null;
            }
            catch (Exception ex)
            {
                // Обработка других исключений
                Console.WriteLine($"An error occurred: {ex.Message}");
                return null;
            }
        }

        private void ClearForm()
        {
            nameTextBox.Clear();
            descriptionTextBox.Clear();
            costTextBox.Clear();
            pictureBox.Image = null;
            statusComboBox.SelectedIndex = -1;
            categoryComboBox.SelectedIndex = -1;
            sellerComboBox.SelectedIndex = -1;
            storageComboBox.SelectedIndex = -1;
        }
    }



}
