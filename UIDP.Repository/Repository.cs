
using UIDP.Utility;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using UIDP.Model;

namespace UIDP.Repository
{
    public abstract class Repository<T> : IRepository<T> where T : class, new()
    {
        public SqlSugarClient db;
        public Repository()
        {
            db = DbFactory.GetSqlSugarClient();
        }
        #region 数据仓库实现

        /// <summary>
        /// 根据主值查询单条数据
        /// </summary>
        /// <param name="pkValue">主键值</param>
        /// <returns>泛型实体</returns>
        public T FindById(object pkValue)
        {
            var entity = db.Queryable<T>().InSingle(pkValue);
            return entity;
        }

        /// <summary>
        /// 查询所有数据(无分页,请慎用)
        /// </summary>
        /// <returns></returns>
        public IEnumerable<T> FindAll()
        {
            var list = db.Queryable<T>().ToList();
            return list;
        }

        /// <summary>
        /// 根据条件查询数据
        /// </summary>
        /// <param name="predicate">条件表达式树</param>
        /// <param name="orderBy">排序</param>
        /// <returns>泛型实体集合</returns>
        public IEnumerable<T> FindListByCondition(Expression<Func<T, bool>> predicate, string orderBy = "")
        {
            var query = db.Queryable<T>().Where(predicate);
            if (!string.IsNullOrEmpty(orderBy))
            {
                query = query.OrderBy(orderBy);
            }
            var entities = query.ToList();
            return entities;
        }

        /// <summary>
        /// 根据条件查询数据
        /// </summary>
        /// <param name="predicate">条件表达式树</param>
        /// <returns></returns>
        public T FindByCondition(Expression<Func<T, bool>> predicate)
        {
            var entity = db.Queryable<T>().First(predicate);
            return entity;
        }

        /// <summary>
        /// 写入实体数据
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns></returns>
        public bool Insert(T entity)
        {
            var i = db.Insertable(entity).ExecuteCommand() > 0;
            return i;
        }

        /// <summary>
        /// 更新实体数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Update(T entity)
        {
            var i = db.Updateable(entity).ExecuteCommand() > 0;
            return i;
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns></returns>
        public bool Delete(T entity)
        {
            var i = db.Deleteable(entity).ExecuteCommand() > 0;
            return i;
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="where">过滤条件</param>
        /// <returns></returns>
        public bool Delete(Expression<Func<T, bool>> condition)
        {
            var i = db.Deleteable<T>(condition).ExecuteCommand() > 0;
            return i;
        }

        /// <summary>
        /// 删除指定ID的数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteById(object id)
        {
            var i = db.Deleteable<T>(id).ExecuteCommand() > 0;
            return i;
        }

        /// <summary>
        /// 删除指定ID集合的数据(批量删除)
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool DeleteByIds(object[] ids)
        {
            var i = db.Deleteable<T>().In(ids).ExecuteCommand() > 0;
            return i;
        }

        /// <summary>
        /// 根据条件查询分页数据
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="orderBy"></param>
        /// <param name="pageIndex">当前页面索引</param>
        /// <param name="pageSize">分布大小</param>
        /// <returns></returns>
        public IPagedList<T> FindPagedList(Expression<Func<T, bool>> predicate, string orderBy = "", int pageIndex = 1, int pageSize = 20)
        {
            var totalCount = 0;
            var result = db.Queryable<T>().Where(predicate).OrderBy(orderBy).ToPageList(pageIndex, pageSize, ref totalCount);
            var list = new PagedList<T>(result, pageIndex, pageSize, totalCount);
            return list;
        }

        /// <summary>
        /// 生成实体类
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public bool CreateEntity(string entityName, string filePath)
        {
            try
            {
                db.DbFirst
                    .IsCreateAttribute()
                    .Where(entityName)
                    .CreateClassFile(filePath, "UIDP.Entity");
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion
    }
}
