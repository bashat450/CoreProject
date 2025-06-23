USE [DynamoEnterprise]
GO

/****** Object:  StoredProcedure [Dynamo].[SP_ManageBlogs]    Script Date: 23-06-2025 16:02:06 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [Dynamo].[SP_ManageBlogs]
    @Action NVARCHAR(10),
    @BlogId INT = NULL,
    @Title NVARCHAR(200) = NULL,
    @Category NVARCHAR(100) = NULL,
    @PublishDate DATE = NULL,
    @Description NVARCHAR(MAX) = NULL,
    @Author NVARCHAR(100) = NULL,
    @ImageUrl NVARCHAR(200) = NULL,

    @Published BIT = 0
AS
BEGIN
    SET NOCOUNT ON;

    IF @Action = 'INSERT'
    BEGIN
        INSERT INTO Blogs (Title, Category, PublishDate, Description, Author, ImageUrl, CreatedDate, Published)
        VALUES (@Title, @Category, @PublishDate, @Description, @Author, @ImageUrl, GETDATE(), @Published)
    END
    ELSE IF @Action = 'UPDATE'
    BEGIN
        UPDATE Blogs
        SET Title = @Title,
            Category = @Category,
            PublishDate = @PublishDate,
            Description = @Description,
            Author = @Author,
            ImageUrl = @ImageUrl,
            Published = @Published
        WHERE BlogId = @BlogId
    END
    ELSE IF @Action = 'DELETE'
    BEGIN
        DELETE FROM Blogs WHERE BlogId = @BlogId
    END
    ELSE IF @Action = 'GET'
    BEGIN
        SELECT * FROM Blogs
    END
    ELSE IF @Action = 'GETBYID'
    BEGIN
        SELECT * FROM Blogs WHERE BlogId = @BlogId
    END
END;
GO


EXEC [Dynamo].[SP_ManageBlogs]
    @Action = 'INSERT',
    @Title = 'Sample Blog Title',
    @Category = 'Tech',
    @PublishDate = '2025-06-23',
    @Description = 'This is a sample blog description.',
    @Author = 'Parween',
    @ImageUrl = 'images/blogimages/1.jpg',
    @Published = 1;

EXEC [Dynamo].[SP_ManageBlogs]
    @Action = 'UPDATE',
    @BlogId = 6,
    @Title = '“Sleeping on Sandpaper? We Reviewed the Worst”',
    @Category = 'Bed Sheets',
    @PublishDate = '2025-06-25',
    @Description = '“Because you deserve more than waking up stuck to polyester. We dive deep into the scratchy, saggy, and straight-up shady world of bad bed sheets.”',
    @Author = 'Tanveer',
    @ImageUrl = 'images/blogimages/6.jpg',
    @Published = 1;




EXEC [Dynamo].[SP_ManageBlogs]
    @Action = 'GET';

EXEC [Dynamo].[SP_ManageBlogs]
    @Action = 'DELETE',
    @BlogId = 7;