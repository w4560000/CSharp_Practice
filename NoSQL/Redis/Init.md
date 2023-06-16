
```

# 創建 docker network
docker network create --driver bridge --subnet 172.18.0.0/24 --gateway 172.18.0.1 redis_network

# pull docker image
docker pull redis:6.0.0

# 建 redis 服務
docker run --name redis1 \
--network=redis_network \
--ip 172.18.0.2 \
-p 6379:6379 \
-v /home/leozheng0411/redis:/usr/local/etc/redis \
-v /home/leozheng0411/redis1data:/data \
-d redis:6.0.0 \
redis-server /usr/local/etc/redis/redis.conf

# 進入 redis 容器
docker exec -it redis1 bash

# 執行 RDB 備份 (會堵塞主線程操作)
docker exec -it redis1 bash redis-cli save

# 執行 RDB 備份 (子線程執行，不堵塞主線程)
docker exec -it redis1 bash redis-cli bgsave
```

# Redis 配置
/data					// RDB、AOF 檔 資料夾目錄
/usr/local/etc/redis	// redis.conf 資料夾目錄

redis.conf 參數
```
# 0.0.0.0 = 綁定所有IP，可設定Redis服務機器實體IP
bind 0.0.0.0

# 保護模式 開啟時只有local能連線，其他主機服務無法連線
protected-mode yes/no
```

# Redis 備份

## RDB 檔
快照備份檔
可設定 每隔多久時間 對 Redis 內存資料做快照到硬碟上，也可以手動執行 RDB 檔備份

redis-cli save => 執行同步操作做 RDB 檔備份，會堵塞當前線程，其他請求無法進入
redis-cli bgsave => 會由子線程做 RDB 檔備份，不會堵塞當前線程

調整 redis.conf
```
dbfilename ${filename}.rdb # RDB 檔名

dir ./	# AOF、RDB 檔案放置路徑

save 900 1		# 可控制 900 秒內 若有 1 個 key 改變 則觸發 bgsave
save 300 10		# 可控制 300 秒內 若有 10 個 key 改變 則觸發 bgsave
```

## AOF 檔
類似 DB 的交易紀錄檔，會將執行的命令寫到 AOF 檔

有一個 aof_buf 緩衝區，命令執行後會加到 aof_buf 中，再寫入 AOF 檔

調整 redis.conf
```
appendonly yes/no # 是否開啟 AOF

appendfilename "appendonly.aof" # AOF 檔名

# fsync策略 (將 aof_buf 資料同步至 AOF 檔中)
# no = 不主動呼叫 fsync，由作業系統控制，Linux 是每30秒1次，不太安全，因不確定同步時間，最多會遺失30秒內的操作資料
# everysec = 每秒同步，最多則遺失1秒內的操作資料
# always = 每次執行的命令都會直接同步，性能較差，但最安全
appendfsync no/everysec/always
```

## AOF 檔 rewrite
因 AOF 檔一直不斷寫入命令後，檔案會不斷肥大
rewrite 是為了處理這個問題，需要定期rewrite AOF 檔
redis 官方的處理流程是 建立新的 AOF 檔，並把內存資料的操作命令寫入新的 AOF 檔，再覆蓋舊的 AOF 檔

手動調用
redis-cli bgrewriteaof


調整 redis.conf
```
# 當寫入 AOF 檔時 而 AOF 檔又同時 rewrite 時，會造成大量 IO 操作，導致寫 AOF 檔阻塞
# no = 會阻塞 AOF 檔寫入，但資料是安全的
# yes = 等同於 appendfsync no，另一邊準備寫入到 AOF 檔的命令 只能先暫存在 aof_buf，若這時 Redis 有異常，則會有資料遺失
no-appendfsync-on-rewrite yes/no

# 當 AOF 檔案超過上次 rewrite 的 AOF 檔案大小的百分比 (若沒有 rewrite 過，則以初始 AOF 檔大小)
auto-aof-rewrite-percentage 0~100 

# AOF 檔案多大時會 rewrite
auto-aof-rewrite-min-size 30mb

# 是否要讀取被截斷字尾的 AOF 檔
# no = 服務器噴錯並且服務不啟動 (此時需修復 AOF 文件 redis-check-aof)
# yes = 載入截斷字尾的 AOF 檔，並且Redis伺服器會開始發出日誌以通知用戶該事件
aof-load-truncated yes/no

# 重寫 AOF 檔案時，Redis 能夠使用AOF文件中的RDB前導碼來加快重寫和恢復速度
aof-use-rdb-preamble yes/no
```

# StackExchange.Redis SDK 配置
AbortOnConnectFail = 為 true 時，連線到 Redis 發生異常就直接觸發 ConnectionFailed 事件
					 為 false時，連線到 Redis 發生異常，會立即重試，直到逾時為止