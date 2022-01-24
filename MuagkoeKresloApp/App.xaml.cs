using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace MuagkoeKreslo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App() {
            //Обрабатываем исполючения
            DispatcherUnhandledException += App_DispatcherUnhandledException;
        }

        /// <summary>
        /// Обработчик необработанных исключений
        /// </summary>
        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            //Обработка исключений
            MessageBox.Show("Что-то пошло не так. Пожалуйста обратитесь к администратору!");
            e.Handled = true;
        }
    }
}
