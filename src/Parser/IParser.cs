using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MidSurfaceNameSpace.Primitive;

namespace MidSurfaceNameSpace.IO
{
    interface IFigureParser
    {
        IFigure ParseFile(string filePath);
    }
}
