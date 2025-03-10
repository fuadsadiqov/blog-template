﻿using System.Linq.Expressions;
using GP.Core.Configurations.Entity;
using GP.Core.Enums.Enitity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace GP.Data
{
    public static class
        ModelBuilderExtensions
    {
        public static void ApplyGlobalFilters<TInterface>(this ModelBuilder modelBuilder, Expression<Func<TInterface, bool>> expression)
        {
            var entities = modelBuilder.Model
                .GetEntityTypes()
                .Where(e => e.ClrType.GetInterface(typeof(TInterface).Name) != null)
                .Select(e => e.ClrType);
            foreach (var entity in entities)
            {
                var newParam = Expression.Parameter(entity);
                var newbody = ReplacingExpressionVisitor.Replace(expression.Parameters.Single(), newParam, expression.Body);
                modelBuilder.Entity(entity).HasQueryFilter(Expression.Lambda(newbody, newParam));
            }
        }

        public static ModelBuilder SetStatusQueryFilter(this ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyGlobalFilters<IEntity>(c => c.Status != RecordStatusEnum.Deleted);

            return modelBuilder;
        }
    }
}
