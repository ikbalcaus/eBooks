-- Create the database
CREATE DATABASE eBooks;
GO

USE eBooks;
GO

CREATE TABLE Roles (
    RoleId INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(50) UNIQUE NOT NULL
);
GO

CREATE TABLE Users (
    UserId INT PRIMARY KEY IDENTITY(1,1),
    FirstName NVARCHAR(100) NOT NULL,
    LastName NVARCHAR(100) NOT NULL,
    UserName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) UNIQUE NOT NULL,
    PasswordHash NVARCHAR(255) NOT NULL,
    RoleId INT FOREIGN KEY REFERENCES Roles(RoleId) NOT NULL,
    RegistrationDate DATETIME DEFAULT GETDATE() NOT NULL
);
GO

CREATE TABLE Genres (
    GenreId INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL
);
GO

CREATE TABLE Books (
    BookId INT PRIMARY KEY IDENTITY(1,1),
    Title NVARCHAR(255) NOT NULL,
    Description NVARCHAR(MAX) NULL,
    GenreId INT FOREIGN KEY REFERENCES Genres(GenreId) NULL,
    Price DECIMAL(10,2) NOT NULL,
    TotalPages INT NOT NULL,
    PDFPath NVARCHAR(255) NULL,
    PublisherId INT FOREIGN KEY REFERENCES Users(UserId) NOT NULL,
    AddedDate DATETIME DEFAULT GETDATE() NOT NULL
);
GO

CREATE TABLE AuthTokens (
    AuthTokenId INT PRIMARY KEY IDENTITY(1,1),
    UserId INT FOREIGN KEY REFERENCES Users(UserId) NOT NULL,
    Token NVARCHAR(255) UNIQUE NOT NULL,
    LoggedInDate DATETIME DEFAULT GETDATE() NOT NULL,
    ExpirationDate DATETIME NOT NULL,
    LoggedOutDate DATETIME DEFAULT GETDATE() NULL
);
GO

CREATE TABLE BookImages (
    ImageId INT PRIMARY KEY IDENTITY(1,1),
    BookId INT FOREIGN KEY REFERENCES Books(BookId) NOT NULL,
    ImagePath NVARCHAR(255) NOT NULL,
    AddedDate DATETIME DEFAULT GETDATE() NOT NULL
);
GO

CREATE TABLE Purchases (
    UserId INT FOREIGN KEY REFERENCES Users(UserId) NOT NULL,
    BookId INT FOREIGN KEY REFERENCES Books(BookId) NOT NULL,
    PurchaseDate DATETIME DEFAULT GETDATE() NOT NULL,
    TotalPrice DECIMAL(10,2) NOT NULL,
	PRIMARY KEY (UserId, BookId)
);
GO

CREATE TABLE Reviews (
    BookId INT FOREIGN KEY REFERENCES Books(BookId) NOT NULL,
    UserId INT FOREIGN KEY REFERENCES Users(UserId) NOT NULL,
    Rating INT CHECK (Rating BETWEEN 1 AND 5) NOT NULL,
    Comment NVARCHAR(MAX) NULL,
    ReviewDate DATETIME DEFAULT GETDATE() NOT NULL,
	PRIMARY KEY (BookId, UserId)
);
GO

CREATE TABLE Wishlists (
    UserId INT FOREIGN KEY REFERENCES Users(UserId) NOT NULL,
    BookId INT FOREIGN KEY REFERENCES Books(BookId) NOT NULL,
    AddedDate DATETIME DEFAULT GETDATE() NOT NULL,
	PRIMARY KEY (UserId, BookId)
);
GO

CREATE TABLE Authors (
    AuthorId INT PRIMARY KEY IDENTITY(1,1),
    FirstName NVARCHAR(100) NOT NULL,
    LastName NVARCHAR(100) NOT NULL,
    Biography NVARCHAR(MAX) NULL
);
GO

CREATE TABLE BookAuthors (
    BookId INT FOREIGN KEY REFERENCES Books(BookId),
    AuthorId INT FOREIGN KEY REFERENCES Authors(AuthorId),
    PRIMARY KEY (BookId, AuthorId)
);
GO

CREATE TABLE AccessRights (
    AccessRightId INT PRIMARY KEY IDENTITY(1,1),
    BookId INT FOREIGN KEY REFERENCES Books(BookId) NOT NULL,
    MinimumAge INT NOT NULL
);
GO

CREATE TABLE ReadingProgress (
    UserId INT FOREIGN KEY REFERENCES Users(UserId) NOT NULL,
    BookId INT FOREIGN KEY REFERENCES Books(BookId) NOT NULL,
    CurrentPage INT NOT NULL,
	PRIMARY KEY (UserId, BookId)
);
GO

CREATE TABLE Favorites (
    UserId INT FOREIGN KEY REFERENCES Users(UserId) NOT NULL,
    BookId INT FOREIGN KEY REFERENCES Books(BookId) NOT NULL,
    AddedDate DATETIME DEFAULT GETDATE() NOT NULL,
	PRIMARY KEY (UserId, BookId)
);
GO

CREATE TABLE PublisherVerification (
    VerificationId INT PRIMARY KEY IDENTITY(1,1),
    PublisherId INT FOREIGN KEY REFERENCES Users(UserId) UNIQUE NOT NULL,
    Verified BIT DEFAULT 0 NOT NULL,
    VerificationDate DATETIME NOT NULL,
    AdminId INT FOREIGN KEY REFERENCES Users(UserId) NOT NULL
);
GO

CREATE TABLE PublisherFollows (
    UserId INT FOREIGN KEY REFERENCES Users(UserId) NOT NULL,
    PublisherId INT FOREIGN KEY REFERENCES Users(UserId) NOT NULL,
    FollowDate DATETIME DEFAULT GETDATE() NOT NULL,
	PRIMARY KEY (UserId, PublisherId)
);
GO

CREATE TABLE BookFollows (
    UserId INT FOREIGN KEY REFERENCES Users(UserId) NOT NULL,
    BookId INT FOREIGN KEY REFERENCES Books(BookId) NOT NULL,
    FollowDate DATETIME DEFAULT GETDATE() NOT NULL,
	PRIMARY KEY (UserId, BookId)
);
GO

CREATE TABLE Notifications (
    NotificationId INT PRIMARY KEY IDENTITY(1,1),
    UserId INT FOREIGN KEY REFERENCES Users(UserId) NOT NULL,
    Message NVARCHAR(MAX) NOT NULL,
    IsRead BIT DEFAULT 0 NOT NULL,
    CreatedDate DATETIME DEFAULT GETDATE() NOT NULL,
    PublisherId INT FOREIGN KEY REFERENCES Users(UserId) NULL,
    BookId INT FOREIGN KEY REFERENCES Books(BookId) NULL
);
GO

CREATE TABLE Languages (
    LanguageId INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) UNIQUE NOT NULL,
    Abbreviation NVARCHAR(10) NOT NULL
);
GO


INSERT INTO Roles (Name) 
VALUES 
    ('user'),
    ('publisher'),
    ('admin');
GO

INSERT INTO Genres (Name) 
VALUES 
    ('Science Fiction'),
    ('Biography'),
    ('History'),
    ('Romance'),
    ('Thriller'),
    ('Horror'),
    ('Self-help'),
    ('Philosophy'),
    ('Psychology'),
    ('Adventure'),
    ('Children'),
    ('Poetry'),
    ('Drama'),
    ('Cooking'),
    ('Travel'),
    ('Health & Fitness'),
    ('Religion'),
    ('Business'),
    ('Education'),
    ('Art'),
    ('Technology'),
    ('Humor'),
    ('Science'),
    ('Politics'),
    ('Sports');
GO

INSERT INTO Languages (Name, Abbreviation) 
VALUES 
    ('English', 'eng'),
    ('Bosnian', 'ba'),
    ('German', 'de'),
    ('French', 'fr'),
    ('Spanish', 'es'),
    ('Italian', 'it'),
    ('Chinese', 'zh'),
    ('Japanese', 'ja'),
    ('Korean', 'ko'),
    ('Russian', 'ru'),
    ('Arabic', 'ar'),
    ('Portuguese', 'pt'),
    ('Hindi', 'hi'),
    ('Dutch', 'nl'),
    ('Swedish', 'sv'),
    ('Norwegian', 'no'),
    ('Finnish', 'fi'),
    ('Danish', 'da'),
    ('Greek', 'el'),
    ('Turkish', 'tr'),
    ('Polish', 'pl'),
    ('Czech', 'cs'),
    ('Hungarian', 'hu'),
    ('Romanian', 'ro'),
    ('Hebrew', 'he');
GO
