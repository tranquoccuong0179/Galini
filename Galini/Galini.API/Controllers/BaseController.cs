﻿using Galini.API.Constants;
using Microsoft.AspNetCore.Mvc;

namespace Galini.API.Controllers
{
    [Route(ApiEndPointConstant.ApiEndpoint)]
    [ApiController]
    public class BaseController<T> : ControllerBase where T : BaseController<T>
    {
        protected ILogger<T> _logger;
        private ILogger<UserInfoController> logger;

        public BaseController(ILogger<T> logger)
        {
            _logger = logger;
        }

        public BaseController(ILogger<UserInfoController> logger)
        {
            this.logger = logger;
        }
    }
}
