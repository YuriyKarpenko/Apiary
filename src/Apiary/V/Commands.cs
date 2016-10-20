﻿using System.Windows.Input;

namespace Apiary.V
{
	public static class Commands
	{
		public static readonly RoutedUICommand Cancel = new RoutedUICommand("Отмена", "Cancel", typeof(Commands));
		public static readonly RoutedUICommand Edit = new RoutedUICommand("Изменить", "Edit", typeof(Commands));
		public static readonly RoutedUICommand Update = new RoutedUICommand("Изменить", "Set", typeof(Commands));

		public static readonly RoutedUICommand Dic_Beehive = new RoutedUICommand("Ульи", "Dic_Beehive", typeof(Commands));
		public static readonly RoutedUICommand Dic_FamilyProperty = new RoutedUICommand("хар-ки семьи", "Dic_FamilyProperty", typeof(Commands));
		public static readonly RoutedUICommand Dic_Operation = new RoutedUICommand("операции с семьей", "Dic_Operation", typeof(Commands));

		public static readonly RoutedUICommand Beehive = new RoutedUICommand("Ульи", "Beehive", typeof(Commands));
		public static readonly RoutedUICommand Family = new RoutedUICommand("Семьи", "Family", typeof(Commands));
		public static readonly RoutedUICommand FamilyProperty = new RoutedUICommand("Свойства семьи", "FamilyProperty", typeof(Commands));


		static Commands()
		{
		}
	}
}
