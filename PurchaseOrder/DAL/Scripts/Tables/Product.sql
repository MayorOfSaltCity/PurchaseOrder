IF EXISTS (SELECT * FROM sys.objects WHERE [name] = 'Product' and [type] = 'U')
BEGIN
	DROP TABLE
		[dbo].Product
END

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Product](
	[ID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[ProductCode] [nchar](64) NOT NULL,
	[SupplierId] [uniqueidentifier] NOT NULL,
	[Description] [nvarchar](256) NOT NULL,
	[Price] [decimal](18, 4) NOT NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
	[UpdatedDate] [datetime2](7) NULL,
	[IsDeleted] [bit] NOT NULL
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[Product] ADD  CONSTRAINT [DF_Product_ID]  DEFAULT (newid()) FOR [ID]
GO
ALTER TABLE [dbo].[Product] ADD  CONSTRAINT [DF_Product_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
