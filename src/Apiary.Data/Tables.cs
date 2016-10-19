
namespace Apiary.Data
{
	public enum Tables : int
	{
		FamilyProperty,			//	хар-ки семьи (справочник)
		Operation,				//	операции с ссемьей (справочник)
		Beehive,				//	улей

		Family,					//	семья
		FamilyInfoProperty,		//	состояния семьи 

		OperationProperty,      //  Параметры операции
		FamilyOperation,		//	операции с ссемьей (журнал)

	}

	//	for menu
	//public enum Dictionary : int
	//{
	//	FamilyProperty,		//	хар-ки семьи (справочник)
	//	FamilyOperation,	//	операции с ссемьей (справочник)
	//}
}
