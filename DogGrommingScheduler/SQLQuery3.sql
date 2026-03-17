-- Actualizamos la agenda de Ana Martínez para mañana y pasado con horarios reales
UPDATE Schedules 
SET StartTime = CAST(Date AS DATETIME) + CAST('09:00:00' AS DATETIME),
    EndTime = CAST(Date AS DATETIME) + CAST('17:00:00' AS DATETIME)
WHERE PetGroomerId = '4fb87e5b-89e4-488d-8f20-49299c1c3d53';