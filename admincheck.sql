
UPDATE dbo.Users
SET PasswordHash = 'W8XIDznVGDodgz4hXZXLxQ==:IJI6D9DIxQISYyZ4v754WvkSo5kit36V/a0zG3R9hFY='
WHERE Email = 'tana.evidence@gmail.com'
  AND RoleID = 1;

SELECT
    UserID,
    Email,
    RoleID,
    IsActive,
    LEN(PasswordHash) AS HashLength
FROM dbo.Users
WHERE Email = 'tana.evidence@gmail.com';
