using OfficeOpenXml;

namespace Documents
{
    public partial class Income : Form
    {
        private readonly DB.HandmadeShopSystemContext _context;
        public Income()
        {
            _context = new DB.HandmadeShopSystemContext();
            InitializeComponent();
            LoadComboBox();
            LoadDataGrid();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            this.Name = "Выручка";
        }

        void LoadComboBox()
        {
            var sellers = _context.Sellers.ToList();
            SellercomboBox.DataSource = sellers;
            SellercomboBox.DisplayMember = "name"; // Замените на имя свойства в вашем классе Seller
            SellercomboBox.DropDownStyle = ComboBoxStyle.DropDownList;

        }
        void LoadDataGrid()
        {
            var seller = (DB.Seller)SellercomboBox.SelectedItem;
            var products = _context.Products
                .Where(p => p.IdSellers == seller.IdSellers && p.Status == "sold")
                .Select(p => new
                {
                    p.Name,
                    p.Cost,
                    CategoryName = p.IdCategoryNavigation.Name,
                    Storage = $"{p.IdStoragesNavigation.IdAddressNavigation.CityNavigation.CityName}, " +
                    $"{p.IdStoragesNavigation.IdAddressNavigation.StreetNavigation.StreetName}, " +
                    $"{p.IdStoragesNavigation.IdAddressNavigation.HomeNumber}"
                })
            .ToList();

            dataGridView1.DataSource = products;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                using (ExcelPackage package = new ExcelPackage())
                {
                    // Добавляем лист в Excel файл
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sheet1");

                    for (int col = 1; col <= dataGridView1.Columns.Count; col++)
                    {
                        worksheet.Cells[1, col].Value = dataGridView1.Columns[col - 1].HeaderText;
                    }

                    for (int row = 0; row < dataGridView1.Rows.Count; row++)
                    {
                        for (int col = 0; col < dataGridView1.Columns.Count; col++)
                        {
                            worksheet.Cells[row + 2, col + 1].Value = dataGridView1.Rows[row].Cells[col].Value;
                        }
                    }

                    // Сохраняем Excel файл на диск
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        FileInfo excelFile = new FileInfo(saveFileDialog.FileName);
                        package.SaveAs(excelFile);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при экспорте таблицы: " + ex.Message);
            }
        }

        private void SellercomboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDataGrid();
        }

        public static void ShowForm(DB.Right right)
        {
            Income income = new Income();
            income.Show();
        }
    }
}
