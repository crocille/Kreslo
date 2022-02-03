using MuagkoeKresloApp.Properties;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading;
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
            SetCulture(Settings.Default.Langeage);
            //Обрабатываем исполючения
            DispatcherUnhandledException += App_DispatcherUnhandledException;
        }

        public void SetCulture(string name) {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(name);
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(name);

            if (Settings.Default.Langeage != name)
            {
                Settings.Default.Langeage = name;
                Settings.Default.Save();
                Process.Start(Application.ResourceAssembly.Location);
                Current.Shutdown();
            }
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
