ALTER TABLE Bookings
ADD CONSTRAINT FK_Bookings_WishlistFolders
FOREIGN KEY (WishlistFolderID)
REFERENCES WishlistFolders(WishlistFolderID);