﻿CREATE PROCEDURE [dbo].[spSale_SaleReport]
	
AS
begin
	SELECT  [s].[SaleDate], [s].[SubTotal], [s].[Tax], [s].[Total], [u].[FirstName], [u].[LastName], [u].[EmailAdress]
	from dbo.Sale s
	inner join dbo.[User] u on s.CashierId=u.Id;
end

