using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

using IT;
using IT.WPF;

namespace Apiary.VM
{
    class VM_Global
    {
        #region WithHidden

        public static event EventHandler<EventArgs<bool>> WithHiddenChanged;

        public static MonitoredProperty<bool> WithHidden { get; private set; }

		//public static void WithHiden_Set(ExecutedRoutedEventArgs e)
		//{
		//	//WithHiden.Value = !withHiden;
		//	onWithHidenChanged(withHiden = !withHiden);
		//}
		private static void onWithHiddenChanged(bool value)
        {
            WithHiddenChanged?.Invoke(WithHidden, new EventArgs<bool>(value));
        }

        #endregion

        static VM_Global()
        {
            WithHidden = new MonitoredProperty<bool>(onWithHiddenChanged);
        }
    }
}
