/*    ==Scripting Parameters==

    Source Server Version : SQL Server 2016 (13.0.4001)
    Source Database Engine Edition : Microsoft SQL Server Express Edition
    Source Database Engine Type : Standalone SQL Server

    Target Server Version : SQL Server 2016
    Target Database Engine Edition : Microsoft SQL Server Express Edition
    Target Database Engine Type : Standalone SQL Server
*/

USE [master]
GO

/****** Object:  Database [TalkBackDB]    Script Date: 11-Jan-18 23:55:29 ******/
if db_id(N'TalkBackDB') is not null DROP DATABASE [TalkBackDB]
GO

/****** Object:  Database [TalkBackDB]    Script Date: 11-Jan-18 23:55:29 ******/
CREATE DATABASE [TalkBackDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'TalkBackDB1', FILENAME = N'C:\Users\Public\TalkBackDB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'TalkBackDB1_log', FILENAME = N'C:\Users\Public\TalkBackDB_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO

ALTER DATABASE [TalkBackDB] SET COMPATIBILITY_LEVEL = 130
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [TalkBackDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [TalkBackDB] SET ANSI_NULL_DEFAULT OFF 
GO

ALTER DATABASE [TalkBackDB] SET ANSI_NULLS OFF 
GO

ALTER DATABASE [TalkBackDB] SET ANSI_PADDING OFF 
GO

ALTER DATABASE [TalkBackDB] SET ANSI_WARNINGS OFF 
GO

ALTER DATABASE [TalkBackDB] SET ARITHABORT OFF 
GO

ALTER DATABASE [TalkBackDB] SET AUTO_CLOSE OFF 
GO

ALTER DATABASE [TalkBackDB] SET AUTO_SHRINK OFF 
GO

ALTER DATABASE [TalkBackDB] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [TalkBackDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [TalkBackDB] SET CURSOR_DEFAULT  GLOBAL 
GO

ALTER DATABASE [TalkBackDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO

ALTER DATABASE [TalkBackDB] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [TalkBackDB] SET QUOTED_IDENTIFIER OFF 
GO

ALTER DATABASE [TalkBackDB] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [TalkBackDB] SET  DISABLE_BROKER 
GO

ALTER DATABASE [TalkBackDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [TalkBackDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO

ALTER DATABASE [TalkBackDB] SET TRUSTWORTHY OFF 
GO

ALTER DATABASE [TalkBackDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO

ALTER DATABASE [TalkBackDB] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [TalkBackDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO

ALTER DATABASE [TalkBackDB] SET HONOR_BROKER_PRIORITY OFF 
GO

ALTER DATABASE [TalkBackDB] SET RECOVERY SIMPLE 
GO

ALTER DATABASE [TalkBackDB] SET  MULTI_USER 
GO

ALTER DATABASE [TalkBackDB] SET PAGE_VERIFY CHECKSUM  
GO

ALTER DATABASE [TalkBackDB] SET DB_CHAINING OFF 
GO

ALTER DATABASE [TalkBackDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO

ALTER DATABASE [TalkBackDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO

ALTER DATABASE [TalkBackDB] SET DELAYED_DURABILITY = DISABLED 
GO

ALTER DATABASE [TalkBackDB] SET QUERY_STORE = OFF
GO

USE [TalkBackDB]
GO

ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO

ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET LEGACY_CARDINALITY_ESTIMATION = PRIMARY;
GO

ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO

ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET MAXDOP = PRIMARY;
GO

ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO

ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET PARAMETER_SNIFFING = PRIMARY;
GO

ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO

ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET QUERY_OPTIMIZER_HOTFIXES = PRIMARY;
GO

ALTER DATABASE [TalkBackDB] SET  READ_WRITE 
GO

/*    ==Scripting Parameters==

    Source Server Version : SQL Server 2016 (13.0.4001)
    Source Database Engine Edition : Microsoft SQL Server Express Edition
    Source Database Engine Type : Standalone SQL Server

    Target Server Version : SQL Server 2016
    Target Database Engine Edition : Microsoft SQL Server Express Edition
    Target Database Engine Type : Standalone SQL Server
*/

USE [TalkBackDB]
GO

/****** Object:  Table [dbo].[Message]    Script Date: 11-Jan-18 23:58:01 ******/
DROP TABLE [dbo].[Message]
GO

/****** Object:  Table [dbo].[Message]    Script Date: 11-Jan-18 23:58:01 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Message](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Content] [nvarchar](400) NOT NULL,
	[Time] [datetime] NOT NULL,
	[SenderName] [nvarchar](20) NOT NULL,
	[ReceiverName] [nvarchar](20) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/*    ==Scripting Parameters==

    Source Server Version : SQL Server 2016 (13.0.4001)
    Source Database Engine Edition : Microsoft SQL Server Express Edition
    Source Database Engine Type : Standalone SQL Server

    Target Server Version : SQL Server 2016
    Target Database Engine Edition : Microsoft SQL Server Express Edition
    Target Database Engine Type : Standalone SQL Server
*/

USE [TalkBackDB]
GO

/****** Object:  Table [dbo].[User]    Script Date: 11-Jan-18 23:58:23 ******/
DROP TABLE [dbo].[User]
GO

/****** Object:  Table [dbo].[User]    Script Date: 11-Jan-18 23:58:23 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[User](
	[Name] [nvarchar](20) NOT NULL,
	[Password] [nvarchar](20) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
