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
-- Description:	Delete a product from a purchase order in the database
-- =============================================

IF EXISTS (SELECT 1 FROM sys.objects WHERE [Name] = 'DeleteProductFromPurchaseOrderById' AND TYPE = 'P')
BEGIN
	DROP PROCEDURE DeleteProductFromPurchaseOrderById
END
GO

CREATE PROCEDURE DeleteProductFromPurchaseOrderById
	-- Add the parameters for the stored procedure here
	@ProductId uniqueidentifier,
	@PurchaseOrderId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DELETE FROM PurchaseOrderItem
	WHERE PurchaseOrderID = @PurchaseOrderId AND
	ProductID = @ProductID
END
GO
