using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Dime.Utilities
{
    /// <summary>
    /// Attribute usable by Entity Framework to set the DateTimeKind on the property
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class DateTimeKindAttribute : Attribute
    {
        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        /// <param name="kind">The kind</param>
        public DateTimeKindAttribute(DateTimeKind kind)
        {
            Kind = kind;
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// Gets or sets the kind
        /// </summary>
        public DateTimeKind Kind { get; }

        #endregion Properties

        #region Methods

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        public static void Apply<T>(T entity)
        {
            if (entity == null)
                return;

            IEnumerable<PropertyInfo> properties = entity.GetType().GetProperties()
                .Where(x => x.PropertyType == typeof(DateTime) || x.PropertyType == typeof(DateTime?));

            foreach (PropertyInfo property in properties)
            {
                DateTimeKindAttribute attribute = property.GetCustomAttribute<DateTimeKindAttribute>();
                if (attribute == null)
                    continue;

                DateTime? dateTime = property.PropertyType == typeof(DateTime?)
                    ? (DateTime?)property.GetValue(entity)
                    : (DateTime)property.GetValue(entity);

                if (dateTime == null)
                    continue;

                property.SetValue(entity, DateTime.SpecifyKind(dateTime.Value, attribute.Kind));
            }
        }

        #endregion Methods
    }
}