using System;
using System.Collections.Generic;
using System.Text;

namespace DbAccess
{
    /// <summary>
    /// Contains the entire database schema
    /// </summary>
    public class DatabaseSchema
    {
        public List<TableSchema> Tables = new List<TableSchema>();
        public List<ViewSchema> Views = new List<ViewSchema>();
    }
}
