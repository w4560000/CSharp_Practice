USE Northwind
GO

-- 依據 UnitPrice 由高到低來排序
SELECT ProductID, ProductName,UnitPrice, CategoryID
FROM Products
ORDER BY UnitPrice DESC;
GO

-- EX1：認識 ROW_NUMBER() 次序函數與 PARTITION BY 引數
-- 資料會 ORDER BY UnitPrice DESC 並加上Row_NUMBER
SELECT ProductID, ProductName, UnitPrice, CategoryID,
 ROW_NUMBER() OVER( ORDER BY UnitPrice DESC) N'RowNumber'
FROM Products;
GO


-- ROW_NUMBER() 次序函數，使用 PARTITION BY 引數依據資料行 CategoryID 分割查詢結果集。 
-- 資料會先以CategoryID 分類 再以 UnitPrice 做排序 DESC，RowNumber則在分類裡各自計算
SELECT ProductID, ProductName, UnitPrice, CategoryID,
 ROW_NUMBER() OVER( PARTITION BY CategoryID
  ORDER BY UnitPrice DESC) N'RowNumber'
FROM Products;
GO


-- EX2：認識 RANK() 次序函數與 PARTITION BY 引數
-- 資料會以UnitPrice做排序 DESC ，再計算名次，若相同條件資料 會並列幾個名次 從缺之後的名次
SELECT ProductID, ProductName, UnitPrice, CategoryID, 
 RANK() OVER( ORDER BY UnitPrice DESC) N'Rank'
FROM Products;
GO


-- RANK() 次序函數，使用 PARTITION BY 引數依據資料行 CategoryID 分割查詢結果集。 
-- 資料會先以 CategoryID 分類，再以 UnitPrice 做排序 DESC，RANK則在分類裡各自計算，但遇到相同條件資料 Rank會並列名次，並看並列幾個名次 從缺之後的名次
SELECT ProductID, ProductName, UnitPrice, CategoryID,
 RANK() OVER( PARTITION BY CategoryID 
  ORDER BY UnitPrice DESC) N'Rank'
FROM Products;
GO


-- EX3：認識 DENSE_RANK() 次序函數與 PARTITION BY 引數
-- 資料會以 UnitPrice做排序 DESC，再計算名次，若相同條件資料 會並列名次，再依序排序，不從缺名次
SELECT ProductID, ProductName, UnitPrice, CategoryID,
 DENSE_RANK() OVER( ORDER BY UnitPrice DESC) N'DenseRank'
FROM Products;
GO


-- DENSE_RANK() 次序函數，使用 PARTITION BY 引數依據資料行 CategoryID 分割查詢結果集。 
-- 資料會先以 CategoryID 分類 再以 UnitPrice 做排序 DESC，RANK則在分類裡各自計算，但遇到相同條件資料 Rank會並列名次，再依序排序，不從缺名次
SELECT ProductID, ProductName, UnitPrice, CategoryID, 
 DENSE_RANK() OVER( PARTITION BY CategoryID 
  ORDER BY UnitPrice DESC) N'DenseRank'
FROM Products;
GO


-- EX4：認識 NTILE() 次序函數與 PARTITION BY 引數
-- 因為沒有指定 PARTITION BY 子句，NTILE() 次序函數應用到在結果集的所有資料列。
-- (1-26), (27-52), (53-77)
-- 資料會先以 UnitPrice 做排序 DESC，並將資料切成3份，標註記號
SELECT ProductID, ProductName, UnitPrice, CategoryID,
 NTILE(3) OVER( ORDER BY UnitPrice DESC) N'Ntile'
FROM Products;
GO


-- NTILE() 次序函數，使用 PARTITION BY 引數依據資料行 CategoryID 分割查詢結果集。 
-- 資料先以 CategoryID 做分類，再以 UnitPrice 做排序 DESC，並將各分類資料切成3份，各自標註記號
SELECT ProductID, ProductName, UnitPrice, CategoryID, 
 NTILE(3) OVER( PARTITION BY CategoryID 
  ORDER BY UnitPrice DESC) N'Ntile'
FROM Products;
GO