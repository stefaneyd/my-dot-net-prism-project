using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrismDemo.Infrastructure
{
    public interface IView2
    {
        IViewModel2 ViewModel { get; set; }
    }
}
