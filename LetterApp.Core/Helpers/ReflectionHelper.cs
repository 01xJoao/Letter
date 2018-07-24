using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace LetterApp.Core.Helpers
{
    public static class ReflectionHelper
    {
        //Used to check if viewcontroller is of type XViewController<> or XTabBarViewController in case of iOS or activity is of type XActivity for Android
        public static bool IsSubclassOfRawGeneric(this Type toCheck, Type generic)
        {
            while (toCheck != null && toCheck != typeof(object))
            {
                var cur = toCheck.GetTypeInfo().IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;

                if (generic == cur)
                    return true;

                toCheck = toCheck.GetTypeInfo().BaseType;
            }
            return false;
        }

        //Used to check if a viewcontroller or activity is associated with a viewmodel, and return all viewmodels types
        public static Type GetBaseGenericType(Type child)
        {
            if (child.GetTypeInfo().BaseType == null)
                return null;

            var generics = child.GetTypeInfo().BaseType.GenericTypeArguments;
            if (generics.Length == 0) 
                return null;
            
            return generics[0];
        }

        public static Type ConvertNullToEmpty<Type>(Type model)
        {
            foreach (var propertyInfo in model.GetType().GetProperties())
            {
                if (propertyInfo.PropertyType == typeof(string))
                {
                    if (propertyInfo.GetValue(model, null) == null)
                    {
                        propertyInfo.SetValue(model, string.Empty, null);
                    }
                }
            }
            return model;
        }

        public static bool HasEmptyOrNullValues(object myObject)
        {   
            foreach (PropertyInfo pi in myObject.GetType().GetProperties())
            {
                if (pi.PropertyType == typeof(string))
                {
                    string value = (string)pi.GetValue(myObject);
                    if (string.IsNullOrEmpty(value))
                        return true;
                }
            }
            return false;
        }

        public static bool IsNull(this object T)
        {
            return T == null;
        } 

        public static List<List<T>> SeparateInLists<T>(List<T> source, string orderBy)
        {
            PropertyInfo property = typeof(T).GetProperty(orderBy);

            if (property == null)
                return null;

            return source
                .GroupBy(s => property)
                .OrderBy(g => g.Key)
                .Select(g => g.ToList())
                .ToList();
        }
    }
}