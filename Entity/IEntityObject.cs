using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp.Entity
{
    /// <summary>
    /// Defines metadata mapping contracts for database entity persistence in the lightweight ORM engine.
    /// </summary>
    public interface IEntityObject
    {
        /// <summary>
        /// Gets the corresponding database table name mapped to this entity.
        /// </summary>
        /// <returns>Database table name.</returns>
        string GetTableName();

        /// <summary>
        /// Gets the primary key column name for this entity.
        /// </summary>
        /// <returns>Primary key column name.</returns>
        string GetKeyName();
    }
}
