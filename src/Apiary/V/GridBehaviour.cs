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
		#region PropertyGrid_Properties

		/// <summary>
		/// Properties of value for show
		/// </summary>
#if dic
		public static readonly DependencyProperty PropertyGrid_PropertiesProperty = DependencyProperty.RegisterAttached(
			"PropertyGrid_Properties", typeof(IEnumerable<KeyValuePair<string, object>>), typeof(GridBehaviour), new PropertyMetadata(null, PropertyGrid_PropertiesChangedCallback));

		public static IEnumerable<KeyValuePair<string, object>> GetPropertyGrid_Properties(DependencyObject obj)
		{
			return (IEnumerable<KeyValuePair<string, object>>)obj.GetValue(PropertyGrid_PropertiesProperty);
		}

		public static void SetPropertyGrid_Properties(DependencyObject obj, IEnumerable<KeyValuePair<string, object>> value)
		{
			obj.SetValue(PropertyGrid_PropertiesProperty, value);
		}

		static void PropertyGrid_PropertiesChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var gr = d as Grid;
			var value = e.NewValue as IEnumerable<KeyValuePair<string, object>>;
			if(gr != null && value != null)
			{
				InitGrid(gr, value);
			}
		}
		private static void InitGrid(Grid gr, IEnumerable<KeyValuePair<string, object>> props)
		{
			InitGridBase(gr, row =>
			{
				if (props != null)
				{
					var t = typeof(KeyValuePair<string, object>);
					foreach (var pi in props)
					{
						gr.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
						gr.Insert(new TextBlock() { Text = pi.Key + " :" }, 0, row);
						gr.Insert(GenerateControl(t.GetProperty("Value")), 2, row);
						row++;
					}
				}
				return row;
			});
		}

#else
		public static readonly DependencyProperty PropertyGrid_DictionaryProperty = DependencyProperty.RegisterAttached(
			"PropertyGrid_Dictionary", typeof(IEnumerable<object>), typeof(GridBehaviour), new PropertyMetadata(null, PropertyGrid_DictionaryChangedCallback));

		public static IEnumerable<object> GetPropertyGrid_Dictionary(DependencyObject obj)
		{
			return (IEnumerable<object>)obj.GetValue(PropertyGrid_DictionaryProperty);
		}

		public static void SetPropertyGrid_Dictionary(DependencyObject obj, IEnumerable<object> value)
		{
			obj.SetValue(PropertyGrid_DictionaryProperty, value);
		}

		static void PropertyGrid_DictionaryChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var gr = d as Grid;
			var value = e.NewValue as IEnumerable<object>;
			if(gr != null && value != null)
			{
				InitGrid(gr, value);
			}
		}

#endif

		private static void InitGrid(Grid gr, IEnumerable<object> props)
		{
			InitGridBase(gr, row =>
			{
				if (props != null)
				{
					foreach (var prop in props)
					{
						gr.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
						gr.Insert(new TextBlock() { Text = prop.GetType().GetProperty("Key").GetValue(prop) + " :" }, 0, row);
						var ctrl = GenerateControl(prop.GetType().GetProperty("Value"));
						ctrl.DataContext = prop;
						gr.Insert(ctrl, 2, row);
						row++;
					}
				}
				return row;
			});
		}

		#endregion

		#region PropertyGrid_Type

		/// <summary>
		/// Type of value
		/// </summary>
		private static readonly DependencyProperty PropertyGrid_TypeProperty = DependencyProperty.RegisterAttached(
			"PropertyGrid_Type", typeof(Type), typeof(GridBehaviour), new PropertyMetadata(null, PropertyGrid_TypeChangedCallback));

		private static Type GetPropertyGrid_Type(DependencyObject obj) { return (Type)obj.GetValue(PropertyGrid_TypeProperty); }

		private static void SetPropertyGrid_Type(DependencyObject obj, object value) { obj.SetValue(PropertyGrid_TypeProperty, value); }

		private static void PropertyGrid_TypeChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var gr = d as Grid;
			if (gr != null && e.OldValue != e.NewValue)
			{
				var pp = GetProperties((Type)e.NewValue, true);
				//SetPropertyGrid_Properties(d, pp);
				InitGrid(gr, pp);
			}
		}

		private static void InitGrid(Grid gr, IEnumerable<PropertyInfo> props)
		{
			InitGridBase(gr, row =>
			{
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
				return row;
			});
		}

		#endregion

		#region PropertyGrid_Value

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

		#endregion

		#region PropertyGrid utils

		private static IEnumerable<PropertyInfo> GetProperties(Type type, bool visableOnly = true)
		{
			if (type != null)
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
			return null;
		}

		private static void ClearGrid(Grid gr)
		{
			gr.ColumnDefinitions.Clear();
			gr.RowDefinitions.Clear();
			gr.Children.Clear();
		}

		private static void InitGridBase(Grid gr, Func<int, int> generateRows)
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

				var row = generateRows(1);

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

		private static FrameworkElement GenerateControl(PropertyInfo pi)
		{
			var isEnabled = pi?.CanWrite ?? pi.GetAttributeValue<EditableAttribute, bool>(i => i.AllowEdit, true);
			FrameworkElement res = null;
			LookupBindingPropertiesAttribute lookUp = pi.DeclaringType
				.GetCustomAttributes<LookupBindingPropertiesAttribute>()
				?.FirstOrDefault(i => i.LookupMember == pi.Name);

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
				case "object":
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
