using System;
using Autofac;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Sample.Web.Test.Automation.Common
{
    public static class AutofacConfig
    {
        private static IContainer _container;

        public static void Bootstrap()
        {
            var builder = new ContainerBuilder();
            builder.Register<IWebDriver>(b => new ChromeDriver(AppDomain.CurrentDomain.BaseDirectory)).SingleInstance();
            _container = builder.Build();
        }

        public static IWebDriver WebDriver()
        {
            return _container.Resolve<IWebDriver>();
        }
    }
}
