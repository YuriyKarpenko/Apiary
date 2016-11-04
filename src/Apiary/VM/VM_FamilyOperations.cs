using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Threading.Tasks;

using IT;
using IT.WPF;
using Apiary.Data;
using Apiary.M;
using System.Windows.Controls;

namespace Apiary.VM
{
    class VM_FamilyOperations : VM_BaseContent
    {
		private static DateTime DateFrom(double value)
		{
			var t = (value * 1000 * 60 * 60 * 24);
			return new DateTime((long)t);
		}
		private static double DateTo(DateTime value)
		{
			return value.Ticks / (1000 * 60 * 60 * 24);
		}

		private List<M.M_Family> _Families;
		private List<M.M_Operation> _Operations;


        public SelectorProperty<M.M_Family> Families { get; private set; }

        public SelectorProperty<M.M_Operation> Operations { get; private set; }

        public SelectorProperty<IM_FamilyOperation> List { get; private set; }

		public MonitoredProperty<DateTime?> Date { get; private set; }
		public SliderProperty<DateTime> DateSlider { get; private set; }

        public IM_FamilyOperation Value { get; private set; }


		public VM_FamilyOperations()
		{
			this.Init();
		}


		private void Init()
		{
			this.Debug("()");
			try
			{
				this._Families = Db.List_Family(VM_Global.WithHidden.Value).ToList();
				this._Operations = Db.List_Operation(VM_Global.WithHidden.Value).ToList();
				//	controls
				this.Families = new SelectorProperty<M.M_Family>(() => this._Families, i => this.Refresh());
				this.Operations = new SelectorProperty<M.M_Operation>(() => this._Operations, i => this.Refresh());
				this.Date = new MonitoredProperty<DateTime?>(d => this.Refresh());
				this.DateSlider = new VM.SliderProperty<DateTime>(DateTo, DateFrom);
				this.List = new SelectorProperty<IM_FamilyOperation>(this.Refresh, this.FamilyOperationSelect);
				//	init first


				this.Content_Set(this.List);
			}
			catch (Exception ex)
			{
				this.Error(ex, "()");
			}
		}

		private IM_FamilyOperation[] Refresh()
		{
			this.Debug("()");
			try
			{
				var list = Db.List_FamilyOperations(true, this.Families.SelectedItem, this.Operations.SelectedItem, this.Date.Value);
				this.List.Reset(list);
				//	filtering controls
				this.Families.Reset(this._Families.Join(list, i => i.Id, i => i.FamilyId, (f, o) => f).ToArray());
				this.Operations.Reset(this._Operations.Join(list, i => i.Id, i => i.OperationId, (f, o) => f).ToArray());

				return list.ToArray();
			}
			catch (Exception ex)
			{
				this.Error(ex, "()");
			}

			return null;
		}

		private void FamilyOperationSelect(IM_FamilyOperation value)
		{

		}

		#region actions

		protected override void Init_Command_Internal(UserControl uc)
		{
			base.Init_Command_Internal(uc);
		}

		private void Act_Delete(ExecutedRoutedEventArgs e)
		{
			try
			{
				var editItem = this.List.SelectedItem;
				if (MsgDlg($"Семья: {this.Families.SelectedItem}\n Операция: {this.Operations.SelectedItem}\n удалить операцию ?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
				{
					editItem.Hide = true;
					//this.db.Set_fa(editItem);

					this.Act_Refresh(e);
				}
			}
			catch (Exception ex)
			{
				this.Error(ex, "()");
			}
		}

		private void Act_Edit(ExecutedRoutedEventArgs e)
		{
			try
			{
				var editItem = this.Families.SelectedItem;
				var vm = new CM_Property_Value(editItem);
				//var vm = new CM_PropertyFamilyInfo(this.Families.SelectedItem);
				if (VM_Dialog.Show<V.UC_EditItem>("Редактирование семьи", new { Value = editItem }, null))
					this.db.Set_Family(editItem);

				this.Act_Refresh(e);
			}
			catch (Exception ex)
			{
				this.Error(ex, "()");
			}
		}

		private void Act_New(ExecutedRoutedEventArgs e)
		{
			try
			{
				var newItem = new M_FamilyOperation();
				newItem.FamilyId = this.Families.SelectedItem.Id;
				newItem.OperationId = this.Operations.SelectedItem.Id;
				newItem.DateWrite = DateTime.Now.Date;
				var vm = new CM_Property_Value(newItem);
				if (VM_Dialog.Show<V.UC_EditItem>("Редактирование семьи", vm, null))
					this.db.Set_FamilyOperations(newItem);

				this.Act_Refresh(e);
			}
			catch (Exception ex)
			{
				this.Error(ex, "()");
			}
		}

		private void Act_Refresh(ExecutedRoutedEventArgs e)
		{
			try
			{
				this.Refresh();
			}
			catch (Exception ex)
			{
				this.Error(ex, "()");
			}
		}

		#endregion
	}
}
