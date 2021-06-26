using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisDistributedCache.Controllers
{
    public class BaseController : Controller
    {
        protected IDistributedCache _distributedcache;
        public BaseController(IDistributedCache distributedCache)
        {
            _distributedcache = distributedCache;

        }
    }
}
