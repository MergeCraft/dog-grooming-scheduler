
-- 1. Insertar los Usuarios con la fecha de creación
INSERT INTO AspNetUsers (Id, UserName, Email, EmailConfirmed, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount, Name, CreatedAt, SecurityStamp, ConcurrencyStamp)
VALUES 
('G-01', 'ana.m@canina.com', 'ana.m@canina.com', 1, 0, 0, 1, 0, 'Ana Martínez', GETDATE(), NEWID(), NEWID()),
('G-02', 'carlos.p@canina.com', 'carlos.p@canina.com', 1, 0, 0, 1, 0, 'Carlos Pérez', GETDATE(), NEWID(), NEWID()),
('G-03', 'lucia.g@canina.com', 'lucia.g@canina.com', 1, 0, 0, 1, 0, 'Lucía Gómez', GETDATE(), NEWID(), NEWID()),
('G-04', 'marcos.s@canina.com', 'marcos.s@canina.com', 1, 0, 0, 1, 0, 'Marcos Sosa', GETDATE(), NEWID(), NEWID()),
('G-05', 'belen.r@canina.com', 'belen.r@canina.com', 1, 0, 0, 1, 0, 'Belén Rodríguez', GETDATE(), NEWID(), NEWID());

-- 2. Insertar los PetGroomers vinculados
INSERT INTO PetGroomers (Id, UserId)
VALUES 
(NEWID(), 'G-01'),
(NEWID(), 'G-02'),
(NEWID(), 'G-03'),
(NEWID(), 'G-04'),
(NEWID(), 'G-05');