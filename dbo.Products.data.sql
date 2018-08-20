SET IDENTITY_INSERT [dbo].[Products] ON
INSERT INTO [dbo].[Products] ([ProductID], [Name], [Description], [Price], [Category]) VALUES (1, N'Kayak', N'A boat for one person', CAST(275.00 AS Decimal(18, 2)), N'Watersports')
INSERT INTO [dbo].[Products] ([ProductID], [Name], [Description], [Price], [Category]) VALUES (2, N'Lifejacket', N'Protective and fashionable', CAST(48.95 AS Decimal(18, 2)), N'Watersports')
INSERT INTO [dbo].[Products] ([ProductID], [Name], [Description], [Price], [Category]) VALUES (3, N'Soccer ball', N'FIFA-approved size and weight', CAST(19.50 AS Decimal(18, 2)), N'Soccer')
INSERT INTO [dbo].[Products] ([ProductID], [Name], [Description], [Price], [Category]) VALUES (4, N'Corner Flags', N'Give yor playing field a professional touch', CAST(34.95 AS Decimal(18, 2)), N'Soccer')
INSERT INTO [dbo].[Products] ([ProductID], [Name], [Description], [Price], [Category]) VALUES (5, N'Stadium', N'Flat-packed 35,000-seat stadium', CAST(79500.00 AS Decimal(18, 2)), N'Soccer')
INSERT INTO [dbo].[Products] ([ProductID], [Name], [Description], [Price], [Category]) VALUES (6, N'Thinking Cap', N'Improve your brain efficiency by 75%', CAST(16.00 AS Decimal(18, 2)), N'Chess')
INSERT INTO [dbo].[Products] ([ProductID], [Name], [Description], [Price], [Category]) VALUES (7, N'Unsteady Chair', N'Secretly give for the family', CAST(29.95 AS Decimal(18, 2)), N'Chess')
INSERT INTO [dbo].[Products] ([ProductID], [Name], [Description], [Price], [Category]) VALUES (8, N'Human Chess Board', N'A fun game for the family', CAST(75.00 AS Decimal(18, 2)), N'Chess')
INSERT INTO [dbo].[Products] ([ProductID], [Name], [Description], [Price], [Category]) VALUES (9, N'Bling-Bling King', N'Gold-plated, diamond-studded King', CAST(1200.00 AS Decimal(18, 2)), N'Chess')
SET IDENTITY_INSERT [dbo].[Products] OFF
