FROM mcr.microsoft.com/mssql/server:2019-latest

# Default SQL Server TCP/Port.
EXPOSE 1433

COPY ./data /

ENTRYPOINT [ "/bin/bash", "entrypoint.sh" ]

# Run SQL Server process.
CMD [ "/opt/mssql/bin/sqlservr" ]