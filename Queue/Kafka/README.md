## 啟動容器
```
docker-compose up -d
```

## 管理 UI
http://localhost:8080/

## 說明
Topic = 主題
Partition = 分區


1 個 Topic 內 有多個 Partition


Consumer 消費 指定 topic、GroupId

enable.auto.commit = 自動提交目前消費的offset
auto.commit.interval.ms = (default 5000 ms ) enable.auto.commit 為 true 時，自動提交的間隔時間
max.poll.records = (default 500 ) 單次消費的消息數
max.poll.interval.ms = (default 30000 ms)若在時間內沒有消費完 poll 的消息，則視為消費失敗，broker會將該consumer移除，觸發rebalance，consumer重新加入group，重新消費

## 參考文章
https://dimosr.github.io/kafka-docker/
https://www.readfog.com/a/1635090175644241920
https://zhuanlan.zhihu.com/p/112745985