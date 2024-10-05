using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels;

namespace Services
{
    public interface IOrderRepository : IDisposable
    {
        public string OrdersSum();
        public OrdersPageDataViewModel GetOrders( int PageID =1);

        public void FinalizedOrder(int OrderID);
        public int CreateOrder(int Amount);
    }
}
