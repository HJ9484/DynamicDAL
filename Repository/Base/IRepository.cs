using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicDAL.Repository.Base
{
    public interface IRepository<TEntity, TKey> : IDisposable
        where TEntity : class/* Common.Models.ObjectModel<TKey>*/
         where TKey : struct
    {
        //Read
        List<TEntity> GetList();
        Task<List<TEntity>> GetListAsync();

        //Create
        ActionResult Create(TEntity instance);
        Task<ActionResult> CreateAsync(TEntity instance);


        //Edit
        ActionResult Edit(TEntity instance);
        Task<ActionResult> EditAsync(TEntity instance);

        //Delete
        ActionResult Delete(TKey id);
        Task<ActionResult> DeleteAsync(TKey id);


        Task<ActionResult> BulkinsertAsync(TEntity instance, int count, int commitCount);

        //Save
        ActionResult Save();
        Task<ActionResult> SaveAsync();

    }
}
