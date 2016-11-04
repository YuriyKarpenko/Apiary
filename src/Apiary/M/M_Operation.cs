using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IT;
using Apiary.Data;

namespace Apiary.M
{
	class M_Operation : M_BaseDic, IM_Operation
	{
	}


	static class IM_Operation_Extentions
	{
		public static M_Operation ToModel(this IM_Operation value)
		{
			return M_Base.BaseToModel<M_Operation>(value);
		}

		public static IEnumerable<M_Operation> ToModel(this IEnumerable<IM_Operation> value)
		{
			return value.Select(i => i.ToModel());
		}
	}
}
