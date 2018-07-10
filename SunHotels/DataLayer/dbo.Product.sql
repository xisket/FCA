CREATE TABLE [dbo].Product
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [Name] NCHAR(100) NULL, 
    [Description] NCHAR(1000) NULL
)
CREATE TABLE [dbo].Provider
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [Name] NCHAR(100) NULL 
)
CREATE TABLE [dbo].ProductProvider
(
	[ProductId] INT NOT NULL , 
	[ProviderId] INT NOT NULL 
)