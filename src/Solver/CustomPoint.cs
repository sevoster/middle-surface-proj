﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MidSurfaceNameSpace.Primitive;
using System.Windows;

namespace MidSurfaceNameSpace.Solver
{
    public class CustomPoint: ICustomPoint
    {
        int N;
        double t;
        Point point;

        public CustomPoint(int N, double t, Point point)
        {
            this.N = N;
            this.t = t;
            this.point = point;
        }

        public int GetN()
        {
            return N;
        }

        public double GetT()
        {
            return t;
        }

        public Point GetPoint()
        {
            return point;
        }

        public static bool ClosePoints(Point a, Point b, double accuracy)
        {
            if ((a - b).Length <= accuracy)
                return true;
            return false;
        }
    }
}
