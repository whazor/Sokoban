﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sokoban.Domain.Things
{
    class Wall : IThing
    {
        public bool Dynamic { get { return false; } }
    }
}
