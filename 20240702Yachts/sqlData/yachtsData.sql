USE [master]
GO
/****** Object:  Database [20240702Yachts]    Script Date: 2024/7/30 下午 01:50:03 ******/
CREATE DATABASE [20240702Yachts]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'20240702Yachts', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\20240702Yachts.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'20240702Yachts_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\20240702Yachts_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [20240702Yachts] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [20240702Yachts].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [20240702Yachts] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [20240702Yachts] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [20240702Yachts] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [20240702Yachts] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [20240702Yachts] SET ARITHABORT OFF 
GO
ALTER DATABASE [20240702Yachts] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [20240702Yachts] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [20240702Yachts] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [20240702Yachts] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [20240702Yachts] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [20240702Yachts] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [20240702Yachts] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [20240702Yachts] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [20240702Yachts] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [20240702Yachts] SET  DISABLE_BROKER 
GO
ALTER DATABASE [20240702Yachts] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [20240702Yachts] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [20240702Yachts] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [20240702Yachts] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [20240702Yachts] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [20240702Yachts] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [20240702Yachts] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [20240702Yachts] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [20240702Yachts] SET  MULTI_USER 
GO
ALTER DATABASE [20240702Yachts] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [20240702Yachts] SET DB_CHAINING OFF 
GO
ALTER DATABASE [20240702Yachts] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [20240702Yachts] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [20240702Yachts] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [20240702Yachts] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [20240702Yachts] SET QUERY_STORE = ON
GO
ALTER DATABASE [20240702Yachts] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [20240702Yachts]
GO
/****** Object:  Table [dbo].[Company]    Script Date: 2024/7/30 下午 01:50:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Company](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[aboutUsHtml] [nvarchar](max) NULL,
	[certificatContent] [nvarchar](max) NULL,
	[certificatVerticalImgJSON] [nvarchar](max) NULL,
	[certificatHorizontalImgJSON] [nvarchar](max) NULL,
	[createDate] [datetime] NOT NULL,
 CONSTRAINT [PK_Company] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Country]    Script Date: 2024/7/30 下午 01:50:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Country](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[countryName] [nvarchar](100) NOT NULL,
	[createTime] [datetime] NOT NULL,
 CONSTRAINT [PK_Country] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Dealers]    Script Date: 2024/7/30 下午 01:50:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Dealers](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[countryId] [int] NOT NULL,
	[area] [nvarchar](100) NOT NULL,
	[dealerImgPath] [nvarchar](100) NULL,
	[name] [nvarchar](100) NULL,
	[contact] [nvarchar](100) NULL,
	[address] [nvarchar](100) NULL,
	[tel] [nvarchar](100) NULL,
	[fax] [nvarchar](100) NULL,
	[email] [nvarchar](100) NULL,
	[link] [nvarchar](100) NULL,
	[createDate] [datetime] NOT NULL,
 CONSTRAINT [PK_Dealers] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DetailTitleSort]    Script Date: 2024/7/30 下午 01:50:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DetailTitleSort](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[detailTitleSort] [nvarchar](max) NULL,
	[creatTime] [datetime] NOT NULL,
 CONSTRAINT [PK_DetailTitleSort] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[News]    Script Date: 2024/7/30 下午 01:50:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[News](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[dateTitle] [date] NOT NULL,
	[headline] [nvarchar](500) NOT NULL,
	[guid] [nvarchar](100) NOT NULL,
	[isTop] [bit] NULL,
	[summary] [nvarchar](max) NULL,
	[thumbnailPath] [nvarchar](200) NULL,
	[newsContentHtml] [nvarchar](max) NULL,
	[newsImageJson] [nvarchar](max) NULL,
	[createTime] [datetime] NULL,
 CONSTRAINT [PK_News] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Specification]    Script Date: 2024/7/30 下午 01:50:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Specification](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[yachtModel_ID] [int] NOT NULL,
	[detailTitleSort_ID] [int] NULL,
	[detail] [nvarchar](max) NULL,
	[createTime] [datetime] NOT NULL,
 CONSTRAINT [PK_Specification] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 2024/7/30 下午 01:50:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[account] [nvarchar](100) NOT NULL,
	[password] [nvarchar](100) NOT NULL,
	[salt] [nvarchar](50) NULL,
	[email] [nvarchar](100) NULL,
	[name] [nvarchar](100) NULL,
	[permission] [nvarchar](50) NULL,
	[createDate] [datetime] NOT NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Yachts]    Script Date: 2024/7/30 下午 01:50:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Yachts](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[yachtsModel] [nvarchar](100) NOT NULL,
	[isNewDesign] [bit] NOT NULL,
	[isNewBuilding] [bit] NOT NULL,
	[createTime] [datetime] NOT NULL,
	[guid] [nvarchar](100) NOT NULL,
	[bannerImgPathJSON] [nvarchar](max) NULL,
	[overviewContentHtml] [nvarchar](max) NULL,
	[overviewDimensionsImgPath] [nvarchar](100) NULL,
	[overviewDownloadsFilePath] [nvarchar](100) NULL,
	[overviewDimensionsJSON] [nvarchar](max) NULL,
	[layoutDeckPlanImgPathJSON] [nvarchar](max) NULL,
 CONSTRAINT [PK_Yachts] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Company] ON 

INSERT [dbo].[Company] ([id], [aboutUsHtml], [certificatContent], [certificatVerticalImgJSON], [certificatHorizontalImgJSON], [createDate]) VALUES (1, N'&lt;p&gt;
	&lt;span style=&quot;color: rgb(63, 63, 63); font-family: Arial, Helvetica, sans-serif; font-size: 14.4px;&quot;&gt;&amp;ldquo;Our aim is to create outstanding styling, live aboard comfort, and safety at sea for every proud Tayana owner.&amp;rdquo;&lt;img alt=&quot;&quot; src=&quot;/Image/imageCompany/images/bbb.jpg&quot; style=&quot;float: right; width: 300px; height: 200px;&quot; /&gt;&lt;/span&gt;&lt;br style=&quot;color: rgb(63, 63, 63); font-family: Arial, Helvetica, sans-serif; font-size: 14.4px;&quot; /&gt;
	&lt;br style=&quot;color: rgb(63, 63, 63); font-family: Arial, Helvetica, sans-serif; font-size: 14.4px;&quot; /&gt;
	&lt;span style=&quot;color: rgb(63, 63, 63); font-family: Arial, Helvetica, sans-serif; font-size: 14.4px;&quot;&gt;Founded in 1973, Ta Yang Building Co., Ltd. Has built over 1400 blue water cruising yachts to date. This world renowned custom yacht builder offers a large compliment of sailboats ranging from 37&amp;rsquo; to 72&amp;rsquo;, many offer aft or center cockpit design, deck saloon or pilothouse options.&lt;/span&gt;&lt;br style=&quot;color: rgb(63, 63, 63); font-family: Arial, Helvetica, sans-serif; font-size: 14.4px;&quot; /&gt;
	&lt;br style=&quot;color: rgb(63, 63, 63); font-family: Arial, Helvetica, sans-serif; font-size: 14.4px;&quot; /&gt;
	&lt;span style=&quot;color: rgb(63, 63, 63); font-family: Arial, Helvetica, sans-serif; font-size: 14.4px;&quot;&gt;In 2003, Tayana introduced the new Tayana 64 Deck Saloon, designed by Robb Ladd, which offers the latest in building techniques, large sail area and a beam of 18 feet.&lt;/span&gt;&lt;br style=&quot;color: rgb(63, 63, 63); font-family: Arial, Helvetica, sans-serif; font-size: 14.4px;&quot; /&gt;
	&lt;br style=&quot;color: rgb(63, 63, 63); font-family: Arial, Helvetica, sans-serif; font-size: 14.4px;&quot; /&gt;
	&lt;span style=&quot;color: rgb(63, 63, 63); font-family: Arial, Helvetica, sans-serif; font-size: 14.4px;&quot;&gt;Tayana Yachts have been considered the leader in building custom interiors for the last two decades, offering it`s clients the luxury of a living arrangement they prefer rather than have to settle for the compromise of a production boat. Using the finest in exotic woods, the best equipment such as Lewmar, Whitlock, Yanmar engines, Selden spars to name a few, Ta yang has achieved the reputation for building one of the finest semi custom blue water cruising yachts in the world, at an affordable price.&lt;/span&gt;&lt;/p&gt;
', N'Certificat Text Content', N'[{"SaveName":"aaa(1).jpg"},{"SaveName":"eee(1).jpg"}]', N'[{"SaveName":"dealers001(1).jpg"},{"SaveName":"dealers002(1).jpg"},{"SaveName":"dealers003(1).jpg"},{"SaveName":"dealers004(1).jpg"}]', CAST(N'2024-07-15T16:15:21.440' AS DateTime))
SET IDENTITY_INSERT [dbo].[Company] OFF
GO
SET IDENTITY_INSERT [dbo].[Country] ON 

INSERT [dbo].[Country] ([id], [countryName], [createTime]) VALUES (2, N'USA', CAST(N'2024-07-03T00:15:26.233' AS DateTime))
INSERT [dbo].[Country] ([id], [countryName], [createTime]) VALUES (8, N'China', CAST(N'2024-07-09T14:07:24.553' AS DateTime))
INSERT [dbo].[Country] ([id], [countryName], [createTime]) VALUES (9, N'Hong Kong', CAST(N'2024-07-10T13:36:33.450' AS DateTime))
SET IDENTITY_INSERT [dbo].[Country] OFF
GO
SET IDENTITY_INSERT [dbo].[Dealers] ON 

INSERT [dbo].[Dealers] ([id], [countryId], [area], [dealerImgPath], [name], [contact], [address], [tel], [fax], [email], [link], [createDate]) VALUES (1, 2, N'Annapolis', N'/Image/imageDealers/dealers001.jpg', N'Sail yard Inc. Noyce Yacht serice', N'Mr. W.Cary Lukens', N'326 First Street Suite 18 Annapolis, Maryland 21403', N'(410)268-4100', N'(410)268-2974', N'', N'http://www.sailyard.com', CAST(N'2024-07-09T16:32:34.713' AS DateTime))
INSERT [dbo].[Dealers] ([id], [countryId], [area], [dealerImgPath], [name], [contact], [address], [tel], [fax], [email], [link], [createDate]) VALUES (2, 2, N'San Francisco', N'/Image/imageDealers/dealers002.jpg', N'Pacific Yacht Imports', N'Mr. Neil Weinberg', N'Grand Marina 2051 Grand Street# 12 Alameda, CA 94501, USA', N'(510)865-2541', N'(510)865-2369', N'tayana@mindspring.com', N'http://www.pacificyachtimports.net/', CAST(N'2024-07-10T13:24:33.860' AS DateTime))
INSERT [dbo].[Dealers] ([id], [countryId], [area], [dealerImgPath], [name], [contact], [address], [tel], [fax], [email], [link], [createDate]) VALUES (3, 2, N'San Diego', N'/Image/imageDealers/dealers003.jpg', N'Cabrillo Yacht sales', N'Mr. Dan Peter', N'5060 N.Harbor Dr.san Diego,CA 92106', N'866-353-0409', N'(619)200-1024', N'danpeter@cabrilloyachtsales.com', N'', CAST(N'2024-07-10T13:26:09.443' AS DateTime))
INSERT [dbo].[Dealers] ([id], [countryId], [area], [dealerImgPath], [name], [contact], [address], [tel], [fax], [email], [link], [createDate]) VALUES (4, 2, N'Seattle', N'/Image/imageDealers/dealers004.jpg', N'Seattle Yachts', N'Ted Griffin', N'7001 Seaview Ave NW Suite 150 Seattle, WA. 98117 U.S.A.', N'206-789-8044', N'206-789-3976', N'ted@seattleyachts.com', N'http://www.seattleyachts.com', CAST(N'2024-07-10T13:29:56.417' AS DateTime))
INSERT [dbo].[Dealers] ([id], [countryId], [area], [dealerImgPath], [name], [contact], [address], [tel], [fax], [email], [link], [createDate]) VALUES (5, 8, N'China', NULL, N'StarBay Boats(Dalian) Co.,Ltd', N'Mr.Peter Dong', N'No.7-5 Zone B, Xinghai Square, Dalian,China P.C.116021', N'+86-411-8464 3093', N'+86-411-8464 3093', N'peter_pme@163.com', N'http://www.me-cn.com', CAST(N'2024-07-10T13:36:02.760' AS DateTime))
INSERT [dbo].[Dealers] ([id], [countryId], [area], [dealerImgPath], [name], [contact], [address], [tel], [fax], [email], [link], [createDate]) VALUES (6, 9, N'Hong Kong', NULL, N'Piercey Marine Limited', N'Mr. Steve Piercey', N'93 Che Keng Tuk Road, Sai Kung, HONG KONG', N'(852) 2791 4106', N'(852) 2791 4124', N'info@pierceymarine.com', N'http://www.pierceymarine.com', CAST(N'2024-07-10T13:37:29.007' AS DateTime))
SET IDENTITY_INSERT [dbo].[Dealers] OFF
GO
SET IDENTITY_INSERT [dbo].[DetailTitleSort] ON 

INSERT [dbo].[DetailTitleSort] ([id], [detailTitleSort], [creatTime]) VALUES (1, N'HULL', CAST(N'2024-07-27T18:32:52.657' AS DateTime))
INSERT [dbo].[DetailTitleSort] ([id], [detailTitleSort], [creatTime]) VALUES (5, N'DECK/HARDWARE', CAST(N'2024-07-27T18:51:38.650' AS DateTime))
INSERT [dbo].[DetailTitleSort] ([id], [detailTitleSort], [creatTime]) VALUES (6, N'ENGINE/MACHINERY', CAST(N'2024-07-27T18:51:45.643' AS DateTime))
INSERT [dbo].[DetailTitleSort] ([id], [detailTitleSort], [creatTime]) VALUES (7, N'STEERING', CAST(N'2024-07-27T18:51:50.997' AS DateTime))
INSERT [dbo].[DetailTitleSort] ([id], [detailTitleSort], [creatTime]) VALUES (8, N'SPARS/RIGGING', CAST(N'2024-07-27T18:51:55.833' AS DateTime))
INSERT [dbo].[DetailTitleSort] ([id], [detailTitleSort], [creatTime]) VALUES (9, N'SAILS', CAST(N'2024-07-27T18:52:03.867' AS DateTime))
INSERT [dbo].[DetailTitleSort] ([id], [detailTitleSort], [creatTime]) VALUES (10, N'INTERIOR', CAST(N'2024-07-27T18:52:09.067' AS DateTime))
INSERT [dbo].[DetailTitleSort] ([id], [detailTitleSort], [creatTime]) VALUES (11, N'ELECTRICAL', CAST(N'2024-07-27T18:52:14.913' AS DateTime))
INSERT [dbo].[DetailTitleSort] ([id], [detailTitleSort], [creatTime]) VALUES (12, N'PLUMBING', CAST(N'2024-07-27T18:52:19.440' AS DateTime))
SET IDENTITY_INSERT [dbo].[DetailTitleSort] OFF
GO
SET IDENTITY_INSERT [dbo].[News] ON 

INSERT [dbo].[News] ([id], [dateTitle], [headline], [guid], [isTop], [summary], [thumbnailPath], [newsContentHtml], [newsImageJson], [createTime]) VALUES (11, CAST(N'2024-07-22' AS Date), N'TAYANA 48 setting mast', N'f0141005-95fa-4d50-a9a4-9fb4daf8f33c64', 1, N'TAYANA 48 setting mast', N'/Image/imageNews/dealers001.jpg', N'&lt;p&gt;
	&lt;a href=&quot;https://web.archive.org/web/20170823004641/http://www.tayanaworld.com/new_view.aspx?id=5a8eb12f-5fcb-482a-a125-b300447b760a&quot; style=&quot;color: rgb(2, 165, 184); text-decoration-line: none; font-family: Arial, Helvetica, sans-serif; font-size: 12.96px; font-weight: 700;&quot;&gt;TAYANA 48 setting mast&lt;/a&gt;&lt;/p&gt;
&lt;p&gt;
	&amp;nbsp;&lt;/p&gt;
', N'[{"SaveName":"i001(1).jpg"},{"SaveName":"i002(1).jpg"}]', CAST(N'2024-07-22T17:22:46.667' AS DateTime))
INSERT [dbo].[News] ([id], [dateTitle], [headline], [guid], [isTop], [summary], [thumbnailPath], [newsContentHtml], [newsImageJson], [createTime]) VALUES (12, CAST(N'2024-07-22' AS Date), N'TAYANA 49', N'c5d7e99f-b797-4ffe-932a-2690392ce62c66', 0, N'TAYANA 49', N'/Image/imageNews/dealers002.jpg', N'&lt;p&gt;
	TAYANA 49&lt;/p&gt;
', N'[]', CAST(N'2024-07-22T17:26:07.677' AS DateTime))
INSERT [dbo].[News] ([id], [dateTitle], [headline], [guid], [isTop], [summary], [thumbnailPath], [newsContentHtml], [newsImageJson], [createTime]) VALUES (13, CAST(N'2024-07-22' AS Date), N'TAYANA 50', N'0e8cff62-1218-4aa0-9225-d22e24dc8a1e17', 0, NULL, NULL, NULL, N'[]', CAST(N'2024-07-22T17:26:31.180' AS DateTime))
INSERT [dbo].[News] ([id], [dateTitle], [headline], [guid], [isTop], [summary], [thumbnailPath], [newsContentHtml], [newsImageJson], [createTime]) VALUES (14, CAST(N'2024-07-22' AS Date), N'TAYANA 51', N'1a3d2c87-6332-4a91-b40b-6457572c50f301', 0, NULL, NULL, NULL, N'[]', CAST(N'2024-07-22T17:26:36.010' AS DateTime))
INSERT [dbo].[News] ([id], [dateTitle], [headline], [guid], [isTop], [summary], [thumbnailPath], [newsContentHtml], [newsImageJson], [createTime]) VALUES (15, CAST(N'2024-07-22' AS Date), N'TAYANA 52', N'a8ce7d7d-aed3-480a-8e12-2d3b92378cf848', 0, NULL, NULL, NULL, N'[]', CAST(N'2024-07-22T17:26:40.483' AS DateTime))
INSERT [dbo].[News] ([id], [dateTitle], [headline], [guid], [isTop], [summary], [thumbnailPath], [newsContentHtml], [newsImageJson], [createTime]) VALUES (16, CAST(N'2024-07-22' AS Date), N'TAYANA 53', N'5602189e-dfd3-4c2b-9770-f8d00664b7ee12', 0, NULL, NULL, NULL, N'[]', CAST(N'2024-07-22T17:26:45.123' AS DateTime))
SET IDENTITY_INSERT [dbo].[News] OFF
GO
SET IDENTITY_INSERT [dbo].[Specification] ON 

INSERT [dbo].[Specification] ([id], [yachtModel_ID], [detailTitleSort_ID], [detail], [createTime]) VALUES (1, 1, 1, N'Hand laid up FRP hull, white with blue cove stripe and boot top.', CAST(N'2024-07-27T18:50:11.707' AS DateTime))
INSERT [dbo].[Specification] ([id], [yachtModel_ID], [detailTitleSort_ID], [detail], [createTime]) VALUES (3, 1, 1, N'Teak rubrail.', CAST(N'2024-07-27T18:51:10.787' AS DateTime))
INSERT [dbo].[Specification] ([id], [yachtModel_ID], [detailTitleSort_ID], [detail], [createTime]) VALUES (4, 1, 5, N'Hand laid up FRP cord deck.', CAST(N'2024-07-27T18:52:44.723' AS DateTime))
INSERT [dbo].[Specification] ([id], [yachtModel_ID], [detailTitleSort_ID], [detail], [createTime]) VALUES (5, 1, 5, N'Seven Lewmar winches&lt;br&gt;Two 40CST jib sheet&lt;br&gt;One 40CSTmain sheet&lt;br&gt;One 30CST staysail&lt;br&gt;One 30CST jib halyard&lt;br&gt;One 30CST main halyard&lt;br&gt;One 30CST staysail halyard&lt;', CAST(N'2024-07-27T18:52:53.500' AS DateTime))
SET IDENTITY_INSERT [dbo].[Specification] OFF
GO
SET IDENTITY_INSERT [dbo].[User] ON 

INSERT [dbo].[User] ([id], [account], [password], [salt], [email], [name], [permission], [createDate]) VALUES (8, N'admin', N'hKoHtLaQj6TKA1nkiCxEeg==', N'jNGJQq54PxxNgh+eZO8L4A==', N'admin@gmail.com', N'Admin', N'admin', CAST(N'2024-07-06T14:58:30.523' AS DateTime))
INSERT [dbo].[User] ([id], [account], [password], [salt], [email], [name], [permission], [createDate]) VALUES (9, N'test1', N'gOTABVIQAjlhPsOPNxcB8Q==', N'FGlDPx8M1uzPxxKxLM/SuQ==', N'test1@gmail.com', N'Test1', N'user', CAST(N'2024-07-06T14:59:33.287' AS DateTime))
INSERT [dbo].[User] ([id], [account], [password], [salt], [email], [name], [permission], [createDate]) VALUES (10, N'test2', N'/9maVDYi9bDGjr0oE0Veeg==', N'KqLxEU4AH4Uz29Ank0OUhQ==', N'test2@gmail.com', N'Test2', N'user', CAST(N'2024-07-06T15:00:10.340' AS DateTime))
INSERT [dbo].[User] ([id], [account], [password], [salt], [email], [name], [permission], [createDate]) VALUES (11, N'test3', N'Lx0a8qvkiWQNgRZ6Ky0g8w==', N'm7mIrfyOAbx8kQNjgKdNNQ==', N'test3@gmail.com', N'Test3', N'user', CAST(N'2024-07-08T17:48:20.083' AS DateTime))
SET IDENTITY_INSERT [dbo].[User] OFF
GO
SET IDENTITY_INSERT [dbo].[Yachts] ON 

INSERT [dbo].[Yachts] ([id], [yachtsModel], [isNewDesign], [isNewBuilding], [createTime], [guid], [bannerImgPathJSON], [overviewContentHtml], [overviewDimensionsImgPath], [overviewDownloadsFilePath], [overviewDimensionsJSON], [layoutDeckPlanImgPathJSON]) VALUES (1, N'Tayana 37', 0, 0, CAST(N'2024-07-22T22:21:34.077' AS DateTime), N'b67054a1-9908-44fe-b694-5f8dc750ec4305', N'[{"SavePath":"dealers001(1).jpg"}]', N'&lt;div class=&quot;title&quot; style=&quot;float: left; width: 713px; background-image: url(&amp;quot;/web/20160915235930im_/http://www.tayanaworld.com/images/icon005.gif&amp;quot;); background-repeat: no-repeat; border-bottom: 1px solid rgb(211, 211, 211); padding-bottom: 7px; background-position: 0px 6px; margin-bottom: 10px; height: 20px; color: rgb(63, 63, 63); font-family: Arial, Helvetica, sans-serif; font-size: 14.4px;&quot;&gt;
	&lt;span style=&quot;color: rgb(52, 169, 212); font-weight: bold; font-size: 17.28px; padding-left: 21px; float: left; margin-top: 5px;&quot;&gt;Tayana 37&lt;/span&gt;&lt;/div&gt;
&lt;div class=&quot;menu_y&quot; style=&quot;float: left; width: 713px; margin-bottom: 25px; color: rgb(63, 63, 63); font-family: Arial, Helvetica, sans-serif; font-size: 14.4px;&quot;&gt;
	&lt;ul style=&quot;margin: 0px; padding-right: 5px; padding-left: 0px; list-style-type: none; float: left; background-image: url(&amp;quot;/web/20160915235930im_/http://www.tayanaworld.com/images/menu_y1_back.jpg&amp;quot;); background-repeat: no-repeat; background-position: right top;&quot;&gt;
		&lt;li class=&quot;menu_y00&quot; style=&quot;margin: 0px; padding: 0px; list-style-type: none; background-image: url(&amp;quot;/web/20160915235930im_/http://www.tayanaworld.com/images/menu_y1_03.jpg&amp;quot;); background-repeat: no-repeat; width: 51px; float: left; display: block; height: 41px; font-size: 0px;&quot;&gt;
			YACHTS&lt;/li&gt;
		&lt;li style=&quot;margin: 0px; padding: 0px; list-style-type: none; float: left; display: block; height: 41px; font-size: 0px;&quot;&gt;
			&lt;a class=&quot;menu_yli01&quot; href=&quot;https://web.archive.org/web/20170823004620/http://www.tayanaworld.com/Yachts_OverView.aspx?id=6d245b62-ff07-463b-95b3-277f0e5aac25&quot; style=&quot;float: left; background-image: url(&amp;quot;/web/20160915235930im_/http://www.tayanaworld.com/images/menu_y1_04.jpg&amp;quot;); background-repeat: no-repeat; width: 104px; color: rgb(139, 139, 139); text-decoration-line: none; height: 41px; display: block;&quot;&gt;Interior&lt;/a&gt;&lt;/li&gt;
		&lt;li style=&quot;margin: 0px; padding: 0px; list-style-type: none; float: left; display: block; height: 41px; font-size: 0px;&quot;&gt;
			&lt;a class=&quot;menu_yli02&quot; href=&quot;https://web.archive.org/web/20170823004620/http://www.tayanaworld.com/Yachts_Layout.aspx?id=6d245b62-ff07-463b-95b3-277f0e5aac25&quot; style=&quot;background-image: url(&amp;quot;/web/20160915235930im_/http://www.tayanaworld.com/images/menu_y1_05.jpg&amp;quot;); width: 160px; color: rgb(139, 139, 139); text-decoration-line: none; height: 41px; display: block;&quot;&gt;Layout &amp;amp; deck plan&lt;/a&gt;&lt;/li&gt;
		&lt;li style=&quot;margin: 0px; padding: 0px; list-style-type: none; float: left; display: block; height: 41px; font-size: 0px;&quot;&gt;
			&lt;a class=&quot;menu_yli03&quot; href=&quot;https://web.archive.org/web/20170823004620/http://www.tayanaworld.com/Yachts_Specification.aspx?id=6d245b62-ff07-463b-95b3-277f0e5aac25&quot; style=&quot;background-image: url(&amp;quot;/web/20160915235930im_/http://www.tayanaworld.com/images/menu_y1_06.jpg&amp;quot;); width: 123px; color: rgb(139, 139, 139); text-decoration-line: none; height: 41px; display: block;&quot;&gt;Specification&lt;/a&gt;&lt;/li&gt;
		&lt;li style=&quot;margin: 0px; padding: 0px; list-style-type: none; float: left; display: block; height: 41px; font-size: 0px;&quot;&gt;
			&amp;nbsp;&lt;/li&gt;
	&lt;/ul&gt;
&lt;/div&gt;
&lt;div class=&quot;box1&quot; style=&quot;float: left; width: 713px; text-align: justify; color: rgb(63, 63, 63); font-family: Arial, Helvetica, sans-serif; font-size: 14.4px;&quot;&gt;
	The Tayana 37 is perhaps the most successful semi-custom cruising boat to be built. It was designed by Bob Perry and introduced in 1975 as a response to the Westsail 32 which were selling in enormous numbers. Today looking back, with the boat still in production with a boat count of 588, most still sailing, and an active and owners community, its very apparent that Perry has succeeded.&lt;br /&gt;
	&lt;br /&gt;
	One could say the boat was designed to ignite imaginations of tropical sunsets in exotic locations; think oodles of teak and a beautiful custom interior, wrapped into traditional double-ender hull with a full keel. Beneath the alluring romance, you&amp;rsquo;ll find a boat that is solidly built, and indeed many Tayana 37s can be found on the blue water cruising circuit around the world.&lt;br /&gt;
	&lt;br /&gt;
	Tayana 37 has been constructed of the finest materials, using the best techniques. There is no better yacht in her size range on the market. With care and proper maintenance she will not only prove to be an excellent investment, she will take you cruising anywhere in the world safely and comfortably. This is being proven almost daily. Ocean crossings by Tayana 37&amp;rsquo;s are routine. Circumnavigations have been reported. By the same token, over 500 of these yachts are the primary homes of their owners.&lt;/div&gt;
', N'ya01(1).jpg', N'test(1).pdf', N'[{"SaveItem":"Video URL","SaveValue":"https://www.youtube.com/watch?v=nL_VoX0ZR0E&list=RDnL_VoX0ZR0E&start_radio=1"},{"SaveItem":"Downloads Title","SaveValue":"testTitle"},{"SaveItem":"Hull length","SaveValue":"36’-8”"},{"SaveItem":"L.W.L.","SaveValue":"31’-0”’"},{"SaveItem":"B. MAX","SaveValue":"11’-6”"},{"SaveItem":"Standard draft","SaveValue":"5’-8”"},{"SaveItem":"Ballast","SaveValue":"8000 lbs"},{"SaveItem":"Displacement","SaveValue":"22500 lbs"},{"SaveItem":"Sail area","SaveValue":"768 sq.ft."},{"SaveItem":"Cutter","SaveValue":"861 sq.ft."}]', N'[{"SavePath":"deckplan01(1).jpg"}]')
INSERT [dbo].[Yachts] ([id], [yachtsModel], [isNewDesign], [isNewBuilding], [createTime], [guid], [bannerImgPathJSON], [overviewContentHtml], [overviewDimensionsImgPath], [overviewDownloadsFilePath], [overviewDimensionsJSON], [layoutDeckPlanImgPathJSON]) VALUES (2, N'Aaa 12', 0, 0, CAST(N'2024-07-23T10:12:11.933' AS DateTime), N'3e6cc15c-307e-4d43-8b1e-0a1d5c0d038891', N'[]', NULL, NULL, NULL, N'[{"SaveItem":"Video URL","SaveValue":""},{"SaveItem":"Downloads Title","SaveValue":""}]', N'[]')
SET IDENTITY_INSERT [dbo].[Yachts] OFF
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_User]    Script Date: 2024/7/30 下午 01:50:04 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_User] ON [dbo].[User]
(
	[account] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Company] ADD  CONSTRAINT [DF_Company_certificatVerticalImgJSON]  DEFAULT ('[]') FOR [certificatVerticalImgJSON]
GO
ALTER TABLE [dbo].[Company] ADD  CONSTRAINT [DF_Company_certificatHorizontalImgJSON]  DEFAULT ('[]') FOR [certificatHorizontalImgJSON]
GO
ALTER TABLE [dbo].[Company] ADD  CONSTRAINT [DF_Company_createDate]  DEFAULT (getdate()) FOR [createDate]
GO
ALTER TABLE [dbo].[Country] ADD  CONSTRAINT [DF_Country_createTime]  DEFAULT (getdate()) FOR [createTime]
GO
ALTER TABLE [dbo].[Dealers] ADD  CONSTRAINT [DF_Dealers_createDate]  DEFAULT (getdate()) FOR [createDate]
GO
ALTER TABLE [dbo].[DetailTitleSort] ADD  CONSTRAINT [DF_DetailTitleSort_creatTime]  DEFAULT (getdate()) FOR [creatTime]
GO
ALTER TABLE [dbo].[News] ADD  CONSTRAINT [DF_News_isTop]  DEFAULT ((0)) FOR [isTop]
GO
ALTER TABLE [dbo].[News] ADD  CONSTRAINT [DF_News_newsImageJson]  DEFAULT ('[]') FOR [newsImageJson]
GO
ALTER TABLE [dbo].[News] ADD  CONSTRAINT [DF_News_createTime]  DEFAULT (getdate()) FOR [createTime]
GO
ALTER TABLE [dbo].[Specification] ADD  CONSTRAINT [DF_Specification_createTime]  DEFAULT (getdate()) FOR [createTime]
GO
ALTER TABLE [dbo].[User] ADD  CONSTRAINT [DF_User_permission]  DEFAULT ('user') FOR [permission]
GO
ALTER TABLE [dbo].[User] ADD  CONSTRAINT [DF_User_createDate]  DEFAULT (getdate()) FOR [createDate]
GO
ALTER TABLE [dbo].[Yachts] ADD  CONSTRAINT [DF_Yachts_createTime]  DEFAULT (getdate()) FOR [createTime]
GO
ALTER TABLE [dbo].[Yachts] ADD  CONSTRAINT [DF_Yachts_bannerImgPathJSON]  DEFAULT ('[]') FOR [bannerImgPathJSON]
GO
ALTER TABLE [dbo].[Yachts] ADD  CONSTRAINT [DF_Yachts_overviewDimensionsJSON]  DEFAULT ('[]') FOR [overviewDimensionsJSON]
GO
ALTER TABLE [dbo].[Yachts] ADD  CONSTRAINT [DF_Yachts_layoutDeckPlanImgPathJSON]  DEFAULT ('[]') FOR [layoutDeckPlanImgPathJSON]
GO
ALTER TABLE [dbo].[Dealers]  WITH CHECK ADD  CONSTRAINT [FK_Dealers_Country] FOREIGN KEY([countryId])
REFERENCES [dbo].[Country] ([id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Dealers] CHECK CONSTRAINT [FK_Dealers_Country]
GO
ALTER TABLE [dbo].[Specification]  WITH CHECK ADD  CONSTRAINT [FK_Specification_DetailTitleSort] FOREIGN KEY([detailTitleSort_ID])
REFERENCES [dbo].[DetailTitleSort] ([id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Specification] CHECK CONSTRAINT [FK_Specification_DetailTitleSort]
GO
ALTER TABLE [dbo].[Specification]  WITH CHECK ADD  CONSTRAINT [FK_Specification_Yachts] FOREIGN KEY([yachtModel_ID])
REFERENCES [dbo].[Yachts] ([id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Specification] CHECK CONSTRAINT [FK_Specification_Yachts]
GO
USE [master]
GO
ALTER DATABASE [20240702Yachts] SET  READ_WRITE 
GO
