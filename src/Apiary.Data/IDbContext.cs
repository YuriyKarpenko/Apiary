using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Apiary.Data
{
    public interface IDbContext
    {
#if EF
#else
        IDbConnection Connection { get; }
#endif
    }

    class DbContext : IDbContext
    {
#if EF
#else
        public IDbConnection Connection { get; }


        public DbContext(IDbConnection connection)
        {
            this.Connection = connection;
        }
#endif
    }
}
