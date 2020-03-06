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
-- Description:	Select Supplier By Supplier Code
-- =============================================
IF EXISTS (SELECT 1 FROM sys.sysobjects WHERE [name] = 'GetSupplierBySupplierCode' AND [type] = 'P')
BEGIN
	DROP PROCEDURE GetSupplierBySupplierCode
END 
GO

CREATE PROCEDURE GetSupplierBySupplierCode
	-- Add the parameters for the stored procedure here
	@SupplierCode nchar(64)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT [ID],RTRIM([SupplierCode]),RTRIM([Name]) FROM Supplier
	WHERE [SupplierCode] = @SupplierCode 
END
GO
