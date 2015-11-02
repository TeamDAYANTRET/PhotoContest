using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace PhotoContest.Tests.Mocks.Identity
{
    class TestableHttpContext : HttpContextBase
    {
        public override IPrincipal User { get; set; }
    }
}
