CREATE PROCEDURE [dbo].[usp_GetVehicleByDescription]
	@VehicleDescription varchar(20)
AS
BEGIN
SET NOCOUNT ON;
	SELECT [v].[Id],[v].[Description]
	FROM [dbo].[Vehicle] [v]
	WHERE [v].[Description] = @VehicleDescription;
END