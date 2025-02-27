IF EXISTS (SELECT * FROM sys.objects WHERE [name] = 'PurchaseOrderItem' and [type] = 'U')
BEGIN
	DROP TABLE
		[dbo].[PurchaseOrderItem]
END

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PurchaseOrderItem](
	[ID] [uniqueidentifier] NOT NULL,
	[PurchaseOrderID] [uniqueidentifier] NOT NULL,
	[ProductID] [uniqueidentifier] NOT NULL,
	[Quantity] [int] NOT NULL,
	[AddedDate] [datetime2](7) NOT NULL
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[Supplier] ADD  CONSTRAINT [DF_Supplier_ID]  DEFAULT (newid()) FOR [ID]
GO
ALTER TABLE [dbo].[Supplier] ADD  CONSTRAINT [DF_Supplier_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Supplier] ADD CONSTRAINT [DF_Supplier_AddedDate] DEFAULT (GETDATE()) FOR [AddedDate]
GO