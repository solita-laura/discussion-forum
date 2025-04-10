CREATE TABLE topics (
    id BIGINT PRIMARY KEY,
    topicname VARCHAR(100),
    messagecount INT,
    lastupdated TIMESTAMP

);

INSERT INTO topics (id, topicname, messagecount, lastupdated)
VALUES (1, 'topic1', 1, '2025-03-03 12:00:00'),
        (2, 'topic2', 7, '2025-01-04 16:00:00');

CREATE TABLE Users (
    id BIGINT PRIMARY KEY,
    username VARCHAR(100),
    password VARCHAR,
    salt VARCHAR
);

INSERT INTO users (id, username, password, salt)
VALUES (1, 'user1', 'eVxgcavKFAURRQ7z70Y+GWFWjJJla2Pb7qgG97sSXMnHtaxpUgYa7p4DQyz0AyMY', 'J6/zykNVdALI4kdjZrhEWA==');

CREATE TABLE messages (
    id BIGINT PRIMARY KEY,
    topicid BIGINT REFERENCES topics(id),
    userid BIGINT REFERENCES users(id),
    content VARCHAR,
    upvotes BIGINT
);

INSERT INTO Messages (Id, topicId, userId, content, upvotes)
VALUES (1, 1, 1, 'Hello World', 0),
        (2, 1, 1, 'Hello World', 0),
        (3, 2, 1, 'Hello World', 0),
        (4, 2, 1, 'Hello World', 0),
        (5, 2, 1, 'Hello World', 0),
        (6, 2, 1, 'Hello World', 0),
        (7, 2, 1, 'Hello World', 0);