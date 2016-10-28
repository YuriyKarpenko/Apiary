using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IT;
using IT.WPF;

namespace Apiary.VM
{
    class VM_Global
    {
        #region WithHiden

        public static event EventHandler<EventArgs<bool>> WithHidenChanged;

        public static MonitoredProperty<bool> WithHiden { get; private set; }

        private static void onWithHidenChanged(bool value)
        {
            WithHidenChanged?.Invoke(WithHiden, new EventArgs<bool>(value));
        }

        #endregion

        static VM_Global()
        {
            WithHiden = new MonitoredProperty<bool>(onWithHidenChanged);
        }
    }
}
