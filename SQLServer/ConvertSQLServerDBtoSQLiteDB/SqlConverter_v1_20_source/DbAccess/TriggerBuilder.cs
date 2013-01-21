using System;
using System.Collections.Generic;
using System.Text;

namespace DbAccess
{
    
    public static class TriggerBuilder
    {
        public static IList<TriggerSchema> GetForeignKeyTriggers(TableSchema dt)
        {
            IList<TriggerSchema> result = new List<TriggerSchema>();

            foreach (ForeignKeySchema fks in dt.ForeignKeys)
            {
                StringBuilder sb = new StringBuilder();
                result.Add(GenerateInsertTrigger(fks));
                result.Add(GenerateUpdateTrigger(fks));
                result.Add(GenerateDeleteTrigger(fks));
            }
            return result;
        }

        private static string MakeTriggerName(ForeignKeySchema fks, string prefix)
        {
            return prefix + "_"+fks.TableName + "_" + fks.ColumnName + "_" + fks.ForeignTableName + "_" + fks.ForeignColumnName;
        }

        public static TriggerSchema GenerateInsertTrigger(ForeignKeySchema fks)
        {
            TriggerSchema trigger = new TriggerSchema();
            trigger.Name = MakeTriggerName(fks, "fki");
            trigger.Type = TriggerType.Before;
            trigger.Event = TriggerEvent.Insert;
            trigger.Table = fks.TableName;

            string nullString = "";
            if (fks.IsNullable)
            { 
                nullString = " NEW." + fks.ColumnName + " IS NOT NULL AND";
            }
             
            trigger.Body = "SELECT RAISE(ROLLBACK, 'insert on table " + fks.TableName +
                          " violates foreign key constraint " + trigger.Name + "')" +
                          " WHERE" + nullString + " (SELECT " + fks.ForeignColumnName +
                          " FROM " + fks.ForeignTableName + " WHERE " + fks.ForeignColumnName + " = NEW." +
                          fks.ColumnName +
                          ") IS NULL; " ;
            return trigger;
        }

        public static TriggerSchema GenerateUpdateTrigger(ForeignKeySchema fks)
        {
            TriggerSchema trigger = new TriggerSchema();
            trigger.Name = MakeTriggerName(fks, "fku");
            trigger.Type = TriggerType.Before;
            trigger.Event = TriggerEvent.Update;
            trigger.Table = fks.TableName;

            string triggerName = trigger.Name;
            string nullString = "";
            if (fks.IsNullable)
            {
                nullString = " NEW." + fks.ColumnName + " IS NOT NULL AND";
            }

            trigger.Body = "SELECT RAISE(ROLLBACK, 'update on table " + fks.TableName +
                                  " violates foreign key constraint " + triggerName + "')" +
                                  " WHERE" + nullString + " (SELECT " + fks.ForeignColumnName +
                                  " FROM " + fks.ForeignTableName + " WHERE " + fks.ForeignColumnName + " = NEW." +
                                  fks.ColumnName +
                                  ") IS NULL; ";

            return trigger;
        }

        public static TriggerSchema GenerateDeleteTrigger(ForeignKeySchema fks)
        {
            TriggerSchema trigger = new TriggerSchema();
            trigger.Name = MakeTriggerName(fks, "fkd");
            trigger.Type = TriggerType.Before;
            trigger.Event = TriggerEvent.Delete;
            trigger.Table = fks.ForeignTableName;

            string triggerName = trigger.Name;
            
            if (!fks.CascadeOnDelete)
            {
                trigger.Body = "SELECT RAISE(ROLLBACK, 'delete on table " + fks.ForeignTableName +
                                      " violates foreign key constraint " + triggerName + "')" +
                                      " WHERE (SELECT " + fks.ColumnName +
                                      " FROM " + fks.TableName + " WHERE " + fks.ColumnName + " = OLD." +
                                      fks.ForeignColumnName +
                                      ") IS NOT NULL; ";
            }
            else
            {
                trigger.Body = "DELETE FROM [" + fks.TableName + "] WHERE " + fks.ColumnName + " = OLD." +
                                      fks.ForeignColumnName + "; ";
                              
            }
            return trigger;
        }
    }
}
