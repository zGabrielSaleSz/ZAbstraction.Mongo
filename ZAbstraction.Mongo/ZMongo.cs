using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZAbstraction.Mongo
{
    public class ZMongo
    {
        public static ZMongoContext Instance { get; private set; }
        public static void Configurate(string connection, string database, string entitiesNamespace)
        {
            Instance = new ZMongoContext(connection, database, entitiesNamespace);
        }
    }
}
