## docker 建立 mongodb 
docker run --name mongo1 -v /home/leozheng0411/MonoDbData:/data/db -d -p 27017:27017 mongo

## 進入 container
docker exec -it mongo1 bash

## 開啟 mongo cli
mongosh

## mongodb URL
mongodb://10.10.10.10:27017

# mongo 語法

## 顯示 所有 database
show dbs

## 切換 database
use SchoolDB

## 建立 collection
db.createCollection("Students")

## 顯示當前 database 所有 collection
show collections

## 新增一筆資料
db.Students.insert({"Name": "Allen", "Age": 20})

## 新增多筆資料
db.Students.insertMany([{"Name": "Jacky", "Age": 22}, {"Name": "Leo", "Age": 23}, {"Name": "Jason", "Age": 24}])

## 查詢 collection
db.Students.find()

## 查詢 collection 顯示 Json
db.Students.find().forEach(printjson)

目前資料
```json
{
  _id: ObjectId("64381617f1a9769370f0fdec"),
  Name: 'Allen',
  Age: 20
}
{
  _id: ObjectId("6438165af1a9769370f0fded"),
  Name: 'Silver',
  Age: 21
}
{
  _id: ObjectId("643816f3f1a9769370f0fdee"),
  Name: 'Jacky',
  Age: 22
}
{
  _id: ObjectId("643816f3f1a9769370f0fdef"),
  Name: 'Leo',
  Age: 23
}
{
  _id: ObjectId("643816f3f1a9769370f0fdf0"),
  Name: 'Jason',
  Age: 24
}
{
  _id: ObjectId("64381a9ff1a9769370f0fdf1"),
  Name: 'James',
  Age: 25
}
```

## 條件查詢 collection

姓名為Jacky的一位學生  
db.Students.findOne({"Name": "Jacky"})

23歲的所有學生  
db.Students.find({"Age": {$eq:23}})

不是23歲的所有學生  
db.Students.find({"Age": {$ne:23}})

23歲以上的所有學生  
db.Students.find({"Age": {$gt:23}})

23歲(含)以上的所有學生  
db.Students.find({"Age": {$gte:23}})

23歲以下的所有學生  
db.Students.find({"Age": {$lt:23}})

23歲(含)以下的所有學生  
db.Students.find({"Age": {$lte:23}})

23歲、25歲的所有學生  
db.Students.find({"Age": {$in:[23,25]}})

不是23歲、25歲的所有學生  
db.Students.find({"Age": {$nin:[23,25]}})

姓名為James 並且 是25歲的所有學生  
db.Students.find({$and:[{"Name": "James"},{"Age": 25}]})

姓名為James 和 是24歲的所有學生  
db.Students.find({$or:[{"Name": "James"},{"Age": 24}]})

姓名不為James 和 不是24歲的所有學生  
db.Students.find({$nor:[{"Name": "James"},{"Age": 24}]})

姓名不為Leo的所有學生  
db.Students.find({"Name": {$not:{$eq:"Leo"}}})

姓名為Allen 並且 是20歲 或 21歲的所有學生  
db.Students.find({"Name": "Allen", $or:[{"Age": 20}, {"Age": 21}]})

## 更新

將姓名為James的學生 改名為 Lebron James  
db.Students.update({"Name": "James"},{$set:{"Name": "Lebron James"}})

先再新增個學生 Allen，模擬將一名姓名為Allen的學生 改為30歲  
db.Students.insert({"Name": "Allen", "Age": 20})

update、updateOne 都是單筆更新  
db.Students.updateOne({"Name": "Allen"},{$set:{"Age": 30}})
db.Students.update({"Name": "Allen"},{$set:{"Age": 30}})

updateMany 多筆更新  
db.Students.updateMany({"Name": "Allen"},{$set:{"Age": 30}})

## 刪除

將姓名為Jacky的一名學生刪除  
db.Students.deleteOne({"Name": "Jacky"})

將姓名為Allen的所有學生刪除  
db.Students.deleteMany({"Name": "Allen"})

將所有學生刪除  
db.Students.deleteMany({})

## 刪除 collection

刪除 Students 的 collction
db.Students.drop()