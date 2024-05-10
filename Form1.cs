using ConfectioneryApp;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private readonly ConfectioneryDbContext _dbContext;

        public Form1()
        {
            InitializeComponent();
            _dbContext = new ConfectioneryDbContext();
        }

        private async void Form1_LoadAsync(object sender, EventArgs e)
        {
            this.productsTableAdapter.Fill(this.confectioneryDBBDataSet.Products);
            await RefreshDataAsync();
        }

        private async Task RefreshDataAsync()
        {
            var products = await _dbContext.Products.ToListAsync();
            productsDataGridView.DataSource = products;
        }

        private void productsBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.productsBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.confectioneryDBBDataSet);
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            await SaveProductAsync();
        }

        private async Task SaveProductAsync()
        {
            var productName = textBox1.Text;
            decimal price;
            if (!decimal.TryParse(textBox2.Text, out price))
            {
                MessageBox.Show("Введите корректную цену.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var category = comboBox1.Text;

            var newProduct = new Product
            {
                Name = productName,
                Price = price,
                Category = category
            };

            _dbContext.Products.Add(newProduct);
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                LogExceptionDetails(ex.InnerException);
                MessageBox.Show("Ошибка сохранения продукта.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            await RefreshDataAsync();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            await DeleteProductAsync();
        }

        private async Task DeleteProductAsync()
        {
            if (productsDataGridView.SelectedRows.Count > 0)
            {
                var selectedRow = productsDataGridView.SelectedRows[0];
                var productId = Convert.ToInt32(selectedRow.Cells["dataGridViewTextBoxColumn1"].Value);
                var productToDelete = await _dbContext.Products.FindAsync(productId);
                if (productToDelete != null)
                {
                    _dbContext.Products.Remove(productToDelete);
                    try
                    {
                        await _dbContext.SaveChangesAsync();
                    }
                    catch (DbUpdateException ex)
                    {
                        LogExceptionDetails(ex.InnerException);
                        MessageBox.Show("Ошибка удаления продукта.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    await RefreshDataAsync();
                }
            }
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            string productName = textBox1.Text;
            double price = Convert.ToDouble(textBox2.Text);
            string category = comboBox1.Text;

            Product newProduct = new Product
            {
                Name = productName,
                Price = (decimal)price,
                Category = category
            };

            await AddProductAsync(newProduct);

            textBox1.Text = "";
            textBox2.Text = "";
            comboBox1.SelectedIndex = -1;
        }

        private async Task AddProductAsync(Product newProduct)
        {
            _dbContext.Products.Add(newProduct);
            await _dbContext.SaveChangesAsync();
        }

        private static void LogExceptionDetails(Exception ex)
        {
            if (ex != null)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
                LogExceptionDetails(ex.InnerException);
            }
        }
    }
}
