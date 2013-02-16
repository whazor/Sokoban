using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Sokoban.Domain.Things
{
    public interface IThing
    {
        bool Dynamic { get; }
        string ResourceName { get; }
        
    }
}
