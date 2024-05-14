using DB;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Products
{
    public partial class EditProductForm : InsertProductForm
    {
        private readonly Product _product;
        private readonly DB.HandmadeShopSystemContext _context;

        public EditProductForm(Product product) : base()
        {
            InitializeComponent();
            base.LoadComboBoxes();
            _product = product ?? throw new ArgumentNullException(nameof(product));
            _context = new();
            InitializeForm();
        }

        private void InitializeForm()
        {
            // Загрузка данных выбранного продукта на форму

            nameTextBox.Text = _product.Name;
            descriptionTextBox.Text = _product.Description;
            costTextBox.Text = _product.Cost.ToString();
            statusComboBox.SelectedItem = _product.Status;
            pictureBox.Image = ConvertByteArrayToImage(_product.Picture);

            //pictureBox.Image = _product.Picture;
            categoryComboBox.SelectedIndex = (_product.IdCategory) - 1;
            sellerComboBox.SelectedIndex = (_product.IdSellers) - 1;
            storageComboBox.SelectedIndex = (_product.IdStorages) - 1;


            this.Text = "Edit Product";

            // Кнопки
            addButton.Text = "Save";
            addButton.Click -= addButton_Click; // Удаление обработчика события кнопки добавления
            addButton.Click += EditButton_Click; // Добавление нового обработчика события кнопки сохранения
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            var selectedStorageId = (int)storageComboBox.SelectedValue;
            // Обновление данных продукта
            _product.Name = nameTextBox.Text;
            _product.Description = descriptionTextBox.Text;
            _product.Cost = int.Parse(costTextBox.Text);
            _product.Status = statusComboBox.SelectedItem.ToString();
            _product.Picture = ConvertImageToByteArray(pictureBox.Image);
            _product.IdCategory = ((Category)categoryComboBox.SelectedItem).IdCategory;
            _product.IdSellers = ((Seller)sellerComboBox.SelectedItem).IdSellers;
            _product.IdStorages = selectedStorageId;
            _product.Status = statusComboBox.SelectedItem.ToString();

            _context.Update(_product);
            _context.SaveChanges();
            MessageBox.Show("Product updated successfully!");
            this.Close();
        }

        protected Bitmap ConvertByteArrayToImage(byte[] byteArray)
        {
            Bitmap image = new Bitmap(Image.FromStream(new MemoryStream(_product.Picture)));
            return image;
        }
    }
}
