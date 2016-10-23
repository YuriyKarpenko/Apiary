using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using Apiary.M;
using IT;
using IT.WPF;

namespace Apiary.VM
{
	class VM_Dictionary<T> : VM_BaseDb //where T : M_Base//, new()
	{
		private Action<T> onDelete = null;

		public List<DataGridColumn> Columns { get { return VM_CRUD.GenerateColumns<T>(); } /*private set; */}
		public IEnumerableProperty<T> List { get; private set; }


		public VM_Dictionary(IEnumerable<T> list, Action<T> onDelete)
			//: base(typeof(T).GetAttributeValueStr<DescriptionAttribute>(a => a.Description))
		{
			Contract.NotNull(list, "list");

			this.onDelete = onDelete;

			this.List = new IEnumerableProperty<T>(() => list);

			this.Content_Set(this.List);
		}



		#region actions

		protected override void Init_Command_Internal(Window w)
		{
			base.Init_Command_Internal(w);

			w.CommandBindings.Add(ApplicationCommands.Delete, this.Act_Delete_Intrnal);
		}


		protected void Act_Delete_Intrnal(ExecutedRoutedEventArgs e)
		{
			this.onDelete?.Invoke(this.List.SelectedItem);
			//base.Act_Delete_Intrnal(e);
			//this.List.SelectedItem.Hide = true;
			//this.repo.Delete(this.List.SelectedItem);
			//this.Act_Refresh(e);
		}

		//protected void Act_Create(ExecutedRoutedEventArgs e)
		//{
		//	//System.Windows.Controls.DataGrid.com
		//	var item = new T();
		//	//this.List.List.Add(item);
		//	//this.Act_Refresh(e);
		//	//this.List.SelectedItem = item;
		//	//this.Content_Set(this.List);
		//}

		//protected override void Act_Update_Intrnal(ExecutedRoutedEventArgs e)
		//{
		//	base.Act_Update_Intrnal(e);
		//	this.repo.Set(this.List.SelectedItem);
		//}

		//protected void Act_Save_Intrnal(ExecutedRoutedEventArgs e)
		//{
		//	//base.Act_Save_Intrnal(e);
		//	this.onSave?.Invoke(this.List.List);
		//	this.Act_Close(e);
		//}

		#endregion
	}
}
