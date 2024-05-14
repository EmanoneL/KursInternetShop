namespace Directory
{
    public partial class InsertBankForm : Form
    {
        private readonly DB.HandmadeShopSystemContext _context;

        protected TextBox nameTextBox;
        protected TextBox phoneTextBox;
        protected Button addButton;
        protected ErrorProvider errorProvider;

        public InsertBankForm()
        {

            _context = new();
            InitializeDynamicControls();
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        }
        protected void InitializeDynamicControls()
        {
            // Создание и настройка элементов управления
            nameTextBox = new TextBox { Location = new Point(10, 30), Width = 200 };
            phoneTextBox = new TextBox { Location = new Point(10, 80), Width = 200 };
            addButton = new Button { Text = "Add", Location = new Point(10, 130), Width = 210 };
            errorProvider = new ErrorProvider();

            // Labels
            var nameLabel = new Label { Text = "Name:", Location = new Point(10, 10) };
            var phoneLabel = new Label { Text = "Phone Number:", Location = new Point(10, 60) };

            // Добавление элементов управления и меток на форму
            Controls.Add(nameLabel);
            Controls.Add(nameTextBox);
            Controls.Add(phoneLabel);
            Controls.Add(phoneTextBox);

            Controls.Add(addButton);

            // Привязка события для кнопки добавления
            addButton.Click += addButton_Click;
        }


        protected void addButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsValidInput())
                {
                    var bank = new DB.Bank
                    {
                        Name = nameTextBox.Text,
                        PhoneNumer = phoneTextBox.Text,
                    };

                    _context.Banks.Add(bank);
                    _context.SaveChanges();
                    MessageBox.Show("Банк успешно добавлен");
                    ClearForm();
                    
                }
            }
            catch
            {
                MessageBox.Show("Ошибка при добавлении банка. Проверьте корректность введенных данных");
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
            if (string.IsNullOrWhiteSpace(phoneTextBox.Text))
            {
                errorProvider.SetError(phoneTextBox, "Phone is required");
                return false;
            }



            return isValid;
        }



        private void ClearForm()
        {
            nameTextBox.Clear();
            phoneTextBox.Clear();
        }
    }
}
