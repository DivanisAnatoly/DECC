using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DECC.Views.Inputs
{
    public interface ICustomInput
    {
        public bool Required { get; }
        public string Value { get;}
    }
}
