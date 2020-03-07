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
-- Description:	Adds a product to a supplier by Supplier ID
-- =============================================

IF EXISTS (SELECT 1 FROM sys.objects WHERE [Name] = 'CreatePurchaseOrder' AND TYPE = 'P')
BEGIN
	DROP PROCEDURE CreatePurchaseOrder
END
GO

CREATE PROCEDURE CreatePurchaseOrder
	-- Add the parameters for the stored procedure here
	@SupplierID uniqueidentifier

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @PoNumber AS INT

	SELECT @PoNumber = COUNT(1) FROM [PurchaseOrder]
	WHERE [SupplierID] = @SupplierID
	DECLARE @POID uniqueidentifier
	SET @POID = NEWID()
    -- Insert statements for procedure here
	INSERT INTO PurchaseOrder ([ID], [SupplierID], [Number], [CreatedDate])
	VALUES (@POID, @SupplierID, @PoNumber, GetDate())
	SELECT @POID
END
GO
