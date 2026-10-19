
IF NOT EXISTS (SELECT 1 FROM Settings WHERE SettingName = 'WebsiteName')
    INSERT INTO Settings (SettingName, SettingValue, Description, UpdatedDate)
    VALUES ('WebsiteName', 'Rx Events Solutions', 'Website name', GETDATE());

IF NOT EXISTS (SELECT 1 FROM Settings WHERE SettingName = 'CompanyEmail')
    INSERT INTO Settings (SettingName, SettingValue, Description, UpdatedDate)
    VALUES ('CompanyEmail', '', 'Public contact email', GETDATE());

IF NOT EXISTS (SELECT 1 FROM Settings WHERE SettingName = 'CompanyPhone')
    INSERT INTO Settings (SettingName, SettingValue, Description, UpdatedDate)
    VALUES ('CompanyPhone', '', 'Public contact phone', GETDATE());

IF NOT EXISTS (SELECT 1 FROM Settings WHERE SettingName = 'CompanyAddress')
    INSERT INTO Settings (SettingName, SettingValue, Description, UpdatedDate)
    VALUES ('CompanyAddress', '', 'Business address', GETDATE());

IF NOT EXISTS (SELECT 1 FROM Settings WHERE SettingName = 'WorkingHours')
    INSERT INTO Settings (SettingName, SettingValue, Description, UpdatedDate)
    VALUES ('WorkingHours', '', 'Business working hours', GETDATE());

IF NOT EXISTS (SELECT 1 FROM Settings WHERE SettingName = 'Facebook')
    INSERT INTO Settings (SettingName, SettingValue, Description, UpdatedDate)
    VALUES ('Facebook', '', 'Facebook URL', GETDATE());

IF NOT EXISTS (SELECT 1 FROM Settings WHERE SettingName = 'Instagram')
    INSERT INTO Settings (SettingName, SettingValue, Description, UpdatedDate)
    VALUES ('Instagram', '', 'Instagram URL', GETDATE());

IF NOT EXISTS (SELECT 1 FROM Settings WHERE SettingName = 'X')
    INSERT INTO Settings (SettingName, SettingValue, Description, UpdatedDate)
    VALUES ('X', '', 'X social media URL', GETDATE());

IF NOT EXISTS (SELECT 1 FROM Settings WHERE SettingName = 'LinkedIn')
    INSERT INTO Settings (SettingName, SettingValue, Description, UpdatedDate)
    VALUES ('LinkedIn', '', 'LinkedIn URL', GETDATE());

IF NOT EXISTS (SELECT 1 FROM Settings WHERE SettingName = 'AboutText')
    INSERT INTO Settings (SettingName, SettingValue, Description, UpdatedDate)
    VALUES ('AboutText', '', 'Website about section', GETDATE());

IF NOT EXISTS (SELECT 1 FROM Settings WHERE SettingName = 'FooterText')
    INSERT INTO Settings (SettingName, SettingValue, Description, UpdatedDate)
    VALUES ('FooterText', '© 2026 Rx Events Solutions. All rights reserved.', 'Website footer', GETDATE());

IF NOT EXISTS (SELECT 1 FROM Settings WHERE SettingName = 'LogoPath')
    INSERT INTO Settings (SettingName, SettingValue, Description, UpdatedDate)
    VALUES ('LogoPath', '', 'Website logo path', GETDATE());

IF NOT EXISTS (SELECT 1 FROM Settings WHERE SettingName = 'FaviconPath')
    INSERT INTO Settings (SettingName, SettingValue, Description, UpdatedDate)
    VALUES ('FaviconPath', '', 'Website favicon path', GETDATE());
