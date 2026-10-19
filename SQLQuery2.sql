INSERT INTO dbo.Gallery
(EventID, ImagePath, Caption)
SELECT
    E.EventID,
    V.ImagePath,
    V.Caption
FROM dbo.Events E
INNER JOIN
(
    VALUES
    ('Traditional Wedding',
     'traditional-wedding-01.jpg',
     'A beautifully planned traditional wedding celebration with elegant floral décor.'),

    ('Traditional Wedding',
     'traditional-wedding-02.jpg',
     'Traditional wedding details carefully arranged for a memorable celebration.'),

    ('Traditional Wedding',
     'traditional-wedding-03.jpg',
     'An elegant wedding ceremony setup created for a special day.'),

    ('Garden Wedding',
     'garden-wedding-01.jpg',
     'A romantic outdoor garden wedding surrounded by beautiful floral arrangements.'),

    ('Garden Wedding',
     'garden-wedding-02.jpg',
     'An elegant garden reception prepared for an unforgettable celebration.'),

    ('Garden Wedding',
     'garden-wedding-03.jpg',
     'A charming outdoor wedding setting designed with beautiful details.'),

    ('Birthday Party',
     'birthday-party-01.jpg',
     'A vibrant birthday celebration filled with beautiful decorations and joyful moments.'),

    ('Birthday Party',
     'birthday-party-02.jpg',
     'An elegant birthday dining setup prepared for a memorable celebration.'),

    ('Birthday Party',
     'birthday-party-03.jpg',
     'A beautifully decorated birthday cake and celebration setup.'),

    ('Corporate Event',
     'corporate-event-01.jpg',
     'A professional corporate event arranged for presentations and networking.'),

    ('Corporate Event',
     'corporate-event-02.jpg',
     'A refined corporate dining and conference setup.'),

    ('Corporate Event',
     'corporate-event-03.jpg',
     'A modern corporate event environment designed for a professional experience.'),

    ('Graduation',
     'graduation-01.jpg',
     'A stylish graduation celebration honoring an important achievement.'),

    ('Graduation',
     'graduation-02.jpg',
     'Graduation details arranged to celebrate an unforgettable milestone.'),

    ('Graduation',
     'graduation-03.jpg',
     'A beautifully decorated graduation venue celebrating the Class of 2025.'),

    ('Baby Shower',
     'baby-shower-01.jpg',
     'A charming baby shower decorated with soft colors and elegant details.'),

    ('Baby Shower',
     'baby-shower-02.jpg',
     'A beautifully arranged baby shower dessert and celebration table.'),

    ('Baby Shower',
     'baby-shower-03.jpg',
     'A warm and welcoming baby shower setup for a special family occasion.'),

    ('Proposal',
     'proposal-01.jpg',
     'A romantic proposal setting designed to create an unforgettable moment.'),

    ('Proposal',
     'proposal-02.jpg',
     'An intimate proposal arrangement featuring roses, candles and a special ring.'),

    ('Proposal',
     'proposal-03.jpg',
     'A romantic evening proposal setup created for a beautiful yes.')
) V(EventName, ImagePath, Caption)
ON E.EventName = V.EventName
WHERE E.IsActive = 1;