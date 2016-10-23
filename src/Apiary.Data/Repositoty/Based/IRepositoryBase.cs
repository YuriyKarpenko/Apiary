using System.Collections.Generic;

using Apiary.Data.Model;

namespace Apiary.Data.Repositoty
{
	public interface IRepositoryBase<T> where T : IEntityBase
	{
        //#if GUID
        //		T this[GUID key] { get; }
        //#else
        //		T this[long key] { get; }
        //#endif

#if GUID
        T Get(Guid id);	//	Refresh
#else
        T Get(long id); //	Refresh
#endif
        //T Create();					//	Create
		int Delete(T item);			//	Delete
		IEnumerable<T> List(bool withHidden = false);	    //	Refresh
		int Set(T item);			//	Update
		int Set(IEnumerable<T> items);	//	Update
	}

    public interface IRepositoryDic<T> : IRepositoryBase<T> where T : IEntityDic
    {
    }

}
