﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MidSurfaceNameSpace.Component;

namespace MidSurfaceNameSpace.Quality
{
    public interface IQuality
    {
        void Check(IModel model, IMidSurface midsurface);
    }
}
