--CREATE  TABLE  IF NOT EXISTS [main].[__MigrationHistory] (
--	[MigrationId]	VARCHAR(255) NOT NULL, 
--	[ContextKey]	VARCHAR(255) NOT NULL, 
--	[Model]			VARCHAR(255), 
--	[ProductVersion]	VARCHAR(255)
--	)
--GO

--	Параметры/свойства семьи
CREATE TABLE IF NOT EXISTS [main].[FamilyProperty] (
	[Id]		INTEGER PRIMARY KEY  AUTOINCREMENT  NOT NULL, 
	[Name]		VARCHAR(50) NOT NULL,		--	Название параметра (тип.)

	[Type]		INTEGER NOT NULL,			--	Тип данных (для валидации)
	--[Unit]		VARCHAR(50) NOT NULL,		--	Единицы
	[Order]		INTEGER NOT NULL DEFAULT 0, --	Порядок (для отчетов)

	[Created]	DATETIME NOT NULL	DEFAULT CURRENT_TIMESTAMP, 
	[Modified]	DATETIME NOT NULL	DEFAULT CURRENT_TIMESTAMP, 
	[Hide]		BOOL NOT NULL DEFAULT 0
);
GO

--IF !EXISTS(SELECT * FROM [main].[FamilyProperty]) BEGIN
INSERT INTO [main].[FamilyProperty] ([Name], [Type], /*[Unit],*/ [Order]) VALUES ('Яйцо (р.)', 2, /*'рамок',*/ 0);
INSERT INTO [main].[FamilyProperty] ([Name], [Type], /*[Unit],*/ [Order]) VALUES ('Расплод (р.)', 2, /*'рамок',*/ 0);
INSERT INTO [main].[FamilyProperty] ([Name], [Type], /*[Unit],*/ [Order]) VALUES ('Пчела (р.)', 2, /*'рамок',*/ 0);
INSERT INTO [main].[FamilyProperty] ([Name], [Type], /*[Unit],*/ [Order]) VALUES ('Мёд (р.)', 2, /*'рамок',*/ 0);
INSERT INTO [main].[FamilyProperty] ([Name], [Type], /*[Unit],*/ [Order]) VALUES ('Мёд (кг.)', 2, /*'килограмм',*/ 0);
INSERT INTO [main].[FamilyProperty] ([Name], [Type], /*[Unit],*/ [Order]) VALUES ('Сахар (кг.)', 2, /*'килограмм',*/ 0);
--END;
GO

--	Операции с пасекой
CREATE TABLE IF NOT EXISTS [main].[Operation] (
	[Id]		INTEGER PRIMARY KEY  AUTOINCREMENT  NOT NULL, 
	[Name]		VARCHAR(50) NOT NULL,

	[Created]	DATETIME NOT NULL	DEFAULT CURRENT_TIMESTAMP, 
	[Modified]	DATETIME NOT NULL	DEFAULT CURRENT_TIMESTAMP, 
	[Hide]		BOOL NOT NULL DEFAULT 0
);
GO
INSERT INTO [main].[Operation] ([Name]) VALUES ('Осмотр');
INSERT INTO [main].[Operation] ([Name]) VALUES ('Кормление');
INSERT INTO [main].[Operation] ([Name]) VALUES ('Качка');
GO


--	Ульи
CREATE  TABLE  IF NOT EXISTS [main].[Beehive] (
	[Id]		INTEGER PRIMARY KEY  AUTOINCREMENT  NOT NULL, 
	[Name]		VARCHAR(50) NOT NULL,	--	Название (№)

	[Address]	VARCHAR(50),			--	Адрес
	[Comment]	VARCHAR(255),			--	Коментарий

	[Created]	DATETIME NOT NULL	DEFAULT CURRENT_TIMESTAMP, 
	[Modified]	DATETIME NOT NULL	DEFAULT CURRENT_TIMESTAMP, 
	[Hide]		BOOL NOT NULL DEFAULT 0
	)
GO

--	Семья	--------------------------------------------------------------------------------------------------------------------
GO

--	Семьи
CREATE  TABLE  IF NOT EXISTS [main].[Family] (
	[Id]		INTEGER PRIMARY KEY  AUTOINCREMENT  NOT NULL, 
	[Name]		VARCHAR(50) NOT NULL,		--	Название (№)

	[BeehiveId] INTEGER NOT NULL,			--	Улей
	[BirthDay]	DATETIME NOT NULL,			--	Дата основания
	[DeathDay]	DATETIME NULL,				--	Дата гибели
	[Comment]	VARCHAR(255),				--	Коментарий

	[Created]	DATETIME NOT NULL	DEFAULT CURRENT_TIMESTAMP, 
	[Modified]	DATETIME NOT NULL	DEFAULT CURRENT_TIMESTAMP, 
	[Hide]		BOOL NOT NULL DEFAULT 0,	--	

	CONSTRAINT FK_Family2Beehive	FOREIGN KEY([BeehiveId])	REFERENCES [Beehive]([Id])
	)
GO

--	Состояние семьи (избыточность)
CREATE TABLE IF NOT EXISTS [main].[FamilyInfoProperty] (
	[Id]			INTEGER PRIMARY KEY  AUTOINCREMENT  NOT NULL, 

	[FamilyId]		INTEGER NOT NULL,			--	Семья
	[FamilyPropertyId] INTEGER NOT NULL,		--	Параметр
	[Value]			VARCHAR(50) NOT NULL,		--	Последнее значение
	[Comment]		VARCHAR(255),				--	Последний коментарий

	[Created]		DATETIME NOT NULL	DEFAULT CURRENT_TIMESTAMP, 
	[Modified]		DATETIME NOT NULL	DEFAULT CURRENT_TIMESTAMP, 
	[Hide]			BOOL NOT NULL DEFAULT 0,

	CONSTRAINT FK_FamilyInfoProperty2Family	FOREIGN KEY([FamilyId])	REFERENCES [Family]([Id]),
	CONSTRAINT FK_FamilyInfoProperty2FamilyProperty	FOREIGN KEY([FamilyPropertyId])	REFERENCES [FamilyProperty]([Id])
	)
GO


--	Семья	--------------------------------------------------------------------------------------------------------------------
GO

--	Набор свойств для операции
CREATE TABLE IF NOT EXISTS [main].[OperationProperty] (
	[Id]			INTEGER PRIMARY KEY  AUTOINCREMENT  NOT NULL, 

	[OperationId]	INTEGER NOT NULL ,			--	Операция
	[FamilyPropertyId] INTEGER NOT NULL ,		--	Параметр
	[Order]			INTEGER NOT NULL DEFAULT 0, --	Порядок (для заполнения)

	[Created]		DATETIME NOT NULL	DEFAULT CURRENT_TIMESTAMP, 
	[Modified]		DATETIME NOT NULL	DEFAULT CURRENT_TIMESTAMP, 
	[Hide]			BOOL NOT NULL DEFAULT 0,

	CONSTRAINT FK_OperftionProperties2Family	FOREIGN KEY([OperationId])	REFERENCES [Operation]([Id]),
	CONSTRAINT FK_OperftionProperties2FamilyProperty	FOREIGN KEY([FamilyPropertyId])	REFERENCES [FamilyProperty]([Id])
);
GO

--	Журнал операций
CREATE TABLE IF NOT EXISTS [main].[FamilyOperation] (
	[Id]			INTEGER PRIMARY KEY  AUTOINCREMENT  NOT NULL, 

	[DateWrite]		DATETIME NOT NULL	DEFAULT CURRENT_TIMESTAMP,	--	Дата заполнения
	[FamilyId]		INTEGER NOT NULL ,		--	Семья
	[OperationId]	INTEGER NOT NULL ,		--	Операция
	[FamilyPropertyId] INTEGER NOT NULL ,	--	Параметр
	[Value]			VARCHAR(50) NOT NULL ,	--	Значение
	[Comment]		VARCHAR(255),			--	Коментарий

	[Created]		DATETIME NOT NULL	DEFAULT CURRENT_TIMESTAMP, 
	[Modified]		DATETIME NOT NULL	DEFAULT CURRENT_TIMESTAMP, 
	[Hide]			BOOL NOT NULL DEFAULT 0,

	CONSTRAINT FK_FamilyOperation2Family	FOREIGN KEY([FamilyId])	REFERENCES [Family]([Id]),
	CONSTRAINT FK_FamilyOperation2Operation	FOREIGN KEY([OperationId])	REFERENCES [Operation]([Id]),
	CONSTRAINT FK_FamilyOperation2FamilyProperty	FOREIGN KEY([FamilyPropertyId])	REFERENCES [FamilyProperty]([Id])
	)
GO
