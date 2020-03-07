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
-- Description:	Get Purchase Order ID
-- =============================================
IF  EXISTS (SELECT 1 FROM sys.objects WHERE [name] = 'GetPurchaseOrderItemsByID' AND [type] = 'P')
BEGIN
	DROP PROCEDURE GetPurchaseOrderItemsByID
END
GO

CREATE PROCEDURE GetPurchaseOrderItemsByID
	-- Add the parameters for the stored procedure here
	@PurchaseOrderID uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT 
		p.[ID],
		[ProductCode], 
		[Price], 
		[Description],
		poi.Quantity
	FROM [Product] p
	INNER JOIN [PurchaseOrderItem] poi
	ON p.ID = poi.ProductId
	WHERE poi.ID = @PurchaseOrderID

END
GO
