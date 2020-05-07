using System;
using System.Linq;
using System.Reflection;

namespace AppShapes.Core
{
    public class ReflectionHelper
    {
        public static object GetField(object source, string fieldName)
        {
            FieldInfo info = GetFieldInfo(source, fieldName);
            if (ReferenceEquals(info, null))
                throw new Exception($"Can't find field '{fieldName}' in {source}");
            return info.GetValue(source);
        }

        public static object GetField(Type type, object source, string fieldName)
        {
            FieldInfo info = GetFieldInfo(type, fieldName);
            if (ReferenceEquals(info, null))
                throw new Exception($"Can't find field '{fieldName}' in {type}");
            return info.GetValue(source);
        }

        public static object GetFieldOrDefault(object source, string fieldName)
        {
            return GetFieldInfo(source, fieldName)?.GetValue(source);
        }

        public static object GetFieldOrDefault(Type type, object source, string fieldName)
        {
            return GetFieldInfo(type, fieldName)?.GetValue(source);
        }

        public static object GetProperty(object source, string propertyName)
        {
            return GetPropertyInfo(source, propertyName).GetValue(source, null);
        }

        public static T GetProperty<T>(object source, string propertyName)
        {
            return (T) GetProperty(source, propertyName);
        }

        public static T InvokeConstructor<T>(params object[] arguments)
        {
            return (T) InvokeConstructor(typeof(T), arguments.Select(o => o.GetType()).ToArray(), arguments);
        }

        public static object InvokeConstructor(Type type, Type[] constructorTypesInOrder, object[] constructorArguments)
        {
            return type.GetConstructor(BindingFlags.CreateInstance | BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic, null, constructorTypesInOrder ?? Type.EmptyTypes, null)?.Invoke(constructorArguments);
        }

        public static object InvokeMethod(object source, string methodName, params object[] arguments)
        {
            return InvokeMethod(source.GetType(), source, methodName, arguments);
        }

        public static object InvokeMethod(Type type, object source, string methodName, object[] arguments)
        {
            return type.InvokeMember(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.InvokeMethod, null, source, arguments);
        }

        public static void SetField(object source, string fieldName, object fieldValue)
        {
            FieldInfo info = GetFieldInfo(source, fieldName);
            if (ReferenceEquals(info, null))
                throw new Exception($"Can't find field '{fieldName}' in {source}");
            info.SetValue(source, fieldValue);
        }

        public static void SetField(Type type, object source, string fieldName, object fieldValue)
        {
            FieldInfo info = GetFieldInfo(type, fieldName);
            if (ReferenceEquals(info, null))
                throw new Exception($"Can't find field '{fieldName}' in {type}");
            info.SetValue(source, fieldValue);
        }

        public static void SetProperty(object source, string propertyName, object propertyValue)
        {
            PropertyInfo info = GetPropertyInfo(source, propertyName);
            info.SetValue(source, propertyValue, null);
        }

        protected static FieldInfo GetFieldInfo(object source, string fieldName)
        {
            return GetFieldInfo(source.GetType(), fieldName);
        }

        protected static FieldInfo GetFieldInfo(Type type, string fieldName)
        {
            return type.GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
        }

        protected static PropertyInfo GetPropertyInfo(object source, string propertyName)
        {
            return source.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
        }
    }
}