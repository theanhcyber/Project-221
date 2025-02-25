USE [master]
GO
/****** Object:  Database [ProjectPRN221]    Script Date: 16/07/2024 3:09:17 am ******/
CREATE DATABASE [ProjectPRN221]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'ProjectPRN221', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\ProjectPRN221.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'ProjectPRN221_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\ProjectPRN221_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [ProjectPRN221] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [ProjectPRN221].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [ProjectPRN221] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [ProjectPRN221] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [ProjectPRN221] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [ProjectPRN221] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [ProjectPRN221] SET ARITHABORT OFF 
GO
ALTER DATABASE [ProjectPRN221] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [ProjectPRN221] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [ProjectPRN221] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [ProjectPRN221] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [ProjectPRN221] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [ProjectPRN221] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [ProjectPRN221] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [ProjectPRN221] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [ProjectPRN221] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [ProjectPRN221] SET  ENABLE_BROKER 
GO
ALTER DATABASE [ProjectPRN221] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [ProjectPRN221] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [ProjectPRN221] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [ProjectPRN221] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [ProjectPRN221] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [ProjectPRN221] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [ProjectPRN221] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [ProjectPRN221] SET RECOVERY FULL 
GO
ALTER DATABASE [ProjectPRN221] SET  MULTI_USER 
GO
ALTER DATABASE [ProjectPRN221] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [ProjectPRN221] SET DB_CHAINING OFF 
GO
ALTER DATABASE [ProjectPRN221] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [ProjectPRN221] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [ProjectPRN221] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [ProjectPRN221] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'ProjectPRN221', N'ON'
GO
ALTER DATABASE [ProjectPRN221] SET QUERY_STORE = ON
GO
ALTER DATABASE [ProjectPRN221] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [ProjectPRN221]
GO
/****** Object:  Table [dbo].[Class]    Script Date: 16/07/2024 3:09:18 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Class](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](10) NULL,
	[TotalStudent] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Room]    Script Date: 16/07/2024 3:09:18 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Room](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](10) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Schedule]    Script Date: 16/07/2024 3:09:18 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Schedule](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[TeacherID] [int] NULL,
	[ClassID] [int] NULL,
	[SubjectID] [int] NULL,
	[RoomID] [int] NULL,
	[Slot] [int] NOT NULL,
	[DayOfWeek] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Student]    Script Date: 16/07/2024 3:09:18 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Student](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[DoB] [date] NULL,
	[Gender] [int] NULL,
	[Phone] [char](10) NULL,
	[Active] [int] NOT NULL,
	[ClassID] [int] NOT NULL,
 CONSTRAINT [PK_Student] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Subject]    Script Date: 16/07/2024 3:09:18 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Subject](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Teacher]    Script Date: 16/07/2024 3:09:18 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Teacher](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[DoB] [date] NULL,
	[Gender] [int] NULL,
	[Phone] [char](10) NULL,
	[Active] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Class] ON 

INSERT [dbo].[Class] ([ID], [Name], [TotalStudent]) VALUES (1, N'1A', 3)
INSERT [dbo].[Class] ([ID], [Name], [TotalStudent]) VALUES (2, N'1B', 0)
INSERT [dbo].[Class] ([ID], [Name], [TotalStudent]) VALUES (3, N'1C', 0)
INSERT [dbo].[Class] ([ID], [Name], [TotalStudent]) VALUES (4, N'1D', 0)
INSERT [dbo].[Class] ([ID], [Name], [TotalStudent]) VALUES (5, N'1E', 0)
INSERT [dbo].[Class] ([ID], [Name], [TotalStudent]) VALUES (6, N'2A', 0)
INSERT [dbo].[Class] ([ID], [Name], [TotalStudent]) VALUES (7, N'2B', 1)
INSERT [dbo].[Class] ([ID], [Name], [TotalStudent]) VALUES (8, N'2C', 0)
INSERT [dbo].[Class] ([ID], [Name], [TotalStudent]) VALUES (9, N'2D', 0)
INSERT [dbo].[Class] ([ID], [Name], [TotalStudent]) VALUES (10, N'2E', 0)
INSERT [dbo].[Class] ([ID], [Name], [TotalStudent]) VALUES (11, N'3A', 1)
INSERT [dbo].[Class] ([ID], [Name], [TotalStudent]) VALUES (12, N'3B', 0)
INSERT [dbo].[Class] ([ID], [Name], [TotalStudent]) VALUES (13, N'3C', 0)
INSERT [dbo].[Class] ([ID], [Name], [TotalStudent]) VALUES (14, N'3D', 0)
INSERT [dbo].[Class] ([ID], [Name], [TotalStudent]) VALUES (15, N'3E', 0)
INSERT [dbo].[Class] ([ID], [Name], [TotalStudent]) VALUES (16, N'4A', 0)
INSERT [dbo].[Class] ([ID], [Name], [TotalStudent]) VALUES (17, N'4B', 0)
INSERT [dbo].[Class] ([ID], [Name], [TotalStudent]) VALUES (18, N'4C', 1)
INSERT [dbo].[Class] ([ID], [Name], [TotalStudent]) VALUES (19, N'4D', 0)
INSERT [dbo].[Class] ([ID], [Name], [TotalStudent]) VALUES (20, N'4E', 0)
INSERT [dbo].[Class] ([ID], [Name], [TotalStudent]) VALUES (21, N'5A', 0)
INSERT [dbo].[Class] ([ID], [Name], [TotalStudent]) VALUES (22, N'5B', 0)
INSERT [dbo].[Class] ([ID], [Name], [TotalStudent]) VALUES (23, N'5C', 0)
INSERT [dbo].[Class] ([ID], [Name], [TotalStudent]) VALUES (24, N'5D', 0)
INSERT [dbo].[Class] ([ID], [Name], [TotalStudent]) VALUES (25, N'5E', 1)
INSERT [dbo].[Class] ([ID], [Name], [TotalStudent]) VALUES (27, N'1F', 0)
SET IDENTITY_INSERT [dbo].[Class] OFF
GO
SET IDENTITY_INSERT [dbo].[Room] ON 

INSERT [dbo].[Room] ([ID], [Name]) VALUES (1, N'101')
INSERT [dbo].[Room] ([ID], [Name]) VALUES (2, N'102')
INSERT [dbo].[Room] ([ID], [Name]) VALUES (3, N'103')
INSERT [dbo].[Room] ([ID], [Name]) VALUES (4, N'104')
INSERT [dbo].[Room] ([ID], [Name]) VALUES (5, N'105')
INSERT [dbo].[Room] ([ID], [Name]) VALUES (6, N'106')
INSERT [dbo].[Room] ([ID], [Name]) VALUES (7, N'201')
INSERT [dbo].[Room] ([ID], [Name]) VALUES (8, N'202')
INSERT [dbo].[Room] ([ID], [Name]) VALUES (9, N'203')
INSERT [dbo].[Room] ([ID], [Name]) VALUES (10, N'204')
INSERT [dbo].[Room] ([ID], [Name]) VALUES (11, N'205')
INSERT [dbo].[Room] ([ID], [Name]) VALUES (12, N'206')
INSERT [dbo].[Room] ([ID], [Name]) VALUES (13, N'301')
INSERT [dbo].[Room] ([ID], [Name]) VALUES (14, N'302')
INSERT [dbo].[Room] ([ID], [Name]) VALUES (15, N'303')
INSERT [dbo].[Room] ([ID], [Name]) VALUES (16, N'304')
INSERT [dbo].[Room] ([ID], [Name]) VALUES (17, N'305')
INSERT [dbo].[Room] ([ID], [Name]) VALUES (18, N'306')
INSERT [dbo].[Room] ([ID], [Name]) VALUES (20, N'107')
SET IDENTITY_INSERT [dbo].[Room] OFF
GO
SET IDENTITY_INSERT [dbo].[Schedule] ON 

INSERT [dbo].[Schedule] ([ID], [TeacherID], [ClassID], [SubjectID], [RoomID], [Slot], [DayOfWeek]) VALUES (2, 2, 2, 2, 2, 1, 2)
INSERT [dbo].[Schedule] ([ID], [TeacherID], [ClassID], [SubjectID], [RoomID], [Slot], [DayOfWeek]) VALUES (3, 3, 3, 3, 3, 2, 3)
INSERT [dbo].[Schedule] ([ID], [TeacherID], [ClassID], [SubjectID], [RoomID], [Slot], [DayOfWeek]) VALUES (4, 4, 4, 4, 4, 2, 3)
INSERT [dbo].[Schedule] ([ID], [TeacherID], [ClassID], [SubjectID], [RoomID], [Slot], [DayOfWeek]) VALUES (5, 5, 5, 5, 5, 1, 4)
INSERT [dbo].[Schedule] ([ID], [TeacherID], [ClassID], [SubjectID], [RoomID], [Slot], [DayOfWeek]) VALUES (6, 6, 6, 6, 6, 1, 7)
INSERT [dbo].[Schedule] ([ID], [TeacherID], [ClassID], [SubjectID], [RoomID], [Slot], [DayOfWeek]) VALUES (7, 7, 7, 7, 7, 2, 5)
INSERT [dbo].[Schedule] ([ID], [TeacherID], [ClassID], [SubjectID], [RoomID], [Slot], [DayOfWeek]) VALUES (8, 8, 8, 8, 8, 2, 5)
INSERT [dbo].[Schedule] ([ID], [TeacherID], [ClassID], [SubjectID], [RoomID], [Slot], [DayOfWeek]) VALUES (9, 9, 9, 9, 9, 1, 6)
INSERT [dbo].[Schedule] ([ID], [TeacherID], [ClassID], [SubjectID], [RoomID], [Slot], [DayOfWeek]) VALUES (10, 10, 10, 1, 10, 1, 7)
INSERT [dbo].[Schedule] ([ID], [TeacherID], [ClassID], [SubjectID], [RoomID], [Slot], [DayOfWeek]) VALUES (12, 21, 19, 1, 20, 1, 2)
INSERT [dbo].[Schedule] ([ID], [TeacherID], [ClassID], [SubjectID], [RoomID], [Slot], [DayOfWeek]) VALUES (14, 1, 1, 1, 1, 4, 2)
INSERT [dbo].[Schedule] ([ID], [TeacherID], [ClassID], [SubjectID], [RoomID], [Slot], [DayOfWeek]) VALUES (15, 20, 14, 9, 20, 2, 5)
INSERT [dbo].[Schedule] ([ID], [TeacherID], [ClassID], [SubjectID], [RoomID], [Slot], [DayOfWeek]) VALUES (17, 3, 3, 3, 3, 3, 3)
INSERT [dbo].[Schedule] ([ID], [TeacherID], [ClassID], [SubjectID], [RoomID], [Slot], [DayOfWeek]) VALUES (18, 4, 4, 4, 4, 4, 4)
INSERT [dbo].[Schedule] ([ID], [TeacherID], [ClassID], [SubjectID], [RoomID], [Slot], [DayOfWeek]) VALUES (19, 5, 5, 5, 5, 2, 5)
INSERT [dbo].[Schedule] ([ID], [TeacherID], [ClassID], [SubjectID], [RoomID], [Slot], [DayOfWeek]) VALUES (20, NULL, NULL, NULL, NULL, 0, 0)
INSERT [dbo].[Schedule] ([ID], [TeacherID], [ClassID], [SubjectID], [RoomID], [Slot], [DayOfWeek]) VALUES (21, 16, 25, 8, 18, 3, 6)
INSERT [dbo].[Schedule] ([ID], [TeacherID], [ClassID], [SubjectID], [RoomID], [Slot], [DayOfWeek]) VALUES (22, 11, 16, 6, 13, 3, 4)
SET IDENTITY_INSERT [dbo].[Schedule] OFF
GO
SET IDENTITY_INSERT [dbo].[Student] ON 

INSERT [dbo].[Student] ([ID], [Name], [DoB], [Gender], [Phone], [Active], [ClassID]) VALUES (1, N'Thành', CAST(N'2003-06-04' AS Date), 1, N'1267132451', 1, 1)
INSERT [dbo].[Student] ([ID], [Name], [DoB], [Gender], [Phone], [Active], [ClassID]) VALUES (2, N'Nam', CAST(N'2003-03-31' AS Date), 1, N'2142135514', 1, 11)
INSERT [dbo].[Student] ([ID], [Name], [DoB], [Gender], [Phone], [Active], [ClassID]) VALUES (3, N'Huyền', CAST(N'2003-10-19' AS Date), 0, NULL, 1, 7)
INSERT [dbo].[Student] ([ID], [Name], [DoB], [Gender], [Phone], [Active], [ClassID]) VALUES (4, N'Thủy', CAST(N'2003-12-31' AS Date), 0, N'7192438437', 1, 18)
INSERT [dbo].[Student] ([ID], [Name], [DoB], [Gender], [Phone], [Active], [ClassID]) VALUES (5, N'Hiếu', CAST(N'2003-12-20' AS Date), 1, NULL, 1, 1)
INSERT [dbo].[Student] ([ID], [Name], [DoB], [Gender], [Phone], [Active], [ClassID]) VALUES (6, N'Mạnh', CAST(N'2003-08-16' AS Date), 1, N'3412341351', 1, 1)
INSERT [dbo].[Student] ([ID], [Name], [DoB], [Gender], [Phone], [Active], [ClassID]) VALUES (7, N'Kiên', CAST(N'2003-04-14' AS Date), 1, N'2341273412', 1, 25)
SET IDENTITY_INSERT [dbo].[Student] OFF
GO
SET IDENTITY_INSERT [dbo].[Subject] ON 

INSERT [dbo].[Subject] ([ID], [Name]) VALUES (1, N'Maths')
INSERT [dbo].[Subject] ([ID], [Name]) VALUES (2, N'Liturature')
INSERT [dbo].[Subject] ([ID], [Name]) VALUES (3, N'English')
INSERT [dbo].[Subject] ([ID], [Name]) VALUES (4, N'Physic')
INSERT [dbo].[Subject] ([ID], [Name]) VALUES (5, N'Chemistry')
INSERT [dbo].[Subject] ([ID], [Name]) VALUES (6, N'History')
INSERT [dbo].[Subject] ([ID], [Name]) VALUES (7, N'Geography')
INSERT [dbo].[Subject] ([ID], [Name]) VALUES (8, N'Biology')
INSERT [dbo].[Subject] ([ID], [Name]) VALUES (9, N'Information Technology')
SET IDENTITY_INSERT [dbo].[Subject] OFF
GO
SET IDENTITY_INSERT [dbo].[Teacher] ON 

INSERT [dbo].[Teacher] ([ID], [Name], [DoB], [Gender], [Phone], [Active]) VALUES (1, N'Thành', CAST(N'2003-06-04' AS Date), 1, N'1980237454', 1)
INSERT [dbo].[Teacher] ([ID], [Name], [DoB], [Gender], [Phone], [Active]) VALUES (2, N'Quyên', CAST(N'2003-06-08' AS Date), 0, N'4324154314', 1)
INSERT [dbo].[Teacher] ([ID], [Name], [DoB], [Gender], [Phone], [Active]) VALUES (3, N'Huyền', CAST(N'2003-10-19' AS Date), 0, N'3425436254', 1)
INSERT [dbo].[Teacher] ([ID], [Name], [DoB], [Gender], [Phone], [Active]) VALUES (4, N'Kiên', NULL, 1, NULL, 1)
INSERT [dbo].[Teacher] ([ID], [Name], [DoB], [Gender], [Phone], [Active]) VALUES (5, N'Thủy', NULL, 0, NULL, 1)
INSERT [dbo].[Teacher] ([ID], [Name], [DoB], [Gender], [Phone], [Active]) VALUES (6, N'Tùng', NULL, 1, NULL, 1)
INSERT [dbo].[Teacher] ([ID], [Name], [DoB], [Gender], [Phone], [Active]) VALUES (7, N'Nam', NULL, 1, NULL, 1)
INSERT [dbo].[Teacher] ([ID], [Name], [DoB], [Gender], [Phone], [Active]) VALUES (8, N'Ngân', NULL, 0, NULL, 1)
INSERT [dbo].[Teacher] ([ID], [Name], [DoB], [Gender], [Phone], [Active]) VALUES (9, N'Chi', NULL, 0, NULL, 1)
INSERT [dbo].[Teacher] ([ID], [Name], [DoB], [Gender], [Phone], [Active]) VALUES (10, N'Đức', NULL, 1, NULL, 1)
INSERT [dbo].[Teacher] ([ID], [Name], [DoB], [Gender], [Phone], [Active]) VALUES (11, N'Minh', NULL, 1, NULL, 1)
INSERT [dbo].[Teacher] ([ID], [Name], [DoB], [Gender], [Phone], [Active]) VALUES (12, N'Quỳnh', NULL, 0, NULL, 1)
INSERT [dbo].[Teacher] ([ID], [Name], [DoB], [Gender], [Phone], [Active]) VALUES (13, N'Giang', NULL, 0, NULL, 1)
INSERT [dbo].[Teacher] ([ID], [Name], [DoB], [Gender], [Phone], [Active]) VALUES (14, N'Sơn', NULL, 1, NULL, 1)
INSERT [dbo].[Teacher] ([ID], [Name], [DoB], [Gender], [Phone], [Active]) VALUES (15, N'Hải', NULL, 1, NULL, 1)
INSERT [dbo].[Teacher] ([ID], [Name], [DoB], [Gender], [Phone], [Active]) VALUES (16, N'Quân', NULL, 1, NULL, 1)
INSERT [dbo].[Teacher] ([ID], [Name], [DoB], [Gender], [Phone], [Active]) VALUES (17, N'Linh', NULL, 0, NULL, 1)
INSERT [dbo].[Teacher] ([ID], [Name], [DoB], [Gender], [Phone], [Active]) VALUES (18, N'Vân Anh', CAST(N'2003-03-26' AS Date), 0, N'5737385953', 1)
INSERT [dbo].[Teacher] ([ID], [Name], [DoB], [Gender], [Phone], [Active]) VALUES (19, N'Thanh', CAST(N'2003-11-20' AS Date), 0, N'1980237476', 0)
INSERT [dbo].[Teacher] ([ID], [Name], [DoB], [Gender], [Phone], [Active]) VALUES (20, N'Minh', CAST(N'2003-05-19' AS Date), 1, N'1980237454', 1)
INSERT [dbo].[Teacher] ([ID], [Name], [DoB], [Gender], [Phone], [Active]) VALUES (21, N'Ngọc', CAST(N'2003-12-15' AS Date), 0, N'1980237476', 1)
SET IDENTITY_INSERT [dbo].[Teacher] OFF
GO
ALTER TABLE [dbo].[Schedule]  WITH CHECK ADD FOREIGN KEY([ClassID])
REFERENCES [dbo].[Class] ([ID])
GO
ALTER TABLE [dbo].[Schedule]  WITH CHECK ADD FOREIGN KEY([RoomID])
REFERENCES [dbo].[Room] ([ID])
GO
ALTER TABLE [dbo].[Schedule]  WITH CHECK ADD FOREIGN KEY([SubjectID])
REFERENCES [dbo].[Subject] ([ID])
GO
ALTER TABLE [dbo].[Schedule]  WITH CHECK ADD FOREIGN KEY([TeacherID])
REFERENCES [dbo].[Teacher] ([ID])
GO
ALTER TABLE [dbo].[Student]  WITH CHECK ADD  CONSTRAINT [FK_Student_Student] FOREIGN KEY([ClassID])
REFERENCES [dbo].[Class] ([ID])
GO
ALTER TABLE [dbo].[Student] CHECK CONSTRAINT [FK_Student_Student]
GO
USE [master]
GO
ALTER DATABASE [ProjectPRN221] SET  READ_WRITE 
GO
