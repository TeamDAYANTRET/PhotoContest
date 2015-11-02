using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PhotoContest.Tests.Mocks.Identity
{
    class TestableControllerContext : ControllerContext
    {
        public TestableHttpContext TestableHttpContext { get; set; }
    }
}
