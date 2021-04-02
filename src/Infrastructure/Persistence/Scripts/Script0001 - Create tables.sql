IF NOT EXISTS(SELECT *
              FROM sys.schemas
              WHERE name = N'todos')
    EXEC ('CREATE SCHEMA [todos] AUTHORIZATION [dbo]');
GO

CREATE TABLE [todos].[TodoList]
(
    [InternalId]           BIGINT PRIMARY KEY IDENTITY (1,1),
    [Id]           UNIQUEIDENTIFIER NOT NULL,
    [Name]    NVARCHAR(50) NOT NULL
)
GO 