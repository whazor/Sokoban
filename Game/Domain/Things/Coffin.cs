using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Sokoban.Domain.Things
{
    class Coffin : IThing
    {
        public bool Dynamic { get { return true; } }
        public string ResourceName
        {
            get
            {
                return OnDestination ? "coffin_ok" : "coffin";
            }
        }

        public bool OnDestination { get; set; }
    }
}
