using DynamicDAL.SQLOperations;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DynamicDAL.Repository.Base
{
    public abstract class ADORepository<TEntity, TKey> : Repository.Base.IRepository<TEntity, TKey>
       where TEntity : class
        where TKey : struct/*, IEquatable<TKey>*/
    {

        protected string tableName;
        protected string propertyName;
        protected string propertyValue;
        private string PropertyNameValue;

        public ADORepository()
        {


        }


        private static List<TEntity> ConvertDataTable<T>(DataTable dt)
        {
            List<TEntity> data = new List<TEntity>();
            foreach (DataRow row in dt.Rows)
            {
                TEntity item = GetItem<TEntity>(row);
                data.Add(item);
            }
            return data;
        }

        private static TEntity GetItem<T>(DataRow dr)
        {
            Type temp = typeof(TEntity);
            TEntity obj = Activator.CreateInstance<TEntity>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)

                        pro.SetValue(obj, dr[column.ColumnName] == DBNull.Value ? null : dr[column.ColumnName], null);
                    else
                        continue;
                }
            }
            return obj;
        }
        public Assembly GetAssemblyByName(string name)
        {
            return AppDomain.CurrentDomain.GetAssemblies().
                   SingleOrDefault(assembly => assembly.GetName().Name == name);
        }
        public List<TEntity> GetList()
        {
            try
            {
                List<TEntity> entities = ConvertDataTable<TEntity>(SQLHelper.ExecuteReaderDataTable("Select * From " + tableName));
                return entities;
            }
            catch (Exception)
            {

                throw;
            }



        }

        public async Task<List<TEntity>> GetListAsync()
        {
            try
            {
                var tabelName = typeof(TEntity).GetCustomAttributes<CustomAttributes.NameAttribute>(true)
                               .Select(ss => ss.TableName).FirstOrDefault();
                List<TEntity> entities = ConvertDataTable<TEntity>(await SQLHelper.ExecuteReaderDataTableAsync("Select * From " + tabelName));
                return entities;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public ActionResult Create(TEntity instance)
        {
            try
            {
                ActionResult result = new ActionResult(true, string.Empty);
                string tabelName = typeof(TEntity).GetCustomAttributes<CustomAttributes.NameAttribute>(true)
                               .Select(ss => ss.TableName).FirstOrDefault();

                var properties = instance.GetType().GetProperties();

                propertyValue = null;
                propertyName = null;
                properties.ToList()
                    .ForEach(f =>
                    {
                        if (Utility.Convertor.IsPrimitive(f.PropertyType) && f.Name != "ID" && !f.Name.Contains("Persian"))
                        {

                            if (f.GetValue(instance, null) == null)
                            {
                                propertyValue = propertyValue + "'" + null + "'" + ",";
                                propertyName = propertyName + f.Name + ",";
                            }
                            else
                            {
                                if (f.PropertyType.Name == "Boolean")
                                {
                                    propertyValue = propertyValue + "'" + "1" + "'" + ",";
                                    propertyName = propertyName + f.Name + ",";
                                }
                                else
                                {
                                    propertyValue = propertyValue + "'" + f.GetValue(instance, null).ToString() + "'" + ",";
                                    propertyName = propertyName + f.Name + ",";
                                }

                            }
                        }


                    });
                propertyValue = propertyValue.Remove(propertyValue.Length - 1);
                propertyName = propertyName.Remove(propertyName.Length - 1);

                result.Success = SQLHelper.ExecuteNonQueryCommand($"INSERT INTO {tabelName} ({propertyName})  VALUES({propertyValue})") > 0;
                return result;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public async Task<ActionResult> CreateAsync(TEntity instance)
        {
            try
            {
                ActionResult result = new ActionResult(true, string.Empty);
                string tabelName = typeof(TEntity).GetCustomAttributes<CustomAttributes.NameAttribute>(true)
                               .Select(ss => ss.TableName).FirstOrDefault();

                var properties = instance.GetType().GetProperties();

                properties.ToList()
                    .ForEach(f =>
                    {
                        if (Utility.Convertor.IsPrimitive(f.PropertyType) && f.Name != "ID" && !f.Name.Contains("Persian"))
                        {

                            if (f.GetValue(instance, null) == null)
                            {
                                propertyValue = propertyValue + "'" + null + "'" + ",";
                                propertyName = propertyName + f.Name + ",";
                            }
                            else
                            {
                                if (f.PropertyType.Name == "Boolean")
                                {
                                    propertyValue = propertyValue + "'" + "1" + "'" + ",";
                                    propertyName = propertyName + f.Name + ",";
                                }
                                else
                                {
                                    propertyValue = propertyValue + "'" + f.GetValue(instance, null).ToString() + "'" + ",";
                                    propertyName = propertyName + f.Name + ",";
                                }

                            }
                        }


                    });
                propertyValue = propertyValue.Remove(propertyValue.Length - 1);
                propertyName = propertyName.Remove(propertyName.Length - 1);

                result.Success = await SQLHelper.ExecuteNonQueryCommandAsync($"INSERT INTO {tabelName} ({propertyName})  VALUES({propertyValue})") > 0;
                return result;
            }
            catch (Exception ex)
            {

                throw;
            }
        }



        public ActionResult Edit(TEntity instance)
        {
            try
            {
                ActionResult result = new ActionResult(true, string.Empty);
                string tabelName = typeof(TEntity).GetCustomAttributes<CustomAttributes.NameAttribute>(true)
                               .Select(ss => ss.TableName).FirstOrDefault();
                PropertyNameValue = null;
                var properties = instance.GetType().GetProperties();

                properties.ToList()
                    .ForEach(f =>
                    {
                        if (Utility.Convertor.IsPrimitive(f.PropertyType) && f.Name != "ID" && !f.Name.Contains("Persian"))
                        {

                            if (f.GetValue(instance, null) == null)
                            {
                                //propertyValue = propertyValue + "'" + null + "'" + ",";
                                //propertyName = propertyName + f.Name + ",";
                                PropertyNameValue = PropertyNameValue + f.Name + "=" + "'" + null + "'" + ",";
                            }
                            else
                            {
                                if (f.PropertyType.Name == "Boolean")
                                {
                                    //propertyValue = propertyValue + "'" + "1" + "'" + ",";
                                    //propertyName = propertyName + f.Name + ",";
                                    PropertyNameValue = PropertyNameValue + f.Name + "=" + "'" + "1" + "'" + ",";
                                }
                                else
                                {
                                    //propertyValue = propertyValue + "'" + f.GetValue(instance, null).ToString() + "'" + ",";
                                    PropertyNameValue = PropertyNameValue + f.Name + "=" + "'" + f.GetValue(instance, null).ToString() + "'" + ",";
                                }

                            }
                        }
                       

                    });

                PropertyNameValue = PropertyNameValue.Remove(PropertyNameValue.Length - 1);
                
                result.Success = SQLHelper.ExecuteNonQueryCommand($"UPDATE {tabelName} SET {PropertyNameValue} WHERE ID = {instance.GetType().GetProperty("ID").GetValue(instance,null)}") > 0;
                return result;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<ActionResult> EditAsync(TEntity instance)
        {
            try
            {
               ActionResult result = new ActionResult(true, string.Empty);
                string tabelName = typeof(TEntity).GetCustomAttributes<CustomAttributes.NameAttribute>(true)
                               .Select(ss => ss.TableName).FirstOrDefault();
                PropertyNameValue = null;
                var properties = instance.GetType().GetProperties();

                properties.ToList()
                    .ForEach(f =>
                    {
                        if (Utility.Convertor.IsPrimitive(f.PropertyType) && f.Name != "ID" && !f.Name.Contains("Persian"))
                        {

                            if (f.GetValue(instance, null) == null)
                            {
                                //propertyValue = propertyValue + "'" + null + "'" + ",";
                                //propertyName = propertyName + f.Name + ",";
                                PropertyNameValue = PropertyNameValue + f.Name + "=" + "'" + null + "'" + ",";
                            }
                            else
                            {
                                if (f.PropertyType.Name == "Boolean")
                                {
                                    //propertyValue = propertyValue + "'" + "1" + "'" + ",";
                                    //propertyName = propertyName + f.Name + ",";
                                    PropertyNameValue = PropertyNameValue + f.Name + "=" + "'" + "1" + "'" + ",";
                                }
                                else
                                {
                                    //propertyValue = propertyValue + "'" + f.GetValue(instance, null).ToString() + "'" + ",";
                                    PropertyNameValue = PropertyNameValue + f.Name + "=" + "'" + f.GetValue(instance, null).ToString() + "'" + ",";
                                }

                            }
                        }


                    });

                PropertyNameValue = PropertyNameValue.Remove(PropertyNameValue.Length - 1);

                result.Success = await SQLHelper.ExecuteNonQueryCommandAsync($"UPDATE {tabelName} SET {PropertyNameValue} WHERE ID = {instance.GetType().GetProperty("ID").GetValue(instance, null)}") > 0;
                return result;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public ActionResult Delete(TKey id)
        {
            try
            {
                ActionResult result = new ActionResult(true, string.Empty);
                string tabelName = typeof(TEntity).GetCustomAttributes<CustomAttributes.NameAttribute>(true)
                               .Select(ss => ss.TableName).FirstOrDefault();
                result.Success = SQLHelper.ExecuteNonQueryCommand($"DELETE FROM {tabelName} WHERE ID = {id}") > 0;
                return result;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public async Task<ActionResult> DeleteAsync(TKey id)
        {
            try
            {
                ActionResult result = new ActionResult(true, string.Empty);
                string tabelName = typeof(TEntity).GetCustomAttributes<CustomAttributes.NameAttribute>(true)
                               .Select(ss => ss.TableName).FirstOrDefault();
                result.Success = await SQLHelper.ExecuteNonQueryCommandAsync($"DELETE FROM {tabelName} WHERE ID = {id}") > 0;
                return result;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public ActionResult Save()
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult> SaveAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult> BulkinsertAsync(TEntity instance, int count, int commitCount)
        {
            throw new NotImplementedException();
        }
    }
}
