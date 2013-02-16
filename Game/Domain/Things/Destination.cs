using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Sokoban.Domain.Things
{
    class Destination : IThing
    {
        public bool Dynamic { get { return false; } }
        public string ResourceName { get { return "destination"; } }
    }
}
