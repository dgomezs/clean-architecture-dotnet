IF NOT EXISTS(SELECT *
              FROM sys.schemas
              WHERE name = N'todos')
    EXEC ('CREATE SCHEMA [todos] AUTHORIZATION [dbo]');
GO

CREATE TABLE [todos].[TodoList]
(
    [InternalId]           BIGINT PRIMARY KEY IDENTITY (1,1),
    [Id]           UNIQUEIDENTIFIER NOT NULL,
    [Name]    NVARCHAR(10) NOT NULL
)
GO
-- CREATE TABLE [booking].[Booking]
-- (
--     [ID]         BIGINT PRIMARY KEY IDENTITY (1,1),
--     [CustomerId] BIGINT       NOT NULL,
--     [BookingRef] NVARCHAR(6)  NOT NULL,
--     [Status]     NVARCHAR(50) NOT NULL,
--     [Amount]     MONEY        NOT NULL,
--     CONSTRAINT FK_Booking_CustomerId_Customer_Id FOREIGN KEY (CustomerId) REFERENCES [booking].Customer (ID)
-- )
-- GO
-- CREATE TABLE [booking].[Payment]
-- (
--     [Id]           UNIQUEIDENTIFIER PRIMARY KEY,
--     [BookingId]    BIGINT         NOT NULL,
--     [Type]         NVARCHAR(50)   NOT NULL,
--     [Status]       NVARCHAR(50)   NOT NULL,
--     [Amount]       MONEY          NOT NULL,
--     [CreatedAt]    DATETIME       NOT NULL,
--     [SucceededAt]  DATETIME       NULL,
--     [FailedAt]     DATETIME       NULL,
--     [FailedReason] NVARCHAR(1000) NULL,
--     CONSTRAINT FK_Payment_BookingId_Booking_Id FOREIGN KEY (BookingId) REFERENCES [booking].Booking (ID)
--         ON DELETE CASCADE
-- )
-- GO
