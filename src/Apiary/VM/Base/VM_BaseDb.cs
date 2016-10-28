using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;

using Apiary.Data;
using Apiary.V;
using IT;
using IT.WPF;

namespace Apiary.VM
{
	class VM_BaseContent : VM_Base
	{
		protected readonly DbProvider db = Db;

		public object Content { get; private set; }


		protected virtual void Content_Set(object content)
		{
			this.Content = content;
			this.OnPropertyChanged(nameof(this.Content));
		}
	}

	class VM_BaseEdit<T> : VM_BaseContent
	{
		public CM_Property_Value Properties { get; protected set; }

		protected VM_BaseEdit() { }
		public VM_BaseEdit(T value)
		{
			this.Properties_Set(new CM_Property_Value(value));
		}


		public void Properties_Set(CM_Property_Value value)
		{
			this.Properties = value;
			this.OnPropertyChanged(nameof(this.Properties));
		}
	}

	class VM_BaseEditHierarchy<TMaster, TDetail> : VM_BaseEdit<TDetail>
	{
		/// <summary>
		/// Метка возле списка мастер-объектов
		/// </summary>
		public string Master_Caption { get; protected set; }

		/// <summary>
		/// Список мастер-объектов
		/// </summary>
		public IEnumerablePropertyReadOnly<TMaster> Master_List { get; protected set; }

		/// <summary>
		/// 
		/// </summary>
		public object Footer { get; protected set; }



		protected VM_BaseEditHierarchy(IEnumerable<TMaster> masterList, string masterCaption = "Выбор владельца : ")
		{
			this.Master_List = new IEnumerablePropertyReadOnly<TMaster>(masterList, this.OnMasterSelect);
			this.Master_Caption = masterCaption;
			this.Init();
		}
		//public VM_BaseEditHierarchy(IEnumerable<TMaster> masterList, Action<TMaster> onSelectMaster, string masterCaption = "Выбор владельца : ", CM_PropertyItem<TDetail> properties = null)
		//	: this(masterList, onSelectMaster, masterCaption)
		//{
		//	this.Properties_Set(properties);
		//}
		public VM_BaseEditHierarchy(IEnumerable<TMaster> masterList, string masterCaption = "Выбор владельца : ", TDetail value = default(TDetail))
			: this(masterList, masterCaption)
		{
			this.Properties_Set(new VM.CM_Property_Value(value));
		}


		protected virtual void OnMasterSelect(TMaster value) { }

		protected virtual void Init_Internal() { }

		protected void Footer_Set(object value)
		{
			this.Footer = value;
			this.OnPropertyChanged(nameof(this.Footer));
		}


		private void Init()
		{
			this.Debug("()");
			try
			{
				this.Init_Internal();
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

			uc.CommandBindings.Add(ApplicationCommands.Save, this.Act_Save, this.Can_Save_Internal);
		}

		protected virtual void Act_Save_Internal(ExecutedRoutedEventArgs e) { }
		protected virtual void Can_Save_Internal(CanExecuteRoutedEventArgs e) { }


		private void Act_Save(ExecutedRoutedEventArgs e)
		{
			this.Debug("()");
			try
			{
				this.Act_Save_Internal(e);
			}
			catch (Exception ex)
			{
				this.Error(ex, "()");
			}
		}

		#endregion

	}

}
