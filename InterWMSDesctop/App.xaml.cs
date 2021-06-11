using ApiApp.Providers.RequestProvider;
using ApiApp.Providers.UserProvider;
using ApiApp.Services.AuthService;
using ApiApp.Services.UserService;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using InterWMSDesctop.ViewModels;
using InterWMSDesctop.Views;
using System;
using System.Net.Http;
using System.Windows;
using ApiApp.Services.DictionaryService;
using ApiApp.Services.StorageAreaService;
using ApiApp.Services.CounterpartyService;
using ApiApp.Services.ProductPriceService;
using ApiApp.Services.ProductService;
using ApiApp.Services.ContractService;
using ApiApp.Services.ReportsService;
using InterWMSDesctop.Services.DialogService;
using MahApps.Metro.Controls.Dialogs;

namespace InterWMSDesctop
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Init();
            Start();
        }

        private void Start()
        {
            var mainVM = new MainVM(AppServices.Instance.GetService<AuthStateProvider>());
            var mainV = new MainWindow();
            mainV.DataContext = mainVM;
            var dashV = new Dashboard();
            if (mainV.ShowDialog() == true)
            {
                var dashVM = AppServices.Instance.GetService<DashboardVM>();
                dashVM.Load();
                dashV.DataContext = dashVM;
            }

            if (dashV.DataContext != null)
            {
                if (dashV.ShowDialog() == false)
                {

                }
            }
        }

        private static void Init()
        {
            var config = new ConfigurationBuilder()
            .Build();

            var nlogConfig = new NLog.Config.XmlLoggingConfiguration("nlog.config");

            NLog.LogManager.Configuration = nlogConfig;

            var services = new ServiceCollection()
            .AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                loggingBuilder.AddNLog(config);
            });
            services.AddSingleton<HttpClient>((provider) =>
            {
                var client = new HttpClient
                {
                    BaseAddress = new Uri("https://localhost:5001/")
                };
                return client;
            });
            services.AddSingleton<IRequestProvider, RequestProvider>();
            services.AddSingleton<IUserProvider, UserProvider>();
            services.AddSingleton<IAuthService>((provider) =>
            {
                var authService = new AuthService(provider.GetService<ILogger<AuthService>>(),
                                                  provider.GetService<HttpClient>());
                return authService;
            });
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<IDictionaryService, DictionaryService>();
            services.AddSingleton<IStorageAreaService, StorageAreaService>();
            services.AddSingleton<ICounterpartyService, CounterpartyService>();
            services.AddSingleton<IProductPriceService, ProductPriceService>();
            services.AddSingleton<IProductService, ProductService>();
            services.AddSingleton<IContractService, ContractService>();
            services.AddSingleton<IReportsService, ReportsService>();
            services.AddSingleton<DashboardVM>();
            services.AddSingleton<IDialogService>(provider=>
            {
                return new DialogService(DialogCoordinator.Instance,
                                         provider.GetService<IUserService>(),
                                         provider.GetService<IContractService>(),
                                         provider.GetService<ICounterpartyService>(),
                                         provider.GetService<IProductService>(),
                                         provider.GetService<IDictionaryService>(),
                                         provider.GetService<DashboardVM>());
            });
            services.AddSingleton<UserVM>();
            services.AddSingleton<StorageAreaVM>();
            services.AddSingleton<ProductsTypeVM>();
            services.AddSingleton<CounterpartyesVM>();
            services.AddSingleton<ProductPriceVM>();
            services.AddSingleton<ProductVM>();
            services.AddSingleton<ContractVM>();
            services.AddSingleton<ReportsVM>();
            services.AddSingleton<UsersVM>();
            services.AddScoped<AuthStateProvider>();
            services.AddScoped<AuthenticationStateProvider>(s => s.GetRequiredService<AuthStateProvider>());

            AppServices.SetInstance(services.BuildServiceProvider());
        }
    }
}
