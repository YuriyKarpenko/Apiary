using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using Apiary.Data;
using IT;
using IT.WPF;

namespace Apiary.VM
{
	class VM_Dictionary<T> : VM_BaseContent where T : IEntityBase//, new()
	{
		public static List<DataGridColumn> GenerateColumns(Type t)
		{
			//var pds = TypeDescriptor.GetProperties(typeof(T));
			var pds = t.GetProperties(true)
				.OrderBy(i => i.GetCustomAttribute<DisplayAttribute>(true)?.GetOrder() ?? 10)
				.ToList();
			if (pds == null || pds.Count == 0)
				return null;

			var lookUps = t.GetCustomAttributes<LookupBindingPropertiesAttribute>();

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
						var intType = Enum.GetUnderlyingType(enumType);
						col = new DataGridComboBoxColumn()
						{
							ItemsSource = Enum.GetValues(enumType)
								.Cast<Enum>()
								.ToDictionary(i => Convert.ChangeType(i, intType), i => i.ToString()),
							SelectedValueBinding = binding,
							SelectedValuePath = "Key",
							DisplayMemberPath = "Value",
						};
					}

					LookupBindingPropertiesAttribute lookUp = lookUps?.FirstOrDefault(i => i.LookupMember == pd.Name);
					if (col == null && lookUp != null)
					{
						col = new DataGridComboBoxColumn()
						{
							DisplayMemberPath = lookUp.DisplayMember,
							//IsEditable = isEnabled,
							//IsEnabled = isEnabled,
							//IsReadOnly = !isEnabled,
							//ItemsSource = (IEnumerable)new System.Windows.Data.Binding(lookUp.DataSource).ProvideValue(),
							SelectedValueBinding = binding,
							SelectedValuePath = lookUp.ValueMember,
						};
						//(col as DataGridComboBoxColumn).Binding(ComboBox.ItemsSourceProperty, new System.Windows.Data.Binding(lookUp.DataSource).ProvideValue);
						return res;
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


		private Action<T> onDelete = null;

		public List<DataGridColumn> Columns { get { return GenerateColumns(typeof(T)); } /*private set; */}
		public IEnumerableProperty<T> List { get; private set; }


		public VM_Dictionary(IEnumerable<T> list, Action<T> onDelete)
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
