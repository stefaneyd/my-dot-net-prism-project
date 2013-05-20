using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using PrismDemo.Infrastructure;

namespace DemoCreatingAViewMVVM.ModuleA
{
    [Module(ModuleName = "ModuleA")]
    public class ModuleAModule : IModule
    {
        IUnityContainer _container;
        IRegionManager _regionManager;

        public ModuleAModule(IUnityContainer container, IRegionManager regionManager)
        {
            _container = container;
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            //_container.RegisterType<ToolbarView>();
            _container.RegisterType<IContentAView, ContentView>();
            _container.RegisterType<IContentViewViewModel, ContentViewViewModel>();

            _regionManager.RegisterViewWithRegion(RegionNames.ToolbarRegion, typeof(ToolbarView));
            
            var vm = _container.Resolve<IContentViewViewModel>();
            _regionManager.Regions[RegionNames.ContentRegion].Add(vm.View);
        }
    }
}
