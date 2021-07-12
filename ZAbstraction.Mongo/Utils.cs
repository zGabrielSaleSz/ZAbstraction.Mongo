using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZAbstraction.Mongo.Attributes;

namespace ZAbstraction.Mongo
{
    internal static class Utils
    {
        internal static string GetCollectionName(Type type)
        {
            System.Attribute[] attributes = System.Attribute.GetCustomAttributes(type);
            foreach (System.Attribute attribute in attributes)
            {
                if (attribute is ZMongoCollection)
                {
                    return ((ZMongoCollection)attribute).collectionName;
                }
            }
            throw new Exception($"ZMongoCollection implementation not found in class '{type.Name}'");
        }
    }
}
