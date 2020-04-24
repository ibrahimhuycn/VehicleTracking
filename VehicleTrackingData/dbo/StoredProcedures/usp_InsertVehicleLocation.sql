CREATE PROCEDURE [dbo].[usp_InsertVehicleLocation]
	@VehicleId int,
	@Longitude decimal(10,7),
	@Latitude decimal(10,7)
AS
BEGIN
	SET XACT_ABORT ON;
	SET NOCOUNT ON;
	BEGIN TRANSACTION;
	BEGIN TRY

		UPDATE [dbo].[VehicleLocation] 
		SET [Longitude] = @Longitude, [Latitude] = @Latitude, [ReportedTime] = (DATEADD(HOUR,5,GETDATE()))
		WHERE [VehicleId] = @VehicleId;
		IF @@ROWCOUNT = 0
		BEGIN
			INSERT INTO [dbo].[VehicleLocation] (VehicleId, Longitude, Latitude)
			VALUES (@VehicleId,@Longitude,@Latitude);
		END

	COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
    IF @@TRANCOUNT > 0
    BEGIN
        ROLLBACK TRANSACTION;
    END
    END CATCH

END
