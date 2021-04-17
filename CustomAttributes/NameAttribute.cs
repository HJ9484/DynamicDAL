using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicDAL.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class NameAttribute : Attribute
    {
        public string EntityName { get; set; }
        public string PropertyName { get; set; }
        public string TableName { get; set; }
        public NameAttribute()
        {

        }
    }
}

