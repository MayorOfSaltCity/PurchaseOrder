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
-- Description:	Get product by product code
-- =============================================
IF  EXISTS (SELECT 1 FROM sys.objects WHERE [name] = 'GetProductsBySupplierID' AND [type] = 'P')
BEGIN
	DROP PROCEDURE GetProductsBySupplierID
END
GO
CREATE PROCEDURE GetProductsBySupplierID
	-- Add the parameters for the stored procedure here
	@SupplierID uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT	p.[ID], 
			[ProductCode], 
			[Description], 
			[Price], 
			s.[Name] as [SupplierName] , 
			s.SupplierCode, 
			s.Id as SupplierId,
			s.CreatedDate as CreatedDate,
			p.IsDeleted,
			p.UpdatedDate
	FROM Product p
	INNER JOIN Supplier s ON s.ID = p.SupplierID
	WHERE s.[ID] = @SupplierID
END
GO
