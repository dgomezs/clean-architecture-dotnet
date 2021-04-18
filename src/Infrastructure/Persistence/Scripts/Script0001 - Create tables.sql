IF NOT EXISTS(SELECT *
              FROM sys.schemas
              WHERE name = N'todos')
    EXEC ('CREATE SCHEMA [todos] AUTHORIZATION [dbo]');
GO

-- TODO add audit trail


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
    [InternalId] BIGINT PRIMARY KEY IDENTITY (1,1),
    [Id]         UNIQUEIDENTIFIER NOT NULL,
    [Name]       NVARCHAR(50)     NOT NULL,
    [CreatedAt]  DATETIME2 DEFAULT SYSUTCDATETIME(),
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
    CONSTRAINT FK_Todo_TodoList_TodoList_InternalId FOREIGN KEY (TodoListId) REFERENCES [todos].[TodoList] (InternalId)
)
GO

