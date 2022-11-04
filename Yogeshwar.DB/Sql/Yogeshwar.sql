CREATE DATABASE [Yogeshwar]
GO

USE [Yogeshwar]
GO

CREATE TABLE [User]
(
	Id int not null primary key identity (1, 1),
	Name varchar(100) not null,
	Email varchar(100),
	Username varchar(25) not null,
	PhoneNo varchar(12) not null,
	[Password] varchar(250) not null,
	[UserType] tinyint not null,
	CreatedDate datetime2(2) not null default (GETDATE())
)
GO

CREATE TABLE [Customer]
(
	Id int not null primary key identity(1, 1),
	FirstName varchar(50) not null,
	LastName varchar(50) not null,
	Email varchar(100),
	PhoneNo varchar(12) not null,
	[Address] varchar(250) not null,
	City varchar(25) not null,
	Pincode int not null,
	CreatedDate datetime2(2) not null default (GETDATE()),
	CreatedBy int not null,
	ModifiedDate datetime2(2),
	ModifiedBy int
)
GO

CREATE TABLE [Product]
(
	Id int not null primary key identity(1, 1),
	[Name] varchar(100) not null,
	[Description] nvarchar(250) not null,
	Price decimal(10, 2) not null,
	[ModelNo] varchar(50) not null,
	[Video] nvarchar(50),
	CreatedDate datetime2(2) not null default (GETDATE())
)
GO

CREATE TABLE [Accessories]
(
	Id int not null primary key identity(1, 1),
	[Name] varchar(100) not null,
	[Description] varchar(250),
	[Image] varchar(50),
	Quantity int not null
)
GO

CREATE TABLE [ProductImages]
(
	Id int not null primary key identity(1, 1),
	ProductId int not null constraint fk_productimages_product_id foreign key references [Product](Id),
	[Image] varchar(50) not null
)
GO

CREATE TABLE [ProductAccessories]
(
	Id int not null primary key identity(1, 1),
	ProductId int not null constraint fk_productaccessories_product_id foreign key references [Product](Id),
	AccessoriesId int not null constraint fk_productaccessories_accessories_id foreign key references [Accessories](Id),
	Quantity int not null
)
GO

CREATE TABLE [Order]
(
	Id int not null primary key identity(1, 1),
	CustomerId int not null constraint fk_order_customer_id foreign key references [Customer](Id),
	Discount decimal(10, 2),
	OrderDate datetime2(4) not null,
	IsCompleted bit not null
)
GO

CREATE TABLE [OrderDetail]
(
	Id int not null primary key identity(1, 1),
	OrderId int not null constraint fk_orderdetail_order_id foreign key references [Order](Id),
	ProductId int not null constraint fk_orderdetail_product_id foreign key references [Product](Id),
	Quantity int not null,
	Amount decimal(10, 2) not null,
	ReceiveDate datetime2(4),
	[Status] tinyint not null
)
GO

CREATE TABLE [Notification]
(
	Id int not null primary key identity(1, 1),
	ProductAccessoriesId int not null constraint fk_notification_productaccessories_id foreign key references [ProductAccessories](Id),
	OrderId int not null constraint fk_notification_order_id foreign key references [Order](Id),
	IsCompleted bit not null,
	[Date] datetime2(3) not null
)
GO

CREATE TABLE [CustomerService]
(
	Id int not null primary key identity(1, 1),
	WorkerName varchar(100) not null,
	OrderId int not null constraint fk_customerservice_order_id foreign key references [Order](Id),
	[Description] varchar(250),
	[ComplainDate] datetime2(2) not null,
	[Status] tinyint not null,
	[ServiceCompletedDate] datetime2(2)
)
GO