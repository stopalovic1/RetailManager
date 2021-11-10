CREATE PROCEDURE [dbo].[spProduct_GetAll]
AS
begin

set nocount on
SELECT Id,ProductName,[Description],RetailPrice,QuantityInStock, IsTaxable
FROM dbo.Product order by ProductName;
end

RETURN 0
