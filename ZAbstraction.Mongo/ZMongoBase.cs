using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using System.Runtime.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Attributes;
using ZAbstraction.Mongo.Lib;

namespace ZAbstraction.Mongo
{
    [BsonIgnoreExtraElements]
    [BsonDiscriminator(Required = true, RootClass = true)]
    public abstract class ZMongoBase
    {
        [DataMember(Name = "_id")]
        [BsonId(IdGenerator = typeof(ObjectIdGenerator))]
        public ObjectId id { get; set; }
        /// <summary>
        /// Perform an update with the current data based on ID attribute
        /// </summary>
        /// <returns>Returns true if any documents have been modified </returns>
        public bool Update()
        {
            return ZMongo.Instance.GetColection(GetCollectionName()).ReplaceOne(u => u.id == this.id, this).ModifiedCount > 0;
        }
        /// <summary>
        /// Perform an insertion with the current data
        /// </summary>
        /// <param name="assignId">If true, the id field will be filled with the generated id</param>
        /// <returns>Returns the ID of the new document</returns>
        public ObjectId Insert(bool assignId = false)
        {
            ZMongo.Instance.Database.GetCollection<ZMongoBase>(GetCollectionName()).InsertOne(this);
            var lastInsertedDocumentID = ZMongo.Instance.Database.GetCollection<ZMongoBase>(GetCollectionName()).Find(f => true).Project(p => p.id).SortByDescending(e => e.id).Limit(1).FirstOrDefault();
            if (assignId)
            {
                this.id = lastInsertedDocumentID;
            }
            return lastInsertedDocumentID;
        }
        /// <summary>
        /// Perform an deletion in the document based on ID field
        /// </summary>
        /// <returns>Returns true if any documents have been deleted</returns>
        public bool Delete()
        {
            return ZMongo.Instance.Database.GetCollection<ZMongoBase>(GetCollectionName()).DeleteOne(u => u.id == this.id).DeletedCount > 0;
        }

        private string GetCollectionName()
        {
            return Helpers.GetCollectionName(this.GetType());
        }
    }
}
