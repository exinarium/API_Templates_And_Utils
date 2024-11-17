using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Full_Application_Blazor.Utils.Helpers.Contants;
using MongoDB.Bson.Serialization.Attributes;

namespace Full_Application_Blazor.Utils.Helpers.Utilities
{
    public static class ExpandoObjects<T> where T : new()
    {
        public static ExpandoObject FromObject(T value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(ExpandoObjectConstants.OBJECT_VALUE_NULL_ERROR);
            }

            var expando = new ExpandoObject();
            var dictionary = (IDictionary<string, object>)expando;

            foreach (var property in value.GetType().GetProperties())
            {
                var bsonElements = property.GetCustomAttributes(typeof(BsonElementAttribute), true) as BsonElementAttribute[];

                if (bsonElements.Length > 0)
                {
                    dictionary.Add(bsonElements[0].ElementName, property.GetValue(value));
                }
                else
                {
                    dictionary.Add(property.Name, property.GetValue(value));
                }
            }

            return expando;
        }

        public static T ToObject(ExpandoObject value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(ExpandoObjectConstants.EXPANDO_VALUE_NULL_ERROR);
            }

            var expandoProperties = value.Select(kvp => kvp).ToArray();
            var returnObject = new T();

            foreach (var property in returnObject.GetType().GetProperties())
            {
                var bsonElements = property.GetCustomAttributes(typeof(BsonElementAttribute), true) as BsonElementAttribute[];
                object expandoValue;

                if (bsonElements.Length > 0)
                {
                    expandoValue = expandoProperties.Where(x => x.Key.Equals(bsonElements[0].ElementName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault().Value;
                }
                else
                {
                    expandoValue = expandoProperties.Where(x => x.Key.Equals(property.Name, StringComparison.OrdinalIgnoreCase) || (x.Key == "_id" && property.Name == "Id")).FirstOrDefault().Value;
                }

                if (expandoValue != null)
                {
                    property.SetValue(returnObject, expandoValue);
                }
            }

            return returnObject;
        }
    }
}

