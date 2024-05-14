namespace Directory
{
    public partial class EditCatForm : InsertCatsForm
    {
        private readonly DB.Category _cat;
        private readonly DB.HandmadeShopSystemContext _context;

        public EditCatForm(DB.Category cat) : base()
        {
            InitializeComponent();
            _cat = cat ?? throw new ArgumentNullException(nameof(cat));
            _context = new();
            InitializeForm();
        }

        private void InitializeForm()
        {
            nameTextBox.Text = _cat.Name;
            descriptionTextBox.Text = _cat.Description;

            this.Text = "Edit Category";

            // Кнопки
            addButton.Text = "Save";
            addButton.Click -= addButton_Click; // Удаление обработчика события кнопки добавления
            addButton.Click += EditButton_Click; // Добавление нового обработчика события кнопки сохранения
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            // Обновление данных продукта
            _cat.Name = nameTextBox.Text;
            _cat.Description = descriptionTextBox.Text;

            _context.Update(_cat);
            _context.SaveChanges();
            MessageBox.Show("Category updated successfully!");
            this.Close();
        }
    }
}
