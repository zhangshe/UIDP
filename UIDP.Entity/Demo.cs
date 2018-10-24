using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace UIDP.Entity
{
    /// <summary>
    /// Demo实体
    /// </summary>
   //[SugarTable("demo")]
    public class Demo
    {
        /// <summary>
        /// Id
        /// </summary>
        public int T_ID { get; set; }
        /// <summary>
        /// 编号
        /// </summary>
        public string T_Code { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string T_Name { get; set; }
    }
}
