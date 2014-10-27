USE [whenisiton]
GO

/****** Object: Table [dbo].[Programme] Script Date: 03/09/2014 20:14:37 ******/
SET ANSI_NULLS OFF
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Programme] (
    [ID]        INT             IDENTITY (1, 1) NOT NULL,
    [Title]     NVARCHAR (500)  NOT NULL,
    [Episode]   NVARCHAR (100)  NULL,
    [Year]      NVARCHAR (4)    NULL,
    [Film]      BIT             NULL,
    [Genre]     NVARCHAR (2000) NULL,
    [StartTime] TIME (7)        NOT NULL,
    [EndTime]   TIME (7)        NOT NULL,
    [Duration]  INT             NOT NULL,
    [Date]      DATE            NOT NULL,
    [ChannelId] INT             NOT NULL
);


