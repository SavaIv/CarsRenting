using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using static CarRenting.Areas.Admin.AdminConstants;

namespace CarRenting.Areas.Admin.Controllers
{
    [Area(AreaName)]
    [Authorize(Roles = AdministratorRoleName)]

    public abstract class AdminController : Controller
    {
    }
}
