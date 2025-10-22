CREATE TABLE Tasks (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(100),
    Description NVARCHAR(MAX),
    IsCompleted BIT,
    DueDate DATETIME NULL,
    Priority NVARCHAR(20)
);
GO

CREATE PROCEDURE sp_GetAllTasks  
AS  
BEGIN  
        SELECT Id,Title,Description,IsCompleted,CONVERT(VARCHAR(10), DueDate, 105) AS DueDate
	,Priority FROM Tasks ORDER BY Id DESC;  
END
GO

CREATE PROCEDURE sp_GetTaskById
    @Id INT
AS
BEGIN
    SELECT * FROM Tasks WHERE Id = @Id;
END
GO

CREATE PROCEDURE sp_InsertTask  
    @Title NVARCHAR(100),  
    @Description NVARCHAR(255),  
    @IsCompleted BIT,  
    @DueDate DATETIME = NULL,  
    @Priority NVARCHAR(20),
	@UserId int
AS  
BEGIN  
    INSERT INTO Tasks (Title, Description, IsCompleted, DueDate, Priority,UserId)  
    VALUES (@Title, @Description, @IsCompleted, @DueDate, @Priority,@UserId);  
  
    SELECT SCOPE_IDENTITY() AS Id;  
END;  
GO

CREATE PROCEDURE sp_UpdateTask
    @Id INT,
    @Title NVARCHAR(100),
    @Description NVARCHAR(MAX),
    @IsCompleted BIT,
    @DueDate DATETIME = NULL,
    @Priority NVARCHAR(20)
AS
BEGIN
    UPDATE Tasks
    SET Title = @Title,
        Description = @Description,
        IsCompleted = @IsCompleted,
        DueDate = @DueDate,
        Priority = @Priority
    WHERE Id = @Id;
END
GO

CREATE PROCEDURE sp_DeleteTask
    @Id INT
AS
BEGIN
    DELETE FROM Tasks WHERE Id = @Id;
END
GO

CREATE PROCEDURE sp_GetTaskById
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

   SELECT Id,Title,Description,IsCompleted,DueDate,Priority FROM Tasks
    WHERE Id = @Id;
END;

Go

CREATE TABLE Users (
    UserId INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(150) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(255) NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE()
);
GO
CREATE PROCEDURE sp_RegisterUser
    @Name NVARCHAR(100),
    @Email NVARCHAR(150),
    @PasswordHash NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM Users WHERE Email = @Email)
    BEGIN
        RAISERROR('Email already exists.', 16, 1);
        RETURN;
    END

    INSERT INTO Users (Name, Email, PasswordHash,CreatedAt)
    VALUES (@Name, @Email, @PasswordHash,GETDATE());
END
GO