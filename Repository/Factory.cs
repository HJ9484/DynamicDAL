using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DynamicDAL.Repository
{
    public static class Factory<TEntity, TKey>
         where TEntity : class
         where TKey : struct
    {
        public static int DBConnectionType = 0;
        public static Repository.Base.IRepository<TEntity, TKey> DALTypeSelector(Assembly types,int dbconnectionType)
        {
            if (types.GetTypes().Where(w => w.Name == (typeof(TEntity).Name + "Repository")).FirstOrDefault().FullName != null && DBConnectionType == 0)
            {
                switch (dbconnectionType)
                {
                    case 0:
                        var Entityinstance = types.GetTypes().Where(w => w.Name == (typeof(TEntity).Name + "Repository")).FirstOrDefault().FullName;
                        return types.CreateInstance(Entityinstance) as Repository.Base.IRepository<TEntity, TKey>;
                    case 1:
                        var ADOinstance = types.GetTypes().Where(w => w.Name.Contains(typeof(TEntity).Name)).Where(w => w.Name.Contains("ADO")).FirstOrDefault().FullName;
                        return types.CreateInstance(ADOinstance) as Repository.Base.IRepository<TEntity, TKey>;

                    default:
                        break;
                }
                return null;

            }
            return null;
        }

        //public static Repository.Base.IRepository<TEntity, TKey> DALTypeSelector(Assembly types)
        //{
        //    if (types.GetTypes().Where(w => w.Name == (typeof(TEntity).Name + "Repository")).FirstOrDefault().FullName != null && DBConnectionType == 0)
        //    {
        //        var Entityinstance = types.GetTypes().Where(w => w.Name == (typeof(TEntity).Name + "Repository")).FirstOrDefault().FullName;
        //        return types.CreateInstance(Entityinstance) as Repository.Base.IRepository<TEntity, TKey>;
        //    }

        //    else if (types.GetTypes().Where(w => w.Name.Contains(typeof(TEntity).Name)).Where(w => w.Name.Contains("ADO")).FirstOrDefault().FullName != null && DBConnectionType == 1)
        //    {

        //        var ADOinstance = types.GetTypes().Where(w => w.Name.Contains(typeof(TEntity).Name)).Where(w => w.Name.Contains("ADO")).FirstOrDefault().FullName;
        //        return types.CreateInstance(ADOinstance) as Repository.Base.IRepository<TEntity, TKey>;
        //    }

        //    return null;

        //}
    }
}
