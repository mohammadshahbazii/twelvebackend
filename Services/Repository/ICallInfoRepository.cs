using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.Text;
using System.Threading.Tasks;
using DataLayer;

namespace Services
{
    public interface ICallInfoRepository : IDisposable
    {
        public List<CallInfoLink> GetCallInfoLinks(int CallInfoID);
        public DataLayer.CallInfo GetByID(int InfoID);
        public bool Delete(DataLayer.CallInfo callInfo );
        public bool Update(DataLayer.CallInfo callInfo ,IFormFile ImageName );
        public bool Create(DataLayer.CallInfo callInfo ,IFormFile ImageName );
        public List<DataLayer.CallInfo> GetCallInfoes();
    }
}
