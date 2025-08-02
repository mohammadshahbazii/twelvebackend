using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;
using ViewModels;

namespace Services
{
    public class OrderRepository : IOrderRepository
    {
        TwelveDbContext db = new TwelveDbContext();
        public int CreateOrder(int Amount)
        {
            Order order = new Order() 
            {
                Amount = Amount,
                IsFinally = false,
                CreateDate = DateTime.Now
            };
            db.Orders.Add(order);
            db.SaveChanges();
            return order.OrderId;
        }

        public void Dispose()
        {
            db.Dispose();
        }

        public void FinalizedOrder(int OrderID)
        {
            var order = db.Orders.Find(OrderID);
            order.IsFinally = true;
            db.Orders.Update(order);
            db.SaveChanges();
        }

        public PaginationViewModel GetPagination(double PageCount, int PageID = 1)
        {
            PaginationViewModel pagination = new PaginationViewModel();
            pagination.PageNumbers = new List<int>();
            int page = Convert.ToInt32(PageCount) + 1;
            if (PageID < 3)
            {
                for (int i = 1; i < 6; i++)
                {
                    if (i < page)
                    {
                        pagination.PageNumbers.Add(i);
                    }
                }
            }
            else
            {
                for (int i = PageID - 2; i <= PageID + 2; i++)
                {

                    if (i < page)
                    {
                        pagination.PageNumbers.Add(i);
                    }
                }
            }

            pagination.PageCount = Convert.ToInt32(PageCount);
            pagination.PageID = PageID;

            return pagination;
        }


        public OrdersPageDataViewModel GetOrders( int PageID = 1)
        {
            OrdersPageDataViewModel model = new OrdersPageDataViewModel();
            int take = 8;
            int skip = (PageID - 1) * take;
            model.Orders = new List<OrderItemViewModel>();
            var orders = db.Orders.OrderByDescending(o => o.CreateDate).Skip(skip).Take(take).ToList();
            orders.ApplyTranslations(db);
            foreach (var item in orders)
            {
                model.Orders.Add(new OrderItemViewModel()
                {
                    OrderID = item.OrderId,
                    CreateDate = DateConvertor.ToShamsi(item.CreateDate),
                    Amount = item.Amount.ToString("#,0"),
                    IsFinally = item.IsFinally
                });
            }
            double pCount = Convert.ToDouble(Convert.ToDouble(db.Orders.ToList().Count()) / Convert.ToDouble(take));
            model.PageCount = pCount;
            model.Pagination = GetPagination(pCount, PageID);
            model.Sum = OrdersSum();
            return model;
        }

        public string OrdersSum()
        {
            var sum = db.Orders.Where(o => o.IsFinally == true).Select(o=> o.Amount).Sum();
            return sum.ToString("#,0") + " ریال ";
        }
    }
}
