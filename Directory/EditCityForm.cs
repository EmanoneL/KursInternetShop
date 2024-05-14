namespace Directory
{
    public partial class EditCityForm : InsertCityForm
    {
        private readonly DB.City _city;
        private readonly DB.HandmadeShopSystemContext _context;

        public EditCityForm(DB.City city) : base()
        {
            InitializeComponent();
            _city = city ?? throw new ArgumentNullException(nameof(city));
            _context = new();
            InitializeForm();
        }

        private void InitializeForm()
        {


            this.Text = "Edit City";

            // Кнопки
            addButton.Text = "Save";
            addButton.Click -= addButton_Click; // Удаление обработчика события кнопки добавления
            addButton.Click += EditButton_Click; // Добавление нового обработчика события кнопки сохранения
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            // Обновление данных продукта
            _city.CityName = nameTextBox.Text;

            _context.Update(_city);
            _context.SaveChanges();
            MessageBox.Show("City updated successfully!");
            this.Close();
        }
    }
}
