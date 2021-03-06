﻿/* Copyright 2010-2011 10gen Inc.
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
* http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using NUnit.Framework;

using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.DefaultSerializer;
using MongoDB.Bson.Serialization;

namespace MongoDB.BsonUnitTests.Jira {
    [TestFixture]
    public class CSharp81Tests {
        private class BaseModel {
            [BsonId]
            public ObjectId Id { get; set; }
        }

        private class User : BaseModel {
            public ObjectId FriendId { get; set; }
        }

        [Test]
        public void TestIdMember() {
            var u = new User { Id = ObjectId.Empty, FriendId = ObjectId.Empty };

            var classMap = BsonClassMap.LookupClassMap(typeof(User));
            var idMemberMap = classMap.IdMemberMap;
            Assert.AreEqual("Id", idMemberMap.MemberName);

            var serializer = BsonSerializer.LookupSerializer(typeof(User));
            var idGenerator = BsonSerializer.LookupIdGenerator(typeof(ObjectId));
            serializer.SetDocumentId(u, idGenerator.GenerateId());
            Assert.IsFalse(idGenerator.IsEmpty(u.Id));
            Assert.IsTrue(idGenerator.IsEmpty(u.FriendId));

            var json = u.ToJson();
        }
    }
}
