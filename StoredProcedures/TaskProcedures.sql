CREATE TABLE Tasks (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(100),
    Description NVARCHAR(MAX),
    IsCompleted BIT,
    DueDate DATETIME NULL,
    Priority NVARCHAR(20),
    UserId int
);
GO

CREATE PROCEDURE [dbo].[sp_GetAllTasks]          
AS          
BEGIN          
SELECT u.Name,t.Id,t.Title,t.Description,t.IsCompleted,CONVERT(VARCHAR(10), t.DueDate, 23) AS DueDate,t.Priority   
FROM Tasks t JOIN Users u ON t.UserId = u.UserId ORDER BY Id DESC;          
END;
GO

CREATE PROCEDURE sp_GetTaskById        
(      
@UserId int      
)      
AS          
BEGIN          
    SELECT Id,Title,Description,IsCompleted,CONVERT(VARCHAR(10), DueDate, 23) AS DueDate        
 ,Priority,UserId FROM Tasks Where UserId = @UserId ORDER BY Id DESC;        
END;
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
    @Description NVARCHAR(255),  
    @IsCompleted BIT,  
    @DueDate DATETIME = NULL,  
    @Priority NVARCHAR(20)  
AS  
BEGIN  
    UPDATE Tasks  
    SET   
        Title = @Title,  
        Description = @Description,  
        IsCompleted = @IsCompleted,  
        DueDate = @DueDate,  
        Priority = @Priority  
    WHERE Id = @Id;  
END;  
GO

CREATE PROCEDURE sp_DeleteTask
    @Id INT
AS
BEGIN
    DELETE FROM Tasks WHERE Id = @Id;
END
GO

CREATE PROCEDURE sp_GetTaskById        
(      
@UserId int      
)      
AS          
BEGIN          
    SELECT Id,Title,Description,IsCompleted,CONVERT(VARCHAR(10), DueDate, 23) AS DueDate        
 ,Priority,UserId FROM Tasks Where UserId = @UserId ORDER BY Id DESC;        
END;

Go

CREATE TABLE Users (
    UserId INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(150) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(255) NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE(),
    Role nvarchar(50)
);
GO
CREATE PROCEDURE sp_RegisterUser    
    @Name NVARCHAR(100),    
    @Email NVARCHAR(150),    
    @PasswordHash NVARCHAR(255),  
    @Role NVARCHAR(50),  
    @Result INT OUTPUT  
AS    
BEGIN    
    SET NOCOUNT ON;    
    
    IF EXISTS (SELECT 1 FROM Users WHERE Email = @Email)    
    BEGIN    
        SET @Result = -1;   
        RETURN;  
    END    
    
    INSERT INTO Users (Name, Email, PasswordHash, Role, CreatedAt)    
    VALUES (@Name, @Email, @PasswordHash, @Role, GETDATE());  
  
    SET @Result = 1;   
END  
GO
CREATE PROCEDURE sp_LoginUser        
(        
@Email nvarchar(500),          
@PasswordHash nvarchar(500)          
)        
AS        
BEGIN        
SELECT UserId,Name,Email,Role AS RoleName FROM Users Where Email=@Email AND PasswordHash=@PasswordHash;        
END;
GO
