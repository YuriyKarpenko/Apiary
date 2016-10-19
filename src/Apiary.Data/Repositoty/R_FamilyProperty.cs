using System.Data;
using System.Linq;

using IT;
using Apiary.Data.Model;
using System.Collections.Generic;

namespace Apiary.Data.Repositoty
{
	public interface IR_FamilyProperty : IRepositoryDic<IM_FamilyProperty> 
    {
    }

    public class R_FamilyProperty : RepositoryBaseDic<M_FamilyProperty, IM_FamilyProperty>, IR_FamilyProperty
    {

#if EF
#else

        #region cmd

        protected override IDbCommand cmdSelect => base.Cmd($@"
	SELECT * 
	FROM [{Tables.FamilyProperty}] 
	WHERE @All = 1 OR [Hide] = 0
	ORDER BY [Order]", CommandType.Text, "All");

        //       protected override IDbCommand cmdInsert => base.Cmd($@"
        //   INSERT INTO [{Tables.FamilyProperty}] (
        //	[Hide], [Name], [Order], [Type], [Unit]
        //) VALUES (
        //	?Hide, ?Name, ?Order, ?Type, ?Unit
        //)", CommandType.Text, "Hide", "Name", "Order", "Type", "Unit");

        //       protected override IDbCommand cmdUpdate => base.Cmd($@"UPDATE [{Tables.FamilyProperty}] SET
        //[Hide] = ?Hide, 
        //[Name] = ?Name, 
        //[Order] = ?Order, 
        //[Type] = ?Type, 
        //[Unit] = ?Unit
        //", CommandType.Text, "?Hide", "Name", "Order", "Type", "Unit");

        #endregion

        public R_FamilyProperty(IDbContext context) : base(context, Tables.FamilyProperty) { }

        //public M_FamilyProperty[] Array => base.List(false).OrderBy(i => i.Order).ToArray();

#endif

    }
}
