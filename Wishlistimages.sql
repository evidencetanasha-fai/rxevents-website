CREATE TABLE [dbo].[WishlistImages]
(
    [WishlistImageID] INT IDENTITY(1,1) NOT NULL,
    [WishlistFolderID] INT NOT NULL,
    [ImagePath] VARCHAR(255) NOT NULL,
    [Caption] VARCHAR(255) NULL,
    [DateAdded] DATETIME NOT NULL
        CONSTRAINT [DF_WishlistImages_DateAdded]
        DEFAULT (GETDATE()),

    CONSTRAINT [PK_WishlistImages]
        PRIMARY KEY CLUSTERED ([WishlistImageID] ASC),

    CONSTRAINT [FK_WishlistImages_WishlistFolders]
        FOREIGN KEY ([WishlistFolderID])
        REFERENCES [dbo].[WishlistFolders] ([WishlistFolderID])
        ON DELETE CASCADE
);