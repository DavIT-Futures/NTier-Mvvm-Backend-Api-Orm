using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

namespace TestApp.Entity
{
    /// <summary>
    /// Base class for domain entity objects providing reflection mapping, WCF serialization known types, and validation.
    /// </summary>
    [DataContract]
    [KnownType("GetKnownTypes")]
    public class EntityObject : IDataErrorInfo, IEntityObject
    {
        /// <summary>
        /// Gets or sets the primary key identifier.
        /// </summary>
        public int Id { get; set; }

        [DataMember(Name = "Name")]
        public string Name { get; set; }

        [DataMember(Name = "Description")]
        public string Description { get; set; }

        public EntityObject()
        {

        }

        public virtual string Error
        {
            get
            {
                return "Error";
            }
        }

        public virtual string this[string columnName]
        {
            get
            {
                string result = string.Empty;
                switch (columnName)
                {
                    case "Id":
                        if (Id <= 0) result = "Id must be greater than 0!"; break;
                    case "Name":
                        if (string.IsNullOrEmpty(Name)) result = "Name is required!"; break;
                    case "Description":
                        if (string.IsNullOrEmpty(Description)) result = "Description is required!"; break;
                };
                return result;
            }
        }

        public override string ToString()
        {
            return String.Format("{0}, {1}, {2}", Id, Name, Description);
        }

        public virtual string GetTableName()
        {
            throw new NotImplementedException("GetTableName() not implemented");
        }

        public virtual string GetKeyName()
        {
            throw new NotImplementedException("GetKeyName() not implemented");
        }

        private static IEnumerable<Type> GetKnownTypes()
        {
            IEnumerable<Type> result = Assembly.GetExecutingAssembly().GetTypes().Where(w => typeof(EntityObject).IsAssignableFrom(w)).ToList();
            return result;
        }
    }
}
