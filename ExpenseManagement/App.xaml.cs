using System;
using System.Windows;
using System.Windows.Threading;
using ExpenseManagement.ViewModel;
using Microsoft.Practices.Unity;
using XM.Logging;

namespace ExpenseManagement
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private Dispatcher dispatcher;
        private readonly ILogService logService = new FileLogService(typeof(MainWindow));

        protected override void OnStartup(StartupEventArgs e)
        {
            Application.Current.DispatcherUnhandledException += new DispatcherUnhandledExceptionEventHandler(Current_DispatcherUnhandledException);
            Initialize();
            dispatcher = this.Dispatcher;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Current_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            //Log the error in the log file
            logService.Fatal(e.Exception.Message);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Initialize()
        {
            Bootstrapper bootstrapper = new Bootstrapper();
            if (bootstrapper.Run(dispatcher))
            {
                var window = new MainWindow();
                window.DataContext = Bootstrapper.UnityContainer.Resolve<XpenseManagementViewModel>();
                window.Show();
            }
        }
    }
}
