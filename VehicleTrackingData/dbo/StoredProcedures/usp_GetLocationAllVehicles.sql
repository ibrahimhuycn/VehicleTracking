CREATE PROCEDURE [dbo].[usp_GetLocationAllVehicles]
AS
BEGIN
SET NOCOUNT ON;
	SELECT [v].[Description] AS [uniqueVehicleName], [vl].[Longitude], [vl].[Latitude], [vl].[ReportedTime]
	FROM [dbo].[VehicleLocation] [vl]
	INNER JOIN [dbo].[Vehicle] [v] ON [vl].[VehicleId] = [v].[Id];
END
