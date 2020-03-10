/*
Initial data import for dbo.Product table
*/
IF NOT EXISTS (SELECT * FROM dbo.Product WHERE Id = 1)
	INSERT INTO dbo.Product (Id, Name, ImgUri, Price) VALUES (1,'TestProduct1','TestUri1', 100)
IF NOT EXISTS (SELECT * FROM dbo.Product WHERE Id = 2)
	INSERT INTO dbo.Product (Id, Name, ImgUri, Price) VALUES (2,'TestProduct2','TestUri2', 200);
IF NOT EXISTS (SELECT * FROM dbo.Product WHERE Id = 3)
	INSERT INTO dbo.Product (Id, Name, ImgUri, Price) VALUES (3,'TestProduct3','TestUri3', 300);