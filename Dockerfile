FROM postgres:16.0
ADD init-db.sql /docker-entrypoint-initdb.d/