﻿using System;
using System.Data.Common;
using System.Data.Entity.Infrastructure.Interception;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using Sample.Core.Logging;

namespace Sample.Core.Configuration
{
    public class SchoolInterceptorTransientErrors : DbCommandInterceptor
    {
        private readonly ILogger _logger = new Logger();
        private int _counter;

        public override void ReaderExecuting(DbCommand command,
            DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
            bool throwTransientErrors = false;
            if (command.Parameters.Count > 0 && command.Parameters[0].Value.ToString() == "Throw")
            {
                throwTransientErrors = true;
                command.Parameters[0].Value = "an";
                command.Parameters[1].Value = "an";
            }

            if (throwTransientErrors && _counter < 4)
            {
                _logger.Information("Returning transient error for command: {0}", command.CommandText);
                _counter++;
                interceptionContext.Exception = CreateDummySqlException();
            }
        }

        private SqlException CreateDummySqlException()
        {
            // The instance of SQL Server you attempted to connect to does not support encryption
            int sqlErrorNumber = 20;

            ConstructorInfo sqlErrorCtor =
                typeof (SqlError).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic)
                    .Where(c => c.GetParameters().Count() == 7)
                    .Single();
            object sqlError = sqlErrorCtor.Invoke(new object[] {sqlErrorNumber, (byte) 0, (byte) 0, "", "", "", 1});

            object errorCollection = Activator.CreateInstance(typeof (SqlErrorCollection), true);
            MethodInfo addMethod = typeof (SqlErrorCollection).GetMethod("Add",
                BindingFlags.Instance | BindingFlags.NonPublic);
            addMethod.Invoke(errorCollection, new[] {sqlError});

            ConstructorInfo sqlExceptionCtor =
                typeof (SqlException).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic)
                    .Where(c => c.GetParameters().Count() == 4)
                    .Single();
            var sqlException =
                (SqlException) sqlExceptionCtor.Invoke(new[] {"Dummy", errorCollection, null, Guid.NewGuid()});

            return sqlException;
        }
    }
}