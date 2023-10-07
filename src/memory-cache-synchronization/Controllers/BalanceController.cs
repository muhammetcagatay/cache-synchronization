using memory_cache_synchronization.Models;
using memory_cache_synchronization.Services;
using Microsoft.AspNetCore.Mvc;

namespace memory_cache_synchronization.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BalanceController : ControllerBase
    {
        private readonly IBalanceService _balanceService;

        public BalanceController(IBalanceService balanceService)
        {
            _balanceService = balanceService;
        }

        [HttpGet("{balanceId}")]
        public IActionResult Get(int balanceId)
        {
            return Ok(_balanceService.GetBalance(balanceId));
        }

        [HttpPost]
        public IActionResult CreateOrUpdate(CustomerBalance customerBalance)
        {
            _balanceService.CreateOrUpdateBalance(customerBalance);

            return Ok();
        }

        [HttpDelete("{balanceId}")]
        public IActionResult Delete(int balanceId)
        {
            _balanceService.DeleteBalance(balanceId);
            return Ok();
        }
    }
}
