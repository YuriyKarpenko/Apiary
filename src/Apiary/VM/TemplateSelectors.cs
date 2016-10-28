using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Apiary.VM
{
	class ContentTemplateSelector : DataTemplateSelector
	{
		public override DataTemplate SelectTemplate(object item, DependencyObject container)
		{
			var el = container as FrameworkElement;
			if(el != null && item != null)
			{
				var gt = typeof(IT.WPF.IEnumerableProperty<>);
				var it = item.GetType();
				while(it != null)
				{
					if (gt.Name == it.Name)
						return el.FindResource("dt_IEnumerableProperty") as DataTemplate;
					it = it.BaseType;
				}
			}
			return base.SelectTemplate(item, container);
		}
	}
}
