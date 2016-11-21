/*
Copyright 2016 James Craig

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

using Canister.Interfaces;
using SerialBox.BaseClasses;
using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;

namespace SerialBox.Default
{
    /// <summary>
    /// JSON Serializer
    /// </summary>
    /// <seealso cref="SerializerBase{String}"/>
    public class JSONSerializer : SerializerBase<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JSONSerializer"/> class.
        /// </summary>
        /// <param name="bootstrapper">The bootstrapper.</param>
        public JSONSerializer(IBootstrapper bootstrapper)
            : base(bootstrapper)
        {
        }

        /// <summary>
        /// Content type (MIME type)
        /// </summary>
        public override string ContentType => "application/json";

        /// <summary>
        /// File type
        /// </summary>
        public override string FileType => ".json";

        /// <summary>
        /// Name
        /// </summary>
        public override string Name => "JSON";

        /// <summary>
        /// JSONP regex filter
        /// </summary>
        private static Regex JsonPRegex = new Regex(@"[^\(]+\(([^\)]*)\);", RegexOptions.IgnoreCase);

        /// <summary>
        /// Deserializes the data
        /// </summary>
        /// <param name="objectType">Object type</param>
        /// <param name="data">Data to deserialize</param>
        /// <returns>The deserialized data</returns>
        public override object Deserialize(Type objectType, string data)
        {
            if (string.IsNullOrEmpty(data) || objectType == null)
                return null;
            data = JsonPRegex.Replace(data, "$1");
            using (MemoryStream Stream = new MemoryStream(Encoding.UTF8.GetBytes(data)))
            {
                var Serializer = new DataContractJsonSerializer(objectType);
                return Serializer.ReadObject(Stream);
            }
        }

        /// <summary>
        /// Serializes the object
        /// </summary>
        /// <param name="objectType">Object type</param>
        /// <param name="data">Data to serialize</param>
        /// <returns>The serialized data</returns>
        public override string Serialize(Type objectType, object data)
        {
            if (data == null || objectType == null)
                return null;
            string ReturnValue = "";
            using (MemoryStream Stream = new MemoryStream())
            {
                var Serializer = new DataContractJsonSerializer(data.GetType());
                Serializer.WriteObject(Stream, data);
                Stream.Flush();
                var ResultingArray = Stream.ToArray();
                ReturnValue = Encoding.UTF8.GetString(ResultingArray, 0, ResultingArray.Length);
            }
            return ReturnValue;
        }
    }
}