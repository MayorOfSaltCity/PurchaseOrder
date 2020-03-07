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
-- Description:	Get Purchase Orders by Supplier ID
-- =============================================
IF  EXISTS (SELECT 1 FROM sys.objects WHERE [name] = 'GetPurchaseOrdersBySupplierID' AND [type] = 'P')
BEGIN
	DROP PROCEDURE GetPurchaseOrdersBySupplierID
END
GO

CREATE PROCEDURE GetPurchaseOrdersBySupplierID
	-- Add the parameters for the stored procedure here
	@SupplierID uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT [PurchaseOrder].[ID] as [PurchaseOrderID], LTRIM(RTRIM([Number])), [PurchaseOrder].[CreatedDate] FROM
	Supplier
	INNER JOIN [PurchaseOrder] ON [PurchaseOrder].[SupplierID] = [Supplier].[ID]
	WHERE [Supplier].[ID]  = @SupplierID
END
GO
