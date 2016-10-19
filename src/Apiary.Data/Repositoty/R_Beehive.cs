using System.Data.Common;

using Apiary.Data.Model;

namespace Apiary.Data.Repositoty
{
    public interface IR_Beehive : IRepositoryDic<IM_Beehive>
    {
    }

    public class R_Beehive : RepositoryBaseDic<M_Beehive, IM_Beehive>, IR_Beehive
    {

#if EF
#else
        public R_Beehive(IDbContext context) : base(context, Tables.Beehive)
        {
        }
#endif

    }
}
