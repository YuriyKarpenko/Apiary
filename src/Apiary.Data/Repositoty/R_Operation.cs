using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Apiary.Data.Model;

namespace Apiary.Data.Repositoty
{
	public interface IR_Operation : IRepositoryDic<IM_Operation>
    {
    }

	class R_Operation : RepositoryBaseDic<M_Operation, IM_Operation>, IR_Operation
	{
        public R_Operation(IDbContext context) : base(context, Tables.Operation)
        {

        }
	}
}
