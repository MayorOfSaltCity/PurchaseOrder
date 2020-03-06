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
-- Description:	Finalizes a purchase order
-- =============================================

IF EXISTS (SELECT 1 FROM sys.objects WHERE [Name] = 'RemoveProductFromPurchaseOrder' AND TYPE = 'P')
BEGIN
	DROP PROCEDURE RemoveProductFromPurchaseOrder
END
GO

CREATE PROCEDURE RemoveProductFromPurchaseOrder
	-- Add the parameters for the stored procedure here
	@ProductID uniqueidentifier,
	@PurchaseOrderID uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DELETE FROM PurchaseOrderItem
	WHERE [ProductID] = @ProductID
	AND [PurchaseOrderID] = @PurchaseOrderID
END
GO
