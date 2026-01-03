CREATE DATABASE GameLeaderboard;
GO

USE GameLeaderboard;
GO

CREATE TABLE PlayerScores (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    PlayerName NVARCHAR(50) NOT NULL,
    Score INT NOT NULL,
    PlayDate DATETIME DEFAULT GETDATE()
);
GO

INSERT INTO PlayerScores (PlayerName, Score) VALUES ('HuyDev', 100);
INSERT INTO PlayerScores (PlayerName, Score) VALUES ('ProGamer', 500);
INSERT INTO PlayerScores (PlayerName, Score) VALUES ('NoobMaster', 10);
GO