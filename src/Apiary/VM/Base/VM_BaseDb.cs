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
	class VM_BaseDb : VM_Base
	{
		protected readonly DB db = DB.Instance;

		public string Caption { get; private set; }
		public object Content { get; private set; }


		public VM_BaseDb(string title = null)
		{
			this.Caption_Set(title // ?? Ap.AppCaption
				);
		}


		protected virtual void Caption_Set(string caption)
		{
			this.Caption = caption;
			this.OnPropertyChanged(nameof(this.Caption));
		}
		protected virtual void Content_Set(object content)
		{
			this.Content = content;
			this.OnPropertyChanged(nameof(this.Content));
		}

	}

	class VM_BaseEdit<TMaster, TDetail> : VM_BaseDb 
	{
		/// <summary>
		/// Метка возле списка мастер-объектов
		/// </summary>
		public string Master_Caption { get; protected set; }

		/// <summary>
		/// Список мастер-объектов
		/// </summary>
		public IEnumerablePropertyReadOnly<TMaster> Master_List { get; protected set; }


		public CM_PropertyItem<TDetail> Properties { get; protected set; }


		public VM_BaseEdit(IEnumerable<TMaster> masterList, Action<TMaster> onSelectMaster, string masterCaption = "Выбор объекта : ", CM_PropertyItem<TDetail> properties = null)
		{
			this.Master_List = new IEnumerablePropertyReadOnly<TMaster>(masterList, onSelectMaster);
			this.Properties_Set(properties);
		}


		public void Properties_Set(CM_PropertyItem<TDetail> value)
		{
			this.Properties = value;
			this.OnPropertyChanged(nameof(this.Properties));
			this.Caption_Set(this.Caption ?? value.GetType().GetNameFromAttributes());
		}
	}

	class VM_BaseDb<T> : VM_BaseDb
	{
		protected T self;

		/// <summary>
		/// Метка возле списка
		/// </summary>
		public string SelfCaption { get; protected set; }

		/// <summary>
		/// Список себе подобных
		/// </summary>
		public IEnumerableProperty<T> SelfList { get; protected set; }

		/// <summary>
		/// Свойства выбранного элемента
		/// </summary>
		public object SelfProperty { get; protected set; }

		/// <summary>
		/// 
		/// </summary>
		public object SelfFooter { get; protected set; }


		public VM_BaseDb(string selfListCaption = "Выбор объекта : ")
		{
			this.SelfCaption = selfListCaption;
			this.Init();
		}


		public virtual void Set_Self(T value)
		{
			this.self = value;
		}


		protected void Set_SelfFooter(object value)
		{
			this.SelfFooter = value;
			this.OnPropertyChanged("SelfFooter");
		}

		protected void Set_SelfProperty(object value)
		{
			this.SelfProperty = value;
			this.OnPropertyChanged("SelfProperty");
		}

		protected virtual void Init_Internal()
		{
			//var p = new Grid();
			//p.ColumnDefinitions.Add(new ColumnDefinition());
			//p.ColumnDefinitions.Add(new ColumnDefinition());

			//var app = Application.Current;
			//var b = new Button() { Style = (Style)app.FindResource("cmd_h"), Command = Commands.Edit, Content = app.FindResource("img_Edit") };
			//p.Children.Add(b);
			//b = new Button() { Style = (Style)app.FindResource("cmd_h"), Command = ApplicationCommands.Save, Content = app.FindResource("img_Save") };
			//p.Children.Add(b);
			//Grid.SetColumn(b, 1);

			this.Set_SelfFooter(new[] { Commands.Edit, ApplicationCommands.Save });
		}

		#region actions

		protected override void Init_Command_Internal(UserControl uc)
		{
			base.Init_Command_Internal(uc);

			//uc.CommandBindings.Add(Commands.Edit, this.Act_Edit);
			uc.CommandBindings.Add(ApplicationCommands.Save, this.Act_Save);
		}

		//private void Act_Edit(ExecutedRoutedEventArgs e)
		//{
		//	this.Debug("()");
		//	try
		//	{

		//	}
		//	catch (Exception ex)
		//	{
		//		this.Error(ex, "()");
		//	}
		//}

		private void Act_Save(ExecutedRoutedEventArgs e)
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

		#endregion

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
	}

}
