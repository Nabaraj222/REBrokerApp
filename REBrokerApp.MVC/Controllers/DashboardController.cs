using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using REBrokerApp.Business.Services;

namespace REBrokerApp.Web.Controllers
{
    [Authorize(Roles = "Broker")]
    public class DashboardController : Controller
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        // GET: Dashboard
        public async Task<IActionResult> Index()
        {
            var userName = User.Identity?.Name;
            if (string.IsNullOrEmpty(userName))
            {
                return RedirectToAction("Index", "Home");
            }

            var dashboardData = await _dashboardService.GetBrokerDashboardAsync(userName);
            return View(dashboardData);
        }
    }
}