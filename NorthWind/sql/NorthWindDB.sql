IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261009180609_InitialCreate'
)
BEGIN
    CREATE TABLE [Orders] (
        [Id] int NOT NULL IDENTITY,
        [CustomerId] nchar(5) NOT NULL,
        [ShipAddress] nvarchar(60) NOT NULL,
        [ShipCity] nvarchar(15) NOT NULL,
        [ShipCountry] nvarchar(15) NOT NULL,
        [ShipPostalCode] nvarchar(10) NOT NULL,
        [ShippingType] int NOT NULL,
        [DiscountType] int NOT NULL,
        [Discount] float NOT NULL,
        [OrderDate] datetime2 NOT NULL,
        CONSTRAINT [PK_Orders] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261009180609_InitialCreate'
)
BEGIN
    CREATE TABLE [OrderDetails] (
        [OrderId] int NOT NULL,
        [ProductId] int NOT NULL,
        [UnitPrice] decimal(8,2) NOT NULL,
        [Quantity] smallint NOT NULL,
        CONSTRAINT [PK_OrderDetails] PRIMARY KEY ([OrderId], [ProductId]),
        CONSTRAINT [FK_OrderDetails_Orders_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [Orders] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261009180609_InitialCreate'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20261009180609_InitialCreate', N'10.0.12');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261009223021_AddProductAndCustomer'
)
BEGIN
    CREATE TABLE [Customers] (
        [Id] nchar(5) NOT NULL,
        [Name] nvarchar(40) NOT NULL,
        [CurrentBalance] decimal(8,2) NOT NULL,
        CONSTRAINT [PK_Customers] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261009223021_AddProductAndCustomer'
)
BEGIN
    CREATE TABLE [Products] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(40) NOT NULL,
        [UnitsInStock] smallint NOT NULL,
        [UnitPrice] decimal(8,2) NOT NULL,
        CONSTRAINT [PK_Products] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261009223021_AddProductAndCustomer'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CurrentBalance', N'Name') AND [object_id] = OBJECT_ID(N'[Customers]'))
        SET IDENTITY_INSERT [Customers] ON;
    EXEC(N'INSERT INTO [Customers] ([Id], [CurrentBalance], [Name])
    VALUES (N''ALFKI'', 0.0, N''Alfreds Futterkiste''),
    (N''ANTON'', 100.0, N''Antonio Moreno''),
    (N''BOLID'', 0.0, N''Bolido Comidas''),
    (N''FAMIA'', 0.0, N''Familia Arquibaldo''),
    (N''GALED'', 0.0, N''Galería del gastrónomo'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CurrentBalance', N'Name') AND [object_id] = OBJECT_ID(N'[Customers]'))
        SET IDENTITY_INSERT [Customers] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261009223021_AddProductAndCustomer'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name', N'UnitPrice', N'UnitsInStock') AND [object_id] = OBJECT_ID(N'[Products]'))
        SET IDENTITY_INSERT [Products] ON;
    EXEC(N'INSERT INTO [Products] ([Id], [Name], [UnitPrice], [UnitsInStock])
    VALUES (1, N''Chai'', 35.0, CAST(20 AS smallint)),
    (2, N''Chang'', 35.0, CAST(0 AS smallint)),
    (3, N''Aniseed Syrup'', 65.0, CAST(30 AS smallint)),
    (4, N''Chef Anton''''s Cajun'', 75.0, CAST(50 AS smallint)),
    (5, N''Chef Anton''''s Gumbo'', 45.0, CAST(15 AS smallint))');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name', N'UnitPrice', N'UnitsInStock') AND [object_id] = OBJECT_ID(N'[Products]'))
        SET IDENTITY_INSERT [Products] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261009223021_AddProductAndCustomer'
)
BEGIN
    CREATE INDEX [IX_Orders_CustomerId] ON [Orders] ([CustomerId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261009223021_AddProductAndCustomer'
)
BEGIN
    CREATE INDEX [IX_OrderDetails_ProductId] ON [OrderDetails] ([ProductId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261009223021_AddProductAndCustomer'
)
BEGIN
    ALTER TABLE [OrderDetails] ADD CONSTRAINT [FK_OrderDetails_Products_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [Products] ([Id]) ON DELETE CASCADE;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261009223021_AddProductAndCustomer'
)
BEGIN
    ALTER TABLE [Orders] ADD CONSTRAINT [FK_Orders_Customers_CustomerId] FOREIGN KEY ([CustomerId]) REFERENCES [Customers] ([Id]) ON DELETE CASCADE;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261009223021_AddProductAndCustomer'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20261009223021_AddProductAndCustomer', N'10.0.12');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261018011148_PosFase1'
)
BEGIN
    ALTER TABLE [Products] ADD [RowVersion] rowversion NOT NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261018011148_PosFase1'
)
BEGIN
    ALTER TABLE [Orders] ADD [Subtotal] decimal(12,2) NOT NULL DEFAULT 0.0;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261018011148_PosFase1'
)
BEGIN
    ALTER TABLE [Orders] ADD [TaxAmount] decimal(12,2) NOT NULL DEFAULT 0.0;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261018011148_PosFase1'
)
BEGIN
    ALTER TABLE [Orders] ADD [Total] decimal(12,2) NOT NULL DEFAULT 0.0;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261018011148_PosFase1'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20261018011148_PosFase1', N'10.0.12');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261018012732_PosFase2'
)
BEGIN
    ALTER TABLE [Customers] ADD [Email] nvarchar(100) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261018012732_PosFase2'
)
BEGIN
    ALTER TABLE [Customers] ADD [Identification] nvarchar(13) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261018012732_PosFase2'
)
BEGIN
    EXEC(N'UPDATE [Customers] SET [Email] = N''alfki@example.com'', [Identification] = N''1710034065''
    WHERE [Id] = N''ALFKI'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261018012732_PosFase2'
)
BEGIN
    EXEC(N'UPDATE [Customers] SET [Email] = N''anton@example.com'', [Identification] = N''1712345675''
    WHERE [Id] = N''ANTON'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261018012732_PosFase2'
)
BEGIN
    EXEC(N'UPDATE [Customers] SET [Email] = N''bolid@example.com'', [Identification] = N''0912345675''
    WHERE [Id] = N''BOLID'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261018012732_PosFase2'
)
BEGIN
    EXEC(N'UPDATE [Customers] SET [Email] = N''famia@example.com'', [Identification] = N''1801234566''
    WHERE [Id] = N''FAMIA'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261018012732_PosFase2'
)
BEGIN
    EXEC(N'UPDATE [Customers] SET [Email] = N''galed@example.com'', [Identification] = N''0102030400''
    WHERE [Id] = N''GALED'';
    SELECT @@ROWCOUNT');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261018012732_PosFase2'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20261018012732_PosFase2', N'10.0.12');
END;

COMMIT;
GO

