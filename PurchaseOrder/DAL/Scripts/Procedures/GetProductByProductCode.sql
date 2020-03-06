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
IF  EXISTS (SELECT 1 FROM sys.objects WHERE [name] = 'GetProductByProductCode' AND [type] = 'P')
BEGIN
	DROP PROCEDURE GetProductByProductCode
END
GO
CREATE PROCEDURE GetProductByProductCode
	-- Add the parameters for the stored procedure here
	@ProductCode nchar(64),
	@IsDeleted bit = NULL,
	@FetchAll bit = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

	-- if fetch all flag is triggered ignore IsDeleted toggle
	IF @FetchAll = 1 
		SELECT [ID], [ProductCode], [Description], [Price]
		FROM Product
		WHERE [ProductCode] = @ProductCode
	ELSE
		SELECT [ID], [ProductCode], [Description], [Price]
		FROM Product
		WHERE [ProductCode] = @ProductCode
		AND ([IsDeleted] = @IsDeleted OR (@IsDeleted IS NULL AND [IsDeleted] = 0))
END
GO
