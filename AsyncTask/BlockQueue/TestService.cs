using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    [InterceptorOfService]
    public class TestService : ContextBoundObject
    {
        
        public void Test()
        {
            Console.WriteLine("Processing");
        }
    }
}
