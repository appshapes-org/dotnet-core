using System.Collections.Generic;
using System.Linq;
using AppShapes.Core.Testing.Core;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace AppShapes.Core.Testing.Infrastructure
{
    public class ModelHelper
    {
        public ModelHelper(IModel model)
        {
            Model = model;
        }

        /// <summary>
        ///     Get index for specified entity and property.
        /// </summary>
        /// <typeparam name="T">entity</typeparam>
        /// <param name="propertyNames">property</param>
        /// <returns></returns>
        public Index GetIndex<T>(params string[] propertyNames)
        {
            EntityType entityType = Assertions.Contains(typeof(T).FullName, (IDictionary<string, EntityType>) ReflectionHelper.GetField(Model, "_entityTypes"));
            IDictionary<IReadOnlyList<IProperty>, Index> indexes = (IDictionary<IReadOnlyList<IProperty>, Index>) ReflectionHelper.GetField(entityType, "_indexes");
            KeyValuePair<IReadOnlyList<IProperty>, Index> index = indexes.FirstOrDefault(x => x.Key.Select(y => y.Name).SequenceEqual(propertyNames));
            Assertions.NotNull(index.Value);
            return index.Value;
        }

        public Property GetProperty<T>(string name)
        {
            EntityType entityTypes = Assertions.Contains(typeof(T).FullName, (IDictionary<string, EntityType>) ReflectionHelper.GetField(Model, "_entityTypes"));
            IDictionary<string, Property> properties = (IDictionary<string, Property>) ReflectionHelper.GetField(entityTypes, "_properties");
            return Assertions.Contains(name, properties);
        }

        private IModel Model { get; }
    }
}