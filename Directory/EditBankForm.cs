namespace Directory
{
    public partial class EditBankForm : InsertBankForm
    {
        private readonly DB.Bank _bank;
        private readonly DB.HandmadeShopSystemContext _context;

        public EditBankForm(DB.Bank bank) : base()
        {
            InitializeComponent();
            _bank = bank ?? throw new ArgumentNullException(nameof(bank));
            _context = new();
            InitializeForm();
        }

        private void InitializeForm()
        {
            nameTextBox.Text = _bank.Name;
            phoneTextBox.Text = _bank.PhoneNumer;


            this.Text = "Edit Bank";

            // Кнопки
            addButton.Text = "Save";
            addButton.Click -= addButton_Click; // Удаление обработчика события кнопки добавления
            addButton.Click += EditButton_Click; // Добавление нового обработчика события кнопки сохранения
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            // Обновление данных продукта
            _bank.Name = nameTextBox.Text;
            _bank.PhoneNumer = phoneTextBox.Text;

            _context.Update(_bank);
            _context.SaveChanges();
            MessageBox.Show("Bank updated successfully!");
            this.Close();
        }
    }
}
