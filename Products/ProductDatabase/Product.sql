CREATE TABLE [dbo].[Product]
(
	[Id] BIGINT NOT NULL PRIMARY KEY, 
    [Name] VARCHAR(50) NOT NULL, 
    [ImgUri] VARCHAR(50) NOT NULL, 
    [Price] DECIMAL NOT NULL, 
    [Description] VARCHAR(MAX) NULL
)
