using System;
using System.Collections.Generic;
using System.Text;

namespace UIDP.Model
{
    /// <summary>
    /// 表格数据，支持分页
    /// </summary>
    public class JsonResult<T>
    {
        /// <summary>
        /// 返回编码
        /// </summary>
        public int code { get; set; }
        /// <summary>
        /// 返回信息
        /// </summary>
        public string message { get; set; }
        /// <summary>
        /// 记录总数
        /// </summary>
        public int total { get; set; }
        /// <summary>
        /// 返回数据集
        /// </summary>
        public IList<T> data { get; set; }
    }
}
