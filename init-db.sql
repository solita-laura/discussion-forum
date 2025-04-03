CREATE TABLE Topics (
    id BIGINT PRIMARY KEY,
    topicName VARCHAR(100),
    messageCount INT,
    lastUpdated TIMESTAMP
);

INSERT INTO Topics (id, topicName, messageCount, lastUpdated)
VALUES (1, 'topic1', 1, '2025-03-03 12:00:00'),
        (2, 'topic2', 7, '2025-01-04 16:00:00');