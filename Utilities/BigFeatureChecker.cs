using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public static class BigFeatureChecker
    {
        public static bool Check(int num)
        {
            List<int> list = new List<int>();
            int first = 1;
            list.Add(first);
            for (int i = 6; i <= 100; i+=6)
            {
                list.Add(i);
                list.Add(i+1);
            }
            if (list.Any(n=> n == num))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
