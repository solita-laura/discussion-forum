CREATE TABLE topics (
    id BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    topicname VARCHAR(20),
    messagecount INT,
    lastupdated TIMESTAMP

);

CREATE TABLE Users (
    id BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    username VARCHAR(100),
    password VARCHAR,
    salt VARCHAR,
    role VARCHAR(20) DEFAULT 'user'
);

INSERT INTO users (username, password, salt)
VALUES ('user1', 'eVxgcavKFAURRQ7z70Y+GWFWjJJla2Pb7qgG97sSXMnHtaxpUgYa7p4DQyz0AyMY', 'J6/zykNVdALI4kdjZrhEWA=='),
 ('user2', 'eVxgcavKFAURRQ7z70Y+GWFWjJJla2Pb7qgG97sSXMnHtaxpUgYa7p4DQyz0AyMY', 'J6/zykNVdALI4kdjZrhEWA==');

CREATE TABLE messages (
    id BIGINT GENERATED ALWAYS AS IDENTITY,
    topicid BIGINT REFERENCES topics(id),
    userid BIGINT REFERENCES users(id),
    content VARCHAR(500),
    upvotes BIGINT DEFAULT 0,
    postdate TIMESTAMP
);
