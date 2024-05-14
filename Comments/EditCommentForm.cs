using Microsoft.EntityFrameworkCore;

namespace Comments
{
    public partial class EditCommentForm : InsertCommentForm
    {
        private readonly DB.Comment _comment;
        private readonly DB.HandmadeShopSystemContext _context;

        public EditCommentForm(DB.Comment comment) : base()
        {
            InitializeComponent();
            base.LoadComboBoxes();
            _comment = comment ?? throw new ArgumentNullException(nameof(comment));
            _context = new();
            InitializeForm();
        }

        private void InitializeForm()
        {
            var products = _context.Products.ToList();
            productComboBox.DataSource = products;

            var customers = _context.Customers.Include(u => u.Carts).ToList();
            customerComboBox.DataSource = customers;

            // Загрузка данных выбранного продукта на форму

            descriptionTextBox.Text = _comment.Description;


            ratingComboBox.SelectedItem = (_comment.Rating);
            productComboBox.SelectedItem = products.FirstOrDefault(c => c.IdProducts == _comment.IdProduct);
            customerComboBox.SelectedItem = customers.FirstOrDefault(c => c.IdCustomers == _comment.IdCustomers);


            this.Text = "Edit Comment";

            // Кнопки
            addButton.Text = "Save";
            addButton.Click -= addButton_Click; // Удаление обработчика события кнопки добавления
            addButton.Click += EditButton_Click; // Добавление нового обработчика события кнопки сохранения
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            // Обновление данных 
            _comment.Description = descriptionTextBox.Text;
            _comment.IdCustomers = ((DB.Customer)customerComboBox.SelectedItem).IdCustomers;
            _comment.IdProduct = ((DB.Product)productComboBox.SelectedItem).IdProducts;
            _comment.Rating = (int?)ratingComboBox.SelectedItem;

            _context.Update(_comment);
            _context.SaveChanges();
            MessageBox.Show("Comment updated successfully!");
            this.Close();
        }

    }
}
