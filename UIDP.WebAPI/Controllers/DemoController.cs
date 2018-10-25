
using Microsoft.AspNetCore.Mvc;
using UIDP.Bussiness;
using UIDP.Entity;
using Microsoft.AspNetCore.Authorization;
using UIDP.Model;
using UIDP.Utility;

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
        [Authorize(Roles ="Admin")]
        [Route("GetPageList")]
        public JsonResult<Demo> GetPageList(int pageIndex, int pageSize)
        {
            return bll.GetPageList(pageIndex, pageSize);
        }

        #region 生成实体类
        /// <summary>
        /// 生成实体类
        /// </summary>
        /// <param name="entityName"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Entity/Create")]
        public JsonResult CreateEntity(string entityName)
        {
            return Json(bll.CreateEntity(entityName, BaseConfigModel.ContentRootPath));
        }
        #endregion
        #region Token
        /// <summary>
        /// 模拟登录，获取JWT
        /// </summary>
        /// <param name="tm"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Token")]
        public JsonResult GetJWTStr(TokenModel tm)
        {
            return Json(JwtHelper.IssueJWT(tm));
        }
        #endregion
    }
}