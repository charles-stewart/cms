﻿CREATE TABLE [dbo].[Provinces] (
	[Id] UNIQUEIDENTIFIER NOT NULL, 
    [RefId]           BIGINT               IDENTITY (1, 1) NOT NULL,
    [Name]         VARCHAR (200)     NOT NULL,
    [DateModified] DATETIME          CONSTRAINT [DF_Provinces_DateModified] DEFAULT (getdate()) NOT NULL,
    [DateCreated]  DATETIME          CONSTRAINT [DF_Provinces_DateCreated] DEFAULT (getdate()) NOT NULL,
    [UserCreated]  VARCHAR (500)     NOT NULL,
    [UserModified] VARCHAR (500)     NOT NULL,
    [CenterPoint]  [sys].[geography] NULL,
   
    CONSTRAINT [PK_Provinces] PRIMARY KEY CLUSTERED ([RefId] ASC)
);

