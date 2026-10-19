
DECLARE @RoleID INT = 1;
DECLARE @FirstName VARCHAR(50) = 'Hilda';
DECLARE @LastName VARCHAR(50) = 'Tanasha';
DECLARE @Email VARCHAR(100) = 'tana.evidence@gmail.com';
DECLARE @Phone VARCHAR(20) = NULL;

DECLARE @PasswordHash VARCHAR(255) =
'rcFde+QYsCag3L1bLi5k5Q==:vwpyQnHwX09n8ucuP4sC39zpuEewaKa5qJVdCjoXuzw=';

IF NOT EXISTS (
    SELECT 1 FROM dbo.Roles WHERE RoleID = @RoleID
)
BEGIN
    PRINT 'ERROR: Admin role does not exist.';
    RETURN;
END;

IF EXISTS (
    SELECT 1 FROM dbo.Users WHERE Email = @Email
)
BEGIN
    PRINT 'ERROR: This email already exists.';
    RETURN;
END;

INSERT INTO dbo.Users
(
    RoleID, FirstName, LastName, Email,
    Phone, PasswordHash, IsActive
)
VALUES
(
    @RoleID, @FirstName, @LastName, @Email,
    @Phone, @PasswordHash, 1
);

SELECT UserID, RoleID, FirstName, LastName, Email, IsActive
FROM dbo.Users
WHERE Email = @Email;
