CREATE TABLE [dbo].[VehicleLocation]
(	
	[Id] int NOT NULL PRIMARY KEY IDENTITY,
	[VehicleId] int NOT NULL UNIQUE,
	[Longitude] decimal(10,7) NOT NULL,
	[Latitude] decimal(10,7) NOT NULL,
	[ReportedTime] datetime NOT NUll DEFAULT (GETDATE()), 
    CONSTRAINT [FK_VehicleLocation_Vehicle] FOREIGN KEY ([VehicleId]) REFERENCES [Vehicle]([Id])
)
