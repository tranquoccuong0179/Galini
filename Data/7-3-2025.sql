USE [master]
GO
/****** Object:  Database [Harmon]    Script Date: 3/7/2025 8:06:24 PM ******/
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
/****** Object:  Table [dbo].[Account]    Script Date: 3/7/2025 8:06:25 PM ******/
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
	[AvatarUrl] [nvarchar](max) NULL,
	[IsActive] [bit] NOT NULL,
	[CreateAt] [datetime] NOT NULL,
	[UpdateAt] [datetime] NOT NULL,
	[DeleteAt] [datetime] NULL,
 CONSTRAINT [PK_Account] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Blog]    Script Date: 3/7/2025 8:06:25 PM ******/
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
/****** Object:  Table [dbo].[Booking]    Script Date: 3/7/2025 8:06:25 PM ******/
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
/****** Object:  Table [dbo].[CallHistory]    Script Date: 3/7/2025 8:06:25 PM ******/
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
/****** Object:  Table [dbo].[Deposit]    Script Date: 3/7/2025 8:06:25 PM ******/
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
/****** Object:  Table [dbo].[DirectChat]    Script Date: 3/7/2025 8:06:25 PM ******/
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
/****** Object:  Table [dbo].[DirectChatParticipant]    Script Date: 3/7/2025 8:06:25 PM ******/
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
/****** Object:  Table [dbo].[FriendShip]    Script Date: 3/7/2025 8:06:25 PM ******/
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
/****** Object:  Table [dbo].[Identification]    Script Date: 3/7/2025 8:06:25 PM ******/
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
/****** Object:  Table [dbo].[ListenerInfo]    Script Date: 3/7/2025 8:06:25 PM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Message]    Script Date: 3/7/2025 8:06:25 PM ******/
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
/****** Object:  Table [dbo].[Notification]    Script Date: 3/7/2025 8:06:25 PM ******/
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
/****** Object:  Table [dbo].[Premium]    Script Date: 3/7/2025 8:06:25 PM ******/
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
/****** Object:  Table [dbo].[Question]    Script Date: 3/7/2025 8:06:25 PM ******/
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
/****** Object:  Table [dbo].[RefreshToken]    Script Date: 3/7/2025 8:06:25 PM ******/
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
/****** Object:  Table [dbo].[Review]    Script Date: 3/7/2025 8:06:25 PM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TestHistory]    Script Date: 3/7/2025 8:06:25 PM ******/
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
/****** Object:  Table [dbo].[Topic]    Script Date: 3/7/2025 8:06:25 PM ******/
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
/****** Object:  Table [dbo].[Transaction]    Script Date: 3/7/2025 8:06:25 PM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserCall]    Script Date: 3/7/2025 8:06:25 PM ******/
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
/****** Object:  Table [dbo].[UserInfo]    Script Date: 3/7/2025 8:06:25 PM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserPresence]    Script Date: 3/7/2025 8:06:25 PM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Wallet]    Script Date: 3/7/2025 8:06:25 PM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WorkShifts]    Script Date: 3/7/2025 8:06:25 PM ******/
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
INSERT [dbo].[Account] ([Id], [UserName], [Password], [Role], [FullName], [Email], [Phone], [DateOfBirth], [Gender], [AvatarUrl], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'a1f7b908-a917-4cf4-b7bb-14c7f1485934', N'khoacute', N'v6plobem2ptzJLRd532mc835oAiq5JhrqBgHaCbjR+Y=', N'Customer', N'Nguyen Dang Khoa', N'khoahdmse171553@gmail.com', N'0903454545', CAST(N'2003-06-19T00:00:00.000' AS DateTime), N'Female', N'https://encrypted-tbn3.gstatic.com/images?q=tbn:ANd9GcSkSXuW2Rn2gOPThRo_XmU7LlaXGbceTVJ8hH40XPwjC1oJMBKentQ6xfpfFugoflI16Ld7_2JpqtyhxjqMHUaLSA', 0, CAST(N'2025-02-28T07:37:42.493' AS DateTime), CAST(N'2025-02-28T07:37:42.493' AS DateTime), NULL)
INSERT [dbo].[Account] ([Id], [UserName], [Password], [Role], [FullName], [Email], [Phone], [DateOfBirth], [Gender], [AvatarUrl], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'3839e1d3-3a37-4b4f-bd66-195dd18efbfe', N'tung', N'hC66tjElhbk0z1DSTxJ6R8mCM6Jk8OgbxuWhIOMrXD4=', N'Customer', N'Tung', N'tung@yopmail.com', N'0815426373', CAST(N'2025-01-29T00:00:00.000' AS DateTime), N'Female', N'https://encrypted-tbn3.gstatic.com/images?q=tbn:ANd9GcSkSXuW2Rn2gOPThRo_XmU7LlaXGbceTVJ8hH40XPwjC1oJMBKentQ6xfpfFugoflI16Ld7_2JpqtyhxjqMHUaLSA', 1, CAST(N'2025-02-28T06:59:56.637' AS DateTime), CAST(N'2025-02-28T06:59:56.637' AS DateTime), NULL)
INSERT [dbo].[Account] ([Id], [UserName], [Password], [Role], [FullName], [Email], [Phone], [DateOfBirth], [Gender], [AvatarUrl], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'b927e5e6-be99-4633-9db0-1f713410ab20', N'khoa', N'v6plobem2ptzJLRd532mc835oAiq5JhrqBgHaCbjR+Y=', N'Customer', N'Khoa', N'khoa@gmail.com', N'0909090909', CAST(N'2021-11-24T00:00:00.000' AS DateTime), N'Female', N'https://encrypted-tbn3.gstatic.com/images?q=tbn:ANd9GcSkSXuW2Rn2gOPThRo_XmU7LlaXGbceTVJ8hH40XPwjC1oJMBKentQ6xfpfFugoflI16Ld7_2JpqtyhxjqMHUaLSA', 0, CAST(N'2025-02-28T06:49:14.037' AS DateTime), CAST(N'2025-02-28T06:49:14.037' AS DateTime), NULL)
INSERT [dbo].[Account] ([Id], [UserName], [Password], [Role], [FullName], [Email], [Phone], [DateOfBirth], [Gender], [AvatarUrl], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'90fba115-620d-45f5-8407-28b2db39b4ef', N'phuc', N'Bd+vZPhs9gPs0NtkQKHsmMVcrluUw7dLmcZ/rvIAghw=', N'Customer', N'Mo Dung Phuc', N'phuc@yopmail.com', N'0903546454', CAST(N'2004-03-10T00:00:00.000' AS DateTime), N'Female', N'https://encrypted-tbn3.gstatic.com/images?q=tbn:ANd9GcSkSXuW2Rn2gOPThRo_XmU7LlaXGbceTVJ8hH40XPwjC1oJMBKentQ6xfpfFugoflI16Ld7_2JpqtyhxjqMHUaLSA', 1, CAST(N'2025-02-28T06:36:08.790' AS DateTime), CAST(N'2025-02-28T06:36:08.790' AS DateTime), NULL)
INSERT [dbo].[Account] ([Id], [UserName], [Password], [Role], [FullName], [Email], [Phone], [DateOfBirth], [Gender], [AvatarUrl], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'451c71a1-a4f4-4607-8ab5-2e07d11f2961', N'khoadang', N'zZfH9HtbH3UHFDJr6NbW/ut4anOjHrN9XYKMr1jMftM=', N'Customer', N'Nguyen Dang Khoa', N'khoadang@gmail.com', N'0901838699', CAST(N'2003-02-27T14:09:50.927' AS DateTime), N'Male', N'https://encrypted-tbn3.gstatic.com/images?q=tbn:ANd9GcSkSXuW2Rn2gOPThRo_XmU7LlaXGbceTVJ8hH40XPwjC1oJMBKentQ6xfpfFugoflI16Ld7_2JpqtyhxjqMHUaLSA', 0, CAST(N'2025-02-27T21:12:31.693' AS DateTime), CAST(N'2025-02-27T21:12:31.693' AS DateTime), NULL)
INSERT [dbo].[Account] ([Id], [UserName], [Password], [Role], [FullName], [Email], [Phone], [DateOfBirth], [Gender], [AvatarUrl], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'32481b25-b91e-4756-a235-31916269c33a', N'khoa223', N'A6xnQhbz4Vx2HuGl4lXwZ5U2I8iziLRFnhP5eNfIRvQ=', N'Customer', N'Nguyen Dang Khoa', N'khoahdmse171553@fpt.edu.vn', N'0974886399', CAST(N'2003-06-10T00:00:00.000' AS DateTime), N'Female', N'https://encrypted-tbn3.gstatic.com/images?q=tbn:ANd9GcSkSXuW2Rn2gOPThRo_XmU7LlaXGbceTVJ8hH40XPwjC1oJMBKentQ6xfpfFugoflI16Ld7_2JpqtyhxjqMHUaLSA', 1, CAST(N'2025-02-28T07:47:41.553' AS DateTime), CAST(N'2025-02-28T07:47:41.553' AS DateTime), NULL)
INSERT [dbo].[Account] ([Id], [UserName], [Password], [Role], [FullName], [Email], [Phone], [DateOfBirth], [Gender], [AvatarUrl], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'54dd3bdc-b1d6-4957-9677-33a3441b4c2d', N'khoaa', N'v6plobem2ptzJLRd532mc835oAiq5JhrqBgHaCbjR+Y=', N'Customer', N'Khoa', N'khoa@yopmail.com', N'0934217652', CAST(N'2005-07-06T00:00:00.000' AS DateTime), N'Female', N'https://encrypted-tbn3.gstatic.com/images?q=tbn:ANd9GcSkSXuW2Rn2gOPThRo_XmU7LlaXGbceTVJ8hH40XPwjC1oJMBKentQ6xfpfFugoflI16Ld7_2JpqtyhxjqMHUaLSA', 1, CAST(N'2025-02-28T06:53:26.667' AS DateTime), CAST(N'2025-02-28T06:53:26.667' AS DateTime), NULL)
INSERT [dbo].[Account] ([Id], [UserName], [Password], [Role], [FullName], [Email], [Phone], [DateOfBirth], [Gender], [AvatarUrl], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'7d172f31-df5d-4f84-8296-397d2a51ff21', N'khang1z2t', N'BUtLr5O56XituO+Hd9c04aaC/MUhxg4RtXK0wYO8RxE=', N'Listener', N'Bảo Khang', N'kbao040@gmail.com', N'0865399254', CAST(N'2004-08-30T03:00:44.380' AS DateTime), N'Male', N'string', 1, CAST(N'2025-02-25T10:01:46.513' AS DateTime), CAST(N'2025-02-25T10:01:46.513' AS DateTime), NULL)
INSERT [dbo].[Account] ([Id], [UserName], [Password], [Role], [FullName], [Email], [Phone], [DateOfBirth], [Gender], [AvatarUrl], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'd26d36c0-0692-4bc6-bafe-49698f45a657', N'cuongtq13', N'0oRK+Tx9/yMmqwjGRKx6uMZsUxD3ROjmSzeqibB24V4=', N'Listener', N'Trần Quốc Cường', N'cuongtq13@gmail.com', N'0363919178', CAST(N'2025-03-05T00:00:00.000' AS DateTime), N'Male', N'123', 1, CAST(N'2025-03-05T23:03:20.553' AS DateTime), CAST(N'2025-03-05T23:03:20.553' AS DateTime), NULL)
INSERT [dbo].[Account] ([Id], [UserName], [Password], [Role], [FullName], [Email], [Phone], [DateOfBirth], [Gender], [AvatarUrl], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'32842bac-c086-4b9c-a656-5301e6085d54', N'cuongtq1', N'Guthc2veM8QtXF0Ng4lFtnXcdsoj+wZIBnuw+tiMVbE=', N'Listener', N'Tran Quoc Cuong', N'tranquoccuong13072002@gmail.com', N'0876256515', CAST(N'2025-02-25T15:42:04.530' AS DateTime), N'Male', N'string', 1, CAST(N'2025-02-25T22:43:57.273' AS DateTime), CAST(N'2025-02-25T22:43:57.273' AS DateTime), NULL)
INSERT [dbo].[Account] ([Id], [UserName], [Password], [Role], [FullName], [Email], [Phone], [DateOfBirth], [Gender], [AvatarUrl], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'6b408dce-34db-46ab-9eba-6e5aae607243', N'hoangdung', N'X4XKk6E7JRW6cJudznjrGIWoqcq/wv3b7pqlhTm2HrQ=', N'Customer', N'Hoàng Dung', N'hdung0831@gmail.com', N'0123456789', CAST(N'2025-02-26T02:37:36.217' AS DateTime), N'Male', N'string', 1, CAST(N'2025-02-26T09:38:18.827' AS DateTime), CAST(N'2025-02-26T09:38:18.827' AS DateTime), NULL)
INSERT [dbo].[Account] ([Id], [UserName], [Password], [Role], [FullName], [Email], [Phone], [DateOfBirth], [Gender], [AvatarUrl], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'eb875212-93e3-4696-96e3-71e3ce01f1aa', N'AnNgu', N'XMX6w43cP+ius3fuEio3cToYfRxr4NawaPeK7iWMz/Q=', N'Listener', N'AnNgu', N'AnNgu@gmail.com', N'0877726621', CAST(N'2025-03-05T00:00:00.000' AS DateTime), N'Other', N'ooooooo', 1, CAST(N'2025-03-05T21:09:45.267' AS DateTime), CAST(N'2025-03-05T21:09:45.267' AS DateTime), NULL)
INSERT [dbo].[Account] ([Id], [UserName], [Password], [Role], [FullName], [Email], [Phone], [DateOfBirth], [Gender], [AvatarUrl], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'10280fb5-bf7e-46b5-be57-78baf041c83f', N'stringgggg', N'm8uML4lrB/EWe7FPTRg1mRghHyAs95JMAGA+IjjF5Xw=', N'Listener', N'stringgggg', N'stringgggg@gmail.com', N'0524545415', CAST(N'2025-03-05T03:45:51.123' AS DateTime), N'Male', N'string', 1, CAST(N'2025-03-05T10:46:31.167' AS DateTime), CAST(N'2025-03-05T10:46:31.167' AS DateTime), NULL)
INSERT [dbo].[Account] ([Id], [UserName], [Password], [Role], [FullName], [Email], [Phone], [DateOfBirth], [Gender], [AvatarUrl], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'2d96b34d-cb42-4dde-bcda-7e3899dde181', N'string1', N'k/7d5DID4KdhchNSIbhjYxNjXXr/+WpJCukGYzBQXUc=', N'Listener', N'string1', N'string1@gmail.com', N'0712562771', CAST(N'2025-03-02T14:41:50.173' AS DateTime), N'Male', N'string', 1, CAST(N'2025-03-02T21:42:13.977' AS DateTime), CAST(N'2025-03-02T21:42:13.977' AS DateTime), NULL)
INSERT [dbo].[Account] ([Id], [UserName], [Password], [Role], [FullName], [Email], [Phone], [DateOfBirth], [Gender], [AvatarUrl], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'931814c2-f7bc-4059-97ef-83aa98baa6fe', N'stringfff', N'SZfuxMR+5dPfADOFjNz0PFsdFgm6NtiVKSYZHKybz2A=', N'Listener', N'string', N'string@gmail.com', N'0901333333', CAST(N'2025-02-27T17:43:11.673' AS DateTime), N'Male', N'string', 0, CAST(N'2025-02-28T01:10:09.527' AS DateTime), CAST(N'2025-02-28T01:10:09.527' AS DateTime), NULL)
INSERT [dbo].[Account] ([Id], [UserName], [Password], [Role], [FullName], [Email], [Phone], [DateOfBirth], [Gender], [AvatarUrl], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'3c7d8982-6f92-4346-812a-901fe3e670c0', N'baquat', N'L3QcCySbsMAMwzWlbIg/qVl2yXVdlODWyZO3ER4foy4=', N'Customer', N'Cao Ba Quat', N'quat@gmail.com', N'0988888888', CAST(N'2025-02-04T00:00:00.000' AS DateTime), N'Female', N'https://encrypted-tbn3.gstatic.com/images?q=tbn:ANd9GcSkSXuW2Rn2gOPThRo_XmU7LlaXGbceTVJ8hH40XPwjC1oJMBKentQ6xfpfFugoflI16Ld7_2JpqtyhxjqMHUaLSA', 0, CAST(N'2025-02-28T01:15:15.487' AS DateTime), CAST(N'2025-02-28T01:15:15.487' AS DateTime), NULL)
INSERT [dbo].[Account] ([Id], [UserName], [Password], [Role], [FullName], [Email], [Phone], [DateOfBirth], [Gender], [AvatarUrl], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'a8c3d7e1-26b6-46f1-945d-9fdb97a582ac', N'cuongtq', N'9F69GG+kpIcM0QHspUF2Cmr9DIODeeEJLnebUpskIvQ=', N'Admin', N'Trần Quốc Cường', N'tranquoccuong0179@gmail.com', N'0363919179', CAST(N'2002-07-13T01:47:26.557' AS DateTime), N'Male', N'string', 1, CAST(N'2025-02-25T08:48:56.627' AS DateTime), CAST(N'2025-02-25T08:48:56.627' AS DateTime), NULL)
INSERT [dbo].[Account] ([Id], [UserName], [Password], [Role], [FullName], [Email], [Phone], [DateOfBirth], [Gender], [AvatarUrl], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'85fb2b10-1dfa-4408-be3a-b5b98234c4c4', N'trung', N'gzJBYNQ72zJ+VVtr6M5lToxpPpXApPEIF0mbueAiSh0=', N'Customer', N'Trung', N'trung@yopmail.com', N'0456452423', CAST(N'2025-01-28T00:00:00.000' AS DateTime), N'Female', N'https://encrypted-tbn3.gstatic.com/images?q=tbn:ANd9GcSkSXuW2Rn2gOPThRo_XmU7LlaXGbceTVJ8hH40XPwjC1oJMBKentQ6xfpfFugoflI16Ld7_2JpqtyhxjqMHUaLSA', 1, CAST(N'2025-02-28T06:57:41.020' AS DateTime), CAST(N'2025-02-28T06:57:41.020' AS DateTime), NULL)
INSERT [dbo].[Account] ([Id], [UserName], [Password], [Role], [FullName], [Email], [Phone], [DateOfBirth], [Gender], [AvatarUrl], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'cab5e345-f36c-4c9f-8485-c92ba65af5df', N'admin', N'jGl25bVBBBW96Qi9Te4V37Fnqchz/Eu4qB9vKrRIqRg=', N'Admin', N'admin', N'antnqe170035@fpt.edu.vn', N'0356623452', CAST(N'2025-03-02T09:53:51.857' AS DateTime), N'Male', N'string', 1, CAST(N'2025-03-02T16:54:46.320' AS DateTime), CAST(N'2025-03-02T16:54:46.320' AS DateTime), NULL)
INSERT [dbo].[Account] ([Id], [UserName], [Password], [Role], [FullName], [Email], [Phone], [DateOfBirth], [Gender], [AvatarUrl], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'27bc7b9d-cca3-4ce4-8a15-cb0cf3cb9115', N'string', N'RzKH+CmNunFjqJeQiVj3wOrnM+JdLgJ5kuou3JvtL6g=', N'Customer', N'string', N'ngocan2003krp@gmail.com', N'0356638270', CAST(N'2025-02-25T08:56:12.127' AS DateTime), N'Male', N'string', 1, CAST(N'2025-02-25T15:56:35.367' AS DateTime), CAST(N'2025-02-25T15:56:35.367' AS DateTime), NULL)
INSERT [dbo].[Account] ([Id], [UserName], [Password], [Role], [FullName], [Email], [Phone], [DateOfBirth], [Gender], [AvatarUrl], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'67c8148c-9726-4259-a5c2-ce73aa73a263', N'cuongtqse160059', N'73l8gRjwLftklgfdXT+MdiMEjJwGPVMsyVxe16iYpk8=', N'Customer', N'Tran Quoc Cuong (K16_HCM)', N'cuongtqse160059@fpt.edu.vn', N'0000000000', CAST(N'2025-02-25T17:48:42.717' AS DateTime), N'Male', N'', 1, CAST(N'2025-02-25T17:48:42.717' AS DateTime), CAST(N'2025-02-25T17:48:42.717' AS DateTime), NULL)
INSERT [dbo].[Account] ([Id], [UserName], [Password], [Role], [FullName], [Email], [Phone], [DateOfBirth], [Gender], [AvatarUrl], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'e9d06dda-5719-49b5-89b3-d0cc184ce119', N'khoa123@', N'xSCccua5+MKeI7Hin1qV4I5Uy3eiaMbPG6Q/DYHS244=', N'Customer', N'Nguyen Dang Khoa', N'test10@yopmail.com', N'0648736511', CAST(N'2007-02-07T00:00:00.000' AS DateTime), N'Female', N'https://encrypted-tbn3.gstatic.com/images?q=tbn:ANd9GcSkSXuW2Rn2gOPThRo_XmU7LlaXGbceTVJ8hH40XPwjC1oJMBKentQ6xfpfFugoflI16Ld7_2JpqtyhxjqMHUaLSA', 1, CAST(N'2025-03-05T15:18:41.373' AS DateTime), CAST(N'2025-03-05T15:18:41.373' AS DateTime), NULL)
INSERT [dbo].[Account] ([Id], [UserName], [Password], [Role], [FullName], [Email], [Phone], [DateOfBirth], [Gender], [AvatarUrl], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'41225bc2-9415-45f5-bcbe-dde495ed27a7', N'asss1', N'9V/xb2b0M2Ama5Xbb4/sAddgMQVDBq5KSzgFmPbP0RQ=', N'Listener', N'a1', N'acom1com1@gmail.com', N'0737667676', CAST(N'2025-03-05T00:00:00.000' AS DateTime), N'Female', N'123', 1, CAST(N'2025-03-05T21:19:56.383' AS DateTime), CAST(N'2025-03-05T21:19:56.383' AS DateTime), NULL)
INSERT [dbo].[Account] ([Id], [UserName], [Password], [Role], [FullName], [Email], [Phone], [DateOfBirth], [Gender], [AvatarUrl], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'a5e3ac44-0840-43f2-8d73-fa614f25e34d', N'asss', N'yIfgKsWIkI35A88yzbWxxqyQSQI4kPOfOG7v2Xmx27o=', N'Listener', N'cuongtq123', N'cuongtq123@gmail.com', N'0823727123', CAST(N'2025-03-05T00:00:00.000' AS DateTime), N'Male', N'ffff', 1, CAST(N'2025-03-05T21:02:17.030' AS DateTime), CAST(N'2025-03-05T21:02:17.030' AS DateTime), NULL)
INSERT [dbo].[Account] ([Id], [UserName], [Password], [Role], [FullName], [Email], [Phone], [DateOfBirth], [Gender], [AvatarUrl], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'100f00b9-58dc-48c5-97b5-fa72902a6522', N'AnNgu1', N'qZrEgyzve2ZjRfqcGVWjEudjHAd383ZOKN2uAr0kdaA=', N'Listener', N'AnNgu1', N'AnNgu1@gmail.com', N'0877726622', CAST(N'2025-03-05T00:00:00.000' AS DateTime), N'Other', N'ooooooo', 1, CAST(N'2025-03-05T21:10:45.017' AS DateTime), CAST(N'2025-03-05T21:10:45.017' AS DateTime), NULL)
GO
INSERT [dbo].[Blog] ([Id], [Title], [Content], [Type], [Views], [Likes], [Status], [Author_id ], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'a16f4a65-fb48-47bb-ace3-0b5efc631635', N'cường', N'<h1>Giới thiệu</h1><p>Đẹp điên</p><p><img></p>', N'Customer', 0, 0, N'PENDING', N'a8c3d7e1-26b6-46f1-945d-9fdb97a582ac', 1, CAST(N'2025-03-04T13:14:59.597' AS DateTime), CAST(N'2025-03-04T13:14:59.597' AS DateTime), NULL)
INSERT [dbo].[Blog] ([Id], [Title], [Content], [Type], [Views], [Likes], [Status], [Author_id ], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'ae0c9281-fdc8-4441-a20c-120a7fcf6c3c', N'Hello', N'Hello', N'Customer', 0, 0, N'PENDING', N'a8c3d7e1-26b6-46f1-945d-9fdb97a582ac', 1, CAST(N'2025-03-04T22:29:55.103' AS DateTime), CAST(N'2025-03-04T22:29:55.103' AS DateTime), NULL)
INSERT [dbo].[Blog] ([Id], [Title], [Content], [Type], [Views], [Likes], [Status], [Author_id ], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'f280d23e-1780-4152-a6df-1fdfb8a2d086', N'Tại sao càng lớn tuổi càng khó ngủ', N'Cảm thấy cực kỳ khó ngủ yên mỗi đêm? Có lẽ bạn từng có thể ngủ tới chiều vào cuối tuần, nhưng gần đây, mới tinh mơ sáng là bạn đã tỉnh giấc rồi. Có lẽ bạn từng ngủ ngay lúc đặt đầu xuống gối, nhưng mấy ngày nay bạn không thể ngủ một cách dễ dàng được', N'Customer', 0, 0, N'PENDING', N'a8c3d7e1-26b6-46f1-945d-9fdb97a582ac', 1, CAST(N'2025-03-04T10:55:56.400' AS DateTime), CAST(N'2025-03-04T10:55:56.400' AS DateTime), NULL)
INSERT [dbo].[Blog] ([Id], [Title], [Content], [Type], [Views], [Likes], [Status], [Author_id ], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'317d94e4-20f7-4b26-952d-2fac875990c5', N'Kkkkkkk', N'<p>Kkkkkk<img></p>', N'Customer', 0, 0, N'PENDING', N'a8c3d7e1-26b6-46f1-945d-9fdb97a582ac', 1, CAST(N'2025-03-04T15:52:59.897' AS DateTime), CAST(N'2025-03-04T15:52:59.897' AS DateTime), NULL)
INSERT [dbo].[Blog] ([Id], [Title], [Content], [Type], [Views], [Likes], [Status], [Author_id ], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'8f7655fa-32b4-436b-a5dd-3286b52d588d', N'Tại sao càng lớn tuổi càng khó ngủ', N'Cảm thấy cực kỳ khó ngủ yên mỗi đêm? Có lẽ bạn từng có thể ngủ tới chiều vào cuối tuần, nhưng gần đây, mới tinh mơ sáng là bạn đã tỉnh giấc rồi. Có lẽ bạn từng ngủ ngay lúc đặt đầu xuống gối, nhưng mấy ngày nay bạn không thể ngủ một cách dễ dàng được', N'Customer', 0, 0, N'PENDING', N'a8c3d7e1-26b6-46f1-945d-9fdb97a582ac', 1, CAST(N'2025-03-04T10:56:26.287' AS DateTime), CAST(N'2025-03-04T10:56:26.287' AS DateTime), NULL)
INSERT [dbo].[Blog] ([Id], [Title], [Content], [Type], [Views], [Likes], [Status], [Author_id ], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'1e915ed1-85ac-4a57-985e-32c7491463c8', N'string', N'string', N'Customer', 0, 0, N'PENDING', N'a8c3d7e1-26b6-46f1-945d-9fdb97a582ac', 1, CAST(N'2025-03-04T22:30:29.433' AS DateTime), CAST(N'2025-03-04T22:30:29.433' AS DateTime), NULL)
INSERT [dbo].[Blog] ([Id], [Title], [Content], [Type], [Views], [Likes], [Status], [Author_id ], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'1dc07319-5bd4-428d-a95d-4c79ac90bd97', N'Tại sao càng lớn tuổi càng khó ngủ', N'<p><strong>Thời lượng ngủ, thức, và khả năng ngủ thay đổi một cách tự nhiên khi ta già đi.</strong> Điều này hoàn toàn bình thường. Mặc dù đôi lúc ta cần điều chỉnh một chút và có thể hơi khó khăn, nhưng không phải lúc nào tình trạng này cũng chỉ toàn vấn đề. Việc hiểu được giấc ngủ thay đổi như thế nào khi bạn già đi có thể mang lại sức mạnh, và có một số điều chỉnh bạn có thể thực hiện trong thói quen của mình để có thể nghỉ ngơi tốt hơn khi năm tháng trôi đi.</p>', N'Customer', 0, 0, N'PENDING', N'a8c3d7e1-26b6-46f1-945d-9fdb97a582ac', 1, CAST(N'2025-03-04T22:25:34.477' AS DateTime), CAST(N'2025-03-04T22:25:34.477' AS DateTime), NULL)
INSERT [dbo].[Blog] ([Id], [Title], [Content], [Type], [Views], [Likes], [Status], [Author_id ], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'1068a694-0949-4b36-9564-551a0201b0e7', N'hello', N'<p><img src="https://firebasestorage.googleapis.com/v0/b/test-ce15e.appspot.com/o/image%2F14cbbb3c-5979-4caf-b2d8-19155edc8a0e_09d2cb443a588506dc49.jpg?alt=media"></p>', N'Customer', 0, 0, N'PENDING', N'a8c3d7e1-26b6-46f1-945d-9fdb97a582ac', 1, CAST(N'2025-03-04T14:21:37.083' AS DateTime), CAST(N'2025-03-04T14:21:37.083' AS DateTime), NULL)
INSERT [dbo].[Blog] ([Id], [Title], [Content], [Type], [Views], [Likes], [Status], [Author_id ], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'ae90b842-0734-4660-81ec-7001b2849207', N'gggggggggggg', N'<p>ggggggggggg<img src="https://firebasestorage.googleapis.com/v0/b/test-ce15e.appspot.com/o/image%2F514211a2-bb29-4ea0-8e80-8882b1bfc394_6e9639ff00b6bee8e7a7.jpg?alt=media"></p>', N'Customer', 0, 0, N'PENDING', N'a8c3d7e1-26b6-46f1-945d-9fdb97a582ac', 1, CAST(N'2025-03-04T15:55:35.037' AS DateTime), CAST(N'2025-03-04T15:55:35.037' AS DateTime), NULL)
INSERT [dbo].[Blog] ([Id], [Title], [Content], [Type], [Views], [Likes], [Status], [Author_id ], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'df6343b6-fa52-4fee-bd17-7f2e801aaa5e', N'Lời nói đầu tiên', N'<h1>Lời nói đầu tiên muốn gửi mọi người</h1><p>Hôm nay là thứ 3</p><p><img src="https://firebasestorage.googleapis.com/v0/b/test-ce15e.appspot.com/o/image%2Faa624bab-f05e-4d46-9105-c0dfc6857c58_logo%20%283%29.png?alt=media"></p>', N'Customer', 0, 0, N'PENDING', N'a8c3d7e1-26b6-46f1-945d-9fdb97a582ac', 1, CAST(N'2025-03-04T21:03:15.970' AS DateTime), CAST(N'2025-03-04T21:03:15.970' AS DateTime), NULL)
INSERT [dbo].[Blog] ([Id], [Title], [Content], [Type], [Views], [Likes], [Status], [Author_id ], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'1fcecac0-00e8-48ff-a18f-8822c09ea663', N'Tại sao càng lớn tuổi càng khó ngủ (Why Is It Harder To Sleep As You Age)?', N'<p>Cảm thấy cực kỳ khó ngủ yên mỗi đêm? Có lẽ bạn từng có thể ngủ tới chiều vào cuối tuần, nhưng gần đây, mới tinh mơ sáng là bạn đã tỉnh giấc rồi. Có lẽ bạn từng ngủ ngay lúc đặt đầu xuống gối, nhưng mấy ngày nay bạn không thể ngủ một cách dễ dàng được.<img src="https://firebasestorage.googleapis.com/v0/b/test-ce15e.appspot.com/o/image%2F34a89c97-a29d-4e71-9919-64108925270f_Screenshot%202025-02-13%20174229.png?alt=media"></p>', N'Customer', 0, 0, N'PENDING', N'a8c3d7e1-26b6-46f1-945d-9fdb97a582ac', 1, CAST(N'2025-03-06T22:16:36.850' AS DateTime), CAST(N'2025-03-06T22:16:36.850' AS DateTime), NULL)
INSERT [dbo].[Blog] ([Id], [Title], [Content], [Type], [Views], [Likes], [Status], [Author_id ], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'8635bf18-0905-4b96-88b0-894d201367d3', N'cường', N'<h1>Giới thiệu</h1><p>Đẹp điên</p>', N'Customer', 0, 0, N'PENDING', N'a8c3d7e1-26b6-46f1-945d-9fdb97a582ac', 1, CAST(N'2025-03-04T13:08:55.707' AS DateTime), CAST(N'2025-03-04T13:08:55.707' AS DateTime), NULL)
INSERT [dbo].[Blog] ([Id], [Title], [Content], [Type], [Views], [Likes], [Status], [Author_id ], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'5bc7d4d9-0a70-418f-b1d3-993bc00e548b', N'gggggggggggg', N'<p>ggggggggggg<img src="https://firebasestorage.googleapis.com/v0/b/test-ce15e.appspot.com/o/image%2F514211a2-bb29-4ea0-8e80-8882b1bfc394_6e9639ff00b6bee8e7a7.jpg?alt=media"></p>', N'Customer', 0, 0, N'PENDING', N'a8c3d7e1-26b6-46f1-945d-9fdb97a582ac', 1, CAST(N'2025-03-04T15:55:05.860' AS DateTime), CAST(N'2025-03-04T15:55:05.860' AS DateTime), NULL)
INSERT [dbo].[Blog] ([Id], [Title], [Content], [Type], [Views], [Likes], [Status], [Author_id ], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'86ad5994-9fa3-4979-9a54-a31837ad8aa3', N'string', N'string', N'Customer', 0, 0, N'PENDING', N'a8c3d7e1-26b6-46f1-945d-9fdb97a582ac', 1, CAST(N'2025-03-04T10:54:58.417' AS DateTime), CAST(N'2025-03-04T10:54:58.417' AS DateTime), NULL)
INSERT [dbo].[Blog] ([Id], [Title], [Content], [Type], [Views], [Likes], [Status], [Author_id ], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'69c62a13-199f-4786-8dee-c8429c1da253', N'Hello', N'<p><img></p>', N'Customer', 0, 0, N'PENDING', N'a8c3d7e1-26b6-46f1-945d-9fdb97a582ac', 1, CAST(N'2025-03-04T15:51:33.187' AS DateTime), CAST(N'2025-03-04T15:51:33.187' AS DateTime), NULL)
INSERT [dbo].[Blog] ([Id], [Title], [Content], [Type], [Views], [Likes], [Status], [Author_id ], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'3aebd148-aeae-4546-b41a-d20a203ec487', N'', N'<p><img src="https://firebasestorage.googleapis.com/v0/b/test-ce15e.appspot.com/o/image%2F368bc619-1b5f-4f9e-8446-54087b9bbb89_logo-instagram.png?alt=media"></p>', N'Customer', 0, 0, N'PENDING', N'a8c3d7e1-26b6-46f1-945d-9fdb97a582ac', 1, CAST(N'2025-03-04T14:20:35.400' AS DateTime), CAST(N'2025-03-04T14:20:35.400' AS DateTime), NULL)
INSERT [dbo].[Blog] ([Id], [Title], [Content], [Type], [Views], [Likes], [Status], [Author_id ], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'864f9056-c2ec-4a16-901c-de0e004dffe4', N'd', N'<p><br></p>', N'Customer', 0, 0, N'PENDING', N'a8c3d7e1-26b6-46f1-945d-9fdb97a582ac', 1, CAST(N'2025-03-04T14:14:34.983' AS DateTime), CAST(N'2025-03-04T14:14:34.983' AS DateTime), NULL)
INSERT [dbo].[Blog] ([Id], [Title], [Content], [Type], [Views], [Likes], [Status], [Author_id ], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'4798ca25-045c-4793-bd75-e47cabe97957', N'cường', N'<h1><em>Giới thiệu</em></h1><p><u>Đẹp điên</u></p><p><img></p>', N'Customer', 0, 0, N'PENDING', N'a8c3d7e1-26b6-46f1-945d-9fdb97a582ac', 1, CAST(N'2025-03-04T13:16:22.990' AS DateTime), CAST(N'2025-03-04T13:16:22.990' AS DateTime), NULL)
INSERT [dbo].[Blog] ([Id], [Title], [Content], [Type], [Views], [Likes], [Status], [Author_id ], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'389408f3-d8c1-4baf-b264-e8416cfc1356', N'Tại sao càng lớn tuổi càng khó ngủ', N'<p><strong>Thời lượng ngủ, thức, và khả năng ngủ thay đổi một cách tự nhiên khi ta già đi.</strong> Điều này hoàn toàn bình thường. Mặc dù đôi lúc ta cần điều chỉnh một chút và có thể hơi khó khăn, nhưng không phải lúc nào tình trạng này cũng chỉ toàn vấn đề. Việc hiểu được giấc ngủ thay đổi như thế nào khi bạn già đi có thể mang lại sức mạnh, và có một số điều chỉnh bạn có thể thực hiện trong thói quen của mình để có thể nghỉ ngơi tốt hơn khi năm tháng trôi đi.</p>', N'Customer', 0, 0, N'PENDING', N'a8c3d7e1-26b6-46f1-945d-9fdb97a582ac', 1, CAST(N'2025-03-04T22:34:25.403' AS DateTime), CAST(N'2025-03-04T22:34:25.403' AS DateTime), NULL)
INSERT [dbo].[Blog] ([Id], [Title], [Content], [Type], [Views], [Likes], [Status], [Author_id ], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'cd05d0f5-c5c9-464d-bf79-e9db713373e0', N'Tại sao càng lớn tuổi càng khó ngủ (Why Is It Harder To Sleep As You Age)?', N'<p>Cảm thấy cực kỳ khó ngủ yên mỗi đêm? Có lẽ bạn từng có thể ngủ tới chiều vào cuối tuần, nhưng gần đây, mới tinh mơ sáng là bạn đã tỉnh giấc rồi. Có lẽ bạn từng ngủ ngay lúc đặt đầu xuống gối, nhưng mấy ngày nay bạn không thể ngủ một cách dễ dàng được.<img src="https://firebasestorage.googleapis.com/v0/b/test-ce15e.appspot.com/o/image%2F34a89c97-a29d-4e71-9919-64108925270f_Screenshot%202025-02-13%20174229.png?alt=media"></p>', N'Customer', 0, 0, N'PENDING', N'a8c3d7e1-26b6-46f1-945d-9fdb97a582ac', 1, CAST(N'2025-03-06T22:16:49.563' AS DateTime), CAST(N'2025-03-06T22:16:49.563' AS DateTime), NULL)
INSERT [dbo].[Blog] ([Id], [Title], [Content], [Type], [Views], [Likes], [Status], [Author_id ], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'6a34454b-6649-4a0d-a283-fd7b204ae55c', N'Hello', N'<p><img></p>', N'Customer', 0, 0, N'PENDING', N'a8c3d7e1-26b6-46f1-945d-9fdb97a582ac', 1, CAST(N'2025-03-04T15:50:02.133' AS DateTime), CAST(N'2025-03-04T15:50:02.133' AS DateTime), NULL)
GO
INSERT [dbo].[Deposit] ([Id], [AccountId], [Code], [Description], [Amount], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'296fc406-65ba-459e-8487-07160d9065be', N'6b408dce-34db-46ab-9eba-6e5aae607243', N'7617493519129627', N'string', CAST(5000 AS Decimal(18, 0)), 1, CAST(N'2025-02-26T13:55:35.193' AS DateTime), CAST(N'2025-02-26T13:55:35.193' AS DateTime), NULL)
INSERT [dbo].[Deposit] ([Id], [AccountId], [Code], [Description], [Amount], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'6d67cdb9-d4d7-4239-a32c-158fe7a8e7ab', N'6b408dce-34db-46ab-9eba-6e5aae607243', N'7626510058908512', N'string', CAST(50000 AS Decimal(18, 0)), 1, CAST(N'2025-02-27T14:58:20.610' AS DateTime), CAST(N'2025-02-27T14:58:20.610' AS DateTime), NULL)
INSERT [dbo].[Deposit] ([Id], [AccountId], [Code], [Description], [Amount], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'c62b8fb2-8a8b-487d-94b3-1aa9f997414b', N'6b408dce-34db-46ab-9eba-6e5aae607243', N'7637507967778638', N'string', CAST(100000 AS Decimal(18, 0)), 1, CAST(N'2025-02-28T21:31:19.683' AS DateTime), CAST(N'2025-02-28T21:31:19.683' AS DateTime), NULL)
INSERT [dbo].[Deposit] ([Id], [AccountId], [Code], [Description], [Amount], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'6b1ce714-5953-407c-a46d-29489e0f184a', N'6b408dce-34db-46ab-9eba-6e5aae607243', N'7643726040463318', N'string', CAST(2000000 AS Decimal(18, 0)), 1, CAST(N'2025-03-01T14:47:40.413' AS DateTime), CAST(N'2025-03-01T14:47:40.413' AS DateTime), NULL)
INSERT [dbo].[Deposit] ([Id], [AccountId], [Code], [Description], [Amount], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'15307f78-43f8-4a61-bf03-2ef13bbc7ebe', N'6b408dce-34db-46ab-9eba-6e5aae607243', N'7637598738836344', N'string', CAST(300000 AS Decimal(18, 0)), 1, CAST(N'2025-02-28T21:46:27.393' AS DateTime), CAST(N'2025-02-28T21:46:27.393' AS DateTime), NULL)
INSERT [dbo].[Deposit] ([Id], [AccountId], [Code], [Description], [Amount], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'f1d3ad9a-70fa-4f4d-bc32-2fab53587047', N'7d172f31-df5d-4f84-8296-397d2a51ff21', N'7608768556504253', N'Nạp tiền vào ví', CAST(50000 AS Decimal(18, 0)), 1, CAST(N'2025-02-25T13:41:25.577' AS DateTime), CAST(N'2025-02-25T13:41:25.577' AS DateTime), NULL)
INSERT [dbo].[Deposit] ([Id], [AccountId], [Code], [Description], [Amount], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'2a7f6dc9-8639-495f-b6e7-35f779cf2ae0', N'6b408dce-34db-46ab-9eba-6e5aae607243', N'7637547944461599', N'string', CAST(30000 AS Decimal(18, 0)), 1, CAST(N'2025-02-28T21:37:59.450' AS DateTime), CAST(N'2025-02-28T21:37:59.450' AS DateTime), NULL)
INSERT [dbo].[Deposit] ([Id], [AccountId], [Code], [Description], [Amount], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'5932ff59-f6b8-48ae-9cce-3c58777b05f7', N'6b408dce-34db-46ab-9eba-6e5aae607243', N'7626274546632370', N'Nap Tien', CAST(10000 AS Decimal(18, 0)), 1, CAST(N'2025-02-27T14:19:05.473' AS DateTime), CAST(N'2025-02-27T14:19:05.473' AS DateTime), NULL)
INSERT [dbo].[Deposit] ([Id], [AccountId], [Code], [Description], [Amount], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'08058806-0f94-4164-8563-3c7d9e9084ba', N'6b408dce-34db-46ab-9eba-6e5aae607243', N'7626551710089277', N'string', CAST(50000 AS Decimal(18, 0)), 1, CAST(N'2025-02-27T15:05:17.120' AS DateTime), CAST(N'2025-02-27T15:05:17.120' AS DateTime), NULL)
INSERT [dbo].[Deposit] ([Id], [AccountId], [Code], [Description], [Amount], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'5dd83544-e960-426b-97d5-3dcee7720f9c', N'6b408dce-34db-46ab-9eba-6e5aae607243', N'7626768165890236', N'string', CAST(50000 AS Decimal(18, 0)), 1, CAST(N'2025-02-27T15:41:21.663' AS DateTime), CAST(N'2025-02-27T15:41:21.663' AS DateTime), NULL)
INSERT [dbo].[Deposit] ([Id], [AccountId], [Code], [Description], [Amount], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'106c1159-0fcf-4de4-850b-5b7b9d006b46', N'6b408dce-34db-46ab-9eba-6e5aae607243', N'7637573971914466', N'string', CAST(10000000 AS Decimal(18, 0)), 1, CAST(N'2025-02-28T21:42:19.737' AS DateTime), CAST(N'2025-02-28T21:42:19.737' AS DateTime), NULL)
INSERT [dbo].[Deposit] ([Id], [AccountId], [Code], [Description], [Amount], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'45028279-3c04-4310-8a36-5bf31a5f0de2', N'6b408dce-34db-46ab-9eba-6e5aae607243', N'7617136299042541', N'nap tien', CAST(5000 AS Decimal(18, 0)), 1, CAST(N'2025-02-26T12:56:03.013' AS DateTime), CAST(N'2025-02-26T12:56:03.013' AS DateTime), NULL)
INSERT [dbo].[Deposit] ([Id], [AccountId], [Code], [Description], [Amount], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'5aab8851-d51f-4059-ad78-6380cdd3d8c6', N'6b408dce-34db-46ab-9eba-6e5aae607243', N'7626860352243183', N'string', CAST(50000 AS Decimal(18, 0)), 1, CAST(N'2025-02-27T15:56:43.547' AS DateTime), CAST(N'2025-02-27T15:56:43.547' AS DateTime), NULL)
INSERT [dbo].[Deposit] ([Id], [AccountId], [Code], [Description], [Amount], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'2ecdca0e-beef-45ea-bbf0-6df99627a38c', N'6b408dce-34db-46ab-9eba-6e5aae607243', N'7637285963311475', N'string', CAST(10000 AS Decimal(18, 0)), 1, CAST(N'2025-02-28T20:54:19.637' AS DateTime), CAST(N'2025-02-28T20:54:19.637' AS DateTime), NULL)
INSERT [dbo].[Deposit] ([Id], [AccountId], [Code], [Description], [Amount], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'4fb41854-1ef7-4bd5-9c57-710163c3dd03', N'6b408dce-34db-46ab-9eba-6e5aae607243', N'7616666343752481', N'string', CAST(5000 AS Decimal(18, 0)), 1, CAST(N'2025-02-26T11:37:43.453' AS DateTime), CAST(N'2025-02-26T11:37:43.453' AS DateTime), NULL)
INSERT [dbo].[Deposit] ([Id], [AccountId], [Code], [Description], [Amount], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'341b3cc8-04e9-4014-9303-744fec899462', N'7d172f31-df5d-4f84-8296-397d2a51ff21', N'7615681431828070', N'Nap tien vao vi', CAST(5000 AS Decimal(18, 0)), 1, CAST(N'2025-02-26T08:53:34.340' AS DateTime), CAST(N'2025-02-26T08:53:34.340' AS DateTime), NULL)
INSERT [dbo].[Deposit] ([Id], [AccountId], [Code], [Description], [Amount], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'e40087f1-fb30-45f1-97bd-82e822a07914', N'7d172f31-df5d-4f84-8296-397d2a51ff21', N'7615694765070248', N'Nap tien vao vi', CAST(5000 AS Decimal(18, 0)), 1, CAST(N'2025-02-26T08:55:47.653' AS DateTime), CAST(N'2025-02-26T08:55:47.653' AS DateTime), NULL)
INSERT [dbo].[Deposit] ([Id], [AccountId], [Code], [Description], [Amount], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'd754386e-6e89-439a-92ee-855b80fb63ac', N'6b408dce-34db-46ab-9eba-6e5aae607243', N'7637538491995925', N'string', CAST(200000 AS Decimal(18, 0)), 1, CAST(N'2025-02-28T21:36:24.927' AS DateTime), CAST(N'2025-02-28T21:36:24.927' AS DateTime), NULL)
INSERT [dbo].[Deposit] ([Id], [AccountId], [Code], [Description], [Amount], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'dc93a9bf-158c-48af-b890-85789aebff8c', N'6b408dce-34db-46ab-9eba-6e5aae607243', N'7643749972681468', N'string', CAST(5000 AS Decimal(18, 0)), 1, CAST(N'2025-03-01T14:51:39.727' AS DateTime), CAST(N'2025-03-01T14:51:39.727' AS DateTime), NULL)
INSERT [dbo].[Deposit] ([Id], [AccountId], [Code], [Description], [Amount], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'07466856-c74c-4f50-ad78-878c7e7a359f', N'6b408dce-34db-46ab-9eba-6e5aae607243', N'7637156873462278', N'string', CAST(10000 AS Decimal(18, 0)), 1, CAST(N'2025-02-28T20:32:48.760' AS DateTime), CAST(N'2025-02-28T20:32:48.760' AS DateTime), NULL)
INSERT [dbo].[Deposit] ([Id], [AccountId], [Code], [Description], [Amount], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'9eb2d682-6671-480f-ac02-9b211fb7ff45', N'6b408dce-34db-46ab-9eba-6e5aae607243', N'7643731413761400', N'string', CAST(5000 AS Decimal(18, 0)), 1, CAST(N'2025-03-01T14:48:34.137' AS DateTime), CAST(N'2025-03-01T14:48:34.137' AS DateTime), NULL)
INSERT [dbo].[Deposit] ([Id], [AccountId], [Code], [Description], [Amount], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'54585d57-666a-4579-b10a-a598a8bc97be', N'6b408dce-34db-46ab-9eba-6e5aae607243', N'7617284159706548', N'Nap Tien', CAST(5000 AS Decimal(18, 0)), 1, CAST(N'2025-02-26T13:20:41.620' AS DateTime), CAST(N'2025-02-26T13:20:41.620' AS DateTime), NULL)
INSERT [dbo].[Deposit] ([Id], [AccountId], [Code], [Description], [Amount], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'0a4034fd-6f34-4edf-805b-a706bee63722', N'6b408dce-34db-46ab-9eba-6e5aae607243', N'7637334900457230', N'string', CAST(100000 AS Decimal(18, 0)), 1, CAST(N'2025-02-28T21:02:29.023' AS DateTime), CAST(N'2025-02-28T21:02:29.023' AS DateTime), NULL)
INSERT [dbo].[Deposit] ([Id], [AccountId], [Code], [Description], [Amount], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'915d020f-1346-4473-a77e-a735d365f0b4', N'6b408dce-34db-46ab-9eba-6e5aae607243', N'7617444375734451', N'Nap tien', CAST(5000 AS Decimal(18, 0)), 1, CAST(N'2025-02-26T13:47:23.777' AS DateTime), CAST(N'2025-02-26T13:47:23.777' AS DateTime), NULL)
INSERT [dbo].[Deposit] ([Id], [AccountId], [Code], [Description], [Amount], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'4967a346-4fb3-47e2-ada1-ab36eddd0e06', N'6b408dce-34db-46ab-9eba-6e5aae607243', N'7615998834842530', N'Nap tien', CAST(5000 AS Decimal(18, 0)), 1, CAST(N'2025-02-26T09:46:28.367' AS DateTime), CAST(N'2025-02-26T09:46:28.367' AS DateTime), NULL)
INSERT [dbo].[Deposit] ([Id], [AccountId], [Code], [Description], [Amount], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'a57d1036-7499-4629-ad40-aefc57213351', N'6b408dce-34db-46ab-9eba-6e5aae607243', N'7626298931971239', N'string', CAST(20000 AS Decimal(18, 0)), 1, CAST(N'2025-02-27T14:23:09.343' AS DateTime), CAST(N'2025-02-27T14:23:09.343' AS DateTime), NULL)
INSERT [dbo].[Deposit] ([Id], [AccountId], [Code], [Description], [Amount], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'd4aab9fa-32d3-4ec8-aeae-d2e16f095134', N'6b408dce-34db-46ab-9eba-6e5aae607243', N'7634383587518605', N'string', CAST(5000 AS Decimal(18, 0)), 1, CAST(N'2025-02-28T12:50:35.897' AS DateTime), CAST(N'2025-02-28T12:50:35.897' AS DateTime), NULL)
INSERT [dbo].[Deposit] ([Id], [AccountId], [Code], [Description], [Amount], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'9a0dbd7f-2ac2-4412-9f95-d3524608e691', N'6b408dce-34db-46ab-9eba-6e5aae607243', N'7615960001405498', N'Nap tien', CAST(5000 AS Decimal(18, 0)), 1, CAST(N'2025-02-26T09:40:00.027' AS DateTime), CAST(N'2025-02-26T09:40:00.027' AS DateTime), NULL)
INSERT [dbo].[Deposit] ([Id], [AccountId], [Code], [Description], [Amount], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'd3b49c4d-3149-4e43-81db-d3891e2c66ac', N'7d172f31-df5d-4f84-8296-397d2a51ff21', N'7615743582859676', N'Nap tien vao vi', CAST(5000 AS Decimal(18, 0)), 1, CAST(N'2025-02-26T09:03:55.830' AS DateTime), CAST(N'2025-02-26T09:03:55.830' AS DateTime), NULL)
INSERT [dbo].[Deposit] ([Id], [AccountId], [Code], [Description], [Amount], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'abdf8e33-e0aa-424b-b68c-d8e0a582c13f', N'6b408dce-34db-46ab-9eba-6e5aae607243', N'7637322041803136', N'string', CAST(100000 AS Decimal(18, 0)), 1, CAST(N'2025-02-28T21:00:20.440' AS DateTime), CAST(N'2025-02-28T21:00:20.440' AS DateTime), NULL)
INSERT [dbo].[Deposit] ([Id], [AccountId], [Code], [Description], [Amount], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'b35c5046-c0da-44dc-a539-dea2f3771db8', N'6b408dce-34db-46ab-9eba-6e5aae607243', N'7637232206993886', N'string', CAST(10000 AS Decimal(18, 0)), 1, CAST(N'2025-02-28T20:45:22.087' AS DateTime), CAST(N'2025-02-28T20:45:22.087' AS DateTime), NULL)
INSERT [dbo].[Deposit] ([Id], [AccountId], [Code], [Description], [Amount], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'a86b9896-0a1b-4ac9-a28c-e2ed19fb1ff5', N'6b408dce-34db-46ab-9eba-6e5aae607243', N'7653420866085589', N'string', CAST(5000 AS Decimal(18, 0)), 1, CAST(N'2025-03-02T17:43:28.683' AS DateTime), CAST(N'2025-03-02T17:43:28.683' AS DateTime), NULL)
INSERT [dbo].[Deposit] ([Id], [AccountId], [Code], [Description], [Amount], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'4804c639-b4e9-4c3e-b59b-e431bc85fd7a', N'6b408dce-34db-46ab-9eba-6e5aae607243', N'7637515512001100', N'string', CAST(100000 AS Decimal(18, 0)), 1, CAST(N'2025-02-28T21:32:35.127' AS DateTime), CAST(N'2025-02-28T21:32:35.127' AS DateTime), NULL)
GO
INSERT [dbo].[ListenerInfo] ([Id], [AccountId], [Description], [Star], [Price], [Type], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'689ac8d0-40b9-40ef-94fa-22b2daf06428', N'41225bc2-9415-45f5-bcbe-dde495ed27a7', N'123', 0, CAST(123 AS Decimal(18, 0)), N'Share', 1, CAST(N'2025-03-05T21:19:56.383' AS DateTime), CAST(N'2025-03-05T22:39:20.233' AS DateTime), CAST(N'2025-03-05T22:39:20.233' AS DateTime))
INSERT [dbo].[ListenerInfo] ([Id], [AccountId], [Description], [Star], [Price], [Type], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'ee2fef91-364a-4a94-80b9-3ebc113e720f', N'a5e3ac44-0840-43f2-8d73-fa614f25e34d', N'dddd', 0, CAST(12 AS Decimal(18, 0)), N'Tarot', 1, CAST(N'2025-03-05T21:02:17.097' AS DateTime), CAST(N'2025-03-06T09:06:22.033' AS DateTime), CAST(N'2025-03-06T09:06:22.030' AS DateTime))
INSERT [dbo].[ListenerInfo] ([Id], [AccountId], [Description], [Star], [Price], [Type], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'18c5314d-4880-45be-9e92-65adcbb3d435', N'd26d36c0-0692-4bc6-bafe-49698f45a657', N'123', 0, CAST(100000 AS Decimal(18, 0)), N'Share', 1, CAST(N'2025-03-05T23:03:20.667' AS DateTime), CAST(N'2025-03-05T23:03:20.667' AS DateTime), NULL)
INSERT [dbo].[ListenerInfo] ([Id], [AccountId], [Description], [Star], [Price], [Type], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'912831e1-04de-46e1-b762-68bb2853d37a', N'10280fb5-bf7e-46b5-be57-78baf041c83f', N'string', 0, CAST(0 AS Decimal(18, 0)), N'Tarot', 1, CAST(N'2025-03-05T10:46:31.363' AS DateTime), CAST(N'2025-03-06T09:08:44.833' AS DateTime), CAST(N'2025-03-06T09:08:44.833' AS DateTime))
INSERT [dbo].[ListenerInfo] ([Id], [AccountId], [Description], [Star], [Price], [Type], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'76d642ee-feb9-4d05-901b-a206339862ec', N'eb875212-93e3-4696-96e3-71e3ce01f1aa', N'kkkkkk', 0, CAST(70000 AS Decimal(18, 0)), N'Tarot', 1, CAST(N'2025-03-05T21:09:45.267' AS DateTime), CAST(N'2025-03-05T21:09:45.267' AS DateTime), NULL)
INSERT [dbo].[ListenerInfo] ([Id], [AccountId], [Description], [Star], [Price], [Type], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'37b6a8b3-b9e9-4356-ac14-c05e17fa93b3', N'100f00b9-58dc-48c5-97b5-fa72902a6522', N'kkkkkk', 0, CAST(70000 AS Decimal(18, 0)), N'Tarot', 1, CAST(N'2025-03-05T21:10:45.017' AS DateTime), CAST(N'2025-03-05T21:44:19.143' AS DateTime), CAST(N'2025-03-05T21:44:19.143' AS DateTime))
INSERT [dbo].[ListenerInfo] ([Id], [AccountId], [Description], [Star], [Price], [Type], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'b241c591-c515-436c-9e50-f0f3aaa4336e', N'2d96b34d-cb42-4dde-bcda-7e3899dde181', N'string', 0, CAST(0 AS Decimal(18, 0)), N'Tarot', 1, CAST(N'2025-03-02T21:42:14.050' AS DateTime), CAST(N'2025-03-05T22:39:38.260' AS DateTime), CAST(N'2025-03-05T22:39:38.260' AS DateTime))
GO
INSERT [dbo].[Question] ([Id], [Content], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'58b10fb6-2e9a-4b65-b34e-282450caf084', N'tâm lý', 1, CAST(N'2025-02-28T22:43:13.550' AS DateTime), CAST(N'2025-02-28T22:47:53.697' AS DateTime), NULL)
INSERT [dbo].[Question] ([Id], [Content], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'e8e220f2-c0c5-404f-afd9-29743cc7a6ae', N'Câu hỏi 1', 1, CAST(N'2025-02-25T09:37:44.773' AS DateTime), CAST(N'2025-02-25T09:37:44.777' AS DateTime), NULL)
GO
INSERT [dbo].[RefreshToken] ([Id], [UserId], [Token], [ExpirationTime]) VALUES (N'd8b40171-dc57-413a-ab41-1bbd63a06685', N'6b408dce-34db-46ab-9eba-6e5aae607243', N'7mK5yo/giG0dq2A8cQ2fEAaSbtPYWeeh8MwbhhxtXjk=', CAST(N'2025-04-05T22:08:27.793' AS DateTime))
INSERT [dbo].[RefreshToken] ([Id], [UserId], [Token], [ExpirationTime]) VALUES (N'48091a51-9894-44ce-a8a6-27143a09a49a', N'6b408dce-34db-46ab-9eba-6e5aae607243', N'GOJ4JHlQobwOVvOxzo/05dgwJyHZ7fwNx6PAGyNRfJM=', CAST(N'2025-04-05T22:15:10.857' AS DateTime))
INSERT [dbo].[RefreshToken] ([Id], [UserId], [Token], [ExpirationTime]) VALUES (N'645fd7cf-f96b-43a0-a3b2-3eebfc1699f3', N'a8c3d7e1-26b6-46f1-945d-9fdb97a582ac', N'tzW0hpWketXh+NcClqXnLjdHYx68cBN79pSMJp9TaOY=', CAST(N'2025-04-05T22:16:03.690' AS DateTime))
INSERT [dbo].[RefreshToken] ([Id], [UserId], [Token], [ExpirationTime]) VALUES (N'd0ef7b34-fef0-48bc-906e-476e676c393a', N'a8c3d7e1-26b6-46f1-945d-9fdb97a582ac', N'WlKsdrRgESD2ZxwkJvrIf24O9pOltOkxkoMWfRJh/jQ=', CAST(N'2025-04-05T17:25:10.697' AS DateTime))
INSERT [dbo].[RefreshToken] ([Id], [UserId], [Token], [ExpirationTime]) VALUES (N'126a9ad8-a6d9-42e0-889e-50746f532302', N'e9d06dda-5719-49b5-89b3-d0cc184ce119', N'heteeohAu76vXrqefFLsFAcjr4Zahqj8h4iQCF+3nEg=', CAST(N'2025-04-05T21:57:51.997' AS DateTime))
INSERT [dbo].[RefreshToken] ([Id], [UserId], [Token], [ExpirationTime]) VALUES (N'bc85cc8e-6fa5-4bef-bb26-69c4dd291e1b', N'6b408dce-34db-46ab-9eba-6e5aae607243', N'UoIPtRHU/7YXezyPy6ecusM0bPfZgKV/fSCBhWZwZNY=', CAST(N'2025-04-05T22:08:27.580' AS DateTime))
INSERT [dbo].[RefreshToken] ([Id], [UserId], [Token], [ExpirationTime]) VALUES (N'f5731e33-6e2a-4b63-bbbd-6b233aa6d7b3', N'a8c3d7e1-26b6-46f1-945d-9fdb97a582ac', N'4fEBdUaGRooCjT2jNXvMATJNfhyiHpMLSju0c0Kb9ug=', CAST(N'2025-04-06T13:32:33.300' AS DateTime))
INSERT [dbo].[RefreshToken] ([Id], [UserId], [Token], [ExpirationTime]) VALUES (N'f33b09d0-0877-49f3-9e2a-79f42b2a08af', N'3c7d8982-6f92-4346-812a-901fe3e670c0', N'UViIpg9qKTYN3UxJS/WWnQhAq5lqBpTMAqiU7Tk09Ws=', CAST(N'2025-04-05T14:27:43.687' AS DateTime))
INSERT [dbo].[RefreshToken] ([Id], [UserId], [Token], [ExpirationTime]) VALUES (N'f603b293-2514-422d-a63a-964047efe9db', N'e9d06dda-5719-49b5-89b3-d0cc184ce119', N'pv0zQbbX6+u3BTIVz4NGQrUQxNhABsA29gHBZZNJ0C4=', CAST(N'2025-04-05T20:47:46.140' AS DateTime))
INSERT [dbo].[RefreshToken] ([Id], [UserId], [Token], [ExpirationTime]) VALUES (N'2e4c1f63-ccfe-4ff9-8c21-99892a1004f6', N'6b408dce-34db-46ab-9eba-6e5aae607243', N'DVReVNSc8XRvtWViHZeFP/OyOlwmF9gj0omN3FzjvdM=', CAST(N'2025-04-05T22:08:37.580' AS DateTime))
INSERT [dbo].[RefreshToken] ([Id], [UserId], [Token], [ExpirationTime]) VALUES (N'5132e05b-bec2-4a8e-955d-9ab23c7834f0', N'cab5e345-f36c-4c9f-8485-c92ba65af5df', N'OnN6OF/tvdpfYB1To9BljQOhKHSxaTZS2qc+FqoLULA=', CAST(N'2025-04-05T20:59:23.260' AS DateTime))
INSERT [dbo].[RefreshToken] ([Id], [UserId], [Token], [ExpirationTime]) VALUES (N'1c6c7af3-fff2-4c43-b9ba-abc2acdf824c', N'6b408dce-34db-46ab-9eba-6e5aae607243', N'44HJ/zRVsgVZRELCCLtX2pi05Iqrc+nX8SCSRn7mpAU=', CAST(N'2025-04-05T22:08:28.027' AS DateTime))
INSERT [dbo].[RefreshToken] ([Id], [UserId], [Token], [ExpirationTime]) VALUES (N'd907cb21-b320-40f7-a485-b07213469aef', N'6b408dce-34db-46ab-9eba-6e5aae607243', N'aqOi6lQ0u9UYmg3qK8OWB4n1MXYQrOTmiyaCU+gCyDY=', CAST(N'2025-04-05T22:15:08.003' AS DateTime))
INSERT [dbo].[RefreshToken] ([Id], [UserId], [Token], [ExpirationTime]) VALUES (N'a01acae4-cc21-4358-a8fe-c3955b56455a', N'6b408dce-34db-46ab-9eba-6e5aae607243', N'X8FwuH0WwU0DQhD/acfziOVU4tnn9+3lj2RDD01fY4I=', CAST(N'2025-04-05T22:08:26.230' AS DateTime))
INSERT [dbo].[RefreshToken] ([Id], [UserId], [Token], [ExpirationTime]) VALUES (N'c90355bf-a995-46e6-8aef-e37f7f116576', N'a8c3d7e1-26b6-46f1-945d-9fdb97a582ac', N'bnYhPsbWPwEaYFko7J6If8ORdbQ5059FDOcisb1KJs4=', CAST(N'2025-04-06T15:25:46.210' AS DateTime))
INSERT [dbo].[RefreshToken] ([Id], [UserId], [Token], [ExpirationTime]) VALUES (N'e4283e6b-9566-4d18-924f-e9dec598a224', N'6b408dce-34db-46ab-9eba-6e5aae607243', N'6G/fELoGzr/QbUWpDbbG7b18wsArBrrJ0KHEk/qvBUY=', CAST(N'2025-04-05T22:07:55.920' AS DateTime))
GO
INSERT [dbo].[Topic] ([Id], [ListenerInfoId], [Name], [Translate], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'27d84878-0a55-4062-844f-124feac7876e', N'b241c591-c515-436c-9e50-f0f3aaa4336e', N'Trầm Cảm', 0, 0, CAST(N'2025-03-02T21:44:53.730' AS DateTime), CAST(N'2025-03-05T22:39:38.250' AS DateTime), CAST(N'2025-03-05T22:39:38.250' AS DateTime))
INSERT [dbo].[Topic] ([Id], [ListenerInfoId], [Name], [Translate], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'2e790c29-bcfd-452d-ad21-4c2ca363763f', N'b241c591-c515-436c-9e50-f0f3aaa4336e', N'Stress Lo Âu', 0, 0, CAST(N'2025-03-02T21:43:14.053' AS DateTime), CAST(N'2025-03-05T22:39:38.260' AS DateTime), CAST(N'2025-03-05T22:39:38.260' AS DateTime))
INSERT [dbo].[Topic] ([Id], [ListenerInfoId], [Name], [Translate], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'6ef5aa17-7e17-499a-9db3-c7eb878b4ec9', N'b241c591-c515-436c-9e50-f0f3aaa4336e', N'Mối Quan Hệ (gia đình, bạn bè, tình yêu)', 0, 0, CAST(N'2025-03-02T22:04:10.983' AS DateTime), CAST(N'2025-03-05T22:39:38.260' AS DateTime), CAST(N'2025-03-05T22:39:38.260' AS DateTime))
GO
INSERT [dbo].[Transaction] ([Id], [WalletId], [DepositId], [Amount], [OrderCode], [IsActive], [CreateAt], [UpdateAt], [DeleteAt], [Status], [Type]) VALUES (N'24e9ef63-9cc5-49fe-8bcf-1c5200bbc598', N'c685a469-ab3d-4a05-8f56-ed7260444a3a', N'd754386e-6e89-439a-92ee-855b80fb63ac', CAST(200000 AS Decimal(18, 0)), 7637538491995925, 1, CAST(N'2025-02-28T21:36:24.977' AS DateTime), CAST(N'2025-02-28T21:36:24.977' AS DateTime), NULL, N'PENDING', N'DEPOSIT')
INSERT [dbo].[Transaction] ([Id], [WalletId], [DepositId], [Amount], [OrderCode], [IsActive], [CreateAt], [UpdateAt], [DeleteAt], [Status], [Type]) VALUES (N'688db60d-bc46-4837-8797-250789602ae7', N'c685a469-ab3d-4a05-8f56-ed7260444a3a', N'45028279-3c04-4310-8a36-5bf31a5f0de2', CAST(5000 AS Decimal(18, 0)), 7617136299042541, 1, CAST(N'2025-02-26T12:56:03.067' AS DateTime), CAST(N'2025-02-26T13:46:16.410' AS DateTime), NULL, N'SUCCESS', N'DEPOSIT')
INSERT [dbo].[Transaction] ([Id], [WalletId], [DepositId], [Amount], [OrderCode], [IsActive], [CreateAt], [UpdateAt], [DeleteAt], [Status], [Type]) VALUES (N'1f065090-7f1f-463c-99d4-25d16fa07168', N'c685a469-ab3d-4a05-8f56-ed7260444a3a', N'4804c639-b4e9-4c3e-b59b-e431bc85fd7a', CAST(100000 AS Decimal(18, 0)), 7637515512001100, 1, CAST(N'2025-02-28T21:32:35.173' AS DateTime), CAST(N'2025-02-28T21:32:35.173' AS DateTime), NULL, N'PENDING', N'DEPOSIT')
INSERT [dbo].[Transaction] ([Id], [WalletId], [DepositId], [Amount], [OrderCode], [IsActive], [CreateAt], [UpdateAt], [DeleteAt], [Status], [Type]) VALUES (N'fc22090f-c1ea-4e3a-87f5-26599a3a7478', N'c685a469-ab3d-4a05-8f56-ed7260444a3a', N'15307f78-43f8-4a61-bf03-2ef13bbc7ebe', CAST(300000 AS Decimal(18, 0)), 7637598738836344, 1, CAST(N'2025-02-28T21:46:27.393' AS DateTime), CAST(N'2025-02-28T21:46:37.950' AS DateTime), NULL, N'FAILED', N'DEPOSIT')
INSERT [dbo].[Transaction] ([Id], [WalletId], [DepositId], [Amount], [OrderCode], [IsActive], [CreateAt], [UpdateAt], [DeleteAt], [Status], [Type]) VALUES (N'aa559a31-1f29-4b74-8deb-2703c7bf2172', N'c685a469-ab3d-4a05-8f56-ed7260444a3a', N'296fc406-65ba-459e-8487-07160d9065be', CAST(5000 AS Decimal(18, 0)), 7617493519129627, 1, CAST(N'2025-02-26T13:55:35.197' AS DateTime), CAST(N'2025-02-26T13:56:24.033' AS DateTime), NULL, N'SUCCESS', N'DEPOSIT')
INSERT [dbo].[Transaction] ([Id], [WalletId], [DepositId], [Amount], [OrderCode], [IsActive], [CreateAt], [UpdateAt], [DeleteAt], [Status], [Type]) VALUES (N'bdb3bbe6-3cd9-496c-8047-31c898f8fb13', N'c685a469-ab3d-4a05-8f56-ed7260444a3a', N'106c1159-0fcf-4de4-850b-5b7b9d006b46', CAST(10000000 AS Decimal(18, 0)), 7637573971914466, 1, CAST(N'2025-02-28T21:42:19.790' AS DateTime), CAST(N'2025-02-28T21:42:29.480' AS DateTime), NULL, N'FAILED', N'DEPOSIT')
INSERT [dbo].[Transaction] ([Id], [WalletId], [DepositId], [Amount], [OrderCode], [IsActive], [CreateAt], [UpdateAt], [DeleteAt], [Status], [Type]) VALUES (N'716a7f87-a077-41d9-b8b2-43e043b6acf2', N'c685a469-ab3d-4a05-8f56-ed7260444a3a', N'a86b9896-0a1b-4ac9-a28c-e2ed19fb1ff5', CAST(5000 AS Decimal(18, 0)), 7653420866085589, 1, CAST(N'2025-03-02T17:43:28.737' AS DateTime), CAST(N'2025-03-02T17:43:28.740' AS DateTime), NULL, N'PENDING', N'DEPOSIT')
INSERT [dbo].[Transaction] ([Id], [WalletId], [DepositId], [Amount], [OrderCode], [IsActive], [CreateAt], [UpdateAt], [DeleteAt], [Status], [Type]) VALUES (N'9a68891e-9e05-48b0-a8f1-4c6c49be3486', N'c685a469-ab3d-4a05-8f56-ed7260444a3a', N'2a7f6dc9-8639-495f-b6e7-35f779cf2ae0', CAST(30000 AS Decimal(18, 0)), 7637547944461599, 1, CAST(N'2025-02-28T21:37:59.497' AS DateTime), CAST(N'2025-02-28T21:38:09.730' AS DateTime), NULL, N'FAILED', N'DEPOSIT')
INSERT [dbo].[Transaction] ([Id], [WalletId], [DepositId], [Amount], [OrderCode], [IsActive], [CreateAt], [UpdateAt], [DeleteAt], [Status], [Type]) VALUES (N'dab9cdd4-5620-4c91-b424-4df46689840a', N'c685a469-ab3d-4a05-8f56-ed7260444a3a', N'9a0dbd7f-2ac2-4412-9f95-d3524608e691', CAST(5000 AS Decimal(18, 0)), NULL, 1, CAST(N'2025-02-26T09:40:00.067' AS DateTime), CAST(N'2025-02-26T09:40:00.067' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Transaction] ([Id], [WalletId], [DepositId], [Amount], [OrderCode], [IsActive], [CreateAt], [UpdateAt], [DeleteAt], [Status], [Type]) VALUES (N'a4e4160e-3d46-4a8b-b101-4ea99c78a4e0', N'c685a469-ab3d-4a05-8f56-ed7260444a3a', N'6b1ce714-5953-407c-a46d-29489e0f184a', CAST(2000000 AS Decimal(18, 0)), 7643726040463318, 1, CAST(N'2025-03-01T14:47:40.430' AS DateTime), CAST(N'2025-03-01T14:48:22.307' AS DateTime), NULL, N'FAILED', N'DEPOSIT')
INSERT [dbo].[Transaction] ([Id], [WalletId], [DepositId], [Amount], [OrderCode], [IsActive], [CreateAt], [UpdateAt], [DeleteAt], [Status], [Type]) VALUES (N'63c20cb1-7a85-4e19-b293-604b14ea75bf', N'c685a469-ab3d-4a05-8f56-ed7260444a3a', N'08058806-0f94-4164-8563-3c7d9e9084ba', CAST(50000 AS Decimal(18, 0)), 7626551710089277, 1, CAST(N'2025-02-27T15:05:17.303' AS DateTime), CAST(N'2025-02-27T15:05:17.303' AS DateTime), NULL, N'PENDING', N'DEPOSIT')
INSERT [dbo].[Transaction] ([Id], [WalletId], [DepositId], [Amount], [OrderCode], [IsActive], [CreateAt], [UpdateAt], [DeleteAt], [Status], [Type]) VALUES (N'5a5c23bc-83e3-493d-94d3-66f56a66ca77', N'c685a469-ab3d-4a05-8f56-ed7260444a3a', N'5932ff59-f6b8-48ae-9cce-3c58777b05f7', CAST(10000 AS Decimal(18, 0)), 7626274546632370, 1, CAST(N'2025-02-27T14:19:05.497' AS DateTime), CAST(N'2025-02-27T14:19:05.497' AS DateTime), NULL, N'PENDING', N'DEPOSIT')
INSERT [dbo].[Transaction] ([Id], [WalletId], [DepositId], [Amount], [OrderCode], [IsActive], [CreateAt], [UpdateAt], [DeleteAt], [Status], [Type]) VALUES (N'6b60a3df-64f1-4fd2-91a6-67557922d488', N'c685a469-ab3d-4a05-8f56-ed7260444a3a', N'07466856-c74c-4f50-ad78-878c7e7a359f', CAST(10000 AS Decimal(18, 0)), 7637156873462278, 1, CAST(N'2025-02-28T20:32:48.830' AS DateTime), CAST(N'2025-02-28T20:32:48.830' AS DateTime), NULL, N'PENDING', N'DEPOSIT')
INSERT [dbo].[Transaction] ([Id], [WalletId], [DepositId], [Amount], [OrderCode], [IsActive], [CreateAt], [UpdateAt], [DeleteAt], [Status], [Type]) VALUES (N'a6ec41a6-3f59-40a4-be3e-69279e8487d6', N'c685a469-ab3d-4a05-8f56-ed7260444a3a', N'4967a346-4fb3-47e2-ada1-ab36eddd0e06', CAST(5000 AS Decimal(18, 0)), NULL, 1, CAST(N'2025-02-26T09:46:28.423' AS DateTime), CAST(N'2025-02-26T09:46:28.423' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Transaction] ([Id], [WalletId], [DepositId], [Amount], [OrderCode], [IsActive], [CreateAt], [UpdateAt], [DeleteAt], [Status], [Type]) VALUES (N'54738004-64b3-4647-b4cc-7aee5b04a19b', N'c685a469-ab3d-4a05-8f56-ed7260444a3a', N'abdf8e33-e0aa-424b-b68c-d8e0a582c13f', CAST(100000 AS Decimal(18, 0)), 7637322041803136, 1, CAST(N'2025-02-28T21:00:20.527' AS DateTime), CAST(N'2025-02-28T21:00:20.527' AS DateTime), NULL, N'PENDING', N'DEPOSIT')
INSERT [dbo].[Transaction] ([Id], [WalletId], [DepositId], [Amount], [OrderCode], [IsActive], [CreateAt], [UpdateAt], [DeleteAt], [Status], [Type]) VALUES (N'42ef4dc5-7a76-46e6-87c5-88190ab4d8a0', N'c685a469-ab3d-4a05-8f56-ed7260444a3a', N'2ecdca0e-beef-45ea-bbf0-6df99627a38c', CAST(10000 AS Decimal(18, 0)), 7637285963311475, 1, CAST(N'2025-02-28T20:54:19.640' AS DateTime), CAST(N'2025-02-28T20:54:19.640' AS DateTime), NULL, N'PENDING', N'DEPOSIT')
INSERT [dbo].[Transaction] ([Id], [WalletId], [DepositId], [Amount], [OrderCode], [IsActive], [CreateAt], [UpdateAt], [DeleteAt], [Status], [Type]) VALUES (N'2539e0d9-7db1-4790-9d31-89f99a73b60a', N'c685a469-ab3d-4a05-8f56-ed7260444a3a', N'6d67cdb9-d4d7-4239-a32c-158fe7a8e7ab', CAST(50000 AS Decimal(18, 0)), 7626510058908512, 1, CAST(N'2025-02-27T14:58:20.843' AS DateTime), CAST(N'2025-02-27T14:58:20.843' AS DateTime), NULL, N'PENDING', N'DEPOSIT')
INSERT [dbo].[Transaction] ([Id], [WalletId], [DepositId], [Amount], [OrderCode], [IsActive], [CreateAt], [UpdateAt], [DeleteAt], [Status], [Type]) VALUES (N'e2d72fa8-73f1-4567-9e7e-8da29e4bcf82', N'c685a469-ab3d-4a05-8f56-ed7260444a3a', N'd4aab9fa-32d3-4ec8-aeae-d2e16f095134', CAST(5000 AS Decimal(18, 0)), 7634383587518605, 1, CAST(N'2025-02-28T12:50:35.957' AS DateTime), CAST(N'2025-02-28T12:51:37.917' AS DateTime), NULL, N'SUCCESS', N'DEPOSIT')
INSERT [dbo].[Transaction] ([Id], [WalletId], [DepositId], [Amount], [OrderCode], [IsActive], [CreateAt], [UpdateAt], [DeleteAt], [Status], [Type]) VALUES (N'1de8a0b3-0dd8-4150-bf26-960fc029d910', N'c685a469-ab3d-4a05-8f56-ed7260444a3a', N'b35c5046-c0da-44dc-a539-dea2f3771db8', CAST(10000 AS Decimal(18, 0)), 7637232206993886, 1, CAST(N'2025-02-28T20:45:22.143' AS DateTime), CAST(N'2025-02-28T20:45:22.143' AS DateTime), NULL, N'PENDING', N'DEPOSIT')
INSERT [dbo].[Transaction] ([Id], [WalletId], [DepositId], [Amount], [OrderCode], [IsActive], [CreateAt], [UpdateAt], [DeleteAt], [Status], [Type]) VALUES (N'6b717e74-6ba2-4507-b868-ac59e410e86d', N'7d172f31-df5d-4f84-8296-397d2a51ff21', N'd3b49c4d-3149-4e43-81db-d3891e2c66ac', CAST(5000 AS Decimal(18, 0)), NULL, 1, CAST(N'2025-02-26T09:03:55.833' AS DateTime), CAST(N'2025-02-26T09:03:55.833' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Transaction] ([Id], [WalletId], [DepositId], [Amount], [OrderCode], [IsActive], [CreateAt], [UpdateAt], [DeleteAt], [Status], [Type]) VALUES (N'4c95a5aa-cde9-416b-bacc-ac6d86a53886', N'c685a469-ab3d-4a05-8f56-ed7260444a3a', N'9eb2d682-6671-480f-ac02-9b211fb7ff45', CAST(5000 AS Decimal(18, 0)), 7643731413761400, 1, CAST(N'2025-03-01T14:48:34.140' AS DateTime), CAST(N'2025-03-01T14:50:10.717' AS DateTime), NULL, N'SUCCESS', N'DEPOSIT')
INSERT [dbo].[Transaction] ([Id], [WalletId], [DepositId], [Amount], [OrderCode], [IsActive], [CreateAt], [UpdateAt], [DeleteAt], [Status], [Type]) VALUES (N'62253d5e-1147-44ec-a2cf-b06173dc01ea', N'c685a469-ab3d-4a05-8f56-ed7260444a3a', N'dc93a9bf-158c-48af-b890-85789aebff8c', CAST(5000 AS Decimal(18, 0)), 7643749972681468, 1, CAST(N'2025-03-01T14:51:39.727' AS DateTime), CAST(N'2025-03-01T14:51:39.727' AS DateTime), NULL, N'PENDING', N'DEPOSIT')
INSERT [dbo].[Transaction] ([Id], [WalletId], [DepositId], [Amount], [OrderCode], [IsActive], [CreateAt], [UpdateAt], [DeleteAt], [Status], [Type]) VALUES (N'5c73ef5c-a764-49f8-a870-beb7667f0792', N'c685a469-ab3d-4a05-8f56-ed7260444a3a', N'54585d57-666a-4579-b10a-a598a8bc97be', CAST(5000 AS Decimal(18, 0)), 7617284159706548, 1, CAST(N'2025-02-26T13:20:41.680' AS DateTime), CAST(N'2025-02-26T13:45:49.993' AS DateTime), NULL, N'SUCCESS', N'DEPOSIT')
INSERT [dbo].[Transaction] ([Id], [WalletId], [DepositId], [Amount], [OrderCode], [IsActive], [CreateAt], [UpdateAt], [DeleteAt], [Status], [Type]) VALUES (N'253f8e5b-e265-4700-9bef-c8f46f6ae646', N'7d172f31-df5d-4f84-8296-397d2a51ff21', N'341b3cc8-04e9-4014-9303-744fec899462', CAST(5000 AS Decimal(18, 0)), NULL, 1, CAST(N'2025-02-26T08:53:34.397' AS DateTime), CAST(N'2025-02-26T08:53:34.397' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Transaction] ([Id], [WalletId], [DepositId], [Amount], [OrderCode], [IsActive], [CreateAt], [UpdateAt], [DeleteAt], [Status], [Type]) VALUES (N'bad495ae-7d50-4952-b8c5-cd3ae8d08848', N'c685a469-ab3d-4a05-8f56-ed7260444a3a', N'a57d1036-7499-4629-ad40-aefc57213351', CAST(20000 AS Decimal(18, 0)), 7626298931971239, 1, CAST(N'2025-02-27T14:23:09.517' AS DateTime), CAST(N'2025-02-27T14:23:09.517' AS DateTime), NULL, N'PENDING', N'DEPOSIT')
INSERT [dbo].[Transaction] ([Id], [WalletId], [DepositId], [Amount], [OrderCode], [IsActive], [CreateAt], [UpdateAt], [DeleteAt], [Status], [Type]) VALUES (N'c3f14aee-14cb-4340-8e61-d58fddceb386', N'c685a469-ab3d-4a05-8f56-ed7260444a3a', N'4fb41854-1ef7-4bd5-9c57-710163c3dd03', CAST(5000 AS Decimal(18, 0)), 7616666343752481, 1, CAST(N'2025-02-26T11:37:43.507' AS DateTime), CAST(N'2025-02-26T11:37:43.507' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Transaction] ([Id], [WalletId], [DepositId], [Amount], [OrderCode], [IsActive], [CreateAt], [UpdateAt], [DeleteAt], [Status], [Type]) VALUES (N'5b075ade-9bb2-470f-b4ac-d93740672698', N'7d172f31-df5d-4f84-8296-397d2a51ff21', N'e40087f1-fb30-45f1-97bd-82e822a07914', CAST(5000 AS Decimal(18, 0)), NULL, 1, CAST(N'2025-02-26T08:55:47.657' AS DateTime), CAST(N'2025-02-26T08:55:47.657' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Transaction] ([Id], [WalletId], [DepositId], [Amount], [OrderCode], [IsActive], [CreateAt], [UpdateAt], [DeleteAt], [Status], [Type]) VALUES (N'52b3276c-ce67-4a2c-ad82-e217828eea23', N'c685a469-ab3d-4a05-8f56-ed7260444a3a', N'c62b8fb2-8a8b-487d-94b3-1aa9f997414b', CAST(100000 AS Decimal(18, 0)), 7637507967778638, 1, CAST(N'2025-02-28T21:31:19.703' AS DateTime), CAST(N'2025-02-28T21:31:19.703' AS DateTime), NULL, N'PENDING', N'DEPOSIT')
INSERT [dbo].[Transaction] ([Id], [WalletId], [DepositId], [Amount], [OrderCode], [IsActive], [CreateAt], [UpdateAt], [DeleteAt], [Status], [Type]) VALUES (N'0384e041-1778-4117-b178-e3d889c680de', N'c685a469-ab3d-4a05-8f56-ed7260444a3a', N'915d020f-1346-4473-a77e-a735d365f0b4', CAST(5000 AS Decimal(18, 0)), 7617444375734451, 1, CAST(N'2025-02-26T13:47:23.817' AS DateTime), CAST(N'2025-02-26T13:48:49.130' AS DateTime), NULL, N'SUCCESS', N'DEPOSIT')
INSERT [dbo].[Transaction] ([Id], [WalletId], [DepositId], [Amount], [OrderCode], [IsActive], [CreateAt], [UpdateAt], [DeleteAt], [Status], [Type]) VALUES (N'5468b3dd-948e-469c-ab39-ee3cbacfde26', N'c685a469-ab3d-4a05-8f56-ed7260444a3a', N'0a4034fd-6f34-4edf-805b-a706bee63722', CAST(100000 AS Decimal(18, 0)), 7637334900457230, 1, CAST(N'2025-02-28T21:02:29.180' AS DateTime), CAST(N'2025-02-28T21:02:29.183' AS DateTime), NULL, N'PENDING', N'DEPOSIT')
INSERT [dbo].[Transaction] ([Id], [WalletId], [DepositId], [Amount], [OrderCode], [IsActive], [CreateAt], [UpdateAt], [DeleteAt], [Status], [Type]) VALUES (N'd6bfcccf-02e0-4c32-915b-f8c2f79bda12', N'c685a469-ab3d-4a05-8f56-ed7260444a3a', N'5dd83544-e960-426b-97d5-3dcee7720f9c', CAST(50000 AS Decimal(18, 0)), 7626768165890236, 1, CAST(N'2025-02-27T15:41:21.663' AS DateTime), CAST(N'2025-02-27T15:41:21.663' AS DateTime), NULL, N'PENDING', N'DEPOSIT')
INSERT [dbo].[Transaction] ([Id], [WalletId], [DepositId], [Amount], [OrderCode], [IsActive], [CreateAt], [UpdateAt], [DeleteAt], [Status], [Type]) VALUES (N'27a3bb44-c60b-416a-968b-fe57d8d7f9eb', N'c685a469-ab3d-4a05-8f56-ed7260444a3a', N'5aab8851-d51f-4059-ad78-6380cdd3d8c6', CAST(50000 AS Decimal(18, 0)), 7626860352243183, 1, CAST(N'2025-02-27T15:56:43.623' AS DateTime), CAST(N'2025-02-27T15:56:43.623' AS DateTime), NULL, N'PENDING', N'DEPOSIT')
GO
INSERT [dbo].[Wallet] ([Id], [AccountId], [Balance], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'9108735f-304f-477b-86a5-0686a156d9e3', N'32481b25-b91e-4756-a235-31916269c33a', CAST(0 AS Decimal(18, 0)), 1, CAST(N'2025-02-28T07:50:32.557' AS DateTime), CAST(N'2025-02-28T07:50:32.557' AS DateTime), NULL)
INSERT [dbo].[Wallet] ([Id], [AccountId], [Balance], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'7d172f31-df5d-4f84-8296-397d2a51ff21', N'7d172f31-df5d-4f84-8296-397d2a51ff21', CAST(0 AS Decimal(18, 0)), 1, CAST(N'2025-02-25T10:03:10.830' AS DateTime), CAST(N'2025-02-25T10:03:10.830' AS DateTime), NULL)
INSERT [dbo].[Wallet] ([Id], [AccountId], [Balance], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'd26d36c0-0692-4bc6-bafe-49698f45a657', N'd26d36c0-0692-4bc6-bafe-49698f45a657', CAST(0 AS Decimal(18, 0)), 1, CAST(N'2025-03-05T23:03:20.733' AS DateTime), CAST(N'2025-03-05T23:03:20.733' AS DateTime), NULL)
INSERT [dbo].[Wallet] ([Id], [AccountId], [Balance], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'3ee86271-a66f-41da-918f-5b82c2346bd7', N'e9d06dda-5719-49b5-89b3-d0cc184ce119', CAST(0 AS Decimal(18, 0)), 1, CAST(N'2025-03-05T15:19:50.683' AS DateTime), CAST(N'2025-03-05T15:19:50.683' AS DateTime), NULL)
INSERT [dbo].[Wallet] ([Id], [AccountId], [Balance], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'eb875212-93e3-4696-96e3-71e3ce01f1aa', N'eb875212-93e3-4696-96e3-71e3ce01f1aa', CAST(0 AS Decimal(18, 0)), 1, CAST(N'2025-03-05T21:09:45.270' AS DateTime), CAST(N'2025-03-05T21:09:45.270' AS DateTime), NULL)
INSERT [dbo].[Wallet] ([Id], [AccountId], [Balance], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'10280fb5-bf7e-46b5-be57-78baf041c83f', N'10280fb5-bf7e-46b5-be57-78baf041c83f', CAST(0 AS Decimal(18, 0)), 1, CAST(N'2025-03-05T10:46:31.420' AS DateTime), CAST(N'2025-03-05T10:46:31.420' AS DateTime), NULL)
INSERT [dbo].[Wallet] ([Id], [AccountId], [Balance], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'2d96b34d-cb42-4dde-bcda-7e3899dde181', N'2d96b34d-cb42-4dde-bcda-7e3899dde181', CAST(0 AS Decimal(18, 0)), 1, CAST(N'2025-03-02T21:42:14.073' AS DateTime), CAST(N'2025-03-02T21:42:14.073' AS DateTime), NULL)
INSERT [dbo].[Wallet] ([Id], [AccountId], [Balance], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'6ff4e82f-93aa-4c4d-92c8-89d2335d06b0', N'90fba115-620d-45f5-8407-28b2db39b4ef', CAST(0 AS Decimal(18, 0)), 1, CAST(N'2025-02-28T06:38:59.643' AS DateTime), CAST(N'2025-02-28T06:38:59.643' AS DateTime), NULL)
INSERT [dbo].[Wallet] ([Id], [AccountId], [Balance], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'c8aebabf-c8c1-45f6-826c-9dd90667a16e', N'85fb2b10-1dfa-4408-be3a-b5b98234c4c4', CAST(0 AS Decimal(18, 0)), 1, CAST(N'2025-02-28T06:58:01.247' AS DateTime), CAST(N'2025-02-28T06:58:01.247' AS DateTime), NULL)
INSERT [dbo].[Wallet] ([Id], [AccountId], [Balance], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'ef2a8c06-9ad1-4fb2-a2e3-c6a8ebd61382', N'3839e1d3-3a37-4b4f-bd66-195dd18efbfe', CAST(0 AS Decimal(18, 0)), 1, CAST(N'2025-02-28T07:00:23.957' AS DateTime), CAST(N'2025-02-28T07:00:23.957' AS DateTime), NULL)
INSERT [dbo].[Wallet] ([Id], [AccountId], [Balance], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'67c8148c-9726-4259-a5c2-ce73aa73a263', N'67c8148c-9726-4259-a5c2-ce73aa73a263', CAST(0 AS Decimal(18, 0)), 1, CAST(N'2025-02-25T17:48:42.860' AS DateTime), CAST(N'2025-02-25T17:48:42.860' AS DateTime), NULL)
INSERT [dbo].[Wallet] ([Id], [AccountId], [Balance], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'41225bc2-9415-45f5-bcbe-dde495ed27a7', N'41225bc2-9415-45f5-bcbe-dde495ed27a7', CAST(0 AS Decimal(18, 0)), 1, CAST(N'2025-03-05T21:19:56.383' AS DateTime), CAST(N'2025-03-05T21:19:56.383' AS DateTime), NULL)
INSERT [dbo].[Wallet] ([Id], [AccountId], [Balance], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'c685a469-ab3d-4a05-8f56-ed7260444a3a', N'6b408dce-34db-46ab-9eba-6e5aae607243', CAST(30000 AS Decimal(18, 0)), 1, CAST(N'2025-02-26T09:38:54.883' AS DateTime), CAST(N'2025-03-01T14:50:10.787' AS DateTime), NULL)
INSERT [dbo].[Wallet] ([Id], [AccountId], [Balance], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'a5e3ac44-0840-43f2-8d73-fa614f25e34d', N'a5e3ac44-0840-43f2-8d73-fa614f25e34d', CAST(0 AS Decimal(18, 0)), 1, CAST(N'2025-03-05T21:02:17.117' AS DateTime), CAST(N'2025-03-05T21:02:17.120' AS DateTime), NULL)
INSERT [dbo].[Wallet] ([Id], [AccountId], [Balance], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'100f00b9-58dc-48c5-97b5-fa72902a6522', N'100f00b9-58dc-48c5-97b5-fa72902a6522', CAST(0 AS Decimal(18, 0)), 1, CAST(N'2025-03-05T21:10:45.017' AS DateTime), CAST(N'2025-03-05T21:10:45.017' AS DateTime), NULL)
INSERT [dbo].[Wallet] ([Id], [AccountId], [Balance], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'd739d47a-a602-4579-82bf-fb1a9d5ad6ae', N'54dd3bdc-b1d6-4957-9677-33a3441b4c2d', CAST(0 AS Decimal(18, 0)), 1, CAST(N'2025-02-28T06:54:53.280' AS DateTime), CAST(N'2025-02-28T06:54:53.280' AS DateTime), NULL)
GO
INSERT [dbo].[WorkShifts] ([Id], [AccountId], [StartTime], [EndTime], [Day], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'07b5724f-848b-4615-8373-02d7ef3fb2e0', N'10280fb5-bf7e-46b5-be57-78baf041c83f', CAST(N'07:00:00' AS Time), CAST(N'09:00:00' AS Time), N'Monday', 1, CAST(N'2025-03-07T19:19:38.380' AS DateTime), CAST(N'2025-03-07T19:19:38.380' AS DateTime), NULL)
INSERT [dbo].[WorkShifts] ([Id], [AccountId], [StartTime], [EndTime], [Day], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'a6e1a5b6-d755-4576-9863-049a2517e607', N'41225bc2-9415-45f5-bcbe-dde495ed27a7', CAST(N'07:00:00' AS Time), CAST(N'09:00:00' AS Time), N'Thursday', 1, CAST(N'2025-03-07T19:19:03.173' AS DateTime), CAST(N'2025-03-07T19:19:03.173' AS DateTime), NULL)
INSERT [dbo].[WorkShifts] ([Id], [AccountId], [StartTime], [EndTime], [Day], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'947c462c-b496-4136-830f-0a27e21ce2b1', N'a5e3ac44-0840-43f2-8d73-fa614f25e34d', CAST(N'07:00:00' AS Time), CAST(N'09:00:00' AS Time), N'Monday', 1, CAST(N'2025-03-07T19:08:39.857' AS DateTime), CAST(N'2025-03-07T19:08:39.857' AS DateTime), NULL)
INSERT [dbo].[WorkShifts] ([Id], [AccountId], [StartTime], [EndTime], [Day], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'828a024a-6653-4e79-80af-0ffe41b8a846', N'eb875212-93e3-4696-96e3-71e3ce01f1aa', CAST(N'07:00:00' AS Time), CAST(N'09:00:00' AS Time), N'Monday', 1, CAST(N'2025-03-07T19:19:45.117' AS DateTime), CAST(N'2025-03-07T19:19:45.117' AS DateTime), NULL)
INSERT [dbo].[WorkShifts] ([Id], [AccountId], [StartTime], [EndTime], [Day], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'8d520192-7f40-4ce4-b993-11bf734726e0', N'41225bc2-9415-45f5-bcbe-dde495ed27a7', CAST(N'07:00:00' AS Time), CAST(N'09:00:00' AS Time), N'Sunday', 1, CAST(N'2025-03-07T19:19:14.953' AS DateTime), CAST(N'2025-03-07T19:19:14.953' AS DateTime), NULL)
INSERT [dbo].[WorkShifts] ([Id], [AccountId], [StartTime], [EndTime], [Day], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'95a808d6-20d6-4857-aedb-2dd656ce4ce9', N'd26d36c0-0692-4bc6-bafe-49698f45a657', CAST(N'07:00:00' AS Time), CAST(N'09:00:00' AS Time), N'Sunday', 1, CAST(N'2025-03-07T19:19:31.100' AS DateTime), CAST(N'2025-03-07T19:19:31.100' AS DateTime), NULL)
INSERT [dbo].[WorkShifts] ([Id], [AccountId], [StartTime], [EndTime], [Day], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'81837445-ab38-4acc-806e-3efe0dca3be2', N'a5e3ac44-0840-43f2-8d73-fa614f25e34d', CAST(N'06:00:00' AS Time), CAST(N'07:00:00' AS Time), N'Tuesday', 1, CAST(N'2025-03-07T19:15:25.637' AS DateTime), CAST(N'2025-03-07T19:15:25.637' AS DateTime), NULL)
INSERT [dbo].[WorkShifts] ([Id], [AccountId], [StartTime], [EndTime], [Day], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'aaf029d0-e044-4eee-8c58-569c72846d9c', N'41225bc2-9415-45f5-bcbe-dde495ed27a7', CAST(N'07:00:00' AS Time), CAST(N'09:00:00' AS Time), N'Saturday', 1, CAST(N'2025-03-07T19:19:11.640' AS DateTime), CAST(N'2025-03-07T19:19:11.640' AS DateTime), NULL)
INSERT [dbo].[WorkShifts] ([Id], [AccountId], [StartTime], [EndTime], [Day], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'779cf0dc-58bf-49eb-97f8-63c93c222a1c', N'41225bc2-9415-45f5-bcbe-dde495ed27a7', CAST(N'07:00:00' AS Time), CAST(N'09:00:00' AS Time), N'Tuesday', 1, CAST(N'2025-03-07T19:18:55.267' AS DateTime), CAST(N'2025-03-07T19:18:55.267' AS DateTime), NULL)
INSERT [dbo].[WorkShifts] ([Id], [AccountId], [StartTime], [EndTime], [Day], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'3bb71236-c7a2-4bbd-8522-7060b4fb04bf', N'a5e3ac44-0840-43f2-8d73-fa614f25e34d', CAST(N'06:00:00' AS Time), CAST(N'07:00:00' AS Time), N'Friday', 1, CAST(N'2025-03-07T19:15:36.303' AS DateTime), CAST(N'2025-03-07T19:15:36.303' AS DateTime), NULL)
INSERT [dbo].[WorkShifts] ([Id], [AccountId], [StartTime], [EndTime], [Day], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'b921a448-6ec2-43ed-be9b-7e44dd025fc2', N'2d96b34d-cb42-4dde-bcda-7e3899dde181', CAST(N'07:00:00' AS Time), CAST(N'09:00:00' AS Time), N'Monday', 1, CAST(N'2025-03-07T19:19:56.610' AS DateTime), CAST(N'2025-03-07T19:19:56.610' AS DateTime), NULL)
INSERT [dbo].[WorkShifts] ([Id], [AccountId], [StartTime], [EndTime], [Day], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'df2e41d5-390c-476f-be4a-882122819093', N'100f00b9-58dc-48c5-97b5-fa72902a6522', CAST(N'07:00:00' AS Time), CAST(N'09:00:00' AS Time), N'Monday', 1, CAST(N'2025-03-07T19:19:50.893' AS DateTime), CAST(N'2025-03-07T19:19:50.893' AS DateTime), NULL)
INSERT [dbo].[WorkShifts] ([Id], [AccountId], [StartTime], [EndTime], [Day], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'46cbf087-8083-4dd0-9725-9f366cbe5f60', N'41225bc2-9415-45f5-bcbe-dde495ed27a7', CAST(N'07:00:00' AS Time), CAST(N'09:00:00' AS Time), N'Friday', 1, CAST(N'2025-03-07T19:19:06.827' AS DateTime), CAST(N'2025-03-07T19:19:06.827' AS DateTime), NULL)
INSERT [dbo].[WorkShifts] ([Id], [AccountId], [StartTime], [EndTime], [Day], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'da642a99-bbef-4837-9e4d-9fcd9e0876d4', N'a5e3ac44-0840-43f2-8d73-fa614f25e34d', CAST(N'06:00:00' AS Time), CAST(N'07:00:00' AS Time), N'Saturday', 1, CAST(N'2025-03-07T19:15:39.850' AS DateTime), CAST(N'2025-03-07T19:15:39.850' AS DateTime), NULL)
INSERT [dbo].[WorkShifts] ([Id], [AccountId], [StartTime], [EndTime], [Day], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'63f3ea8c-9bfb-4806-aa8c-b64eefd82926', N'41225bc2-9415-45f5-bcbe-dde495ed27a7', CAST(N'07:00:00' AS Time), CAST(N'09:00:00' AS Time), N'Monday', 1, CAST(N'2025-03-07T19:08:25.173' AS DateTime), CAST(N'2025-03-07T19:08:25.173' AS DateTime), NULL)
INSERT [dbo].[WorkShifts] ([Id], [AccountId], [StartTime], [EndTime], [Day], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'2cbed34a-db3d-4cb4-956d-c0035aa1ef1f', N'a5e3ac44-0840-43f2-8d73-fa614f25e34d', CAST(N'06:00:00' AS Time), CAST(N'07:00:00' AS Time), N'Sunday', 1, CAST(N'2025-03-07T19:15:43.220' AS DateTime), CAST(N'2025-03-07T19:15:43.220' AS DateTime), NULL)
INSERT [dbo].[WorkShifts] ([Id], [AccountId], [StartTime], [EndTime], [Day], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'f91f445d-311c-4a1b-9366-ca15d6ea8d2b', N'a5e3ac44-0840-43f2-8d73-fa614f25e34d', CAST(N'06:00:00' AS Time), CAST(N'07:00:00' AS Time), N'Wednesday', 1, CAST(N'2025-03-07T19:15:27.710' AS DateTime), CAST(N'2025-03-07T19:15:27.710' AS DateTime), NULL)
INSERT [dbo].[WorkShifts] ([Id], [AccountId], [StartTime], [EndTime], [Day], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'5baa7ade-fc9c-4fa0-a65d-d2d19a0920cf', N'a5e3ac44-0840-43f2-8d73-fa614f25e34d', CAST(N'06:00:00' AS Time), CAST(N'07:00:00' AS Time), N'Monday', 1, CAST(N'2025-03-07T19:15:16.927' AS DateTime), CAST(N'2025-03-07T19:15:16.927' AS DateTime), NULL)
INSERT [dbo].[WorkShifts] ([Id], [AccountId], [StartTime], [EndTime], [Day], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'8fa102f1-6c35-47ae-9d0a-de659c9458b6', N'41225bc2-9415-45f5-bcbe-dde495ed27a7', CAST(N'07:00:00' AS Time), CAST(N'09:00:00' AS Time), N'Wednesday', 1, CAST(N'2025-03-07T19:18:59.767' AS DateTime), CAST(N'2025-03-07T19:18:59.767' AS DateTime), NULL)
INSERT [dbo].[WorkShifts] ([Id], [AccountId], [StartTime], [EndTime], [Day], [IsActive], [CreateAt], [UpdateAt], [DeleteAt]) VALUES (N'3c3099b5-aeab-434d-958f-f49fcaad0ca9', N'a5e3ac44-0840-43f2-8d73-fa614f25e34d', CAST(N'06:00:00' AS Time), CAST(N'07:00:00' AS Time), N'Thursday', 1, CAST(N'2025-03-07T19:15:29.910' AS DateTime), CAST(N'2025-03-07T19:15:29.910' AS DateTime), NULL)
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_Account_UserName]    Script Date: 3/7/2025 8:06:26 PM ******/
ALTER TABLE [dbo].[Account] ADD  CONSTRAINT [UQ_Account_UserName] UNIQUE NONCLUSTERED 
(
	[UserName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [UQ_AccountId_ListenerInfo]    Script Date: 3/7/2025 8:06:26 PM ******/
ALTER TABLE [dbo].[ListenerInfo] ADD  CONSTRAINT [UQ_AccountId_ListenerInfo] UNIQUE NONCLUSTERED 
(
	[AccountId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [Index_UserId]    Script Date: 3/7/2025 8:06:26 PM ******/
CREATE NONCLUSTERED INDEX [Index_UserId] ON [dbo].[RefreshToken]
(
	[UserId] DESC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [UQ_BookingId_Review]    Script Date: 3/7/2025 8:06:26 PM ******/
ALTER TABLE [dbo].[Review] ADD  CONSTRAINT [UQ_BookingId_Review] UNIQUE NONCLUSTERED 
(
	[BookingId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [UQ_DepositId_Transaction]    Script Date: 3/7/2025 8:06:26 PM ******/
ALTER TABLE [dbo].[Transaction] ADD  CONSTRAINT [UQ_DepositId_Transaction] UNIQUE NONCLUSTERED 
(
	[DepositId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [UQ_AccountId_UserInfo]    Script Date: 3/7/2025 8:06:26 PM ******/
ALTER TABLE [dbo].[UserInfo] ADD  CONSTRAINT [UQ_AccountId_UserInfo] UNIQUE NONCLUSTERED 
(
	[AccountId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [UQ_AccountId_UserPresence]    Script Date: 3/7/2025 8:06:26 PM ******/
ALTER TABLE [dbo].[UserPresence] ADD  CONSTRAINT [UQ_AccountId_UserPresence] UNIQUE NONCLUSTERED 
(
	[AccountId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [UQ_AccountId_Wallet]    Script Date: 3/7/2025 8:06:26 PM ******/
ALTER TABLE [dbo].[Wallet] ADD  CONSTRAINT [UQ_AccountId_Wallet] UNIQUE NONCLUSTERED 
(
	[AccountId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
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
