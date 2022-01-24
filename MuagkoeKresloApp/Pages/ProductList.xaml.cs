using MuagkoeKreslo.Database;
using MuagkoeKreslo.Windows;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MuagkoeKreslo.Pages
{
    /// <summary>
    /// Interaction logic for ProductList.xaml
    /// </summary>
    public partial class ProductList : Page
    {
        /// <summary>
        /// Число записей на странице
        /// </summary>
        const int NumInPage = 20;
        /// <summary>
        /// Текущий номер страницы
        /// </summary>
        int PageNum = 0;

        /// <summary>
        /// Конструктор страницы списка товаров
        /// </summary>
        public ProductList()
        {
            InitializeComponent();
            //Обновляем данные
            UpdateData();
            //Формируем список типов из БД
            List<ProductType> productTypes = EfModel.Init().ProductTypes.ToList();
            //Добавляем к списку "Все типы" первым пунктом
            productTypes.Insert(0, new ProductType { Title = "Все типы" });
            //Устанавливаем источник элементов для комбобокса
            CbFilter.ItemsSource = productTypes;
            //Устанавливаем активынй элемент на нулевой
            CbFilter.SelectedIndex = 0;

            //Добавляем к комбобоксу CbSort варианты сортировки
            CbSort.Items.Add("▲ Наименование");
            CbSort.Items.Add("▼ Наименование");
            CbSort.Items.Add("▲ Номер цеха");
            CbSort.Items.Add("▼ Номер цеха");
            CbSort.Items.Add("▲ Минимальная стоимость");
            CbSort.Items.Add("▼ Минимальная стоимость");
            //Устанавливаем активным первый вариант
            CbSort.SelectedIndex = 0;
        }


        /// <summary>
        /// Метод обновления данных
        /// </summary>
        private void UpdateData() {
            //Получаем данные из БД, фильтруем их по поисковому запросу
            IEnumerable<Product> products = EfModel.Init().Products
                .Where(p=>p.Title.Contains(TbSearch.Text) || p.Description.Contains(TbSearch.Text));

            //Если выбранный фильтр больше 0 (не "Все типы")
            if(CbFilter.SelectedIndex>0)
                //Устанавливаем фильтрацию по типу товара
                products = products.Where(p => p.ProductType.ID == (CbFilter.SelectedItem as ProductType).ID);

            //Обработка сортировки
            switch (CbSort.SelectedIndex)
            {
                //Если выбран 0 пункт - сортируем по наименованию - по возрастанию
                case 0:
                    products= products.OrderBy(p => p.Title);
                    break;
                //Если выбран 0 пункт - сортируем по наименованию - по убыванию
                case 1:
                    products = products.OrderByDescending(p => p.Title);
                    break;
                //...
                case 3:
                    products = products.OrderBy(p => p.ProductionWorkshopNumber);
                    break;
                case 4:
                    products = products.OrderByDescending(p => p.ProductionWorkshopNumber);
                    break;
                case 5:
                    products = products.OrderBy(p => p.MinCostForAgent);
                    break;
                case 6:
                    products = products.OrderByDescending(p => p.MinCostForAgent);
                    break;
            }

            //Выводим список товаров постранично - пропускаем товары в начале (число товаров на странице*номер страницы)
            LVProducts.ItemsSource = products.Skip(NumInPage * PageNum).Take(NumInPage).ToList();
            //Очищаем список товаров
            LVProducts.SelectedItems.Clear();

            //Очизщаем список страниц
            StackPages.Children.Clear();
            //Считаем число страниц
            int PageCount = (products.Count() - 1 ) / NumInPage + 1;
            //Собираем кнопки с номерами страниц, добавляем их в StackPages
            for (int i=0; i<PageCount; i++)
            {
                Button button = new Button { Content = new TextBlock { Text = (i + 1).ToString() }, Tag = i };
                //Добавляем действие при клике
                button.Click += PageClick;
                //Если это текущая страница
                if(i == PageNum)
                    //Подчеркиваем ее
                    (button.Content as TextBlock).TextDecorations = TextDecorations.Underline;
                //Добавляем страницу в StackPages
                StackPages.Children.Add(button);
            }
            //Изначально устанавливаем видимость кнопок вперед/назад
            btBackPage.Visibility = Visibility.Visible;
            BtNextPage.Visibility = Visibility.Visible;

            //Если текущая страница 0 - скрываем кнопку назад
            if (PageNum == 0)
                btBackPage.Visibility = Visibility.Collapsed;
            //Если текущая страница последняя - скрываем кнопку вперед
            if (PageNum >= PageCount-1)
                BtNextPage.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Клик на кнопку с номером страницы
        /// </summary>
        /// <param name="sender">Кнопка</param>
        private void PageClick(object sender, RoutedEventArgs e)
        {
            //Получаем из отправителя кнопку, берем из нее тег, в котором ранее записан номер страницы
            //Устанавливаем номер страницы в номер текущей страницы
            PageNum = Convert.ToInt32((sender as Button).Tag);
            //Обновляем данные
            UpdateData();
        }

        /// <summary>
        /// Изменение цены выбрнанным товарам
        /// </summary>
        private void BtChangeCostClick(object sender, RoutedEventArgs e)
        {
            //Получаем список товаров из выбранного списка (cast - приведение типа)
            IEnumerable<Product> products =  LVProducts.SelectedItems.Cast<Product>();
            //Открываем окно изменения цены товара и передаем в него полученный список
            new ChangeMinCost(products).ShowDialog();
            //После закрытия окна - обновляем список
            UpdateData();
        }

        /// <summary>
        /// Клик по кнопке добавления товара
        /// </summary>
        private void btAddClick(object sender, RoutedEventArgs e)
        {
            //Переключаем на страницу добавления товара.
            NavigationService.Navigate(new ProductEditPage(new Product()));
        }

        /// <summary>
        /// Клик по кнопке изменения товара
        /// </summary>
        private void BtEditClick(object sender, RoutedEventArgs e)
        {
            //Если выбранных товаров больше 0
            if (LVProducts.SelectedItems.Count > 0)
            {
                //Получаем товар
                Product product = LVProducts.SelectedItem as Product;
                //Переходим на страницу редактирования
                NavigationService.Navigate(new ProductEditPage(product));
            }
        }

        /// <summary>
        /// При возврате на предыдщую страницу (изменении видимости страницы)
        /// </summary>
        private void ProductListVisChange(object sender, DependencyPropertyChangedEventArgs e)
        {
            //Обновляем данные
            UpdateData();
        }

        /// <summary>
        /// При изменении текста поиска
        /// </summary>
        private void SearchChange(object sender, TextChangedEventArgs e)
        {
            //Обновляем данные
            UpdateData();
        }

        /// <summary>
        /// При изменении сортировки
        /// </summary>
        private void SortChange(object sender, SelectionChangedEventArgs e)
        {
            //Обновляем данные
            UpdateData();
        }

        /// <summary>
        /// При нажатии на кнопку предыдущей страницы
        /// </summary>
        private void BtBackPageClick(object sender, RoutedEventArgs e)
        {
            //Меняем номер активной страницы
            PageNum--;
            //Обновляем данные
            UpdateData();
        }

        /// <summary>
        /// При нажатии на кнопку следующей страницы
        /// </summary>
        private void BtNextPageClick(object sender, RoutedEventArgs e)
        {
            //Меняем номер активной страницы
            PageNum++;
            //Обновляем данные
            UpdateData();
        }
    }
}
