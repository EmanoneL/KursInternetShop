namespace Directory
{
    public partial class InsertCatsForm : Form
    {
        private readonly DB.HandmadeShopSystemContext _context;

        protected TextBox nameTextBox;
        protected TextBox descriptionTextBox;
        protected Button addButton;
        protected ErrorProvider errorProvider;

        public InsertCatsForm()
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
            descriptionTextBox = new TextBox { Location = new Point(10, 80), Width = 200 };
            addButton = new Button { Text = "Add", Location = new Point(10, 130), Width = 210 };
            errorProvider = new ErrorProvider();

            // Labels
            var nameLabel = new Label { Text = "Name:", Location = new Point(10, 10) };
            var descLabel = new Label { Text = "Description:", Location = new Point(10, 60) };

            // Добавление элементов управления и меток на форму
            Controls.Add(nameLabel);
            Controls.Add(nameTextBox);
            Controls.Add(descLabel);
            Controls.Add(descriptionTextBox);

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
                    var cat = new DB.Category
                    {
                        Name = nameTextBox.Text,
                        Description = descriptionTextBox.Text,
                    };

                    _context.Categories.Add(cat);
                    _context.SaveChanges();
                    MessageBox.Show("Категория успешно добавлена");
                    ClearForm();
                }
            }
            catch
            {
                MessageBox.Show("Ошибка при добавлении категории. Проверьте корректность введенных данных");
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



            return isValid;
        }



        private void ClearForm()
        {
            nameTextBox.Clear();
            descriptionTextBox.Clear();
        }
    }
}
