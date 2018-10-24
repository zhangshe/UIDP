using UIDP.Entity;
using UIDP.IService;
using UIDP.Model;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using UIDP.Utility;
using UIDP.Repository;

namespace UIDP.Service
{
    public class DemoService : Repository<Demo>, IDemoService
    {
        //private readonly IRepository<Demo> _repository;
      
        //public DemoService(IRepository<Demo> repository) : base(repository)
        //{
        //    _repository = repository;
        //}
        //public DemoService()
        //{
        //}
    }
}
