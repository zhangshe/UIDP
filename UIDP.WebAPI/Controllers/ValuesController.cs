using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace UIDP.WebAPI.Controllers
{
    /// <summary>
    /// 测试接口
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        /// <summary>
        /// 获取全部
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        /// <summary>
        /// 获取单个 api/values/5
        /// </summary>
        /// <param name="id">测试ID</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        /// <summary>
        /// 提交方法POST api/values
        /// </summary>
        /// <param name="value">表单元素</param>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        /// <summary>
        ///  删除PUT api/values/5
        /// </summary>
        /// <param name="id">主键</param>
        /// <param name="value">内容</param>
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        /// <summary>
        /// 删除DELETE api/values/5
        /// </summary>
        /// <param name="id">主键</param>
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
