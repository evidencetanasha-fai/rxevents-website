CREATE TABLE [dbo].[Addresses]
(
    [AddressID] INT IDENTITY(1,1) NOT NULL,
    [UserID] INT NOT NULL,

    [AddressLine1] VARCHAR(150) NOT NULL,
    [AddressLine2] VARCHAR(150) NULL,
    [City] VARCHAR(100) NOT NULL,
    [State] VARCHAR(100) NOT NULL,
    [PostalCode] VARCHAR(20) NOT NULL,
    [Country] VARCHAR(100) NOT NULL,

    [IsDefault] BIT NOT NULL
        CONSTRAINT [DF_Addresses_IsDefault] DEFAULT (0),

    [CreatedDate] DATETIME NOT NULL
        CONSTRAINT [DF_Addresses_CreatedDate] DEFAULT (GETDATE()),

    [UpdatedDate] DATETIME NOT NULL
        CONSTRAINT [DF_Addresses_UpdatedDate] DEFAULT (GETDATE()),

    CONSTRAINT [PK_Addresses]
        PRIMARY KEY CLUSTERED ([AddressID] ASC),

    CONSTRAINT [FK_Addresses_Users]
        FOREIGN KEY ([UserID])
        REFERENCES [dbo].[Users] ([UserID])
);