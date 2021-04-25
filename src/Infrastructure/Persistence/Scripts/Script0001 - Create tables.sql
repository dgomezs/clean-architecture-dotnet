IF NOT EXISTS(SELECT *
              FROM sys.schemas
              WHERE name = N'todos')
    EXEC ('CREATE SCHEMA [todos] AUTHORIZATION [dbo]');
GO

CREATE TABLE [todos].[User]
(
    [InternalId] BIGINT PRIMARY KEY IDENTITY (1,1),
    [Id]         UNIQUEIDENTIFIER NOT NULL,
    [Name]       NVARCHAR(MAX)    NOT NULL,
    [Email]      NVARCHAR(254)    NOT NULL,
    [CreatedAt]  DATETIME2 DEFAULT SYSUTCDATETIME(),
)
GO


CREATE TABLE [todos].[TodoList]
(
    [InternalId]      BIGINT PRIMARY KEY IDENTITY (1,1),
    [Id]              UNIQUEIDENTIFIER NOT NULL,
    [InternalOwnerId] BIGINT           NOT NULL,
    [OwnerId]         UNIQUEIDENTIFIER NOT NULL,
    [Name]            NVARCHAR(50)     NOT NULL,
    [CreatedAt]       DATETIME2 DEFAULT SYSUTCDATETIME(),
    CONSTRAINT FK_TodoList_InternalOwnerId_User_InternalId FOREIGN KEY (InternalOwnerId) REFERENCES [todos].[User] (InternalId)
)
GO

CREATE TABLE [todos].[Todo]
(
    [InternalId]  BIGINT PRIMARY KEY IDENTITY (1,1),
    [Id]          UNIQUEIDENTIFIER NOT NULL,
    [Description] NVARCHAR(255)    NOT NULL,
    [TodoListId]  BIGINT           NOT NULL,
    [Done]        BIT              NOT NULL,
    [CreatedAt]   DATETIME2 DEFAULT SYSUTCDATETIME(),
    CONSTRAINT FK_Todo_TodoListId_TodoList_InternalId FOREIGN KEY (TodoListId) REFERENCES [todos].[TodoList] (InternalId)
)
GO

