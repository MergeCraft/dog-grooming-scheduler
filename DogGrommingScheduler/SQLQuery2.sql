-- Borramos agendas previas para evitar duplicados en las pruebas
DELETE FROM Schedules;

-- Insertamos agendas para los próximos 3 días para cada peluquero
-- Horario: 09:00 a 17:00
-- Borramos agendas previas para limpiar
DELETE FROM Schedules;

-- Insertamos agendas con tus IDs reales de la base de datos
INSERT INTO Schedules (Id, PetGroomerId, Date, StartTime, EndTime)
VALUES 
-- Agendas para Ana Martínez (ID: 4fb87e5b...)
(NEWID(), '4fb87e5b-89e4-488d-8f20-49299c1c3d53', CAST(GETDATE() AS DATE), CAST(GETDATE() AS DATE), CAST(GETDATE() AS DATE)),
(NEWID(), '4fb87e5b-89e4-488d-8f20-49299c1c3d53', CAST(GETDATE()+1 AS DATE), '2026-03-18 09:00:00', '2026-03-18 17:00:00'),

-- Agendas para Carlos Pérez (ID: 4d1d2047...)
(NEWID(), '4d1d2047-8020-473b-a293-35f6d43b0314', CAST(GETDATE() AS DATE), '2026-03-17 08:00:00', '2026-03-17 14:00:00'),
(NEWID(), '4d1d2047-8020-473b-a293-35f6d43b0314', CAST(GETDATE()+1 AS DATE), '2026-03-18 08:00:00', '2026-03-18 14:00:00'),

-- Agendas para Lucía Gómez (ID: 2864b026...)
(NEWID(), '2864b026-1c02-4e02-8a77-944aa4fa7e87', CAST(GETDATE() AS DATE), '2026-03-17 10:00:00', '2026-03-17 18:00:00'),
(NEWID(), '2864b026-1c02-4e02-8a77-944aa4fa7e87', CAST(GETDATE()+1 AS DATE), '2026-03-18 10:00:00', '2026-03-18 18:00:00'),

-- Agendas para Marcos Sosa (ID: 4fed3efc...)
(NEWID(), '4fed3efc-ae4c-4405-bd46-027612a6a416', CAST(GETDATE() AS DATE), '2026-03-17 09:00:00', '2026-03-17 15:00:00'),

-- Agendas para Belén Rodríguez (ID: 231e0844...)
(NEWID(), '231e0844-bc5a-47f6-88e1-7b72910f9ca4', CAST(GETDATE() AS DATE), '2026-03-17 11:00:00', '2026-03-17 19:00:00');