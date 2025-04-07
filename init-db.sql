CREATE TABLE Topics (
    Id BIGINT PRIMARY KEY,
    topicName VARCHAR(100),
    messageCount INT,
    lastUpdated TIMESTAMP
);

INSERT INTO Topics (Id, topicName, messageCount, lastUpdated)
VALUES (1, 'topic1', 1, '2025-03-03 12:00:00'),
        (2, 'topic2', 7, '2025-01-04 16:00:00');

CREATE TABLE Users (
    Id BIGINT PRIMARY KEY,
    username VARCHAR(100),
    password VARCHAR,
    salt VARCHAR
);

INSERT INTO users (Id, username, password, salt)
VALUES (1, 'user1', 'eVxgcavKFAURRQ7z70Y+GWFWjJJla2Pb7qgG97sSXMnHtaxpUgYa7p4DQyz0AyMY', 'J6/zykNVdALI4kdjZrhEWA==')