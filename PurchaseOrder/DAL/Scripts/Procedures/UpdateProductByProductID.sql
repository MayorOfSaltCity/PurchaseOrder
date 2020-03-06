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

IF EXISTS (SELECT 1 FROM sys.objects WHERE [Name] = 'UpdateProductByProductID' AND TYPE = 'P')
BEGIN
	DROP PROCEDURE UpdateProductByProductID
END
GO

CREATE PROCEDURE UpdateProductByProductID 
	-- Add the parameters for the stored procedure here
	@Id uniqueidentifier,
	@ProductCode nchar(64),
	@Description nvarchar(256),
	@Price decimal (18,4)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE Product SET 
		IsDeleted = 1,
		UpdatedDate = GetDate()
	WHERE [ID] = @Id
	DECLARE @newProductID uniqueidentifier = NEWID()
	DECLARE @SupplierID uniqueidentifier 
	SELECT @SupplierID = SupplierId FROM Product WHERE Id = @Id

	INSERT INTO Product ([ID],[ProductCode],[Description], [Price], [CreatedDate], SupplierId)
	VALUES (@newProductID, @ProductCode, @Description, @Price, GetDate(), @SupplierID)

	SELECT @newProductID
END
GO
