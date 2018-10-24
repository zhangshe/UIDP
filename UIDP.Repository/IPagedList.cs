using System;
using System.Collections.Generic;
using System.Text;

namespace UIDP.Repository
{
    /// <summary>
    /// 分页接口
    /// </summary>
    public interface IPagedList<T> : IList<T>
    {
        /// <summary>
        /// 页码
        /// </summary>
        int PageIndex { get; }
        /// <summary>
        /// 分页大小
        /// </summary>
        int PageSize { get; }
        /// <summary>
        /// 记录总条数
        /// </summary>
        int TotalCount { get; }
        /// <summary>
        /// 数据总页数
        /// </summary>
        int TotalPages { get; }
        /// <summary>
        /// 是否有上一页
        /// </summary>
        bool HasPreviousPage { get; }
        /// <summary>
        /// 是否有下一页
        /// </summary>
        bool HasNextPage { get; }
    }
}
