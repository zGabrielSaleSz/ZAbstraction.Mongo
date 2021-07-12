using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZAbstraction.Mongo.Attributes
{
    [System.AttributeUsage(System.AttributeTargets.Class |
                       System.AttributeTargets.Struct)
]
    public class ZMongoCollection : System.Attribute
    {
        public string collectionName;

        public ZMongoCollection(string collectionName)
        {
            this.collectionName = collectionName;
        }
    }
}
