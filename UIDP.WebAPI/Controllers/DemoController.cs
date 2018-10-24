using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using UIDP.Bussiness;
using UIDP.Entity;

using UIDP.Model;

namespace UIDP.Controllers
{
    /// <summary>
    /// Demo模块
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class DemoController : Controller
    {
        private DemoBLL bll = new DemoBLL();
        /// <summary>
        /// 获取分页列表数据
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页大小</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPageList")]
        public JsonResult<Demo> GetPageList(int pageIndex, int pageSize)
        {
            return bll.GetPageList(pageIndex, pageSize);
        }
    }
}