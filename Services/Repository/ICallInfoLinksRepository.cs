using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface ICallInfoLinksRepository : IDisposable
    {
        public CallInfoLink GetByID(int LinkID);
        public List<CallInfoLink> GetLinks(int CallInfoID);
        public bool Create(CallInfoLink callInfoLink);
        public bool Update(CallInfoLink callInfoLink);
        public bool Delete(CallInfoLink callInfoLink);
    }
}
