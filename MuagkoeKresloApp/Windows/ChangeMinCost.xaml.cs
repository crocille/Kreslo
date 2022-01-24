using MuagkoeKreslo.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MuagkoeKreslo.Windows
{
    /// <summary>
    /// Interaction logic for ChangeMinCost.xaml
    /// </summary>
    public partial class ChangeMinCost : Window
    {
        /// <summary>
        /// Список товаров в виде строки
        /// </summary>
        public string ProductListStr { get; set; }
        /// <summary>
        /// Цена товаров
        /// </summary>
        public Decimal Price { get; set; }
        /// <summary>
        /// Список редактируемых товаров
        /// </summary>
        IEnumerable<Product> Products;

        /// <summary>
        /// Конструктор окна - получаем список товаров для редактирования
        /// </summary>
        public ChangeMinCost(IEnumerable<Product> products)
        {
            //Записывааем список товаров в переменную
            Products = products;
            //Собираем товары через запятую
            ProductListStr = String.Join(", ", products.Select(p => p.Title));
            //Считаем среднюю цену
            Price = products.Average(p => p.MinCostForAgent);
            //Устанавливаем в качестве контекста текуще окно
            DataContext = this;
            InitializeComponent();
        }

        private void BtSaveClick(object sender, RoutedEventArgs e)
        {
            //Если цена больше 0
            if(Price > 0)
            {
                //Перебираем товары
                foreach (Product product in Products)
                    //Устанавливаем каждому товару новую цену
                    product.MinCostForAgent = Price;
                //Сохраняем изменения
                EfModel.Init().SaveChanges();
                //Закрываем окно
                Close();
            }
        }
    }
}
