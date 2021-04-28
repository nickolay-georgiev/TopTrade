namespace TopTrade.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class BaseApiController : Controller
    {
    }
}
