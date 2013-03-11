using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;
using Microsoft.Practices.Unity;
using ExpenseManagement.ViewModel;

namespace ExpenseManagement
{
    public class Bootstrapper
    {
        private static readonly UnityContainer _unityContianer = new UnityContainer();

        /// <summary>
        /// 
        /// </summary>
        public static UnityContainer UnityContainer
        {
            get
            {
                return _unityContianer;
            }
        }

        /// <summary>
        /// _unityContianer.RegisterInstance<interface>(CLassName);
        //_unityContianer.RegisterInstance<interface>(CLassName, new COntainerControlled);
        //_unityContianer.RegisterType<SomeInterface, XpenseManagementViewModel>(new ContainerControlledLifetimeManager());
        /// </summary>
        /// <param name="dispatcher"></param>
        public bool Run(Dispatcher dispatcher)
        {
            _unityContianer.RegisterType<XpenseManagementViewModel>(new ContainerControlledLifetimeManager());
            return true;
        }
    }
}
