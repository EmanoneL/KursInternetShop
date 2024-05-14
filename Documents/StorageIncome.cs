using DB;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Documents
{
    public partial class StorageIncome : Form
    {
        private readonly DB.HandmadeShopSystemContext _context;
        public StorageIncome()
        {
            _context = new DB.HandmadeShopSystemContext();
            InitializeComponent();
            LoadComboBox();
            LoadDataGrid();
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            this.Name = "Поступления на склад";
        }

        void LoadComboBox()
        {
            var storages = _context.Storages
                          .Include(s => s.IdAddressNavigation)
                              .ThenInclude(a => a.CityNavigation)
                          .Include(s => s.IdAddressNavigation)
                              .ThenInclude(a => a.StreetNavigation)
                          .ToList();

            var storageDisplayItems = storages.Select(s => new StorageDisplayItem
            {
                Id = s.IdStorages,
                AddressString = s.IdAddressNavigation != null && s.IdAddressNavigation.CityNavigation != null && s.IdAddressNavigation.StreetNavigation != null
                    ? $"{s.IdAddressNavigation.CityNavigation.CityName}, {s.IdAddressNavigation.StreetNavigation.StreetName}, {s.IdAddressNavigation.HomeNumber}"
                    : "Unknown Address"
            }).ToList();

            StorageComboBox.DisplayMember = "AddressString";
            StorageComboBox.ValueMember = "Id";
            StorageComboBox.DataSource = storageDisplayItems;
            StorageComboBox.DropDownStyle = ComboBoxStyle.DropDownList;

        }
        void LoadDataGrid()
        {
            var storage = (DB.StorageDisplayItem)StorageComboBox.SelectedItem;
            var products = _context.Products
                .Where(p => p.IdStorages == storage.Id && p.Status == "in stock")
                .Select(p => new
                {
                    p.Name,
                    p.Cost,
                    CategoryName = p.IdCategoryNavigation.Name,
                    Seller = p.IdSellersNavigation.Name
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
            StorageIncome income = new();
            income.Show();
        }
    }
}
