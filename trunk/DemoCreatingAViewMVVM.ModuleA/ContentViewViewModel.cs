using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoCreatingAViewMVVM.ModuleA
{
    public class ContentViewViewModel : IContentViewViewModel
    {
        public PrismDemo.Infrastructure.IView View { get; set; }

        public ContentViewViewModel(IContentAView view)
        {
            View = view;
            View.ViewModel = this;
        }
    }
}
