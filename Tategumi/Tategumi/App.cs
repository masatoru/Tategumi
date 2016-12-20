using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Prism.Unity;
using Tategumi.Models;
using Tategumi.Views;
using Xamarin.Forms;

namespace Tategumi
{
  public partial class App : PrismApplication
  {
    protected override void RegisterTypes()
    {
      Container.RegisterTypeForNavigation<NavigationPage>();
      Container.RegisterTypeForNavigation<Views.HonbunPage>();
      Container.RegisterTypeForNavigation<BookListPage>();
      Container.RegisterType<IBookManager, BookManager>(new ContainerControlledLifetimeManager());
    }

    protected override void OnInitialized()
    {
      NavigationService.NavigateAsync("NavigationPage/BookListPage");
    }
  }
}
