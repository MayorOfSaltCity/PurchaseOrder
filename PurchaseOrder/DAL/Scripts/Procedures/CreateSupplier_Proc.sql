-- ================================================
-- Template generated from Template Explorer using:
-- Create Procedure (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 
-- values below.
--
-- This block of comments will not be included in
-- the definition of the procedure.
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Paul Harrington
-- Create date: 05 March 2020
-- Description:	Create a New Supplier in the Database
-- =============================================
IF  EXISTS (SELECT 1 FROM sys.objects WHERE [name] = 'CreateSupplier' AND [type] = 'P')
BEGIN
	DROP PROCEDURE CreateSupplier
END
GO

CREATE PROCEDURE CreateSupplier 
	-- Add the parameters for the stored procedure here
	@SupplierCode nchar(64), 
	@Name nvarchar(256)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @SupplierID uniqueidentifier

	SELECT @SupplierID = NEWID()
    -- Insert statements for procedure here
	INSERT INTO dbo.Supplier ([ID], [Name], [SupplierCode], [CreatedDate])
	VALUES (@SupplierID, @Name, @SupplierCode, GETDATE())
	SELECT @SupplierID
END
GO
