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

IF EXISTS (SELECT 1 FROM sys.objects WHERE [Name] = 'AddProductToSupplierBySupplierID' AND TYPE = 'P')
BEGIN
	DROP PROCEDURE AddProductToSupplierBySupplierID
END
GO

CREATE PROCEDURE AddProductToSupplierBySupplierID
	-- Add the parameters for the stored procedure here
	@ProductCode nvarchar(64),
	@Description nvarchar(256),
	@Price decimal (18,4),
	@SupplierID uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @ProductID uniqueidentifier = NEWID()
    -- Insert statements for procedure here
	INSERT INTO Product ([ID],[ProductCode],[Description], [Price], [SupplierID], [CreatedDate])
	VALUES (@ProductID, @ProductCode, @Description, @Price, @SupplierID, GetDate())

	SELECT @ProductID
END
GO
