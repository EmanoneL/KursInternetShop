namespace Directory
{
    public partial class EditStreetForm : InsertStreetForm
    {
        private readonly DB.Street _street;
        private readonly DB.HandmadeShopSystemContext _context;

        public EditStreetForm(DB.Street street) : base()
        {
            InitializeComponent();
            _street = street ?? throw new ArgumentNullException(nameof(street));
            _context = new();
            InitializeForm();
        }

        private void InitializeForm()
        {


            this.Text = "Edit Street";

            // Кнопки
            addButton.Text = "Save";
            addButton.Click -= addButton_Click; // Удаление обработчика события кнопки добавления
            addButton.Click += EditButton_Click; // Добавление нового обработчика события кнопки сохранения
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            // Обновление данных продукта
            _street.StreetName = nameTextBox.Text;

            _context.Update(_street);
            _context.SaveChanges();
            MessageBox.Show("Street updated successfully!");
            this.Close();
        }
    }
}
