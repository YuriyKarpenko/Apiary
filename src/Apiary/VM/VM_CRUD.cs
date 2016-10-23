using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Reflection;
using System.Windows.Input;

using Apiary.V;
using IT;
using IT.WPF;

namespace Apiary.VM
{
	abstract class VM_CRUD : VM_Base, ICRUD
	{
		public static List<DataGridColumn> GenerateColumns<T>()
		{
			//var pds = TypeDescriptor.GetProperties(typeof(T));
			var pds = typeof(T).GetProperties(true)
				.OrderBy(i => i.GetCustomAttribute<DisplayAttribute>(true)?.GetOrder() ?? 10)
				.ToList();
			if (pds == null || pds.Count == 0)
				return null;

			var res = new List<DataGridColumn>();
			foreach (var pd in pds)
			{
				if (pd.GetAttributeValue<BrowsableAttribute, bool>(a => a.Browsable, true))
				{
					DataGridColumn col = null;
					var binding = new System.Windows.Data.Binding(pd.Name);

					var enumType = pd.GetAttributeValue<EnumDataTypeAttribute, Type>(a => a.EnumType, null);
					if (enumType != null)
					{
						var cbCol = new DataGridComboBoxColumn();
						var intType = Enum.GetUnderlyingType(enumType);
						cbCol.ItemsSource = Enum.GetValues(enumType)
							.Cast<Enum>()
							.ToDictionary(i => Convert.ChangeType(i, intType), i => i.ToString());
						cbCol.SelectedValueBinding = binding;
						cbCol.SelectedValuePath = "Key";
						cbCol.DisplayMemberPath = "Value";

						col = cbCol;
					}

					if (col == null && pd.PropertyType == typeof(bool))
						col = new DataGridCheckBoxColumn() { Binding = binding };

					//if (col == null && pd.PropertyType == typeof(DateTime))
					//	col = new DataGrid() { Binding = binding };

					if (col == null)
						col = new DataGridTextColumn() { Binding = binding };

					col.IsReadOnly = !pd.GetAttributeValue<EditableAttribute, bool>(a => a.AllowEdit, true);
					col.Header = pd.GetNameFromAttributes(pd.Name);

					res.Add(col);
				}
			}

			res.Sort((c1, c2) => c1.DisplayIndex - c2.DisplayIndex);
			for (int i = 0; i < res.Count; i++)
				res[i].DisplayIndex = i;
			return res;
		}

		public string Caption { get; private set; }
		public object Content { get; protected set; }

		public VM_CRUD(string title, VM_Base content = null)
		{
			this.Caption = title;
			this.Content = content;
		}

		#region actions

		protected override void Init_Command_Internal(Window w)
		{
			base.Init_Command_Internal(w);

			w.CommandBindings.Add(ApplicationCommands.New, this.Act_Create);
			w.CommandBindings.Add(NavigationCommands.Refresh, this.Act_Refresh);
			w.CommandBindings.Add(Commands.Edit, this.Act_Update);
			w.CommandBindings.Add(ApplicationCommands.Delete, this.Act_Delete);
			w.CommandBindings.Add(ApplicationCommands.Save, this.Act_Save);
		}

		protected virtual void Act_Create_Intrnal(ExecutedRoutedEventArgs e) { }
		public void Act_Create(ExecutedRoutedEventArgs e)
		{
			this.Debug("()");
			try
			{
				this.Act_Create_Intrnal(e);
			}
			catch (Exception ex)
			{
				this.Error(ex, "()");
			}
		}

		protected virtual void Act_Refresh_Intrnal(ExecutedRoutedEventArgs e) { }
		public void Act_Refresh(ExecutedRoutedEventArgs e)
		{
			this.Debug("()");
			try
			{
				this.Act_Refresh_Intrnal(e);
			}
			catch (Exception ex)
			{
				this.Error(ex, "()");
			}
		}

		protected virtual void Act_Update_Intrnal(ExecutedRoutedEventArgs e) { }
		public void Act_Update(ExecutedRoutedEventArgs e)
		{
			this.Debug("()");
			try
			{
				this.Act_Update_Intrnal(e);
			}
			catch (Exception ex)
			{
				this.Error(ex, "()");
			}
		}

		protected virtual void Act_Delete_Intrnal(ExecutedRoutedEventArgs e) { }
		public void Act_Delete(ExecutedRoutedEventArgs e)
		{
			this.Debug("()");
			try
			{
				this.Act_Delete_Intrnal(e);
			}
			catch (Exception ex)
			{
				this.Error(ex, "()");
				throw;
			}
		}

		protected virtual void Act_Save_Intrnal(ExecutedRoutedEventArgs e) { }
		public void Act_Save(ExecutedRoutedEventArgs e)
		{
			this.Debug("()");
			try
			{
				this.Act_Save_Intrnal(e);
			}
			catch (Exception ex)
			{
				this.Error(ex, "()");
			}
		}

		#endregion

		protected void Content_Set(object value)
		{
			this.Debug("({0})", value);
			try
			{
				this.Content = value;
				this.OnPropertyChanged("Content");
			}
			catch (Exception ex)
			{
				this.Error(ex, "({0})", value);
			}
		}
	}
}
