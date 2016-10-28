using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IT;
using IT.WPF;
using Apiary.Data;

namespace Apiary.VM
{
    class VM_OperationEdit : VM_BaseContent
    {
        public IEnumerablePropertyReadOnly<M.M_Family> Familie { get; private set; }
        public IEnumerablePropertyReadOnly<IM_Operation> Operation { get; private set; }



    }
}
