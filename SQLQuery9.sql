ALTER TABLE dbo.Bookings
ALTER COLUMN UserID INT NULL;

IF COL_LENGTH('dbo.Bookings', 'GuestCustomerName') IS NULL
BEGIN
    ALTER TABLE dbo.Bookings
    ADD GuestCustomerName VARCHAR(200) NULL;
END;