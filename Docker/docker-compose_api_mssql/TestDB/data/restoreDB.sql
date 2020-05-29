RESTORE DATABASE core FROM DISK = '/testDB.bak' 
					  WITH MOVE 'core' TO '/var/opt/mssql/data/core.mdf', 
					       MOVE 'core_Log' TO '/var/opt/mssql/data/core_Log.ldf';
						   
						   GO