using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using PrismDemo.Infrastructure;

namespace DemoCreatingAViewMVVM_ViewFirst.ModelA
{
    public class MuduleAModule : IModule
    {
        IUnityContainer _container;
        IRegionManager _regionManager;

        public MuduleAModule(IUnityContainer container, IRegionManager regionManager)
        {
            _container = container;
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            _container.RegisterType<ToolbarView>();
            _container.RegisterType<ContentView>();
            _container.RegisterType<IContentAViewViewModel, ContentAViewViewModel>();

            _regionManager.RegisterViewWithRegion(RegionNames.ToolbarRegion, typeof(ToolbarView));

            var vm = _container.Resolve<IView2>();
            _regionManager.Regions[RegionNames.ContentRegion].Add(vm);
        }
    }
}
