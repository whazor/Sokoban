﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Sokoban.Domain.Things
{
    class Wall : IThing
    {
        public string ResourceName { get { return "wall"; } }
    }
}
