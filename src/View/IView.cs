﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using View;

namespace MidSurface.Component
{
    public interface IView
    {
        void Paint(IVisibleData data);
    }
}
