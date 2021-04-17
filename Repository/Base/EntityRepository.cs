using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicDAL.Repository.Base
{

    public abstract class EntityRepository<TEntity, TKey> : IRepository<TEntity, TKey>, IDisposable
       where TEntity : class /*Common.Models.ObjectModel<TKey>*/
        where TKey : struct
    {
        protected DbContext _context;


        protected DbSet<TEntity> EntityCollection => _context.Set<TEntity>();

        public abstract string TranslateSqlException(int errorNumber, string errorMessage);

        public abstract DbContext CreateContext();
       
        

        public EntityRepository()
        {
            _context = CreateContext();
        }

        public virtual List<TEntity> GetList()
        {
            try
            {
                return EntityCollection.ToList();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public virtual async Task<List<TEntity>> GetListAsync()
        {
            try
            {
                return await EntityCollection.ToListAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public virtual ActionResult Create(TEntity instance)
        {
            ActionResult result = new ActionResult(true, string.Empty);
            try
            {
                EntityCollection.Add(instance);
                return Save();
            }
            catch (Exception ex)
            {
                return HandleException(ex, result);
            }
        }

        public virtual async Task<ActionResult> CreateAsync(TEntity instance)
        {
            ActionResult result = new ActionResult(true, string.Empty);
            try
            {
                EntityCollection.Add(instance);
                return await SaveAsync();
            }
            catch (Exception ex)
            {
                return await HandleExceptionAsync(ex, result);
            }
        }


        public virtual ActionResult Edit(TEntity instance)
        {
            ActionResult result = new ActionResult() { Success = true };
            try
            {
                _context.Entry(instance).State = EntityState.Modified;
                return Save();

            }
            catch (Exception ex)
            {
                return HandleException(ex, result);
            }
        }

        public virtual async Task<ActionResult> EditAsync(TEntity instance)
        {

            ActionResult result = new ActionResult() { Success = true };
            try
            {
                _context.Entry(instance).State = EntityState.Modified;
                return await SaveAsync();

            }
            catch (Exception ex)
            {
                return await HandleExceptionAsync(ex, result);
            }
        }
        public virtual ActionResult Delete(TKey id)
        {
            ActionResult result = new ActionResult() { Success = true };

            try
            {
                var instance = EntityCollection.Find(id);
                _context.Entry(instance).State = EntityState.Deleted;
                return Save();
            }
            catch (Exception ex)
            {
                return HandleException(ex, result);
            }
        }

        public virtual async Task<ActionResult> DeleteAsync(TKey id)
        {

            ActionResult result = new ActionResult() { Success = true };

            try
            {
                var instance = EntityCollection.FindAsync(id);
                _context.Entry(instance).State = EntityState.Deleted;
                return await SaveAsync();
            }
            catch (Exception ex)
            {
                return await HandleExceptionAsync(ex, result);
            }
        }

        public virtual ActionResult Save()
        {
            ActionResult result = new ActionResult(true, string.Empty);
            try
            {
                result.Success = _context.SaveChanges() > 0;
                return result;
            }
            catch (Exception ex)
            {
                return HandleException(ex, result);
            }
        }

        public virtual async Task<ActionResult> SaveAsync()
        {

            ActionResult result = new ActionResult(true, string.Empty);
            try
            {
                result.Success = await _context.SaveChangesAsync() > 0;
                return result;
            }
            catch (Exception ex)
            {
                return await HandleExceptionAsync(ex, result);
            }
        }

        protected ActionResult HandleException(Exception ex, ActionResult result)
        {
            result.Success = false;
            if (ex is System.Data.Entity.Validation.DbEntityValidationException)
            {
                System.Data.Entity.Validation.DbEntityValidationException entityEx = (ex as System.Data.Entity.Validation.DbEntityValidationException);
                result.Errors.AddRange(entityEx.EntityValidationErrors.SelectMany(s => s.ValidationErrors));

                result.ErrorMessage = string.Join("\n",
                entityEx.EntityValidationErrors
                    .SelectMany(s => s.ValidationErrors)
                    .Select(s => s.ErrorMessage).ToArray());
                return result;
            }

            if (ex is System.Data.Entity.Infrastructure.DbUpdateException)
            {
                Exception exception = FindInnerException(ex);
                if (exception is System.Data.SqlClient.SqlException)
                {
                    System.Data.SqlClient.SqlException sqlException = (exception as System.Data.SqlClient.SqlException);
                    result.ErrorMessage = TranslateSqlException(sqlException.Number, sqlException.Message);
                }
                return result;
            }

            result.ErrorMessage = ex.Message;
            return result;

        }
        private async Task<ActionResult> HandleExceptionAsync(Exception ex, ActionResult result)
        {
            result.Success = false;
            if (ex is System.Data.Entity.Validation.DbEntityValidationException)
            {
                System.Data.Entity.Validation.DbEntityValidationException entityEx = (ex as System.Data.Entity.Validation.DbEntityValidationException);
                result.Errors.AddRange(entityEx.EntityValidationErrors.SelectMany(s => s.ValidationErrors));

                result.ErrorMessage = string.Join("\n",
                entityEx.EntityValidationErrors
                    .SelectMany(s => s.ValidationErrors)
                    .Select(s => s.ErrorMessage).ToArray());
                return result;
            }

            if (ex is System.Data.Entity.Infrastructure.DbUpdateException)
            {
                Exception exception = await FindInnerExceptionAsync(ex);
                if (exception is System.Data.SqlClient.SqlException)
                {
                    System.Data.SqlClient.SqlException sqlException = (exception as System.Data.SqlClient.SqlException);
                    result.ErrorMessage = TranslateSqlException(sqlException.Number, sqlException.Message);
                }
                return result;
            }

            result.ErrorMessage = ex.Message;
            return result;

        }


        protected Exception FindInnerException(Exception ex)
        {
            if (ex.InnerException == null)
                return ex;
            return FindInnerException(ex.InnerException);
        }
        protected async Task<Exception> FindInnerExceptionAsync(Exception ex)
        {
            if (ex.InnerException == null)
                return ex;
            return await FindInnerExceptionAsync(ex.InnerException);
        }

      

        public async Task<ActionResult> BulkinsertAsync(TEntity instance, int count, int commitCount)
        {
            ActionResult result = new ActionResult(true, string.Empty);
            try
            {
                EntityCollection.Add(instance);

                if (count % commitCount == 0)
                {

                    result = await SaveAsync();
                   
                    return result;
                }
                return result;
            }
            catch (Exception ex)
            {

                return await HandleExceptionAsync(ex, result);
            }
        }

        public void Dispose()
        {
            if (_context != null)
                _context.Dispose();
        }


    }
}
