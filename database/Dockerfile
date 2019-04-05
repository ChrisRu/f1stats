FROM mysql:8.0.15

ADD ./data/f1db.sql /source.sql
ADD ./optimize.sql /optimize.sql

RUN cat /optimize.sql >> /source.sql
RUN mv /source.sql /docker-entrypoint-initdb.d