IF EXISTS (SELECT * FROM sys.objects WHERE [name] = 'PurchaseOrder' and [type] = 'U')
BEGIN
	DROP TABLE
		[dbo].PurchaseOrder
END

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PurchaseOrder](
	[ID] [uniqueidentifier] NOT NULL,
	[SupplierID] [uniqueidentifier] NOT NULL,
	[Number] [int] NOT NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
	[FinalizedDate] [datetime2](7) NULL,
	[IsFinalized] [bit] NOT NULL
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[PurchaseOrder] ADD  CONSTRAINT [DF_PurchaseOrder_ID]  DEFAULT (newid()) FOR [ID]
GO
ALTER TABLE [dbo].[PurchaseOrder] ADD  CONSTRAINT [DF_PurchaseOrder_IsFinalized]  DEFAULT ((0)) FOR [IsFinalized]
GO
ALTER TABLE [dbo].[PurchaseOrder] ADD CONSTRAINT [DF_PurchaseOrder_CreatedDate] DEFAULT (GetDate()) FOR [CreatedDate]
GO