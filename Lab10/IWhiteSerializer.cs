using Lab9.White;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab10
{
    public interface IWhiteSerializer
    {
        void Serialize(Lab9.White.White obj);
        Lab9.White.White Deserialize();
    }
}
