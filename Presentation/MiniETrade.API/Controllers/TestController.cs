using Microsoft.AspNetCore.Mvc;
using MiniETrade.Application.Common.Abstractions.Caching;

namespace MiniETrade.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly ICachingService _cachingService;
        public TestController(ICachingService cachingService)
        {
            _cachingService = cachingService;
        }

        [HttpGet("husotest")]
        public async Task<IActionResult> AddCaching()
        {
            await _cachingService.SetAsync("huso1", "husokanuslaşmaktır.");
            await _cachingService.SetAsync("husoObj1", new HusoRecord("record1", 31, DateTime.Now));
            await _cachingService.SetAsync("huso2", "husokanuslaşmaktır.2");
            await _cachingService.SetAsync("husoObj2", new HusoRecord("record2", 32, DateTime.Now));
            return Ok();
        }

        [HttpGet("husogettest")]
        public async Task<IActionResult> GetCaching()
        {
            var result1 = await _cachingService.GetAsync<string>("huso1");
            var result2 = await _cachingService.GetAsync<HusoRecord>("husoObj1");
            await _cachingService.RemoveAsync("huso2");
            var result3 = await _cachingService.GetAsync<string>("huso2");
            var result4 = await _cachingService.GetAsync<HusoRecord>("husoObj2");

            return Ok( new
            {
                huso1 = result1,
                huso2 = result2,
                huso3 = result3,
                huso4 = result4
            } );
        }
    }
}

public record HusoRecord(string Name, int Age, DateTime Date);
