
```
docker pull redis:7.0

docker run -d --name redis1 -p 6379:6379 redis

docker run -d --name redis1 -p 6379:6379 -v /home/leozheng0411/redis/:/usr/local/etc/redis/ redis

docker exec -it redis1 bash
```

# Redis 配置
/usr/local/redis/db/    // RDB、AOF檔

# StackExchange.Redis SDK 配置
AbortOnConnectFail = 為 true 時，連線到 Redis 發生異常就直接觸發 ConnectionFailed 事件
					 為 false時，連線到 Redis 發生異常，會立即重試，直到逾時為止