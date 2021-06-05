using Microsoft.Extensions.DependencyInjection;
using System;

namespace InterWMSDesctop
{
    class AppServices
    {
        #region Singolton
        private static ServiceProvider _instance;
        public static ServiceProvider Instance
        {
            get { return _instance; }
        }
        public static void SetInstance(ServiceProvider instance)
        {
            if (_instance != null)
                throw new ArgumentException("Instance already set");
            _instance = instance;
        }
        #endregion
    }
}
