IF OBJECT_ID('dbo.Pedido', 'U') IS NOT NULL DROP TABLE dbo.Pedido;
IF OBJECT_ID('dbo.Bolo',   'U') IS NOT NULL DROP TABLE dbo.Bolo;
GO

CREATE TABLE dbo.Bolo (
                          Id UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID() PRIMARY KEY,
                          Nome  NVARCHAR(120) NOT NULL,
                          Sabor NVARCHAR(120) NOT NULL,
                          Preco DECIMAL(10,2) NOT NULL CHECK (Preco >= 0)
);
GO

CREATE TABLE dbo.Pedido (
                            Id UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID() PRIMARY KEY,
                            BoloId UNIQUEIDENTIFIER NOT NULL,
                            NomeCliente NVARCHAR(120) NOT NULL,
                            CONSTRAINT FK_Pedido_Bolo FOREIGN KEY (BoloId) REFERENCES dbo.Bolo(Id)
);
GO

INSERT INTO dbo.Bolo (Id, Nome, Sabor, Preco) VALUES
  (NEWID(), 'Floresta Negra',  'Chocolate com cereja', 25.00),
  (NEWID(), 'Cenoura',         'Cenoura com chocolate', 19.00);
GO
