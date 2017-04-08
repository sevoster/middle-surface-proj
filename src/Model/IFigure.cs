﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidSurfaceNameSpace.Primitive
{
    public interface IFigure: IEnumerable<IContour>
    {
        IEnumerable<IContour> GetContours();
    }
}
