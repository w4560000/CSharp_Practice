# Aggregate 測試

## $sort 排序 1 = ASC, -1 = DESC
## $limit 資料筆數
取出最多讚數的兩篇貼文  
```
db.Post.aggregate({$sort : { "Like" : -1 }},{$limit : 2})
```

## $project 只取出某些欄位

取出最多讚數的兩篇貼文的作者  
```
db.Post.aggregate({$sort : { "Like" : -1 }}, {$limit : 2}, {$project: {"Author": 1}})
```

排除_id, 因_id 會預設顯示  
```
db.Post.aggregate({$sort : { "Like" : -1 }}, {$limit : 2}, {$project: {"_id": 0, "Author": -1}})
```

## $match 符合某些條件

取出 Lee 超過 900讚數的貼文  
```
db.Post.aggregate({$match : { "Author": "Lee", "Like": { $gt: 900 }}})
```
搜尋 OjbectId  
```
db.Post.aggregate({$match : { "_id": ObjectId("6445468c3849dca0002cddde")}})
```
## $group 分群
## $sum 總和

列出所有作者的總讚數  
```
db.Post.aggregate({$group : { "_id": "$Author", "TotalLIke" : {$sum: "$Like"}}})
```
## $addFields 加運算欄位 (ex: Date 轉成 timeStamp 幫助判斷)
## $count 回傳總筆數

```
db.Post.aggregate([{ $addFields: { PostTimestamp: { $toLong: "$Date" } } },{$match: { PostTimestamp: { $gte: 1682261644000 }}}, {$count: "TotalCount"}])
```

## lookup (Join)

- 測試資料
```
db.score.insertMany([
  { s_student_id: 1, student: 1, test: 1, math: 20, english: 45 },
  { s_student_id: 2, student: 2, test: 1, math: 30, english: 80 },
  { s_student_id: 3, student: 3, test: 1, math: 85, english: 83 }])
  
db.profile.insertMany([
  { p_student_id: 1, name: "小明", sex: 0, seat_number: 23 },
  { p_student_id: 2, name: "小王", sex: 0, seat_number: 17 },
  { p_student_id: 3, name: "小美", sex: 1, seat_number: 6 }])


db.profile.aggregate([
  {
    $lookup: {
      from: "score", // 從哪一張資料表join資料
      as: "test_score", // join的資料會新增在一個欄位內，as要填就是該欄位的名稱
      localField: "p_student_id", // 主表要Join的欄位
      foreignField: "s_student_id" // from 資料表的 foreignKey
    }
  }
])
```

輸出  
```json
{
  _id: ObjectId("6447e2378b782c4190c5e42a"),
  p_student_id: 1,
  name: '小明',
  sex: 0,
  seat_number: 23,
  test_score: [
    {
      _id: ObjectId("6447e2328b782c4190c5e427"),
      s_student_id: 1,
      student: 1,
      test: 1,
      math: 20,
      english: 45
    }
  ]
}
{
  _id: ObjectId("6447e2378b782c4190c5e42b"),
  p_student_id: 2,
  name: '小王',
  sex: 0,
  seat_number: 17,
  test_score: [
    {
      _id: ObjectId("6447e2328b782c4190c5e428"),
      s_student_id: 2,
      student: 2,
      test: 1,
      math: 30,
      english: 80
    }
  ]
}
{
  _id: ObjectId("6447e2378b782c4190c5e42c"),
  p_student_id: 3,
  name: '小美',
  sex: 1,
  seat_number: 6,
  test_score: [
    {
      _id: ObjectId("6447e2328b782c4190c5e429"),
      s_student_id: 3,
      student: 3,
      test: 1,
      math: 85,
      english: 83
    }
  ]
}
```
