using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Microsoft.EntityFrameworkCore;


namespace Disunity.Store.Extensions {

    public static class DbContextExtensions {

        public static IEnumerable<Type> DbSetTypes(this DbContext context) {
            return context.GetType()
                          .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                          .Select(p => p.PropertyType)
                          .Where(t => t.IsGenericType)
                          .Where(t => typeof(DbSet<>).IsAssignableFrom(t.GetGenericTypeDefinition()));
        }

        public static IEnumerable<Type> DbEntityTypes(this DbContext context) {
            return context.DbSetTypes().Select(t => t.GetGenericArguments()[0]);
        }

    }

}