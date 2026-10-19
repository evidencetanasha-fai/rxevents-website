CREATE TABLE [dbo].[WishlistFolders]
(
    [WishlistFolderID] INT IDENTITY(1,1) NOT NULL,
    [UserID] INT NOT NULL,
    [CategoryID] INT NOT NULL,
    [FolderName] VARCHAR(150) NOT NULL,
    [Budget] DECIMAL(18,2) NULL,
    [Notes] VARCHAR(2000) NULL,
    [CreatedDate] DATETIME NOT NULL
        CONSTRAINT [DF_WishlistFolders_CreatedDate]
        DEFAULT (GETDATE()),
    [UpdatedDate] DATETIME NULL,

    CONSTRAINT [PK_WishlistFolders]
        PRIMARY KEY CLUSTERED ([WishlistFolderID] ASC),

    CONSTRAINT [FK_WishlistFolders_Users]
        FOREIGN KEY ([UserID])
        REFERENCES [dbo].[Users] ([UserID]),

    CONSTRAINT [FK_WishlistFolders_EventCategories]
        FOREIGN KEY ([CategoryID])
        REFERENCES [dbo].[EventCategories] ([CategoryID])
);