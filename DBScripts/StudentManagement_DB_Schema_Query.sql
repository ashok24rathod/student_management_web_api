USE [master]
GO

/****** Object:  Database [StudentManager]    Script Date: 03-06-2022 01:57:11 ******/
CREATE DATABASE [StudentManager]
GO

USE [StudentManager]
GO

/****** Object:  Table [dbo].[Student]    Script Date: 03-06-2022 01:57:52 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Student](
	[StudentId] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[DateOfBirth] [nvarchar](10) NULL,
	[Mobile] [varchar](10) NULL,
	[Email] [nvarchar](320) NULL,
PRIMARY KEY CLUSTERED 
(
	[StudentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Student]  WITH CHECK ADD  CONSTRAINT [CK_Student_Mobile] CHECK  (([Mobile] like '[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]'))
GO

ALTER TABLE [dbo].[Student] CHECK CONSTRAINT [CK_Student_Mobile]
GO


CREATE TABLE [dbo].[Department](
	[DepartmentId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[DepartmentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE TABLE [dbo].[StudentDepartment](
	[StudentId] [int] NOT NULL,
	[DepartmentId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[StudentId] ASC,
	[DepartmentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[StudentDepartment]  WITH CHECK ADD FOREIGN KEY([DepartmentId])
REFERENCES [dbo].[Department] ([DepartmentId])
GO

ALTER TABLE [dbo].[StudentDepartment]  WITH CHECK ADD FOREIGN KEY([StudentId])
REFERENCES [dbo].[Student] ([StudentId])
GO



CREATE PROCEDURE [dbo].[GetAllStudents]
AS
BEGIN
SELECT st.StudentId,st.FirstName,st.LastName,st.DateOfBirth,st.Mobile,st.Email, STRING_AGG(sd.DepartmentId , ',') As Departments
FROM Student st
LEFT OUTER JOIN StudentDepartment sd on st.StudentId = sd.StudentId
GROUP BY st.StudentId,st.FirstName,st.LastName,st.DateOfBirth,st.Mobile,st.Email

END
GO


CREATE PROCEDURE [dbo].[AddStudent]
@FirstName nvarchar(100),
@LastName nvarchar(100),
@DateOfBirth nvarchar(10),
@Mobile nvarchar(10),
@Email nvarchar(320),
@Departments nvarchar(100)
AS
BEGIN
DECLARE @STUDENTID int;

BEGIN TRY
INSERT INTO Student (FirstName,LastName,DateOfBirth,Mobile,Email)
VALUES
(@FirstName,@LastName,@DateOfBirth,@Mobile,@Email);

SET @STUDENTID=(SELECT CONVERT(int,SCOPE_IDENTITY()));

INSERT INTO StudentDepartment (StudentId,DepartmentId)
SELECT  @STUDENTID, value
FROM STRING_SPLIT(@Departments, ',')

SELECT @STUDENTID;
END TRY
BEGIN CATCH
SELECT  ERROR_NUMBER() AS ErrorNumber  
       ,ERROR_MESSAGE() AS ErrorMessage; 
END CATCH

END
GO



CREATE PROCEDURE [dbo].[GetStudentById]
@StudentId int
AS
BEGIN
SELECT st.StudentId,st.FirstName,st.LastName,st.DateOfBirth,st.Mobile,st.Email,STRING_AGG(sd.DepartmentId , ',') As Departments
FROM Student st
LEFT OUTER JOIN StudentDepartment sd on st.StudentId = sd.StudentId
WHERE st.StudentId=@StudentId 
GROUP BY st.StudentId,st.FirstName,st.LastName,st.DateOfBirth,st.Mobile,st.Email

END
GO


CREATE PROCEDURE [dbo].[UpdateStudent]
@StudentId int,
@FirstName nvarchar(100),
@LastName nvarchar(100),
@DateOfBirth nvarchar(10),
@Mobile nvarchar(10),
@Email nvarchar(320),
@Departments nvarchar(100)
AS
BEGIN

BEGIN TRY
UPDATE Student SET  
FirstName=@FirstName,LastName=@LastName,DateOfBirth=@DateOfBirth,Mobile=@Mobile,Email=@Email
WHERE StudentId=@StudentId;

IF OBJECT_ID('tempdb..#NewStudentDepartments') IS NOT NULL
BEGIN
DROP TABLE #NewStudentDepartments
END

CREATE TABLE #NewStudentDepartments (StudentId int,DepartmentId int)

INSERT INTO #NewStudentDepartments
Select @StudentId,value As DepartmentId 
FROM STRING_SPLIT(@Departments, ',')

DELETE FROM StudentDepartment 
Where DepartmentId NOT IN (Select DepartmentId from #NewStudentDepartments)
AND StudentId=@StudentId;


Insert into StudentDepartment
SELECT d.StudentId,d.[DepartmentId] 
FROM #NewStudentDepartments d
LEFT OUTER JOIN dbo.StudentDepartment sd ON sd.DepartmentId=d.DepartmentId and sd.StudentId=d.StudentId
Where sd.DepartmentId Is NULL

END TRY
BEGIN CATCH
SELECT ERROR_NUMBER() AS ErrorNumber  
       ,ERROR_MESSAGE() AS ErrorMessage; 
END CATCH

END
GO


CREATE PROCEDURE [dbo].[DeleteStudentDetails]
@StudentId int
AS
BEGIN

DELETE FROM StudentDepartment WHERE StudentId=@StudentId;


DELETE FROM Student WHERE StudentId=@StudentId;


END
GO


CREATE PROCEDURE [dbo].[GetAllDepartments]
AS
BEGIN

SELECT DepartmentId,Name FROM Department;

END
GO

