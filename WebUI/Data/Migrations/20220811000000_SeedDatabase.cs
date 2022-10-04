using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebUI.Data.Migrations
{
    /// <summary>
    /// This migration seeds the database for development, don't use in production, hence the #if DEBUG preprocessor directives.
    /// See https://docs.microsoft.com/en-us/ef/core/modeling/data-seeding,
    /// especially https://docs.microsoft.com/en-us/ef/core/modeling/data-seeding#manual-migration-customization.
    /// </summary>
    public partial class SeedDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
#if DEBUG
            // Only seed database when DEBUG is set (in development).
            string seedIdentity = @"INSERT [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'109440a0-c6d0-47ce-b4d5-1bd05821a169', N'Admin', N'ADMIN', N'e59676a1-0230-450d-95d3-05f2ee524073')
                                    GO
                                    INSERT [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'6f8de09e-6329-44ff-93b1-51ed51dacdb8', N'SuperAdmin', N'SUPERADMIN', N'9df6e51d-a565-4845-973b-8c08529a15ed')
                                    GO
                                    INSERT [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'e6dccb05-a916-4862-af64-08fbbc1d3bca', N'User', N'USER', N'63dc3ea2-7883-4340-98b7-2d701d0aa942')
                                    GO
                                    INSERT [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'13c80d1a-5108-43a6-90c5-2815a4319f62', N'user001@techriver.net', N'USER001@TECHRIVER.NET', N'user001@techriver.net', N'USER001@TECHRIVER.NET', 1, N'AQAAAAEAACcQAAAAEM4nFRYTBiqINZvyVP/3k6w8h5yWLUXc3uTVzyAd7w0f1XQRH1xDc3i/Dsu1MxcYkg==', N'7NB2533PTYZHMWPQL24RUGZ7WNIDQREV', N'5aca6ab3-1001-48ab-990c-7005225b4578', NULL, 0, 0, NULL, 1, 0)
                                    GO
                                    INSERT [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'18245080-3a9c-41a3-b9bc-bd215b6fd084', N'user002@techriver.net', N'USER002@TECHRIVER.NET', N'user002@techriver.net', N'USER002@TECHRIVER.NET', 1, N'AQAAAAEAACcQAAAAENSgIfllsFCDAE73cxHUPZEay64izDFr0AJksMyDGwdFyi0/rTQP7FoaQu+1HGboRQ==', N'4W5NU6RQZGJ5DDFX4OKQSTK4LLZRTYPB', N'e8ec97b9-6782-4c39-a586-32751ec58471', NULL, 0, 0, NULL, 1, 0)
                                    GO
                                    INSERT [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'28cb1858-fd6e-412f-9d20-0ffc07759a1e', N'user003@techriver.net', N'USER003@TECHRIVER.NET', N'user003@techriver.net', N'USER003@TECHRIVER.NET', 1, N'AQAAAAEAACcQAAAAEBJ95/cGVdBYALACT2j51yIvSxgnTq2jGhkp1GZ/XsSjFWAyoQaLM6H7lh5Blrwjng==', N'24VXIG5MZYKL6PC2FQ3WS67BF7ZZFMHT', N'638fa44f-6c89-4b11-9b2e-3a62dca31e5a', NULL, 0, 0, NULL, 1, 0)
                                    GO
                                    INSERT [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'71a9301d-26f1-4056-8714-db2d3ffb88c3', N'user004@gmail.com', N'USER004@GMAIL.COM', N'user004@gmail.com', N'USER004@GMAIL.COM', 1, N'AQAAAAEAACcQAAAAECUEa2xCM4Z2kdWUChy9y2GzWHfhKOYuby7kwLMIu2xyg2ddGqlbf5ZMGwM+RwHLcw==', N'XRRNVCF3BNGCE6RGHHXXWUD5L4VK3BHL', N'd7546012-22a2-4a1e-88bb-a9990877aa2e', NULL, 0, 0, NULL, 1, 0)
                                    GO
                                    INSERT [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'790c56a6-669b-44a1-a299-2471525afc3f', N'user005@techriver.net', N'USER005@TECHRIVER.NET', N'user005@techriver.net', N'USER005@TECHRIVER.NET', 1, N'AQAAAAEAACcQAAAAEPwaTc5iMlx3xzPq3fKeSVDS+v/JJ3MRhNEdAn/4D713xmvNqpq/OxoIwg1WcsNMhA==', N'YJZDKYA3BZTWPXVQSTWOMYJIOTIKFEPD', N'5b346b7c-2daa-4d62-abce-2ad56dea2980', NULL, 0, 0, NULL, 1, 0)
                                    GO
                                    INSERT [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'8b7f558c-b1a7-410e-88c6-d2a723ae8596', N'user006@techriver.net', N'USER006@TECHRIVER.NET', N'user006@techriver.net', N'USER006@TECHRIVER.NET', 1, N'AQAAAAEAACcQAAAAECuN435ClWGwWuFn+vGKuUogTveo6EfeIw+NT0RXnQkx0yy5ND/5uyuqi32f9ZbQqg==', N'OMHPJK6ABVCFZAXNRBHAICVJLLUC562N', N'3f9cb763-6bf8-4e47-beec-827c5ab13b04', NULL, 0, 0, NULL, 1, 0)
                                    GO
                                    INSERT [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'9d02ee22-eb3b-43e3-8f0d-0e96dc271d5d', N'user007@techriver.net', N'USER007@TECHRIVER.NET', N'user007@techriver.net', N'USER007@TECHRIVER.NET', 1, N'AQAAAAEAACcQAAAAEAxjb0CWoyUeAdvP0fNXGUJTKbvASm0WMdcAj3xirxJm405uL2WN5F6G57v6wnFZmg==', N'MIMNWM4C3DKEXS5WP6WLQE4XKGTWRJAC', N'f0546dad-7f9b-4b00-a0e1-02ab81ab8942', NULL, 0, 0, NULL, 1, 0)
                                    GO
                                    INSERT [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'c1fde4db-6383-47ba-a31e-a02225f205ef', N'user008@hotmail.com', N'USER008@HOTMAIL.COM', N'user008@hotmail.com', N'USER008@HOTMAIL.COM', 1, N'AQAAAAEAACcQAAAAELNcIKp3RlmBtQaVs0a5g3CCRXIY6nYKvxo4N/UgGjLc6Ong6rXDcAEMSHogO0zf4A==', N'LMZF3HR7DG55HDKJ4AHATBVKFYK7IFYF', N'cd1bd728-02dc-4748-a5eb-1be721dab73c', NULL, 0, 0, NULL, 1, 0)
                                    GO
                                    INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'9d02ee22-eb3b-43e3-8f0d-0e96dc271d5d', N'109440a0-c6d0-47ce-b4d5-1bd05821a169')
                                    GO
                                    INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'13c80d1a-5108-43a6-90c5-2815a4319f62', N'6f8de09e-6329-44ff-93b1-51ed51dacdb8')
                                    GO
                                    INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'18245080-3a9c-41a3-b9bc-bd215b6fd084', N'6f8de09e-6329-44ff-93b1-51ed51dacdb8')
                                    GO
                                    INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'28cb1858-fd6e-412f-9d20-0ffc07759a1e', N'6f8de09e-6329-44ff-93b1-51ed51dacdb8')
                                    GO
                                    INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'71a9301d-26f1-4056-8714-db2d3ffb88c3', N'6f8de09e-6329-44ff-93b1-51ed51dacdb8')
                                    GO
                                    INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'790c56a6-669b-44a1-a299-2471525afc3f', N'6f8de09e-6329-44ff-93b1-51ed51dacdb8')
                                    GO
                                    INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'8b7f558c-b1a7-410e-88c6-d2a723ae8596', N'6f8de09e-6329-44ff-93b1-51ed51dacdb8')
                                    GO
                                    INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'c1fde4db-6383-47ba-a31e-a02225f205ef', N'6f8de09e-6329-44ff-93b1-51ed51dacdb8')
                                    GO";
            
            string seedClients = @"SET IDENTITY_INSERT [dbo].[Clients] ON 
                                    GO
                                    INSERT [dbo].[Clients] ([ClientId], [Name], [ShortName], [PaymentTerms], [StreetAddress], [StreetAddress2], [City], [State], [PostalCode], [PhoneNumber], [Notes], [BillingRate], [CreatedDateTime], [CreatedById], [ChangedDateTime], [ChangedById], [Deleted], [DeletedById], [DeletedDateTime], [Active]) VALUES (1, N'Test Client - The first client I added 00001', N'Test 00001', N'Due on reciept', N'1 Main Street', N'Apt. 23', N'Boston', N'MA', N'33029', N'1-891-112-3131', N'This is a test', 233.0000, CAST(N'2020-11-12T20:40:17.7630201' AS DateTime2), N'9d02ee22-eb3b-43e3-8f0d-0e96dc271d5d', CAST(N'2021-03-10T21:25:44.4177446' AS DateTime2), N'13c80d1a-5108-43a6-90c5-2815a4319f62', 0, N'9d02ee22-eb3b-43e3-8f0d-0e96dc271d5d', CAST(N'2020-11-12T20:40:28.7492068' AS DateTime2), 1)
                                    GO
                                    INSERT [dbo].[Clients] ([ClientId], [Name], [ShortName], [PaymentTerms], [StreetAddress], [StreetAddress2], [City], [State], [PostalCode], [PhoneNumber], [Notes], [BillingRate], [CreatedDateTime], [CreatedById], [ChangedDateTime], [ChangedById], [Deleted], [DeletedById], [DeletedDateTime], [Active]) VALUES (2, N'Great Name for a client 00002', N'Great 00002', N'Payment ', N'Little Cow Road', NULL, N'Big Town', N'NY', N'98039', N'9090900087', N'Notes', 333.0000, CAST(N'2020-11-12T20:43:13.5701440' AS DateTime2), N'9d02ee22-eb3b-43e3-8f0d-0e96dc271d5d', CAST(N'2021-03-10T21:25:54.7731371' AS DateTime2), N'13c80d1a-5108-43a6-90c5-2815a4319f62', 0, N'9d02ee22-eb3b-43e3-8f0d-0e96dc271d5d', CAST(N'2020-11-15T19:54:02.3098146' AS DateTime2), 1)
                                    GO
                                    INSERT [dbo].[Clients] ([ClientId], [Name], [ShortName], [PaymentTerms], [StreetAddress], [StreetAddress2], [City], [State], [PostalCode], [PhoneNumber], [Notes], [BillingRate], [CreatedDateTime], [CreatedById], [ChangedDateTime], [ChangedById], [Deleted], [DeletedById], [DeletedDateTime], [Active]) VALUES (3, N'Test Name', N'name', N'terms', N'addr', N'addr2', N'city', N'state', N'99999', N'9999999999', N'notes', 399.0000, CAST(N'2020-11-15T17:59:34.1888868' AS DateTime2), N'9d02ee22-eb3b-43e3-8f0d-0e96dc271d5d', CAST(N'2021-03-12T10:29:16.9416091' AS DateTime2), N'13c80d1a-5108-43a6-90c5-2815a4319f62', 1, N'13c80d1a-5108-43a6-90c5-2815a4319f62', CAST(N'2021-03-12T10:29:27.9310298' AS DateTime2), 1)
                                    GO
                                    INSERT [dbo].[Clients] ([ClientId], [Name], [ShortName], [PaymentTerms], [StreetAddress], [StreetAddress2], [City], [State], [PostalCode], [PhoneNumber], [Notes], [BillingRate], [CreatedDateTime], [CreatedById], [ChangedDateTime], [ChangedById], [Deleted], [DeletedById], [DeletedDateTime], [Active]) VALUES (4, N'sdf', N'fsad', N'fsda', N'fsd', N'fasd', N'e', N'w3', N'33', N'asdf', N'dfsa', 44.0000, CAST(N'2020-11-15T19:53:55.3138627' AS DateTime2), N'9d02ee22-eb3b-43e3-8f0d-0e96dc271d5d', NULL, NULL, 1, N'9d02ee22-eb3b-43e3-8f0d-0e96dc271d5d', CAST(N'2020-11-15T19:54:13.3455057' AS DateTime2), 1)
                                    GO
                                    SET IDENTITY_INSERT [dbo].[Clients] OFF
                                    GO";
            
            string seedProjects = @"SET IDENTITY_INSERT [dbo].[Projects] ON 
                                    GO
                                    INSERT [dbo].[Projects] ([ProjectId], [ClientId], [Name], [StartDate], [EndDate], [InvoiceFrequency], [Notes], [BillingType], [BillingRate], [Deposit], [ReferralPercent], [EstimatedRevenue], [CreatedDateTime], [CreatedById], [ChangedDateTime], [ChangedById], [Deleted], [DeletedById], [DeletedDateTime], [Active]) VALUES (5, NULL, N'Test Project - GB changed last', CAST(N'2020-01-01T00:00:00.0000000' AS DateTime2), CAST(N'2020-09-20T00:00:00.0000000' AS DateTime2), 1, N'This is a test', 1, 200.0000, 10000.0000, 3.0000, 3000000.0000, CAST(N'2020-11-11T19:18:43.0780416' AS DateTime2), N'9d02ee22-eb3b-43e3-8f0d-0e96dc271d5d', CAST(N'2020-11-12T21:10:04.8484244' AS DateTime2), N'18245080-3a9c-41a3-b9bc-bd215b6fd084', 0, N'9d02ee22-eb3b-43e3-8f0d-0e96dc271d5d', CAST(N'2020-11-12T17:38:55.8304947' AS DateTime2), 1)
                                    GO
                                    INSERT [dbo].[Projects] ([ProjectId], [ClientId], [Name], [StartDate], [EndDate], [InvoiceFrequency], [Notes], [BillingType], [BillingRate], [Deposit], [ReferralPercent], [EstimatedRevenue], [CreatedDateTime], [CreatedById], [ChangedDateTime], [ChangedById], [Deleted], [DeletedById], [DeletedDateTime], [Active]) VALUES (6, NULL, N'Test Number 2', CAST(N'2020-01-01T00:00:00.0000000' AS DateTime2), CAST(N'2020-09-20T00:00:00.0000000' AS DateTime2), 3, N'This is a note', 3, 333.0000, 3333.0000, 3.0000, 333333.0000, CAST(N'2020-11-11T20:07:30.3720935' AS DateTime2), N'9d02ee22-eb3b-43e3-8f0d-0e96dc271d5d', CAST(N'2020-11-12T17:39:24.6807267' AS DateTime2), N'9d02ee22-eb3b-43e3-8f0d-0e96dc271d5d', 0, N'9d02ee22-eb3b-43e3-8f0d-0e96dc271d5d', CAST(N'2020-11-12T17:38:58.1972085' AS DateTime2), 1)
                                    GO
                                    INSERT [dbo].[Projects] ([ProjectId], [ClientId], [Name], [StartDate], [EndDate], [InvoiceFrequency], [Notes], [BillingType], [BillingRate], [Deposit], [ReferralPercent], [EstimatedRevenue], [CreatedDateTime], [CreatedById], [ChangedDateTime], [ChangedById], [Deleted], [DeletedById], [DeletedDateTime], [Active]) VALUES (7, 1, N'Test Project number 3 ', CAST(N'2020-01-01T00:00:00.0000000' AS DateTime2), CAST(N'2020-09-20T00:00:00.0000000' AS DateTime2), 1, N'Notes for 3', 2, 333.0000, 3333.0000, 3.0000, 33333.0000, CAST(N'2020-11-12T15:11:29.8261065' AS DateTime2), N'9d02ee22-eb3b-43e3-8f0d-0e96dc271d5d', CAST(N'2020-11-12T18:21:12.6291911' AS DateTime2), N'9d02ee22-eb3b-43e3-8f0d-0e96dc271d5d', 0, N'9d02ee22-eb3b-43e3-8f0d-0e96dc271d5d', CAST(N'2020-11-12T21:13:39.3216159' AS DateTime2), 1)
                                    GO
                                    INSERT [dbo].[Projects] ([ProjectId], [ClientId], [Name], [StartDate], [EndDate], [InvoiceFrequency], [Notes], [BillingType], [BillingRate], [Deposit], [ReferralPercent], [EstimatedRevenue], [CreatedDateTime], [CreatedById], [ChangedDateTime], [ChangedById], [Deleted], [DeletedById], [DeletedDateTime], [Active]) VALUES (8, 2, N'Project 4 [EDITED]', CAST(N'2020-01-01T00:00:00.0000000' AS DateTime2), CAST(N'2020-09-20T00:00:00.0000000' AS DateTime2), 2, N'good notes', 1, 48.0000, 444.0000, 8.0000, 4444.0000, CAST(N'2020-11-12T17:35:00.4497312' AS DateTime2), N'9d02ee22-eb3b-43e3-8f0d-0e96dc271d5d', CAST(N'2020-11-15T19:54:28.0276936' AS DateTime2), N'9d02ee22-eb3b-43e3-8f0d-0e96dc271d5d', 0, N'18245080-3a9c-41a3-b9bc-bd215b6fd084', CAST(N'2020-11-12T17:46:46.1535536' AS DateTime2), 1)
                                    GO
                                    INSERT [dbo].[Projects] ([ProjectId], [ClientId], [Name], [StartDate], [EndDate], [InvoiceFrequency], [Notes], [BillingType], [BillingRate], [Deposit], [ReferralPercent], [EstimatedRevenue], [CreatedDateTime], [CreatedById], [ChangedDateTime], [ChangedById], [Deleted], [DeletedById], [DeletedDateTime], [Active]) VALUES (9, 2, N'P5 [EDITED ONCE AGAIN]', CAST(N'2020-01-01T00:00:00.0000000' AS DateTime2), CAST(N'2020-09-20T00:00:00.0000000' AS DateTime2), 3, N'Test P5 notes', 1, 5.0000, 5.0000, 5.0000, 5.0000, CAST(N'2020-11-12T17:37:17.4566414' AS DateTime2), N'9d02ee22-eb3b-43e3-8f0d-0e96dc271d5d', CAST(N'2020-11-12T21:17:54.8425335' AS DateTime2), N'9d02ee22-eb3b-43e3-8f0d-0e96dc271d5d', 0, N'18245080-3a9c-41a3-b9bc-bd215b6fd084', CAST(N'2020-11-12T17:47:23.6067143' AS DateTime2), 1)
                                    GO
                                    INSERT [dbo].[Projects] ([ProjectId], [ClientId], [Name], [StartDate], [EndDate], [InvoiceFrequency], [Notes], [BillingType], [BillingRate], [Deposit], [ReferralPercent], [EstimatedRevenue], [CreatedDateTime], [CreatedById], [ChangedDateTime], [ChangedById], [Deleted], [DeletedById], [DeletedDateTime], [Active]) VALUES (10, NULL, N'New One 6', CAST(N'2020-01-01T00:00:00.0000000' AS DateTime2), CAST(N'2020-09-20T00:00:00.0000000' AS DateTime2), 2, N'Notes', 2, 33.0000, 33.0000, 33.0000, 333.0000, CAST(N'2020-11-12T17:48:05.7081813' AS DateTime2), N'18245080-3a9c-41a3-b9bc-bd215b6fd084', CAST(N'2020-11-15T17:02:42.5832813' AS DateTime2), N'9d02ee22-eb3b-43e3-8f0d-0e96dc271d5d', 0, N'9d02ee22-eb3b-43e3-8f0d-0e96dc271d5d', CAST(N'2020-11-12T21:13:24.1457554' AS DateTime2), 1)
                                    GO
                                    INSERT [dbo].[Projects] ([ProjectId], [ClientId], [Name], [StartDate], [EndDate], [InvoiceFrequency], [Notes], [BillingType], [BillingRate], [Deposit], [ReferralPercent], [EstimatedRevenue], [CreatedDateTime], [CreatedById], [ChangedDateTime], [ChangedById], [Deleted], [DeletedById], [DeletedDateTime], [Active]) VALUES (11, NULL, N'Project', CAST(N'2020-01-01T00:00:00.0000000' AS DateTime2), CAST(N'2020-09-20T00:00:00.0000000' AS DateTime2), 2, NULL, 2, 0.0000, 0.0000, 0.0000, 0.0000, CAST(N'2020-11-12T18:30:11.6299614' AS DateTime2), N'9d02ee22-eb3b-43e3-8f0d-0e96dc271d5d', CAST(N'2020-11-12T21:13:12.2158989' AS DateTime2), N'9d02ee22-eb3b-43e3-8f0d-0e96dc271d5d', 1, N'9d02ee22-eb3b-43e3-8f0d-0e96dc271d5d', CAST(N'2020-11-12T21:13:18.7491997' AS DateTime2), 1)
                                    GO
                                    INSERT [dbo].[Projects] ([ProjectId], [ClientId], [Name], [StartDate], [EndDate], [InvoiceFrequency], [Notes], [BillingType], [BillingRate], [Deposit], [ReferralPercent], [EstimatedRevenue], [CreatedDateTime], [CreatedById], [ChangedDateTime], [ChangedById], [Deleted], [DeletedById], [DeletedDateTime], [Active]) VALUES (12, NULL, N'This is the name', CAST(N'2020-01-01T00:00:00.0000000' AS DateTime2), CAST(N'2020-09-20T00:00:00.0000000' AS DateTime2), 0, NULL, 0, 0.0000, 0.0000, 0.0000, 0.0000, CAST(N'2020-11-12T18:32:51.5780946' AS DateTime2), N'9d02ee22-eb3b-43e3-8f0d-0e96dc271d5d', CAST(N'2020-11-12T21:13:14.5175797' AS DateTime2), N'9d02ee22-eb3b-43e3-8f0d-0e96dc271d5d', 1, N'9d02ee22-eb3b-43e3-8f0d-0e96dc271d5d', CAST(N'2020-11-12T21:13:28.2595312' AS DateTime2), 0)
                                    GO
                                    INSERT [dbo].[Projects] ([ProjectId], [ClientId], [Name], [StartDate], [EndDate], [InvoiceFrequency], [Notes], [BillingType], [BillingRate], [Deposit], [ReferralPercent], [EstimatedRevenue], [CreatedDateTime], [CreatedById], [ChangedDateTime], [ChangedById], [Deleted], [DeletedById], [DeletedDateTime], [Active]) VALUES (13, NULL, N'Name [Changed]', CAST(N'2020-01-01T00:00:00.0000000' AS DateTime2), CAST(N'2020-09-20T00:00:00.0000000' AS DateTime2), 1, N'Test', 2, 3.0000, 3.0000, 0.0700, 0.1100, CAST(N'2020-11-12T19:53:22.5198903' AS DateTime2), N'9d02ee22-eb3b-43e3-8f0d-0e96dc271d5d', CAST(N'2020-11-15T17:58:40.2456146' AS DateTime2), N'9d02ee22-eb3b-43e3-8f0d-0e96dc271d5d', 1, N'9d02ee22-eb3b-43e3-8f0d-0e96dc271d5d', CAST(N'2020-11-15T17:58:44.7698591' AS DateTime2), 1)
                                    GO
                                    INSERT [dbo].[Projects] ([ProjectId], [ClientId], [Name], [StartDate], [EndDate], [InvoiceFrequency], [Notes], [BillingType], [BillingRate], [Deposit], [ReferralPercent], [EstimatedRevenue], [CreatedDateTime], [CreatedById], [ChangedDateTime], [ChangedById], [Deleted], [DeletedById], [DeletedDateTime], [Active]) VALUES (14, 2, N'This one has a client!', CAST(N'2020-02-17T00:00:00.0000000' AS DateTime2), CAST(N'2020-11-19T00:00:00.0000000' AS DateTime2), 2, N'Notes', 1, 0.0000, 0.0000, 0.0000, 0.0000, CAST(N'2020-11-12T21:04:34.6378207' AS DateTime2), N'9d02ee22-eb3b-43e3-8f0d-0e96dc271d5d', CAST(N'2020-11-12T21:10:23.5476005' AS DateTime2), N'18245080-3a9c-41a3-b9bc-bd215b6fd084', 0, NULL, NULL, 1)
                                    GO
                                    INSERT [dbo].[Projects] ([ProjectId], [ClientId], [Name], [StartDate], [EndDate], [InvoiceFrequency], [Notes], [BillingType], [BillingRate], [Deposit], [ReferralPercent], [EstimatedRevenue], [CreatedDateTime], [CreatedById], [ChangedDateTime], [ChangedById], [Deleted], [DeletedById], [DeletedDateTime], [Active]) VALUES (15, 2, N'Omega', CAST(N'2020-11-20T00:00:00.0000000' AS DateTime2), CAST(N'2020-11-30T00:00:00.0000000' AS DateTime2), 1, N'Some notes', 1, 150.0000, 9000.0000, 25.0000, 25000.0000, CAST(N'2020-11-13T07:26:20.7692080' AS DateTime2), N'28cb1858-fd6e-412f-9d20-0ffc07759a1e', NULL, NULL, 1, N'9d02ee22-eb3b-43e3-8f0d-0e96dc271d5d', CAST(N'2020-11-15T17:58:58.9535106' AS DateTime2), 1)
                                    GO
                                    INSERT [dbo].[Projects] ([ProjectId], [ClientId], [Name], [StartDate], [EndDate], [InvoiceFrequency], [Notes], [BillingType], [BillingRate], [Deposit], [ReferralPercent], [EstimatedRevenue], [CreatedDateTime], [CreatedById], [ChangedDateTime], [ChangedById], [Deleted], [DeletedById], [DeletedDateTime], [Active]) VALUES (16, 1, N'New [Changed]', CAST(N'2020-11-10T00:00:00.0000000' AS DateTime2), CAST(N'2020-11-12T00:00:00.0000000' AS DateTime2), 2, N'Notes', 1, 0.0000, 0.0000, 0.0000, 0.0000, CAST(N'2020-11-15T18:28:47.3474118' AS DateTime2), N'9d02ee22-eb3b-43e3-8f0d-0e96dc271d5d', CAST(N'2021-03-10T21:26:09.8233131' AS DateTime2), N'13c80d1a-5108-43a6-90c5-2815a4319f62', 0, NULL, NULL, 0)
                                    GO
                                    INSERT [dbo].[Projects] ([ProjectId], [ClientId], [Name], [StartDate], [EndDate], [InvoiceFrequency], [Notes], [BillingType], [BillingRate], [Deposit], [ReferralPercent], [EstimatedRevenue], [CreatedDateTime], [CreatedById], [ChangedDateTime], [ChangedById], [Deleted], [DeletedById], [DeletedDateTime], [Active]) VALUES (17, 1, N'This is a test [Edited]', CAST(N'2020-11-02T00:00:00.0000000' AS DateTime2), CAST(N'2020-11-20T00:00:00.0000000' AS DateTime2), 2, NULL, 1, 0.0000, 0.0000, 0.0000, 0.0000, CAST(N'2020-11-15T19:31:23.3688596' AS DateTime2), N'9d02ee22-eb3b-43e3-8f0d-0e96dc271d5d', CAST(N'2020-11-16T15:43:21.6473928' AS DateTime2), N'9d02ee22-eb3b-43e3-8f0d-0e96dc271d5d', 0, NULL, NULL, 0)
                                    GO
                                    INSERT [dbo].[Projects] ([ProjectId], [ClientId], [Name], [StartDate], [EndDate], [InvoiceFrequency], [Notes], [BillingType], [BillingRate], [Deposit], [ReferralPercent], [EstimatedRevenue], [CreatedDateTime], [CreatedById], [ChangedDateTime], [ChangedById], [Deleted], [DeletedById], [DeletedDateTime], [Active]) VALUES (18, 1, N'GSGFSDF', CAST(N'2020-10-26T00:00:00.0000000' AS DateTime2), CAST(N'2020-11-10T00:00:00.0000000' AS DateTime2), 2, NULL, 1, 0.0000, 0.0000, 0.0000, 0.0000, CAST(N'2020-11-15T19:41:01.9522242' AS DateTime2), N'9d02ee22-eb3b-43e3-8f0d-0e96dc271d5d', NULL, NULL, 1, N'9d02ee22-eb3b-43e3-8f0d-0e96dc271d5d', CAST(N'2020-11-15T19:41:05.6753244' AS DateTime2), 0)
                                    GO
                                    INSERT [dbo].[Projects] ([ProjectId], [ClientId], [Name], [StartDate], [EndDate], [InvoiceFrequency], [Notes], [BillingType], [BillingRate], [Deposit], [ReferralPercent], [EstimatedRevenue], [CreatedDateTime], [CreatedById], [ChangedDateTime], [ChangedById], [Deleted], [DeletedById], [DeletedDateTime], [Active]) VALUES (19, 1, N'TEST [Changed]', CAST(N'2020-11-01T00:00:00.0000000' AS DateTime2), CAST(N'2020-11-09T00:00:00.0000000' AS DateTime2), 2, N'asdfsdf', 1, 0.0000, 0.0000, 0.0000, 0.0000, CAST(N'2020-11-15T19:47:24.0233674' AS DateTime2), N'9d02ee22-eb3b-43e3-8f0d-0e96dc271d5d', CAST(N'2020-11-16T15:43:37.5930626' AS DateTime2), N'9d02ee22-eb3b-43e3-8f0d-0e96dc271d5d', 0, NULL, NULL, 1)
                                    GO
                                    SET IDENTITY_INSERT [dbo].[Projects] OFF
                                    GO";

            migrationBuilder.Sql(seedIdentity);
            migrationBuilder.Sql(seedClients);
            migrationBuilder.Sql(seedProjects);
#endif

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
#if DEBUG
            string deleteEverything = @"DELETE FROM [dbo].[Projects]
                                    GO
                                    DELETE FROM [dbo].[Clients]
                                    GO
                                    DELETE FROM [dbo].[AspNetUserTokens]
                                    GO
                                    DELETE FROM [dbo].[AspNetUserLogins]
                                    GO
                                    DELETE FROM [dbo].[AspNetUserClaims]
                                    GO
                                    DELETE FROM [dbo].[AspNetUserRoles]
                                    GO
                                    DELETE FROM [dbo].[AspNetUsers]
                                    GO
                                    DELETE FROM [dbo].[AspNetRoleClaims]
                                    GO
                                    DELETE FROM [dbo].[AspNetRoles]
                                    GO";

            migrationBuilder.Sql(deleteEverything);
#endif
        }
    }
}
