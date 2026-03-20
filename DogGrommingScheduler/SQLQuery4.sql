-- 1. Limpieza de agendas previas para evitar solapamientos en las pruebas
DELETE FROM Schedules;

-- 2. Inserción de Agendas Dinámicas (Hoy y Mañana)
-- Usamos DATEADD y DATEDIFF para asegurar que el componente hora sea exacto sobre la fecha actual.

INSERT INTO Schedules (Id, PetGroomerId, Date, StartTime, EndTime)
VALUES 
-----------------------------------------------------------------------
-- ANA MARTÍNEZ (ID: 4fb87e5b-89e4-488d-8f20-49299c1c3d53)
-- Horario: 09:00 a 17:00
-----------------------------------------------------------------------
-- Hoy
(NEWID(), '4fb87e5b-89e4-488d-8f20-49299c1c3d53', 
 CAST(GETDATE() AS DATE), 
 DATEADD(HOUR, 9, CAST(CAST(GETDATE() AS DATE) AS DATETIME)), 
 DATEADD(HOUR, 17, CAST(CAST(GETDATE() AS DATE) AS DATETIME))),
-- Mañana
(NEWID(), '4fb87e5b-89e4-488d-8f20-49299c1c3d53', 
 DATEADD(DAY, 1, CAST(GETDATE() AS DATE)), 
 DATEADD(HOUR, 9, CAST(DATEADD(DAY, 1, CAST(GETDATE() AS DATE)) AS DATETIME)), 
 DATEADD(HOUR, 17, CAST(DATEADD(DAY, 1, CAST(GETDATE() AS DATE)) AS DATETIME))),

-----------------------------------------------------------------------
-- CARLOS PÉREZ (ID: 4d1d2047-8020-473b-a293-35f6d43b0314)
-- Horario: 08:00 a 14:00
-----------------------------------------------------------------------
-- Hoy
(NEWID(), '4d1d2047-8020-473b-a293-35f6d43b0314', 
 CAST(GETDATE() AS DATE), 
 DATEADD(HOUR, 8, CAST(CAST(GETDATE() AS DATE) AS DATETIME)), 
 DATEADD(HOUR, 14, CAST(CAST(GETDATE() AS DATE) AS DATETIME))),
-- Mañana
(NEWID(), '4d1d2047-8020-473b-a293-35f6d43b0314', 
 DATEADD(DAY, 1, CAST(GETDATE() AS DATE)), 
 DATEADD(HOUR, 8, CAST(DATEADD(DAY, 1, CAST(GETDATE() AS DATE)) AS DATETIME)), 
 DATEADD(HOUR, 14, CAST(DATEADD(DAY, 1, CAST(GETDATE() AS DATE)) AS DATETIME))),

-----------------------------------------------------------------------
-- LUCÍA GÓMEZ (ID: 2864b026-1c02-4e02-8a77-944aa4fa7e87)
-- Horario: 10:00 a 18:00
-----------------------------------------------------------------------
-- Hoy
(NEWID(), '2864b026-1c02-4e02-8a77-944aa4fa7e87', 
 CAST(GETDATE() AS DATE), 
 DATEADD(HOUR, 10, CAST(CAST(GETDATE() AS DATE) AS DATETIME)), 
 DATEADD(HOUR, 18, CAST(CAST(GETDATE() AS DATE) AS DATETIME))),
-- Mañana
(NEWID(), '2864b026-1c02-4e02-8a77-944aa4fa7e87', 
 DATEADD(DAY, 1, CAST(GETDATE() AS DATE)), 
 DATEADD(HOUR, 10, CAST(DATEADD(DAY, 1, CAST(GETDATE() AS DATE)) AS DATETIME)), 
 DATEADD(HOUR, 18, CAST(DATEADD(DAY, 1, CAST(GETDATE() AS DATE)) AS DATETIME))),

-----------------------------------------------------------------------
-- MARCOS SOSA (ID: 4fed3efc-ae4c-4405-bd46-027612a6a416)
-- Horario: 09:00 a 15:00
-----------------------------------------------------------------------
-- Hoy
(NEWID(), '4fed3efc-ae4c-4405-bd46-027612a6a416', 
 CAST(GETDATE() AS DATE), 
 DATEADD(HOUR, 9, CAST(CAST(GETDATE() AS DATE) AS DATETIME)), 
 DATEADD(HOUR, 15, CAST(CAST(GETDATE() AS DATE) AS DATETIME))),

-----------------------------------------------------------------------
-- BELÉN RODRÍGUEZ (ID: 231e0844-bc5a-47f6-88e1-7b72910f9ca4)
-- Horario: 11:00 a 19:00
-----------------------------------------------------------------------
-- Hoy
(NEWID(), '231e0844-bc5a-47f6-88e1-7b72910f9ca4', 
 CAST(GETDATE() AS DATE), 
 DATEADD(HOUR, 11, CAST(CAST(GETDATE() AS DATE) AS DATETIME)), 
 DATEADD(HOUR, 19, CAST(CAST(GETDATE() AS DATE) AS DATETIME)));