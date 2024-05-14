using Microsoft.EntityFrameworkCore;

namespace Comments
{
    public partial class InsertCommentForm : Form
    {
        private readonly DB.HandmadeShopSystemContext _context;

        protected ComboBox productComboBox;
        protected ComboBox customerComboBox;
        protected ComboBox ratingComboBox;
        protected TextBox descriptionTextBox;
        protected Button addButton;
        protected ErrorProvider errorProvider;

        public InsertCommentForm()
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
            LoadProducts();
            LoadCustomers();
            LoadRating();
        }
        protected void InitializeDynamicControls()
        {


            customerComboBox = new ComboBox { Location = new Point(10, 30), Width = 200 };
            productComboBox = new ComboBox { Location = new Point(10, 80), Width = 200 };
            ratingComboBox = new ComboBox { Location = new Point(10, 130), Width = 200 };
            descriptionTextBox = new TextBox { Location = new Point(10, 180), Width = 200 };

            addButton = new Button { Text = "Add", Location = new Point(10, 230), Width = 210 };
            errorProvider = new ErrorProvider();

            // Labels
            var customerLabel = new Label { Text = "Покупатель:", Location = new Point(10, 10) };
            var productLabel = new Label { Text = "Товар:", Location = new Point(10, 60) };
            var ratingLabel = new Label { Text = "Оценка:", Location = new Point(10, 110) };
            var descriptionLabel = new Label { Text = "Описание:", Location = new Point(10, 160) };



            // Добавление элементов управления и меток на форму
            Controls.Add(customerLabel);
            Controls.Add(customerComboBox);
            Controls.Add(productLabel);
            Controls.Add(productComboBox);
            Controls.Add(ratingLabel);
            Controls.Add(ratingComboBox);
            Controls.Add(descriptionLabel);
            Controls.Add(descriptionTextBox);
            Controls.Add(addButton);

            // Привязка события для кнопки добавления
            addButton.Click += addButton_Click;
        }


        private void LoadProducts()
        {
            // Загрузка продуктов в ComboBox
            var products = _context.Products.ToList();
            productComboBox.DataSource = products;
            productComboBox.DisplayMember = "Name";
            productComboBox.ValueMember = "IdProducts";
            productComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void LoadCustomers()
        {
            // Загрузка продуктов в ComboBox
            var customers = _context.Customers.Include(u => u.Carts).ToList();
            customerComboBox.DataSource = customers;
            customerComboBox.DisplayMember = "Name";
            customerComboBox.ValueMember = "IdCustomers";
            customerComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void LoadRating()
        {
            var rating = new List<int> { 1, 2, 3, 4, 5 };
            ratingComboBox.DataSource = rating;
            ratingComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        }



        protected void addButton_Click(object sender, EventArgs e)
        {
            if (IsValidInput())
            {
                DB.Product selectedProduct = (DB.Product)productComboBox.SelectedItem;
                DB.Customer currentCustomer = ((DB.Customer)customerComboBox.SelectedItem);

                var comment = new DB.Comment
                {
                    IdProduct = ((DB.Product)productComboBox.SelectedItem).IdProducts,
                    IdCustomers = ((DB.Customer)customerComboBox.SelectedItem).IdCustomers,
                    Rating = (int?)ratingComboBox.SelectedItem,
                    Description = descriptionTextBox.Text,
                    Date = DateTime.Now.ToString("dd.MM.yyyy HH:mm")
                };

                _context.Comments.Add(comment);
                _context.SaveChanges();
                MessageBox.Show("Комментарий успешно добавлен");
                ClearForm();

            }
            else
            {
                MessageBox.Show("Ошибка при добавлении комментария. Проверьте корректность введенных данных");
            }

        }




        private bool IsValidInput()
        {
            bool isValid = true;
            errorProvider.Clear();

            if (string.IsNullOrWhiteSpace(customerComboBox.Text))
            {
                errorProvider.SetError(customerComboBox, "Customer is required");
                return false;
            }

            if (string.IsNullOrWhiteSpace(productComboBox.Text))
            {
                errorProvider.SetError(customerComboBox, "Product is required");
                return false;
            }

            if (string.IsNullOrWhiteSpace(descriptionTextBox.Text))
            {
                errorProvider.SetError(descriptionTextBox, "Description is required");
                return false;
            }


            return isValid;
        }



        private void ClearForm()
        {
            productComboBox.SelectedIndex = -1;
            customerComboBox.SelectedIndex = -1;
            descriptionTextBox.Text = string.Empty;
            ratingComboBox.SelectedIndex = -1;
        }
    }
}
