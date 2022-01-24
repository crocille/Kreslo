using MuagkoeKreslo.Database;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
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
    /// Interaction logic for ProductEditPage.xaml
    /// </summary>
    public partial class ProductEditPage : Page
    {
        /// <summary>
        /// Редактируемый/добавляемый товар
        /// </summary>
        Product product;

        /// <summary>
        /// Конструктор страницы добавления/редактирования товара
        /// </summary>
        /// <param name="product">Товар</param>
        public ProductEditPage(Product product)
        {
            //Записываем товар в переменную
            this.product = product;
            
            //Записываем товар в DataContext
            DataContext = product;
            InitializeComponent();
            //Получаем список материалов из бд, учитывая материалы, которые уже присутствуют в товаре
            CbMaterials.ItemsSource = EfModel.Init().Materials.ToList()
                .Where(m => !product.ProductMaterials.Select(pm => pm.Material).Contains(m));
            //Получаем список типов товаров из бд
            CbProductTypes.ItemsSource = EfModel.Init().ProductTypes.ToList();
            //Записываем список используемых материалов в DGV
            DgvMaterialsList.ItemsSource = product.ProductMaterials;
        }

        /// <summary>
        /// Метод загрузки картинки
        /// </summary>
        private void ImageChangeClick(object sender, MouseButtonEventArgs e)
        {
            //Формируем диалог открытия картинки
            OpenFileDialog openFile = new OpenFileDialog { Filter = "Jpeg files|*.jpg|All Files|*.*" };
            //Открываем его
            if(openFile.ShowDialog() == true)
            {
                //Если файл выбран - грузим его в product.Image
                product.Image = File.ReadAllBytes(openFile.FileName);
            }
        }

        /// <summary>
        /// Сохранение товара
        /// </summary>
        private void BtSaveClick(object sender, RoutedEventArgs e)
        {
           //Отлавливаем исключения
            try
            {
                //Если добавляем
                if (product.ID == 0)
                {
                    //Добавляем товар в соответствующий список
                    EfModel.Init().Products.Add(product);
                    //Если артикул существует в БД
                    if (EfModel.Init().Products.FirstOrDefault(p => p.ArticleNumber == product.ArticleNumber) != null)
                    {
                        //Выдаем ошибку
                        MessageBox.Show("Артикул уже используется другим товаром!");
                        return;
                    }
                }
                EfModel.Init().SaveChanges();
            }catch(DbEntityValidationException ex)//Если случилось исключение проверки Entity
            {
                //Добавляем ошибку в сообщение об ошибке
                MessageBox.Show(String.Join(Environment.NewLine, ex.EntityValidationErrors.Last().ValidationErrors.Select(ve => ve.ErrorMessage)));
            }
        }
        /// <summary>
        /// При нажатии на кнопку удаления
        /// </summary>
        private void BtDelClick(object sender, RoutedEventArgs e)
        {
            //Обрабатываем исключения
            try
            {
                //Если товар продавался
                if (product.ProductSales.Count > 0)
                {
                    //Удалять его нельзя - уведомляем пользователя
                    MessageBox.Show("Удаление запрещено! Продукт продавался!");
                    //Заканчиваем выполнение данного метода
                    return;
                }

                //Спрашиваем пользователя - хочет ли он удалить товар
                if (MessageBox.Show("Вы действительно хотите удалить товар " + product.Title + "?",
                    "Удалить товар?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    //Очищаем список материалов
                    product.ProductMaterials.Clear();
                    //Очищаем историю изменения цены
                    product.ProductCostHistories.Clear();
                    //Удаляем сам товар из БД
                    EfModel.Init().Products.Remove(product);
                    //Сохраняем изменения
                    EfModel.Init().SaveChanges();
                    product.ID = 0;
                    //Если есть возможность переидти назад
                    if (NavigationService.CanGoBack)
                        //Возвращаемся к списку товаров
                        NavigationService.GoBack();
                }                
            }
            //Если произошло исключение
            catch (Exception)
            {
                //Выводим соответствующую ошибку
                MessageBox.Show("Удаление невозможно!");
            }
        }

        /// <summary>
        /// Нажатие кнопки добавление материала в список
        /// </summary>
        private void btMaterialAddClick(object sender, RoutedEventArgs e)
        {
            //Задаем количество материалов
            int count;
            //Если материал выбран, количество задано и его можно привести к типу int
            if (CbMaterials.SelectedItem != null
                && TbCount.Text.Length > 0
                && Int32.TryParse(TbCount.Text, out count)
            )
            {
                //Добавляем материал в список материалов в товаре
                product.ProductMaterials.Add(
                    //Задаем материал и количество
                    new ProductMaterial { Material = (CbMaterials.SelectedItem as Material), Count = count }
                );
                //Заполняем список материалов в товаре
                DgvMaterialsList.ItemsSource = product.ProductMaterials.ToList();
                //Обновляем материалы в комбобоксе - фильтруем список по наличию материалов в товаре
                CbMaterials.ItemsSource = EfModel.Init().Materials.ToList()
                    .Where(m => !product.ProductMaterials.Select(pm => pm.Material).Contains(m));
            }
            else {
                //Если что-то пошло не так - просим пользователя проверить данные
                MessageBox.Show("Проверьте данные!");
            }
        }

        /// <summary>
        /// Кнопка удаления материала из заказа
        /// </summary>
        private void btMaterialDelClick(object sender, RoutedEventArgs e)
        {
            //Получаем кнопку
            Button button = sender as Button;
            //Получаем из кнопки материал в заказе и удалем его из товара
            product.ProductMaterials.Remove(button.DataContext as ProductMaterial);
            //Обновляем список материалов в товаре
            DgvMaterialsList.ItemsSource = product.ProductMaterials.ToList();
            //Обновляем материалы в комбобоксе - фильтруем список по наличию материалов в товаре
            CbMaterials.ItemsSource = EfModel.Init().Materials.ToList()
                .Where(m => !product.ProductMaterials.Select(pm => pm.Material).Contains(m));
        }
    }
}
