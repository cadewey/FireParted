using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FireParted
{
    class PartitionException : Exception
    {
        public PartitionException(string Error) 
            : base(Error)
        {
        }
    }
}
