using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Threading.Tasks;

namespace SqlDistributedCache.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class sessionController : ControllerBase
    {
        private readonly IDistributedCache _distributedCache;
        public sessionController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }


        [Route("add-cache-no-time-options")]
        [HttpGet]
        public async Task<IActionResult> AddCacheNoTimeOptions(string key,string value)
        {
            
            await _distributedCache.SetStringAsync(key, value);
            return Ok("success");
        }

        [Route("add-cache")]
        [HttpGet]
        public async Task<IActionResult> AddCache(string key, string value)
        {
         
            DistributedCacheEntryOptions options = new DistributedCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromMinutes(1440),
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1440)
            };
            await _distributedCache.SetStringAsync(key, value, options);
            return Ok("success");
        }
        [Route("get-cache")]
        [HttpGet]
        public async Task<IActionResult> GetCache(string key)
        {
            string name = await _distributedCache.GetStringAsync(key);
            if (name == null)
            {
                return Ok(true);
            }
            else
            {
                return Ok(false);
            }

           
        }
        
    }
}