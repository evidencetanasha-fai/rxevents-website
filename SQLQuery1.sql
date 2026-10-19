INSERT INTO dbo.PackageServices
(PackageID, ServiceID, Quantity, Notes)
VALUES

-- Platinum Package
(1, 1, 1, 'Complete event planning'),
(1, 2, 1, 'Luxury venue decoration'),
(1, 3, 1, 'Premium catering service'),
(1, 4, 1, 'Professional photography'),
(1, 5, 1, 'Entertainment and music'),
(1, 6, 1, 'Complete event coordination'),

-- Diamond Package
(2, 1, 1, 'Complete event planning'),
(2, 2, 1, 'Premium venue decoration'),
(2, 3, 1, 'Premium catering service'),
(2, 4, 1, 'Professional photography'),
(2, 5, 1, 'Entertainment and music'),
(2, 6, 1, 'Professional event coordination'),

-- Gold Package
(3, 1, 1, 'Event planning'),
(3, 2, 1, 'Elegant venue decoration'),
(3, 3, 1, 'Catering service'),
(3, 4, 1, 'Professional photography'),
(3, 6, 1, 'Event coordination'),

-- Silver Package
(4, 1, 1, 'Event planning'),
(4, 2, 1, 'Beautiful venue decoration'),
(4, 3, 1, 'Catering service'),
(4, 6, 1, 'Event coordination'),

-- Bronze Package
(5, 1, 1, 'Basic event planning'),
(5, 2, 1, 'Venue decoration'),
(5, 6, 1, 'Event coordination'),

-- Custom Experience Package
(6, 1, 1, 'Customized event planning'),
(6, 2, 1, 'Customized decoration'),
(6, 3, 1, 'Catering based on requirements'),
(6, 6, 1, 'Flexible event coordination'),

-- Forever Moments Package
(7, 1, 1, 'Personalized event planning'),
(7, 2, 1, 'Personalized decoration'),
(7, 4, 1, 'Professional photography'),
(7, 6, 1, 'Event coordination');