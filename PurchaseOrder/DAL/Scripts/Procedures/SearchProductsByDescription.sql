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
-- Create date: 06 March 2020
-- Description:	Searches for products by description
-- =============================================

IF EXISTS (SELECT (1) FROM sys.sysobjects WHERE [Name] = 'SearchProductsByDescription' AND [type] = 'P')
BEGIN
	DROP PROCEDURE SearchProductsByDescription
END
GO

CREATE PROCEDURE SearchProductsByDescription
	-- Add the parameters for the stored procedure here
	@searchDescription nvarchar(256),
	@isDeleted BIT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT [ID], [ProductCode], [Description], [Price]
	FROM [Product]
	WHERE [Description] LIKE '%' + @searchDescription + '%'
	AND [IsDeleted] = @isDeleted
END
GO
