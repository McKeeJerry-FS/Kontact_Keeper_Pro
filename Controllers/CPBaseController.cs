using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Kontact_Keeper_Pro.Controllers
{
    [Controller]
    public abstract class CPBaseController : Controller
    {
        protected string? _userId => User.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}
