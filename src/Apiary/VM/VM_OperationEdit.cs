using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IT;
using IT.WPF;
using Apiary.Data;
using System.Windows.Controls;

namespace Apiary.VM
{
    class VM_OperationEdit : VM_BaseContent
    {
        public IEnumerablePropertyReadOnly<M.M_Family> Families { get; private set; }
        public IEnumerablePropertyReadOnly<M.M_Operation> Operations { get; private set; }


		public VM_OperationEdit()
		{
			this.Init();
		}


		private void Init()
		{
			this.Debug("()");
			try
			{
				this.Families = new IEnumerablePropertyReadOnly<M.M_Family>(Db.List_Family(VM_Global.WithHidden.Value), i => this.Refresh());
				this.Operations = new IEnumerablePropertyReadOnly<M.M_Operation>(Db.List_Operation(VM_Global.WithHidden.Value), i => this.Refresh());

			}
			catch (Exception ex)
			{
				this.Error(ex, "()");
			}
		}

		private void Refresh()
		{
			this.Debug("()");
			try
			{

			}
			catch (Exception ex)
			{
				this.Error(ex, "()");
			}
		}

		#region actions

		protected override void Init_Command_Internal(UserControl uc)
		{
			base.Init_Command_Internal(uc);
		}

		#endregion
	}
}
