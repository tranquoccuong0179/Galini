USE [master]
GO
/****** Object:  Database [Harmon]    Script Date: 3/14/2025 8:54:50 PM ******/
CREATE DATABASE [Harmon]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Harmon', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\Harmon.mdf' , SIZE = 73728KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Harmon_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\Harmon_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [Harmon] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Harmon].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Harmon] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Harmon] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Harmon] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Harmon] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Harmon] SET ARITHABORT OFF 
GO
ALTER DATABASE [Harmon] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [Harmon] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Harmon] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Harmon] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Harmon] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Harmon] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Harmon] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Harmon] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Harmon] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Harmon] SET  ENABLE_BROKER 
GO
ALTER DATABASE [Harmon] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Harmon] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Harmon] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Harmon] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Harmon] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Harmon] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Harmon] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Harmon] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [Harmon] SET  MULTI_USER 
GO
ALTER DATABASE [Harmon] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Harmon] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Harmon] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Harmon] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [Harmon] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [Harmon] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [Harmon] SET QUERY_STORE = ON
GO
ALTER DATABASE [Harmon] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [Harmon]
GO
/****** Object:  Table [dbo].[Account]    Script Date: 3/14/2025 8:54:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Account](
	[Id] [uniqueidentifier] NOT NULL,
	[UserName] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](50) NOT NULL,
	[Role] [nvarchar](50) NOT NULL,
	[FullName] [nvarchar](100) NOT NULL,
	[Email] [nvarchar](100) NOT NULL,
	[Phone] [nvarchar](50) NOT NULL,
	[DateOfBirth] [datetime] NOT NULL,
	[Gender] [nvarchar](15) NOT NULL,
	[Duration] [int] NULL,
	[AvatarUrl] [nvarchar](max) NULL,
	[IsActive] [bit] NOT NULL,
	[CreateAt] [datetime] NOT NULL,
	[UpdateAt] [datetime] NOT NULL,
	[DeleteAt] [datetime] NULL,
 CONSTRAINT [PK_Account] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_Account_UserName] UNIQUE NONCLUSTERED 
(
	[UserName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Blog]    Script Date: 3/14/2025 8:54:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Blog](
	[Id] [uniqueidentifier] NOT NULL,
	[Title] [nvarchar](max) NOT NULL,
	[Content] [nvarchar](max) NOT NULL,
	[Type] [nvarchar](50) NOT NULL,
	[Views] [int] NULL,
	[Likes] [int] NULL,
	[Status] [nvarchar](50) NOT NULL,
	[Author_id ] [uniqueidentifier] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreateAt] [datetime] NOT NULL,
	[UpdateAt] [datetime] NOT NULL,
	[DeleteAt] [datetime] NULL,
 CONSTRAINT [PK_Blog] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Booking]    Script Date: 3/14/2025 8:54:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Booking](
	[Id] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[WorkShiftId] [uniqueidentifier] NOT NULL,
	[ListenerId] [uniqueidentifier] NOT NULL,
	[Date] [datetime] NOT NULL,
	[Status] [nvarchar](50) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreateAt] [datetime] NOT NULL,
	[UpdateAt] [datetime] NOT NULL,
	[DeleteAt] [datetime] NULL,
 CONSTRAINT [PK_Booking] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CallHistory]    Script Date: 3/14/2025 8:54:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CallHistory](
	[Id] [uniqueidentifier] NOT NULL,
	[TimeStart] [datetime] NOT NULL,
	[TimeEnd] [datetime] NOT NULL,
	[Duration] [int] NOT NULL,
	[IsMissCall] [bit] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreateAt] [datetime] NOT NULL,
	[UpdateAt] [datetime] NOT NULL,
	[DeleteAt] [datetime] NULL,
 CONSTRAINT [PK_CallHistory] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Deposit]    Script Date: 3/14/2025 8:54:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Deposit](
	[Id] [uniqueidentifier] NOT NULL,
	[AccountId] [uniqueidentifier] NOT NULL,
	[Code] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[Amount] [decimal](18, 0) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreateAt] [datetime] NOT NULL,
	[UpdateAt] [datetime] NOT NULL,
	[DeleteAt] [datetime] NULL,
 CONSTRAINT [PK_Deposit] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DirectChat]    Script Date: 3/14/2025 8:54:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DirectChat](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](max) NULL,
	[IsActive] [bit] NOT NULL,
	[CreateAt] [datetime] NOT NULL,
	[UpdateAt] [datetime] NOT NULL,
	[DeleteAt] [datetime] NULL,
 CONSTRAINT [PK_DirectChat] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DirectChatParticipant]    Script Date: 3/14/2025 8:54:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DirectChatParticipant](
	[Id] [uniqueidentifier] NOT NULL,
	[AccountId] [uniqueidentifier] NOT NULL,
	[DirectChatId] [uniqueidentifier] NOT NULL,
	[NickName] [nvarchar](max) NULL,
	[IsActive] [bit] NOT NULL,
	[CreateAt] [datetime] NOT NULL,
	[UpdateAt] [datetime] NOT NULL,
	[DeleteAt] [datetime] NULL,
 CONSTRAINT [PK_DirectChatParticipant] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FriendShip]    Script Date: 3/14/2025 8:54:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FriendShip](
	[Id] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[FriendId] [uniqueidentifier] NOT NULL,
	[Status] [nvarchar](50) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreateAt] [datetime] NOT NULL,
	[UpdateAt] [datetime] NOT NULL,
	[DeleteAt] [datetime] NULL,
 CONSTRAINT [PK_FriendShip] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Identification]    Script Date: 3/14/2025 8:54:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Identification](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdentificationCode] [nvarchar](50) NOT NULL,
	[Duration] [int] NOT NULL,
 CONSTRAINT [PK_Identification] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ListenerInfo]    Script Date: 3/14/2025 8:54:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ListenerInfo](
	[Id] [uniqueidentifier] NOT NULL,
	[AccountId] [uniqueidentifier] NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[Star] [float] NULL,
	[Price] [decimal](18, 0) NOT NULL,
	[Type] [nvarchar](50) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreateAt] [datetime] NOT NULL,
	[UpdateAt] [datetime] NOT NULL,
	[DeleteAt] [datetime] NULL,
 CONSTRAINT [PK_ListenerInfo] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_AccountId_ListenerInfo] UNIQUE NONCLUSTERED 
(
	[AccountId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Message]    Script Date: 3/14/2025 8:54:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Message](
	[Id] [uniqueidentifier] NOT NULL,
	[DirectChatId] [uniqueidentifier] NOT NULL,
	[SenderId] [uniqueidentifier] NOT NULL,
	[Content] [nvarchar](max) NOT NULL,
	[Type] [nvarchar](50) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreateAt] [datetime] NOT NULL,
	[UpdateAt] [datetime] NOT NULL,
	[DeleteAt] [datetime] NULL,
 CONSTRAINT [PK_Message] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Notification]    Script Date: 3/14/2025 8:54:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Notification](
	[Id] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[Type] [nvarchar](50) NOT NULL,
	[Content] [nvarchar](max) NOT NULL,
	[IsRead] [bit] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreateAt] [datetime] NOT NULL,
	[UpdateAt] [datetime] NOT NULL,
	[DeleteAt] [datetime] NULL,
 CONSTRAINT [PK_Notification] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Premium]    Script Date: 3/14/2025 8:54:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Premium](
	[Id] [uniqueidentifier] NOT NULL,
	[Type] [nvarchar](50) NOT NULL,
	[Friend] [int] NOT NULL,
	[Timelimit] [bit] NOT NULL,
	[Match] [int] NOT NULL,
	[Price] [float] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreateAt] [datetime] NOT NULL,
	[UpdateAt] [datetime] NOT NULL,
	[DeleteAt] [datetime] NULL,
 CONSTRAINT [PK_Premium] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Question]    Script Date: 3/14/2025 8:54:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Question](
	[Id] [uniqueidentifier] NOT NULL,
	[Content] [nvarchar](max) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreateAt] [datetime] NOT NULL,
	[UpdateAt] [datetime] NOT NULL,
	[DeleteAt] [datetime] NULL,
 CONSTRAINT [PK_Question] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RefreshToken]    Script Date: 3/14/2025 8:54:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RefreshToken](
	[Id] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[Token] [nvarchar](max) NOT NULL,
	[ExpirationTime] [datetime] NOT NULL,
 CONSTRAINT [PK_RefreshToken] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Review]    Script Date: 3/14/2025 8:54:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Review](
	[Id] [uniqueidentifier] NOT NULL,
	[BookingId] [uniqueidentifier] NOT NULL,
	[ListenerId] [uniqueidentifier] NOT NULL,
	[ReviewMessage] [nvarchar](max) NULL,
	[ReplyMessage] [nvarchar](max) NULL,
	[Star] [float] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreateAt] [datetime] NOT NULL,
	[UpdateAt] [datetime] NOT NULL,
	[DeleteAt] [datetime] NULL,
 CONSTRAINT [PK_Review] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_BookingId_Review] UNIQUE NONCLUSTERED 
(
	[BookingId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TestHistory]    Script Date: 3/14/2025 8:54:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TestHistory](
	[Id] [uniqueidentifier] NOT NULL,
	[AccountId] [uniqueidentifier] NOT NULL,
	[Grade] [int] NOT NULL,
	[Status] [nvarchar](50) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreateAt] [datetime] NOT NULL,
	[UpdateAt] [datetime] NOT NULL,
	[DeleteAt] [datetime] NULL,
 CONSTRAINT [PK_TestHistory] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Topic]    Script Date: 3/14/2025 8:54:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Topic](
	[Id] [uniqueidentifier] NOT NULL,
	[ListenerInfoId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Translate] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreateAt] [datetime] NOT NULL,
	[UpdateAt] [datetime] NOT NULL,
	[DeleteAt] [datetime] NULL,
 CONSTRAINT [PK_Topic] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Transaction]    Script Date: 3/14/2025 8:54:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Transaction](
	[Id] [uniqueidentifier] NOT NULL,
	[WalletId] [uniqueidentifier] NOT NULL,
	[DepositId] [uniqueidentifier] NULL,
	[Amount] [decimal](18, 0) NOT NULL,
	[OrderCode] [bigint] NULL,
	[IsActive] [bit] NOT NULL,
	[CreateAt] [datetime] NOT NULL,
	[UpdateAt] [datetime] NOT NULL,
	[DeleteAt] [datetime] NULL,
	[Status] [nvarchar](50) NULL,
	[Type] [nvarchar](50) NULL,
 CONSTRAINT [PK_Transaction] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_DepositId_Transaction] UNIQUE NONCLUSTERED 
(
	[DepositId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserCall]    Script Date: 3/14/2025 8:54:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserCall](
	[Id] [uniqueidentifier] NOT NULL,
	[AccountId] [uniqueidentifier] NOT NULL,
	[CallHistoryId] [uniqueidentifier] NOT NULL,
	[CallRole] [nvarchar](50) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreateAt] [datetime] NOT NULL,
	[UpdateAt] [datetime] NOT NULL,
	[DeleteAt] [datetime] NULL,
 CONSTRAINT [PK_UserCall] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserInfo]    Script Date: 3/14/2025 8:54:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserInfo](
	[Id] [uniqueidentifier] NOT NULL,
	[AccountId] [uniqueidentifier] NOT NULL,
	[PremiumId] [uniqueidentifier] NOT NULL,
	[DateStart] [datetime] NOT NULL,
	[DateEnd] [datetime] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreateAt] [datetime] NOT NULL,
	[UpdateAt] [datetime] NOT NULL,
	[DeleteAt] [datetime] NULL,
 CONSTRAINT [PK_UserInfo] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_AccountId_UserInfo] UNIQUE NONCLUSTERED 
(
	[AccountId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserPresence]    Script Date: 3/14/2025 8:54:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserPresence](
	[Id] [uniqueidentifier] NOT NULL,
	[AccountId] [uniqueidentifier] NOT NULL,
	[Offline] [bit] NOT NULL,
	[Online] [bit] NOT NULL,
	[InCall] [bit] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreateAt] [datetime] NOT NULL,
	[UpdateAt] [datetime] NOT NULL,
	[DeleteAt] [datetime] NULL,
 CONSTRAINT [PK_UserPresence] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_AccountId_UserPresence] UNIQUE NONCLUSTERED 
(
	[AccountId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Wallet]    Script Date: 3/14/2025 8:54:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Wallet](
	[Id] [uniqueidentifier] NOT NULL,
	[AccountId] [uniqueidentifier] NOT NULL,
	[Balance] [decimal](18, 0) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreateAt] [datetime] NOT NULL,
	[UpdateAt] [datetime] NOT NULL,
	[DeleteAt] [datetime] NULL,
 CONSTRAINT [PK_Walet] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_AccountId_Wallet] UNIQUE NONCLUSTERED 
(
	[AccountId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WithdrawRequests]    Script Date: 3/14/2025 8:54:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WithdrawRequests](
	[Id] [uniqueidentifier] NOT NULL,
	[ListenerId] [uniqueidentifier] NOT NULL,
	[AdminId] [uniqueidentifier] NOT NULL,
	[Amount] [decimal](18, 2) NOT NULL,
	[Status] [nvarchar](50) NOT NULL,
	[CreateAt] [datetime] NOT NULL,
	[UpdateAt] [datetime] NOT NULL,
	[DeleteAt] [datetime] NULL,
 CONSTRAINT [PK_WithdrawRequests] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WorkShifts]    Script Date: 3/14/2025 8:54:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WorkShifts](
	[Id] [uniqueidentifier] NOT NULL,
	[AccountId] [uniqueidentifier] NOT NULL,
	[StartTime] [time](7) NOT NULL,
	[EndTime] [time](7) NOT NULL,
	[Day] [nvarchar](50) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreateAt] [datetime] NOT NULL,
	[UpdateAt] [datetime] NOT NULL,
	[DeleteAt] [datetime] NULL,
 CONSTRAINT [PK_WorkShifts] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Index [Index_UserId]    Script Date: 3/14/2025 8:54:51 PM ******/
CREATE NONCLUSTERED INDEX [Index_UserId] ON [dbo].[RefreshToken]
(
	[UserId] DESC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Blog]  WITH CHECK ADD  CONSTRAINT [FK_Blog_Account] FOREIGN KEY([Author_id ])
REFERENCES [dbo].[Account] ([Id])
GO
ALTER TABLE [dbo].[Blog] CHECK CONSTRAINT [FK_Blog_Account]
GO
ALTER TABLE [dbo].[Booking]  WITH CHECK ADD  CONSTRAINT [FK_Booking_Account] FOREIGN KEY([UserId])
REFERENCES [dbo].[Account] ([Id])
GO
ALTER TABLE [dbo].[Booking] CHECK CONSTRAINT [FK_Booking_Account]
GO
ALTER TABLE [dbo].[Booking]  WITH CHECK ADD  CONSTRAINT [FK_Booking_WorkShifts] FOREIGN KEY([WorkShiftId])
REFERENCES [dbo].[WorkShifts] ([Id])
GO
ALTER TABLE [dbo].[Booking] CHECK CONSTRAINT [FK_Booking_WorkShifts]
GO
ALTER TABLE [dbo].[Deposit]  WITH CHECK ADD  CONSTRAINT [FK_Deposit_Account] FOREIGN KEY([AccountId])
REFERENCES [dbo].[Account] ([Id])
GO
ALTER TABLE [dbo].[Deposit] CHECK CONSTRAINT [FK_Deposit_Account]
GO
ALTER TABLE [dbo].[DirectChatParticipant]  WITH CHECK ADD  CONSTRAINT [FK_DirectChatParticipant_Account] FOREIGN KEY([AccountId])
REFERENCES [dbo].[Account] ([Id])
GO
ALTER TABLE [dbo].[DirectChatParticipant] CHECK CONSTRAINT [FK_DirectChatParticipant_Account]
GO
ALTER TABLE [dbo].[DirectChatParticipant]  WITH CHECK ADD  CONSTRAINT [FK_DirectChatParticipant_DirectChat] FOREIGN KEY([DirectChatId])
REFERENCES [dbo].[DirectChat] ([Id])
GO
ALTER TABLE [dbo].[DirectChatParticipant] CHECK CONSTRAINT [FK_DirectChatParticipant_DirectChat]
GO
ALTER TABLE [dbo].[FriendShip]  WITH CHECK ADD  CONSTRAINT [FK_FriendShip_Friend] FOREIGN KEY([FriendId])
REFERENCES [dbo].[Account] ([Id])
GO
ALTER TABLE [dbo].[FriendShip] CHECK CONSTRAINT [FK_FriendShip_Friend]
GO
ALTER TABLE [dbo].[FriendShip]  WITH CHECK ADD  CONSTRAINT [FK_FriendShip_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[Account] ([Id])
GO
ALTER TABLE [dbo].[FriendShip] CHECK CONSTRAINT [FK_FriendShip_User]
GO
ALTER TABLE [dbo].[ListenerInfo]  WITH CHECK ADD  CONSTRAINT [FK_ListenerInfo_Account] FOREIGN KEY([AccountId])
REFERENCES [dbo].[Account] ([Id])
GO
ALTER TABLE [dbo].[ListenerInfo] CHECK CONSTRAINT [FK_ListenerInfo_Account]
GO
ALTER TABLE [dbo].[Message]  WITH CHECK ADD  CONSTRAINT [FK_Message_Account] FOREIGN KEY([SenderId])
REFERENCES [dbo].[Account] ([Id])
GO
ALTER TABLE [dbo].[Message] CHECK CONSTRAINT [FK_Message_Account]
GO
ALTER TABLE [dbo].[Message]  WITH CHECK ADD  CONSTRAINT [FK_Message_DirectChat] FOREIGN KEY([DirectChatId])
REFERENCES [dbo].[DirectChat] ([Id])
GO
ALTER TABLE [dbo].[Message] CHECK CONSTRAINT [FK_Message_DirectChat]
GO
ALTER TABLE [dbo].[Notification]  WITH CHECK ADD  CONSTRAINT [FK_Notification_Account] FOREIGN KEY([UserId])
REFERENCES [dbo].[Account] ([Id])
GO
ALTER TABLE [dbo].[Notification] CHECK CONSTRAINT [FK_Notification_Account]
GO
ALTER TABLE [dbo].[RefreshToken]  WITH CHECK ADD  CONSTRAINT [FK_RefreshToken_Account] FOREIGN KEY([UserId])
REFERENCES [dbo].[Account] ([Id])
GO
ALTER TABLE [dbo].[RefreshToken] CHECK CONSTRAINT [FK_RefreshToken_Account]
GO
ALTER TABLE [dbo].[Review]  WITH CHECK ADD  CONSTRAINT [FK_Review_Booking] FOREIGN KEY([BookingId])
REFERENCES [dbo].[Booking] ([Id])
GO
ALTER TABLE [dbo].[Review] CHECK CONSTRAINT [FK_Review_Booking]
GO
ALTER TABLE [dbo].[TestHistory]  WITH CHECK ADD  CONSTRAINT [FK_TestHistory_Account] FOREIGN KEY([AccountId])
REFERENCES [dbo].[Account] ([Id])
GO
ALTER TABLE [dbo].[TestHistory] CHECK CONSTRAINT [FK_TestHistory_Account]
GO
ALTER TABLE [dbo].[Topic]  WITH CHECK ADD  CONSTRAINT [FK_Topic_ListenerInfo] FOREIGN KEY([ListenerInfoId])
REFERENCES [dbo].[ListenerInfo] ([Id])
GO
ALTER TABLE [dbo].[Topic] CHECK CONSTRAINT [FK_Topic_ListenerInfo]
GO
ALTER TABLE [dbo].[Transaction]  WITH CHECK ADD  CONSTRAINT [FK_Transaction_Deposit] FOREIGN KEY([DepositId])
REFERENCES [dbo].[Deposit] ([Id])
GO
ALTER TABLE [dbo].[Transaction] CHECK CONSTRAINT [FK_Transaction_Deposit]
GO
ALTER TABLE [dbo].[Transaction]  WITH CHECK ADD  CONSTRAINT [FK_Transaction_Wallet] FOREIGN KEY([WalletId])
REFERENCES [dbo].[Wallet] ([Id])
GO
ALTER TABLE [dbo].[Transaction] CHECK CONSTRAINT [FK_Transaction_Wallet]
GO
ALTER TABLE [dbo].[UserCall]  WITH CHECK ADD  CONSTRAINT [FK_UserCall_Account] FOREIGN KEY([AccountId])
REFERENCES [dbo].[Account] ([Id])
GO
ALTER TABLE [dbo].[UserCall] CHECK CONSTRAINT [FK_UserCall_Account]
GO
ALTER TABLE [dbo].[UserCall]  WITH CHECK ADD  CONSTRAINT [FK_UserCall_CallHistory] FOREIGN KEY([CallHistoryId])
REFERENCES [dbo].[CallHistory] ([Id])
GO
ALTER TABLE [dbo].[UserCall] CHECK CONSTRAINT [FK_UserCall_CallHistory]
GO
ALTER TABLE [dbo].[UserInfo]  WITH CHECK ADD  CONSTRAINT [FK_UserInfo_Account] FOREIGN KEY([AccountId])
REFERENCES [dbo].[Account] ([Id])
GO
ALTER TABLE [dbo].[UserInfo] CHECK CONSTRAINT [FK_UserInfo_Account]
GO
ALTER TABLE [dbo].[UserInfo]  WITH CHECK ADD  CONSTRAINT [FK_UserInfo_Premium] FOREIGN KEY([PremiumId])
REFERENCES [dbo].[Premium] ([Id])
GO
ALTER TABLE [dbo].[UserInfo] CHECK CONSTRAINT [FK_UserInfo_Premium]
GO
ALTER TABLE [dbo].[UserPresence]  WITH CHECK ADD  CONSTRAINT [FK_UserPresence_Account] FOREIGN KEY([AccountId])
REFERENCES [dbo].[Account] ([Id])
GO
ALTER TABLE [dbo].[UserPresence] CHECK CONSTRAINT [FK_UserPresence_Account]
GO
ALTER TABLE [dbo].[Wallet]  WITH CHECK ADD  CONSTRAINT [FK_Wallet_Account] FOREIGN KEY([AccountId])
REFERENCES [dbo].[Account] ([Id])
GO
ALTER TABLE [dbo].[Wallet] CHECK CONSTRAINT [FK_Wallet_Account]
GO
ALTER TABLE [dbo].[WorkShifts]  WITH CHECK ADD  CONSTRAINT [FK_WorkShifts_Account] FOREIGN KEY([AccountId])
REFERENCES [dbo].[Account] ([Id])
GO
ALTER TABLE [dbo].[WorkShifts] CHECK CONSTRAINT [FK_WorkShifts_Account]
GO
USE [master]
GO
ALTER DATABASE [Harmon] SET  READ_WRITE 
GO
