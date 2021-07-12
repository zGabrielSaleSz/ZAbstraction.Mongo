using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
namespace ZAbstraction.Mongo
{
    public class ZMongoContext
    {
        internal IMongoDatabase Database { get; }
        internal MongoClient MongoClient { get; }
        public ZMongoContext(string connectinString, string database, string entitiesNamespace)
        {
            try
            {
                SetDefaultConvention(entitiesNamespace);
                MongoClient = new MongoClient(connectinString);
                Database = MongoClient.GetDatabase(database);
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível se conectar com o servidor.", ex);
            }
        }

        internal IMongoCollection<ZMongoBase> GetColection(string name)
        {
            return Database.GetCollection<ZMongoBase>(name);
        }

        public IMongoCollection<T> GetCollection<T>() where T : ZMongoBase
        {
            Type type = typeof(T);
            string collectionName = Utils.GetCollectionName(type);
            return Database.GetCollection<T>(collectionName);
        }

        private Func<Type, bool> HandleEntitiesNamespace(string namespaceArray)
        {
            string name = namespaceArray;
            bool startsWith = false;
            bool endsWith = false;
            if (name.StartsWith("*"))
            {
                startsWith = true;
                name = name.TrimStart('*');
            }
            if (name.EndsWith("*"))
            {
                endsWith = true;
                name = name.TrimEnd('*');
            }
            if (startsWith && endsWith)
                return a => a.FullName.Contains(name);
            if (startsWith)
                return a => a.FullName.StartsWith(name);
            if (endsWith)
                return a => a.FullName.EndsWith(name);
            return a => a.FullName == name;
        }

        private void SetDefaultConvention(string entitiesNamespace)
        {
            var pack = new ConventionPack();
            pack.Add(new NamedExtraElementsMemberConvention(new string[] { "_t", "Collection" }));
            pack.Add(new IgnoreExtraElementsConvention(true));
            ConventionRegistry.Register("zMongo Conventions", pack, HandleEntitiesNamespace(entitiesNamespace));
        }
    }
}
