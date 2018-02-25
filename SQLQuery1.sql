--你SELECT DISTINCT sUBSTRING(目标节点,1,5) FROM dbo.临时目标节点 WHERE 识别=0 
--AND  目标节点 LIKE '%-%'
SELECT * FROM dbo.临时目标节点 WHERE 目标节点 LIKE N'%月底%' --AND 识别=0