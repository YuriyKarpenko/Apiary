using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

using IT;

namespace Apiary.V
{
	public static class GridBehaviour
	{
		#region PropertyGrid

		/// <summary>
		/// Type of value
		/// </summary>
		public static readonly DependencyProperty PropertyGrid_TypeProperty = DependencyProperty.RegisterAttached(
			"PropertyGrid_Type", typeof(Type), typeof(GridBehaviour), new PropertyMetadata(null, PropertyGrid_TypeChangedCallback));

		public static Type GetPropertyGrid_Type(DependencyObject obj) { return (Type)obj.GetValue(PropertyGrid_TypeProperty); }

		public static void SetPropertyGrid_Type(DependencyObject obj, object value) { obj.SetValue(PropertyGrid_TypeProperty, value); }

		static void PropertyGrid_TypeChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			//SetPropertyGrid_Properties(d, null);
			var gr = d as Grid;
			if (gr != null && e.OldValue != e.NewValue)
			{
				var pp = GetProperties((Type)e.NewValue, true);
				//SetPropertyGrid_Properties(d, pp);
				InitGrid(gr, pp);
			}
		}

		///// <summary>
		///// Properties of value for show
		///// </summary>
		//public static readonly DependencyProperty PropertyGrid_PropertiesProperty = DependencyProperty.RegisterAttached(
		//	"PropertyGrid_Properties", typeof(IEnumerable<MemberInfo>), typeof(GridBehaviour), new PropertyMetadata(null, PropertyGrid_PropertiesChangedCallback));

		//public static IEnumerable<MemberInfo> GetPropertyGrid_Properties(DependencyObject obj)
		//{
		//	return (IEnumerable<MemberInfo>)obj.GetValue(PropertyGrid_PropertiesProperty);
		//}

		//public static void SetPropertyGrid_Properties(DependencyObject obj, IEnumerable<MemberInfo> value)
		//{
		//	obj.SetValue(PropertyGrid_PropertiesProperty, value);
		//}

		//static void PropertyGrid_PropertiesChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
		//{

		//}



		public static readonly DependencyProperty PropertyGrid_ValueProperty = DependencyProperty.RegisterAttached(
			"PropertyGrid_Value", typeof(object), typeof(GridBehaviour), new PropertyMetadata(null, PropertyGrid_ValueChangedCallback));

		public static object GetPropertyGrid_Value(DependencyObject obj) { return obj.GetValue(PropertyGrid_ValueProperty); }

		public static void SetPropertyGrid_Value(DependencyObject obj, object value) { obj.SetValue(PropertyGrid_ValueProperty, value); }

		static void PropertyGrid_ValueChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var gr = d as Grid;
			if (gr != null)
			{
				//gr.DataContext = e.NewValue;
				var t = e.NewValue?.GetType();
				SetPropertyGrid_Type(d, t);
				foreach (var el in gr.Children.OfType<FrameworkElement>())
					el.DataContext = e.NewValue;
			}
		}


		private static IEnumerable<PropertyInfo> GetProperties(Type type, bool visableOnly = true)
		{
			IEnumerable<PropertyInfo> res = type.GetProperties();
			if (visableOnly)
			{
				var lookups = type.GetCustomAttributes<LookupBindingPropertiesAttribute>(true);
				res = res
					.Where(i => lookups.Any(a => a.LookupMember == i.Name) ||
					(i.GetCustomAttribute<DisplayAttribute>()?.GetAutoGenerateField() ?? true) &&
					(i.GetCustomAttribute<BrowsableAttribute>()?.Browsable ?? true)
				);
			}
			return res?.OrderBy(i => i.GetCustomAttribute<DisplayAttribute>()?.GetOrder() ?? 10);
		}
		private static void ClearGrid(Grid gr)
		{
			gr.ColumnDefinitions.Clear();
			gr.RowDefinitions.Clear();
			gr.Children.Clear();
		}
		private static void InitGrid(Grid gr, IEnumerable<PropertyInfo> props)
		{
			if (gr != null)
			{
				ClearGrid(gr);

				gr.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
				gr.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(5, GridUnitType.Pixel) });
				gr.ColumnDefinitions.Add(new ColumnDefinition());

				gr.RowDefinitions.Add(new RowDefinition() { Name = "RowHeader", IsEnabled = false, Height = GridLength.Auto });
				gr.Insert(new TextBlock() { Text = "Свойство", HorizontalAlignment = HorizontalAlignment.Center, FontWeight = FontWeights.Bold }, 0, 0);
				gr.Insert(new TextBlock() { Text = "Значение", HorizontalAlignment = HorizontalAlignment.Center, FontWeight = FontWeights.Bold }, 2, 0);

				var spliter = new GridSplitter();
				gr.Insert(spliter, 1, 0);

				var row = 1;
				if (props != null)
				{
					foreach (var pi in props)
					{
						gr.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
						gr.Insert(new TextBlock() { Text = pi.GetNameFromAttributes(pi.Name) + " :" }, 0, row);
						gr.Insert(GenerateControl(pi), 2, row);
						row++;
					}
				}

				gr.RowDefinitions.Add(new RowDefinition());
				Grid.SetRowSpan(spliter, row);
			}
		}
		private static void Insert(this Grid gr, UIElement el, int col, int row)
		{
			if (gr != null && el != null)
			{
				Grid.SetColumn(el, col);
				Grid.SetRow(el, row);
				gr.Children.Add(el);
			}
		}
		private static UIElement GenerateControl(PropertyInfo pi)
		{
			var isEnabled = pi?.CanWrite ?? pi.GetAttributeValue<EditableAttribute, bool>(i => i.AllowEdit, true);
			FrameworkElement res = null;
			LookupBindingPropertiesAttribute lookUp = pi.DeclaringType
				.GetCustomAttributes<LookupBindingPropertiesAttribute>()
				?.FirstOrDefault(i => i.LookupMember == pi.Name);//"ComboBox", "Name", "Id", "BeehiveId")]

			if(lookUp != null)
			{
				res = new ComboBox()
				{
					DisplayMemberPath = lookUp.DisplayMember,
					//IsEditable = isEnabled,
					IsEnabled = isEnabled,
					IsReadOnly = !isEnabled,
					SelectedValuePath = lookUp.ValueMember,
					//IsTextSearchEnabled = false,
				};
				res.SetBinding(ComboBox.ItemsSourceProperty, new Binding(lookUp.DataSource));
				res.SetBinding(ComboBox.SelectedValueProperty, new Binding(lookUp.LookupMember));
				return res;
			}

			Type t = pi.PropertyType.FromNullable();
			switch (t.Name.ToLowerInvariant())
			{
				case "bool":
				case "boolean":
					res = new CheckBox();
					res.SetBinding(ToggleButton.IsCheckedProperty, new Binding(pi.Name) { Mode = pi.CanWrite ? BindingMode.TwoWay : BindingMode.OneWay });
					break;

				case "datetime":
					res = new DatePicker() { FirstDayOfWeek = DayOfWeek.Monday, SelectedDateFormat = DatePickerFormat.Short };
					res.SetBinding(DatePicker.SelectedDateProperty, new Binding(pi.Name) { Mode = pi.CanWrite ? BindingMode.TwoWay : BindingMode.OneWay });
					break;

				case "int":
				case "int32":
				case "int64":
				case "long":
				case "string":
					res = new TextBox();
					res.SetBinding(TextBox.TextProperty, new Binding(pi.Name) { Mode = pi.CanWrite ? BindingMode.TwoWay : BindingMode.OneWay });
					break;

				default:
					res = new TextBlock();
					res.SetBinding(TextBlock.TextProperty, new Binding(pi.Name) { Mode = BindingMode.OneWay });
					break;
			}

			if (res != null)
				res.IsEnabled = isEnabled;

			return res;
		}

		#endregion
	}
}
