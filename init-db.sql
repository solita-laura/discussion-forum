CREATE TABLE topics (
    id BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    topicname VARCHAR(20),
    messagecount INT,
    lastupdated TIMESTAMP

);

INSERT INTO topics (topicname, messagecount, lastupdated)
VALUES ('topic1', 0, '2025-03-03 12:00:00'),
        ('topic2', 0, '2025-01-04 16:00:00');

CREATE TABLE Users (
    id BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    username VARCHAR(100),
    password VARCHAR,
    salt VARCHAR
);

INSERT INTO users (username, password, salt)
VALUES ('user1', 'eVxgcavKFAURRQ7z70Y+GWFWjJJla2Pb7qgG97sSXMnHtaxpUgYa7p4DQyz0AyMY', 'J6/zykNVdALI4kdjZrhEWA==');

CREATE TABLE messages (
    id BIGINT GENERATED ALWAYS AS IDENTITY,
    topicid BIGINT REFERENCES topics(id),
    userid BIGINT REFERENCES users(id),
    content VARCHAR(500),
    upvotes BIGINT DEFAULT 0,
    postdate TIMESTAMP
);
