# 步驟

* build出TestDB的Image
  
  cd至  TestDB  (dockerfile所在資料夾)

  docker build -t mssqlimagetest .


* build出API的Image

  cd至 docker-compose_api_mssql (dockerfile所在資料夾)

  docker build -t leo/netcoreapi .


* compose run images

  cd至 docker-compose_api_mssql (dockerfile所在資料夾)

  docker-compose up -d

* http://localhost:5000/weatherforecast/1  查看是否連線至mssql