IF EXISTS (SELECT * FROM sys.objects WHERE [name] = 'Supplier' and [type] = 'U')
BEGIN
	DROP TABLE
		[dbo].Supplier
END
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Supplier](
	[ID] [uniqueidentifier] NOT NULL,
	[SupplierCode] [nchar](64) NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
	[UpdatedDate] [datetime2](7) NULL,
	[IsDeleted] [bit] NOT NULL
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[Supplier] ADD  CONSTRAINT [DF_Supplier_ID]  DEFAULT (newid()) FOR [ID]
GO
ALTER TABLE [dbo].[Supplier] ADD  CONSTRAINT [DF_Supplier_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
