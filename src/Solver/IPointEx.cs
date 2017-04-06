﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MidSurface.Primitive;

namespace Solver
{
    public interface IPointEx
    {
        List<ICustomPoint> GetParents();
        IPointF GetMSPoint();
        double GetDistance(IPointEx p);
    }
}