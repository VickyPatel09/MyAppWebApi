using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyAngularAppCore.Entities;
using MyAngularAppWebApi.Model;
using MyAngularAppWebApi.Utility;

namespace MyAngularAppWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        [HttpGet, Route("GetOrderList")]
        public IActionResult GetOrderList()
        {
            using (var context = new MyAngularAppContext())
            {
                var BuyOrder=context.BuyOrder.Select(x=> new 
                {
                    Order_ID=x.OrderId,
                    Name=x.User.FullName,
                    Coin=x.Coin,
                    Rate=x.Rate
                }).ToList();


                var SaleOrder=context.SaleOrder.Select(x=> new 
                {
                    Order_ID=x.OrderId,
                    Name=x.User.FullName,
                    Coin=x.Coin,
                    Rate=x.Rate
                }).ToList();

                var Orders=new {BuyOrder=BuyOrder,SaleOrder=SaleOrder };

                return this.Ok(ApiResponse.SetResponse(ApiResponseStatus.Ok, " ", Orders));
            }
        }

        [HttpPost, Route("BuyCoin")]
        public IActionResult BuyCoin(BuyOrderModel model)
        {
            using (var context = new MyAngularAppContext())
            {
                var saleData=context.SaleOrder.Where(x=>x.Rate <= model.Rate).ToList();
                var totalCoin=model.Coin;
                var buyCoin=0;
                var isCompleted=false;
                foreach (var item in saleData)
                {
                    if(item.Coin <= (totalCoin - buyCoin))
                    {
                        Transaction objTransaction=new Transaction();
                        objTransaction.UserId = 1;
                        objTransaction.TransactionType = 1;
                        objTransaction.SaleUserId = item.UserId;
                        objTransaction.TransactionDateTime = DateTime.Now;
                        objTransaction.Coin = item.Coin;
                        objTransaction.Rate = model.Rate;
                        context.Transaction.Add(objTransaction);
                        context.SaveChanges();
                        buyCoin += (int)objTransaction.Coin;

                        var saleCoin=context.SaleOrder.Find(item.OrderId);
                        if (saleCoin != null)
                        {
                            var remaining_coin=saleCoin.Coin - objTransaction.Coin;
                            if (remaining_coin <= 0)
                            {
                                context.SaleOrder.Remove(saleCoin);
                            }
                            else
                            {
                                saleCoin.Coin = remaining_coin;
                            }
                            context.SaveChanges();
                        }
                    } 
                    else
                    {
                        Transaction objTransaction=new Transaction();
                        objTransaction.UserId = 1;
                        objTransaction.TransactionType = 1;
                        objTransaction.SaleUserId = item.UserId;
                        objTransaction.TransactionDateTime = DateTime.Now;
                        objTransaction.Coin = (totalCoin - buyCoin);
                        objTransaction.Rate = item.Rate;
                        context.Transaction.Add(objTransaction);
                        context.SaveChanges();
                        buyCoin += (int)objTransaction.Coin;

                        var saleCoin=context.SaleOrder.Find(item.OrderId);
                        if(saleCoin!=null)
                        {
                            var remaining_coin=saleCoin.Coin - objTransaction.Coin;
                            if(remaining_coin <=0)
                            {
                                context.SaleOrder.Remove(saleCoin);
                            }
                            else
                            {
                                saleCoin.Coin = remaining_coin;
                            }
                            context.SaveChanges();
                        }

                    }

                    if(totalCoin == buyCoin)
                    {
                        isCompleted = true;
                        break;
                    }
                }

                if (!isCompleted)
                {
                    BuyOrder  objBuyOrder=new BuyOrder();
                    objBuyOrder.Coin = (totalCoin - buyCoin);
                    objBuyOrder.Rate = model.Rate;
                    objBuyOrder.UserId = 1;
                    context.BuyOrder.Add(objBuyOrder);
                    context.SaveChanges();
                }
                return this.Ok(ApiResponse.SetResponse(ApiResponseStatus.Ok, "Order Placed Successfully ", null));
            }
        }

        [HttpPost, Route("SaleCoin")]
        public IActionResult SaleCoin(SaleOrderModel model)
        {

            using (var context = new MyAngularAppContext())
            {
                var saleData=context.BuyOrder.Where(x=>x.Rate <= model.Rate).ToList();
                var totalCoin=model.Coin;
                var coinTosale=0;
                var isCompleted=false;
                foreach (var item in saleData)
                {
                    if (item.Coin <= (totalCoin - coinTosale))
                    {
                        Transaction objTransaction=new Transaction();
                        objTransaction.UserId = 1;
                        objTransaction.TransactionType = 2;
                        objTransaction.SaleUserId = item.UserId;
                        objTransaction.TransactionDateTime = DateTime.Now;
                        objTransaction.Coin = item.Coin;
                        objTransaction.Rate = item.Rate;
                        context.Transaction.Add(objTransaction);
                        context.SaveChanges();
                        coinTosale += (int)objTransaction.Coin;

                        var buyCoin=context.BuyOrder.Find(item.OrderId);
                        if (buyCoin != null)
                        {
                            var remaining_coin=buyCoin.Coin - objTransaction.Coin;
                            if(remaining_coin <=0)
                            {
                                context.BuyOrder.Remove(buyCoin);
                            }
                            else
                            {
                                buyCoin.Coin = remaining_coin;
                            }
                            
                            context.SaveChanges();
                        }
                    }
                    else
                    {
                        Transaction objTransaction=new Transaction();
                        objTransaction.UserId = 1;
                        objTransaction.TransactionType = 1;
                        objTransaction.SaleUserId = item.UserId;
                        objTransaction.TransactionDateTime = DateTime.Now;
                        objTransaction.Coin = (totalCoin - coinTosale);
                        objTransaction.Rate = model.Rate;
                        context.Transaction.Add(objTransaction);
                        context.SaveChanges();
                        coinTosale += (int)objTransaction.Coin;

                        var buyCoin=context.BuyOrder.Find(item.OrderId);
                        if (buyCoin != null)
                        {
                            var remaining_coin=buyCoin.Coin - objTransaction.Coin;
                            if (remaining_coin <= 0)
                            {
                                context.BuyOrder.Remove(buyCoin);
                            }
                            else
                            {
                                buyCoin.Coin = remaining_coin;
                            }

                            context.SaveChanges();
                        }

                    }

                    if (totalCoin == coinTosale)
                    {
                        isCompleted = true;
                        break;
                    }
                }

                if (!isCompleted)
                {
                    SaleOrder objSaleOrder=new SaleOrder();
                    objSaleOrder.Coin = (totalCoin - coinTosale);
                    objSaleOrder.Rate = model.Rate;
                    objSaleOrder.UserId = 1;
                    context.SaleOrder.Add(objSaleOrder);
                    context.SaveChanges();
                }
                return this.Ok(ApiResponse.SetResponse(ApiResponseStatus.Ok, "Order Placed Successfully ", null));
            }


            //using (var context = new MyAngularAppContext())
            //{
            //    SaleOrder objSaleOrder=new SaleOrder();
            //    objSaleOrder.Coin = model.Coin;
            //    objSaleOrder.Rate = model.Rate;
            //    objSaleOrder.UserId = 1;
            //    context.SaleOrder.Add(objSaleOrder);
            //    context.SaveChanges();
            //    return this.Ok(ApiResponse.SetResponse(ApiResponseStatus.Ok, "Order Placed Successfully ", null));
            //}
        }

        [HttpGet, Route("GetTransaction")]
        public IActionResult GetTransaction()
        {
            using (var context = new MyAngularAppContext())
            {
                var Transactions=context.Transaction.Select(x=> new
                {
                    Name=x.User.FullName,
                    Coin=x.Coin,
                    Rate=x.Rate,
                    Date =x.TransactionDateTime.Value.ToString("dd/MM/yyyy HH:mm"),
                    Type=(x.TransactionType==1 ? "Buy" : "Sale")
                }).ToList();

                var MyTransactions = new {Transactions=Transactions };

                return this.Ok(ApiResponse.SetResponse(ApiResponseStatus.Ok, " ", MyTransactions));
            }
        }

        [HttpPost, Route("DeleteBuyOrder")]
        public IActionResult DeleteBuyOrder(int id)
        {
            using (var context = new MyAngularAppContext())
            {
                var BuyOrderDetail=context.BuyOrder.Find(id);
                if(BuyOrderDetail!=null)
                {
                    context.BuyOrder.Remove(BuyOrderDetail);
                    context.SaveChanges();
                }
               
                return this.Ok(ApiResponse.SetResponse(ApiResponseStatus.Ok, "Order Deleted Successfully ", null));
            }
        }

        [HttpPost, Route("DeleteSaleOrder")]
        public IActionResult DeleteSaleOrder(int id)
        {
            using (var context = new MyAngularAppContext())
            {
                var SaleOrderDetail=context.SaleOrder.Find(id);
                if (SaleOrderDetail != null)
                {
                    context.SaleOrder.Remove(SaleOrderDetail);
                    context.SaveChanges();
                }

                return this.Ok(ApiResponse.SetResponse(ApiResponseStatus.Ok, "Order Deleted Successfully ", null));
            }
        }


    }
}