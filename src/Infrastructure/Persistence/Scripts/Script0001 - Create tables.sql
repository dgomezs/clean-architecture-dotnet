IF NOT EXISTS(SELECT *
              FROM sys.schemas
              WHERE name = N'todos')
    EXEC ('CREATE SCHEMA [todos] AUTHORIZATION [dbo]');
GO

CREATE TABLE [todos].[TodoList]
(
    [InternalId] BIGINT PRIMARY KEY IDENTITY (1,1),
    [Id]         UNIQUEIDENTIFIER NOT NULL,
    [Name]       NVARCHAR(50)     NOT NULL
)
GO

CREATE TABLE [todos].[Todo]
(
    [InternalId]  BIGINT PRIMARY KEY IDENTITY (1,1),
    [Id]          UNIQUEIDENTIFIER NOT NULL,
    [Description] NVARCHAR(255)    NOT NULL,
    [TodoListId]  BIGINT           NOT NULL,
    [Done]        BIT              NOT NULL,
    CONSTRAINT FK_Todo_TodoList_TodoList_InternalId FOREIGN KEY (TodoListId) REFERENCES [todos].[TodoList] (InternalId)
)
GO 