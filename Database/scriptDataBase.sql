USE master
GO

CREATE DATABASE [Online_Banking_DB]
GO

USE [Online_Banking_DB]
GO
/****** Object:  Table [dbo].[Admintb]    Script Date: 17-Apr-19 5:35:29 PM ******/

CREATE TABLE [dbo].[Admintb](
	[AdminId] [int] IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[LoginName] [varchar](255) NOT NULL,
	[LoginPassword] [varchar](255) NOT NULL,
)
GO

CREATE TABLE [dbo].[Customer](
	[CustomerId] [int] IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[FirstName] [varchar](20) NOT NULL,
	[LastName] [varchar](40) NOT NULL,
	[LoginPassword] [varchar](255) NOT NULL,
	[TransactionPassword] [varchar](255) NOT NULL,
	[Email] [varchar](255) NOT NULL,
	[Address] [varchar](255) NOT NULL,
	[PhoneNumber] [varchar](15) NOT NULL,
	[Gender] [bit] NOT NULL,
	[LockedStatus] [bit] NOT NULL ,
	[CreateDate] [datetime] NULL,
) 

GO


CREATE TABLE [dbo].[BankAccount](
	[AccountNumber] [varchar](30)  PRIMARY KEY NOT NULL,
	[CustomerId] [int] NOT NULL,
	[Balance] [decimal](18, 3) NOT NULL,
	[CreateDate] [datetime] NULL,
 )
 GO
 

CREATE TABLE [dbo].[Cheque](
	[ChequeID] [int] IDENTITY(1,1)  PRIMARY KEY NOT NULL,
	[AccountNumber] [varchar](30) NOT NULL,
	[IssuedDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
	[NumberOfChequeBook] [smallint] NOT NULL,
	[Status] [bit] NULL,
	
) 

GO

CREATE TABLE [dbo].[FAQ](
	[Id] [int] IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[Question] [text] NOT NULL,
	[Answer] [text] NOT NULL,
 )
GO

CREATE TABLE [dbo].[Transaction](
	[TransactionId] [int] IDENTITY(1,1) NOT NULL,
	[CustomerId] [int] NOT NULL,
	[SourceAccountNumber] [varchar](30) NOT NULL,
	[TargetAccountNumber] [varchar](30) NOT NULL,
	[Date] [datetime] NOT NULL,
	[Description] [text] NULL,
	[SendReceiveStatus] [bit] NOT NULL,
	[Amount] [decimal](18, 3) NOT NULL,
	[Balance] [decimal](18, 3) NULL,
 )
GO

ALTER TABLE dbo.BankAccount
ADD FOREIGN KEY (CustomerId) REFERENCES dbo.Customer(CustomerId);


ALTER TABLE dbo.[Transaction]
ADD FOREIGN KEY (CustomerId) REFERENCES dbo.Customer(CustomerId);



ALTER TABLE dbo.Cheque
ADD FOREIGN KEY (AccountNumber) REFERENCES dbo.BankAccount(AccountNumber);


INSERT INTO dbo.Admintb
        ( LoginName, LoginPassword )
VALUES  ( 'admin', -- LoginName - varchar(255)
          '21232f297a57a5a743894a0e4a801fc3'  -- LoginPassword - varchar(255)
          )
GO
          
INSERT INTO dbo.Customer
        ( FirstName ,
          LastName ,
          LoginPassword ,
          TransactionPassword ,
          Email ,
          Address ,
          PhoneNumber ,
          Gender ,
          LockedStatus ,
          CreateDate
        )
VALUES  ( 'David' , -- FirstName - varchar(20)
          'Trump' , -- LastName - varchar(40)
          '827ccb0eea8a706c4c34a16891f84e7b' , -- LoginPassword - varchar(255)
          '827ccb0eea8a706c4c34a16891f84e7b' , -- TransactionPassword - varchar(255)
          'nhandang714@gmail.com' , -- Email - varchar(255)
          'Ho Chi Minh City , Viet Nam' , -- Address - varchar(255)
          '0372595797' , -- PhoneNumber - varchar(15)
          1 , -- Gender - bit
          1 , -- LockedStatus - bit
          GETDATE()  -- CreateDate - datetime
        )
GO

INSERT INTO dbo.Customer
        ( FirstName ,
          LastName ,
          LoginPassword ,
          TransactionPassword ,
          Email ,
          Address ,
          PhoneNumber ,
          Gender ,
          LockedStatus ,
          CreateDate
        )
VALUES  ( 'Johnvin' , -- FirstName - varchar(20)
          'Duck' , -- LastName - varchar(40)
          '827ccb0eea8a706c4c34a16891f84e7b' , -- LoginPassword - varchar(255)
          '827ccb0eea8a706c4c34a16891f84e7b' , -- TransactionPassword - varchar(255)
          'nhandang797@gmail.com' , -- Email - varchar(255)
          'Ho Chi Minh City , Viet Nam' , -- Address - varchar(255)
          '0372595797' , -- PhoneNumber - varchar(15)
          1 , -- Gender - bit
          1 , -- LockedStatus - bit
          GETDATE()  -- CreateDate - datetime
        )
GO

INSERT INTO dbo.BankAccount
        ( AccountNumber ,
          CustomerId ,
          Balance ,
          CreateDate
        )
VALUES  ( 'OB-1236547890' , -- AccountNumber - varchar(30)
          1 , -- CustomerId - int
          10 , -- Balance - decimal
          GETDATE()  -- CreateDate - datetime
        )

		
		
INSERT INTO dbo.BankAccount
        ( AccountNumber ,
          CustomerId ,
          Balance ,
          CreateDate
        )
VALUES  ( 'OB-128517803' , -- AccountNumber - varchar(30)
          1 , -- CustomerId - int
          10 , -- Balance - decimal
          GETDATE()  -- CreateDate - datetime
        )

	
INSERT INTO dbo.BankAccount
        ( AccountNumber ,
          CustomerId ,
          Balance ,
          CreateDate
        )
VALUES  ( 'OB-852114033' , -- AccountNumber - varchar(30)
          1 , -- CustomerId - int
          50 , -- Balance - decimal
          GETDATE()  -- CreateDate - datetime
        )
GO
        
		INSERT INTO dbo.BankAccount
        ( AccountNumber ,
          CustomerId ,
          Balance ,
          CreateDate
        )
VALUES  ( 'OB-880221360' , -- AccountNumber - varchar(30)
          2 , -- CustomerId - int
          100 , -- Balance - decimal
          GETDATE()  -- CreateDate - datetime
        )
GO
        
-- insert FAQs
INSERT INTO dbo.FAQ
        ( Question, Answer )
VALUES  ( 'What is OnlineBanking?', -- Question - text
          'OnlineBanking is a smart mobile banking service that allows individual customers to conduct financial, non-financial  transactions and other advanced features provided by BIDV.'  -- Answer - text
          )

		  INSERT INTO dbo.FAQ
        ( Question, Answer )
VALUES  ( 'Features of OnlineBanking', -- Question - text
          '- Domestic money transfer <br/>
			- Transfer to charitable funds'  -- Answer - text
          )

INSERT INTO dbo.FAQ
        ( Question, Answer )
VALUES  ( 'How can I register to use OnlineBanking?', -- Question - text
          'You can register for OnlineBanking service in the branches/transaction offices.'  -- Answer - text
          )

INSERT INTO dbo.FAQ
        ( Question, Answer )
VALUES  ( 'Register online function is locked when I am registering online for OnlineBanking?', -- Question - text
          'When registering online for OnlineBanking through the app, when entering Ebanking service information.'  -- Answer - text
          )

INSERT INTO dbo.FAQ
        ( Question, Answer )
VALUES  ( 'Can I use OnlineBanking overseas?', -- Question - text
          'OnlineBanking can be used overseas in case the phone number receiving OTP registers for international roaming service.'  -- Answer - text
          )
