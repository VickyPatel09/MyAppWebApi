using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace MyAngularAppWebApi.Utility.Datatable
{
    public class GridData
    {
        public GridData(GridParameter gridParameter)
        {
            var obj = new GridCommon();
            this.ConnectionString = gridParameter.ConnectionString;

            switch (gridParameter.Type)
            {
                case "SupportTicketByStatus":
                    this.ColumnsName = "Id,SendBy,Email,UserId,Subject,Title,Message,Status,Reply,OnDate";
                    this.PageNumber = gridParameter.PageNumber;
                    this.RecordPerPage = this.GetPageRecord(gridParameter.RecordPerPage);
                    this.SortColumn = this.GetSortedColumn(gridParameter.Orders, gridParameter.Columns, "OnDate");
                    this.SortOrder = this.GetSortedOrder(gridParameter.SortOrder);
                    this.TableName = "Support WITH(NOLOCK)";
                    this.Draw = gridParameter.Draw;
                    this.WhereClause = this.GetWhereCondition(
                        "SendBy != 'Admin'",
                        gridParameter.Search,
                        gridParameter.Columns);
                    this.JsonData = obj.GetJsonAsync(this);
                    break;
                case "Currency":
                    this.ColumnsName =
                        "Id,Name,Symbol,MinWithdrawLimit,MaxWithdrawLimit,DepositStatus,WithdrawFee,WithdrawStatus,IsActive,IsBaseCurrency,IsAutoWithdraw,ApiSymbol";
                    this.PageNumber = gridParameter.PageNumber;
                    this.RecordPerPage = this.GetPageRecord(gridParameter.RecordPerPage);
                    this.SortColumn = this.GetSortedColumn(gridParameter.Orders, gridParameter.Columns, "Id");
                    this.SortOrder = this.GetSortedOrder(gridParameter.SortOrder);
                    this.TableName = "Currency WITH(NOLOCK)";
                    this.Draw = gridParameter.Draw;
                    this.WhereClause = this.GetWhereCondition(
                        string.Empty,
                        gridParameter.Search,
                        gridParameter.Columns);
                    this.JsonData = obj.GetJsonAsync(this);
                    break;
                case "MatchPair":
                    this.ColumnsName = "Id,Name,CreatedDate,MinTrade,TradeFees,IsActive,MinPrice,StepPrice,MinQty,StepQty";
                    this.PageNumber = gridParameter.PageNumber;
                    this.RecordPerPage = this.GetPageRecord(gridParameter.RecordPerPage);
                    this.SortColumn = this.GetSortedColumn(gridParameter.Orders, gridParameter.Columns, "Id");
                    this.SortOrder = this.GetSortedOrder(gridParameter.SortOrder);
                    this.TableName = "MatchPair WITH(NOLOCK)";
                    this.Draw = gridParameter.Draw;
                    this.WhereClause = this.GetWhereCondition(
                        string.Empty,
                        gridParameter.Search,
                        gridParameter.Columns);
                    this.JsonData = obj.GetJsonAsync(this);
                    break;
                case "News":
                    this.ColumnsName = "Id,Title,Description,CreatedDate,IsActive,IsNews";
                    this.PageNumber = gridParameter.PageNumber;
                    this.RecordPerPage = this.GetPageRecord(gridParameter.RecordPerPage);
                    this.SortColumn = this.GetSortedColumn(gridParameter.Orders, gridParameter.Columns, "Id");
                    this.SortOrder = this.GetSortedOrder(gridParameter.SortOrder);
                    this.TableName = "News WITH(NOLOCK)";
                    this.Draw = gridParameter.Draw;
                    this.WhereClause = this.GetWhereCondition(
                       gridParameter.Where,
                        gridParameter.Search,
                        gridParameter.Columns);
                    this.JsonData = obj.GetJsonAsync(this);
                    break;
                case "Announcement":
                    this.ColumnsName = "Id,Title,Description,CreatedDate,IsActive,IsNews";
                    this.PageNumber = gridParameter.PageNumber;
                    this.RecordPerPage = this.GetPageRecord(gridParameter.RecordPerPage);
                    this.SortColumn = this.GetSortedColumn(gridParameter.Orders, gridParameter.Columns, "Id");
                    this.SortOrder = this.GetSortedOrder(gridParameter.SortOrder);
                    this.TableName = "News WITH(NOLOCK)";
                    this.Draw = gridParameter.Draw;
                    this.WhereClause = this.GetWhereCondition(gridParameter.Where, gridParameter.Search, gridParameter.Columns);
                    this.JsonData = obj.GetJsonAsync(this);
                    break;
                case "IPList":
                    this.ColumnsName = "Id,IP";
                    this.PageNumber = gridParameter.PageNumber;
                    this.RecordPerPage = this.GetPageRecord(gridParameter.RecordPerPage);
                    this.SortColumn = this.GetSortedColumn(gridParameter.Orders, gridParameter.Columns, "Id");
                    this.SortOrder = this.GetSortedOrder(gridParameter.SortOrder);
                    this.TableName = "LoginIPRestriction WITH(NOLOCK)";
                    this.Draw = gridParameter.Draw;
                    this.WhereClause = this.GetWhereCondition(
                        string.Empty,
                        gridParameter.Search,
                        gridParameter.Columns);
                    this.JsonData = obj.GetJsonAsync(this);
                    break;
                case "Country":
                    this.ColumnsName = "CountryId,CountryName,IsActive";
                    this.PageNumber = gridParameter.PageNumber;
                    this.RecordPerPage = this.GetPageRecord(gridParameter.RecordPerPage);
                    this.SortColumn = this.GetSortedColumn(gridParameter.Orders, gridParameter.Columns, "CountryId");
                    this.SortOrder = this.GetSortedOrder(gridParameter.SortOrder);
                    this.TableName = "Country WITH(NOLOCK)";
                    this.Draw = gridParameter.Draw;
                    this.WhereClause = this.GetWhereCondition(
                        string.Empty,
                        gridParameter.Search,
                        gridParameter.Columns);
                    this.JsonData = obj.GetJsonAsync(this);
                    break;
                case "User":
                    this.ColumnsName =
                        @"u.Email, u.Username, u.IsActive, u.FirstName, u.LastName,u.FirstName+' '+u.LastName AS FullName, u.State, u.City,
                        u.CountryId,u.EmailConfirmCode, u.Status,u.IsEmailConfirm,u.CreatedDate,u.MobileNo,u.UserId,u.Address,c.CountryName";
                    this.PageNumber = gridParameter.PageNumber;
                    this.RecordPerPage = this.GetPageRecord(gridParameter.RecordPerPage);
                    this.SortColumn = this.GetSortedColumn(gridParameter.Orders, gridParameter.Columns, "u.UserId");
                    this.SortOrder = this.GetSortedOrder(gridParameter.SortOrder);
                    this.TableName =
                        @"[User] u WITH(NOLOCK) LEFT JOIN Country c WITH(NOLOCK) ON u.CountryId = c.CountryId";
                    this.Draw = gridParameter.Draw;
                    this.WhereClause = this.GetWhereCondition("IsActive = " + gridParameter.Where + string.Empty,
                        gridParameter.Search,
                        gridParameter.Columns);
                    this.JsonData = obj.GetJsonAsync(this);
                    break;
                case "Setting":
                    this.ColumnsName = "Id,Tag,Value,SettingMsg";
                    this.PageNumber = gridParameter.PageNumber;
                    this.RecordPerPage = this.GetPageRecord(gridParameter.RecordPerPage);
                    this.SortColumn = this.GetSortedColumn(gridParameter.Orders, gridParameter.Columns, "Id");
                    this.SortOrder = this.GetSortedOrder(gridParameter.SortOrder);
                    this.TableName = "Setting WITH(NOLOCK)";
                    this.Draw = gridParameter.Draw;
                    this.WhereClause = this.GetWhereCondition(
                        string.Empty,
                        gridParameter.Search,
                        gridParameter.Columns);
                    this.JsonData = obj.GetJsonAsync(this);
                    break;
                case "EmailHistory":
                    this.ColumnsName = "Id,Subject,Body,OnDate";
                    this.PageNumber = gridParameter.PageNumber;
                    this.RecordPerPage = this.GetPageRecord(gridParameter.RecordPerPage);
                    this.SortColumn = this.GetSortedColumn(gridParameter.Orders, gridParameter.Columns, "Id");
                    this.SortOrder = this.GetSortedOrder(gridParameter.SortOrder);
                    this.TableName = "EmailHistory WITH(NOLOCK)";
                    this.Draw = gridParameter.Draw;
                    this.WhereClause = this.GetWhereCondition(
                        string.Empty,
                        gridParameter.Search,
                        gridParameter.Columns);
                    this.JsonData = obj.GetJsonAsync(this);
                    break;
                case "EmailPending":
                    this.ColumnsName =
                        @"u.UserId,u.FirstName+' '+u.LastName AS FullName,u.Email,u.EmailConfirmCode,u.IsEmailConfirm,u.CreatedDate";
                    this.PageNumber = gridParameter.PageNumber;
                    this.RecordPerPage = this.GetPageRecord(gridParameter.RecordPerPage);
                    this.SortColumn = this.GetSortedColumn(gridParameter.Orders, gridParameter.Columns, "u.UserId");
                    this.SortOrder = this.GetSortedOrder(gridParameter.SortOrder);
                    this.TableName =
                        @"[User] u WITH(NOLOCK) LEFT JOIN Country c WITH(NOLOCK) ON u.CountryId = c.CountryId";
                    this.Draw = gridParameter.Draw;
                    this.WhereClause = this.GetWhereCondition(
                        "IsEmailConfirm = 0",
                        gridParameter.Search,
                        gridParameter.Columns);
                    this.JsonData = obj.GetJsonAsync(this);
                    break;
                case "BlockedWallet":
                    this.ColumnsName = @"UserId,Email,IsWalletBlock,CreatedDate";
                    this.PageNumber = gridParameter.PageNumber;
                    this.RecordPerPage = this.GetPageRecord(gridParameter.RecordPerPage);
                    this.SortColumn = this.GetSortedColumn(gridParameter.Orders, gridParameter.Columns, "u.UserId");
                    this.SortOrder = this.GetSortedOrder(gridParameter.SortOrder);
                    this.TableName = "[User] WITH(NOLOCK)";
                    this.Draw = gridParameter.Draw;
                    this.WhereClause = this.GetWhereCondition(
                        "IsWalletBlock = 1",
                        gridParameter.Search,
                        gridParameter.Columns);
                    this.JsonData = obj.GetJsonAsync(this);
                    break;
                case "GetOpenOrder":
                    this.ColumnsName = @"UM.Email, Isnull(UM.FirstName + ' ' + UM.LastName, '') AS FullName,    
   OD.Id,    
   OD.OrderNo,    
   MP.[Name] Pair,
   MP.Id PairId,    
   CASE WHEN  OD.OrderType = 1 THEN 'BUY' ELSE 'SELL' END OrderType,    
   OD.Price,    
   OD.Amount,    
   CAST(OD.Amount * OD.Price AS DECIMAL(18,8)) AS Total,    
   OD.CreatedDate AS OnDate";
                    this.PageNumber = gridParameter.PageNumber;
                    this.RecordPerPage = this.GetPageRecord(gridParameter.RecordPerPage);
                    this.SortColumn = this.GetSortedColumn(gridParameter.Orders, gridParameter.Columns, "OD.CreatedDate");
                    this.SortOrder = this.GetSortedOrder(gridParameter.SortOrder);
                    this.TableName = @"[Order] OD WITH(NOLOCK)    
  INNER JOIN MatchPair MP WITH(NOLOCK) ON OD.PairId = MP.Id
  INNER JOIN[User] UM WITH(NOLOCK) ON OD.UserId = UM.UserId";
                    this.Draw = gridParameter.Draw;
                    this.WhereClause = this.GetWhereCondition(
                        gridParameter.Where,
                        gridParameter.Search,
                        gridParameter.Columns);
                    this.JsonData = obj.GetJsonAsync(this);
                    break;
                case "ConfirmOrder":
                    this.ColumnsName = @"B.Email AS BuyerID,
                        S.Email AS SellerID,
                        M.[Name] AS Pair,
                        O.price,
                        O.amount,
                        O.ondate AS OnDate";
                    this.PageNumber = gridParameter.PageNumber;
                    this.RecordPerPage = this.GetPageRecord(gridParameter.RecordPerPage);
                    this.SortColumn = this.GetSortedColumn(gridParameter.Orders, gridParameter.Columns, "o.id");
                    this.SortOrder = this.GetSortedOrder(gridParameter.SortOrder);
                    this.TableName = @"OrderConfirmList O
                      INNER JOIN[User] B ON B.UserID = O.mid
                      INNER JOIN[User] S ON S.UserID = O.tomid
                      INNER JOIN[MatchPair] M ON M.id = O.PairID";
                    this.Draw = gridParameter.Draw;
                    this.WhereClause = "";
                    this.JsonData = obj.GetJsonAsync(this);
                    break;
                case "TradeHistory":
                    this.ColumnsName = @"US.Email, US.FirstName + ' ' + US.LastName AS FullName,
			  OrderNo,
			  MP.[Name]  AS Market,  
			  CASE WHEN ORD.OrderType = 1 Then 'BUY' WHEN ORD.OrderType = 2 Then 'SELL' END AS [Type],  
			  ISNULL(Amount, 0) AS Amount,  
			  ISNULL(Price, 0) AS Price,  
			  ISNULL(REPLACE(FeeRemark, 'Charge:', CAST(Fee as varchar(20)) + ' '), '-') AS Fee,  
			  ISNULL((Amount + Price + Fee) ,0) AS Total,  
			  ORD.CreatedDate AS OnDate,ORD.[Status]";
                    this.PageNumber = gridParameter.PageNumber;
                    this.RecordPerPage = this.GetPageRecord(gridParameter.RecordPerPage);
                    this.SortColumn = this.GetSortedColumn(gridParameter.Orders, gridParameter.Columns, "ORD.Id");
                    this.SortOrder = this.GetSortedOrder(gridParameter.SortOrder);
                    this.TableName = @"[Order] ORD WITH(NOLOCK)   
			 INNER JOIN MatchPair MP WITH(NOLOCK) ON MP.Id = ORD.PairId 
			 LEFT JOIN [User] US WITH(NOLOCK) ON US.UserId = ORD.UserId";
                    this.Draw = gridParameter.Draw;
                    this.WhereClause = this.GetWhereCondition(
                        "1=1 AND [Status]=2",
                        gridParameter.Search,
                        gridParameter.Columns); ;
                    this.JsonData = obj.GetJsonAsync(this);
                    break;
                case "ManageWallet":
                    this.ColumnsName = @"u.Email, c.Symbol, m.ManageType, m.Amount, m.Remark, m.CreatedDate";
                    this.PageNumber = gridParameter.PageNumber;
                    this.RecordPerPage = this.GetPageRecord(gridParameter.RecordPerPage);
                    this.SortColumn = this.GetSortedColumn(gridParameter.Orders, gridParameter.Columns, "m.CreatedDate");
                    this.SortOrder = this.GetSortedOrder(gridParameter.SortOrder);
                    this.TableName = @"[ManageWalletBalance] m WITH(NOLOCK) 
                                        LEFT JOIN [User] u WITH(NOLOCK) ON u.UserId = m.UserId
                                        LEFT JOIN [Currency] c WITH(NOLOCK) ON c.Id = m.CurrencyId";
                    this.Draw = gridParameter.Draw;
                    this.WhereClause = this.GetWhereCondition(
                        string.Empty,
                        gridParameter.Search,
                        gridParameter.Columns);
                    this.JsonData = obj.GetJsonAsync(this);
                    break;
                default:
                    return;
            }
        }

        public string Columns { get; set; }

        public string ColumnsName { get; set; }

        public string ConnectionString { get; set; }

        public int Draw { get; set; }

        public Task<GridFinalData> JsonData { get; set; }

        public int PageNumber { get; set; }

        public int RecordPerPage { get; set; }

        public string SortColumn { get; set; }

        public string SortOrder { get; set; }

        public string TableName { get; set; }

        public string WhereClause { get; set; }

        public string GetColumn(List<Column> lstColumns)
        {
            var columns = string.Empty;
            foreach (var t in lstColumns)
            {
                if (!string.IsNullOrEmpty(t.Data))
                {
                    columns += string.IsNullOrEmpty(columns) ? t.Data : "," + t.Data;
                }
                else
                {
                    break;
                }
            }

            return columns;
        }

        public int GetPageRecord(int recordPerPage) =>
            string.IsNullOrEmpty(Convert.ToString(recordPerPage, CultureInfo.CurrentCulture)) ? 10 : recordPerPage;

        public string GetSortedColumn(List<Order> lstOrder, List<Column> lstColumns, string defaultColName)
        {
            if (lstOrder.Count > 0 && lstColumns.Count > 0
                                   && string.IsNullOrEmpty(
                                       Convert.ToString(lstOrder[0].Column, CultureInfo.CurrentCulture)))
            {
                return defaultColName;
            }

            var colName = Convert.ToString(lstColumns[lstOrder[0].Column].Data, CultureInfo.CurrentCulture);

            return string.IsNullOrEmpty(colName) ? defaultColName : colName;
        }

        public string GetSortedOrder(string sortOrder) => string.IsNullOrEmpty(sortOrder) ? "asc" : sortOrder;

        public string GetWhereCondition(string where, string search, List<Column> lstColumns)
        {
            var whereforall = string.Empty;
            if (string.IsNullOrEmpty(search))
            {
                return where;
            }

            var columns = this.GetColumn(lstColumns).Split(',');

            whereforall = columns.Where(col => col.ToLower(CultureInfo.CurrentCulture) != "rownumber").Aggregate(
                whereforall,
                (current, col) => current + (string.IsNullOrEmpty(current)
                                                 ? col + " LIKE '%" + search + "%'"
                                                 : " OR " + col + " LIKE '%" + search + "%'"));

            where += string.IsNullOrEmpty(where) ? "(" + whereforall + ")" : " AND (" + whereforall + ")";

            return where;
        }

    }
}
