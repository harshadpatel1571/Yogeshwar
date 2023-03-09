CREATE DATABASE [Yogeshwar]
GO
    
USE [Yogeshwar]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Accessories](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [Name] [nvarchar](100) NOT NULL,
    [Description] [nvarchar](max) NULL,
    [Image] [varchar](50) NULL,
    [Quantity] [int] NOT NULL,
    [CreatedDate] [datetime2](0) NOT NULL,
    [CreatedBy] [int] NOT NULL,
    [ModifiedDate] [datetime2](0) NULL,
    [ModifiedBy] [int] NULL,
    [IsActive] [bit] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    CONSTRAINT [PK__Accessor__3214EC0762AD5F61] PRIMARY KEY CLUSTERED
(
[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
    ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
    GO
    SET ANSI_NULLS ON
    GO
    SET QUOTED_IDENTIFIER ON
    GO
CREATE TABLE [dbo].[Category](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [Name] [nvarchar](50) NOT NULL,
    [Image] [varchar](50) NULL,
    [IsDeleted] [bit] NOT NULL,
    CONSTRAINT [PK_Category] PRIMARY KEY CLUSTERED
(
[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
    ) ON [PRIMARY]
    GO
    SET ANSI_NULLS ON
    GO
    SET QUOTED_IDENTIFIER ON
    GO
CREATE TABLE [dbo].[Customer](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [FirstName] [nvarchar](25) NOT NULL,
    [LastName] [nvarchar](25) NOT NULL,
    [Email] [varchar](50) NULL,
    [PhoneNo] [varchar](12) NOT NULL,
    [Address] [nvarchar](250) NOT NULL,
    [City] [varchar](25) NOT NULL,
    [Pincode] [int] NOT NULL,
    [CreatedDate] [datetime2](0) NOT NULL,
    [CreatedBy] [int] NULL,
    [ModifiedDate] [datetime2](0) NULL,
    [ModifiedBy] [int] NULL,
    [IsActive] [bit] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    CONSTRAINT [PK__Customer__3214EC07A6E7EE92] PRIMARY KEY CLUSTERED
(
[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
    ) ON [PRIMARY]
    GO
    SET ANSI_NULLS ON
    GO
    SET QUOTED_IDENTIFIER ON
    GO
CREATE TABLE [dbo].[CustomerService](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [WorkerName] [nvarchar](50) NOT NULL,
    [OrderId] [int] NOT NULL,
    [Description] [nvarchar](max) NULL,
    [ComplainDate] [datetime2](0) NOT NULL,
    [Status] [tinyint] NOT NULL,
    [ServiceCompletionDate] [datetime2](0) NULL,
    [IsDeleted] [bit] NOT NULL,
    CONSTRAINT [PK__Customer__3214EC0743B23ADF] PRIMARY KEY CLUSTERED
(
[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
    ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
    GO
    SET ANSI_NULLS ON
    GO
    SET QUOTED_IDENTIFIER ON
    GO
CREATE TABLE [dbo].[Notification](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [ProductAccessoriesId] [int] NOT NULL,
    [OrderId] [int] NOT NULL,
    [IsCompleted] [bit] NOT NULL,
    [Date] [datetime2](0) NOT NULL,
    CONSTRAINT [PK__Notifica__3214EC07EEE41DF9] PRIMARY KEY CLUSTERED
(
[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
    ) ON [PRIMARY]
    GO
    SET ANSI_NULLS ON
    GO
    SET QUOTED_IDENTIFIER ON
    GO
CREATE TABLE [dbo].[Order](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [CustomerId] [int] NOT NULL,
    [Discount] [decimal](10, 2) NULL,
    [OrderDate] [datetime2](0) NOT NULL,
    [IsCompleted] [bit] NOT NULL,
    [IsDeleted] [bit] NOT NULL,
    [CreatedDate] [datetime2](0) NOT NULL,
    [CreatedBy] [int] NULL,
    [ModifiedDate] [datetime2](0) NULL,
    [ModifiedBy] [int] NULL,
    CONSTRAINT [PK__Order__3214EC0751AAEB3F] PRIMARY KEY CLUSTERED
(
[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
    ) ON [PRIMARY]
    GO
    SET ANSI_NULLS ON
    GO
    SET QUOTED_IDENTIFIER ON
    GO
CREATE TABLE [dbo].[OrderDetail](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [OrderId] [int] NOT NULL,
    [ProductId] [int] NOT NULL,
    [Quantity] [int] NOT NULL,
    [Amount] [decimal](10, 2) NOT NULL,
    [ReceiveDate] [datetime2](0) NULL,
    [Status] [tinyint] NOT NULL,
    CONSTRAINT [PK__OrderDet__3214EC071C9029AE] PRIMARY KEY CLUSTERED
(
[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
    ) ON [PRIMARY]
    GO
    SET ANSI_NULLS ON
    GO
    SET QUOTED_IDENTIFIER ON
    GO
CREATE TABLE [dbo].[Product](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [Name] [nvarchar](100) NOT NULL,
    [Description] [nvarchar](max) NOT NULL,
    [Price] [decimal](10, 2) NOT NULL,
    [ModelNo] [nvarchar](50) NOT NULL,
    [Video] [varchar](50) NULL,
    [CreatedDate] [datetime2](0) NOT NULL,
    [CreatedBy] [int] NOT NULL,
    [ModifiedDate] [datetime2](0) NULL,
    [ModifiedBy] [int] NULL,
    CONSTRAINT [PK__Product__3214EC070625F62F] PRIMARY KEY CLUSTERED
(
[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
    ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
    GO
    SET ANSI_NULLS ON
    GO
    SET QUOTED_IDENTIFIER ON
    GO
CREATE TABLE [dbo].[ProductAccessories](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [ProductId] [int] NOT NULL,
    [AccessoriesId] [int] NOT NULL,
    [Quantity] [int] NOT NULL,
    PRIMARY KEY CLUSTERED
(
[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
    ) ON [PRIMARY]
    GO
    SET ANSI_NULLS ON
    GO
    SET QUOTED_IDENTIFIER ON
    GO
CREATE TABLE [dbo].[ProductCategories](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [ProductId] [int] NOT NULL,
    [CategoryId] [int] NOT NULL,
    CONSTRAINT [PK_ProductCategory] PRIMARY KEY CLUSTERED
(
[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
    ) ON [PRIMARY]
    GO
    SET ANSI_NULLS ON
    GO
    SET QUOTED_IDENTIFIER ON
    GO
CREATE TABLE [dbo].[ProductImages](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [ProductId] [int] NOT NULL,
    [Image] [varchar](50) NOT NULL,
    PRIMARY KEY CLUSTERED
(
[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
    ) ON [PRIMARY]
    GO
    SET ANSI_NULLS ON
    GO
    SET QUOTED_IDENTIFIER ON
    GO
CREATE TABLE [dbo].[User](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [Name] [nvarchar](50) NOT NULL,
    [Email] [varchar](50) NULL,
    [Username] [varchar](25) NOT NULL,
    [PhoneNo] [varchar](12) NOT NULL,
    [Password] [varchar](250) NOT NULL,
    [UserType] [tinyint] NOT NULL,
    [CreatedDate] [datetime2](0) NOT NULL,
    [ModifiedDate] [datetime2](0) NULL,
    CONSTRAINT [PK__User__3214EC07244638E6] PRIMARY KEY CLUSTERED
(
[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
    ) ON [PRIMARY]
    GO
ALTER TABLE [dbo].[Customer] ADD  CONSTRAINT [DF__Customer__Create__47DBAE45]  DEFAULT (getdate()) FOR [CreatedDate]
    GO
ALTER TABLE [dbo].[Product] ADD  CONSTRAINT [DF__Product__Created__48CFD27E]  DEFAULT (getdate()) FOR [CreatedDate]
    GO
ALTER TABLE [dbo].[User] ADD  CONSTRAINT [DF__User__CreatedDat__49C3F6B7]  DEFAULT (getdate()) FOR [CreatedDate]
    GO
ALTER TABLE [dbo].[CustomerService]  WITH CHECK ADD  CONSTRAINT [fk_customerservice_order_id] FOREIGN KEY([OrderId])
    REFERENCES [dbo].[Order] ([Id])
    GO
ALTER TABLE [dbo].[CustomerService] CHECK CONSTRAINT [fk_customerservice_order_id]
    GO
ALTER TABLE [dbo].[Notification]  WITH CHECK ADD  CONSTRAINT [fk_notification_order_id] FOREIGN KEY([OrderId])
    REFERENCES [dbo].[Order] ([Id])
    GO
ALTER TABLE [dbo].[Notification] CHECK CONSTRAINT [fk_notification_order_id]
    GO
ALTER TABLE [dbo].[Notification]  WITH CHECK ADD  CONSTRAINT [fk_notification_productaccessories_id] FOREIGN KEY([ProductAccessoriesId])
    REFERENCES [dbo].[ProductAccessories] ([Id])
    GO
ALTER TABLE [dbo].[Notification] CHECK CONSTRAINT [fk_notification_productaccessories_id]
    GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [fk_order_customer_id] FOREIGN KEY([CustomerId])
    REFERENCES [dbo].[Customer] ([Id])
    GO
ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [fk_order_customer_id]
    GO
ALTER TABLE [dbo].[OrderDetail]  WITH CHECK ADD  CONSTRAINT [fk_orderdetail_order_id] FOREIGN KEY([OrderId])
    REFERENCES [dbo].[Order] ([Id])
    GO
ALTER TABLE [dbo].[OrderDetail] CHECK CONSTRAINT [fk_orderdetail_order_id]
    GO
ALTER TABLE [dbo].[OrderDetail]  WITH CHECK ADD  CONSTRAINT [fk_orderdetail_product_id] FOREIGN KEY([ProductId])
    REFERENCES [dbo].[Product] ([Id])
    GO
ALTER TABLE [dbo].[OrderDetail] CHECK CONSTRAINT [fk_orderdetail_product_id]
    GO
ALTER TABLE [dbo].[ProductAccessories]  WITH CHECK ADD  CONSTRAINT [fk_productaccessories_accessories_id] FOREIGN KEY([AccessoriesId])
    REFERENCES [dbo].[Accessories] ([Id])
    GO
ALTER TABLE [dbo].[ProductAccessories] CHECK CONSTRAINT [fk_productaccessories_accessories_id]
    GO
ALTER TABLE [dbo].[ProductAccessories]  WITH CHECK ADD  CONSTRAINT [fk_productaccessories_product_id] FOREIGN KEY([ProductId])
    REFERENCES [dbo].[Product] ([Id])
    GO
ALTER TABLE [dbo].[ProductAccessories] CHECK CONSTRAINT [fk_productaccessories_product_id]
    GO
ALTER TABLE [dbo].[ProductCategories]  WITH CHECK ADD  CONSTRAINT [FK_ProductCategories_Category] FOREIGN KEY([CategoryId])
    REFERENCES [dbo].[Category] ([Id])
    GO
ALTER TABLE [dbo].[ProductCategories] CHECK CONSTRAINT [FK_ProductCategories_Category]
    GO
ALTER TABLE [dbo].[ProductCategories]  WITH CHECK ADD  CONSTRAINT [FK_ProductCategories_Product] FOREIGN KEY([ProductId])
    REFERENCES [dbo].[Product] ([Id])
    GO
ALTER TABLE [dbo].[ProductCategories] CHECK CONSTRAINT [FK_ProductCategories_Product]
    GO
ALTER TABLE [dbo].[ProductImages]  WITH CHECK ADD  CONSTRAINT [fk_productimages_product_id] FOREIGN KEY([ProductId])
    REFERENCES [dbo].[Product] ([Id])
    GO
ALTER TABLE [dbo].[ProductImages] CHECK CONSTRAINT [fk_productimages_product_id]
    GO
    USE [master]
    GO
ALTER DATABASE [Yogeshwar] SET  READ_WRITE 
GO
