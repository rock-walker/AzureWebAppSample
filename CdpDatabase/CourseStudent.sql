CREATE TABLE [dbo].[CourseStudent](
	[CourseId] [int] NOT NULL,
	[StudentId] [int] NOT NULL,
 CONSTRAINT [PK_dbo.CourseStudent] PRIMARY KEY CLUSTERED 
(
	[CourseId] ASC,
	[StudentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[CourseStudent]  WITH CHECK ADD  CONSTRAINT [FK_dbo.CourseStudent_dbo.Courses_CourseId] FOREIGN KEY([CourseId])
REFERENCES [dbo].[Courses] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[CourseStudent] CHECK CONSTRAINT [FK_dbo.CourseStudent_dbo.Courses_CourseId]
GO

ALTER TABLE [dbo].[CourseStudent]  WITH CHECK ADD  CONSTRAINT [FK_dbo.CourseStudent_dbo.Students_StudentId] FOREIGN KEY([StudentId])
REFERENCES [dbo].[Students] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[CourseStudent] CHECK CONSTRAINT [FK_dbo.CourseStudent_dbo.Students_StudentId]
GO


