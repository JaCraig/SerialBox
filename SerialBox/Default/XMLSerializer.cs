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

using SerialBox.BaseClasses;
using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace SerialBox.Default
{
    /// <summary>
    /// XML serializer
    /// </summary>
    public class XMLSerializer : SerializerBase<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XMLSerializer"/> class.
        /// </summary>
        public XMLSerializer()
        {
        }

        /// <summary>
        /// Content type (MIME type)
        /// </summary>
        public override string ContentType => "text/xml";

        /// <summary>
        /// File type
        /// </summary>
        public override string FileType => ".xml";

        /// <summary>
        /// Name
        /// </summary>
        public override string Name => "XML";

        /// <summary>
        /// Deserializes the data
        /// </summary>
        /// <param name="objectType">Object type</param>
        /// <param name="data">Data to deserialize</param>
        /// <returns>The deserialized data</returns>
        public override object? Deserialize(Type objectType, string data)
        {
            if (string.IsNullOrEmpty(data) || objectType == null)
                return null;
            using var Stream = new MemoryStream(Encoding.UTF8.GetBytes(data));
            var Serializer = new XmlSerializer(objectType);
            return Serializer.Deserialize(Stream);
        }

        /// <summary>
        /// Serializes the object
        /// </summary>
        /// <param name="objectType">Object type</param>
        /// <param name="data">Data to serialize</param>
        /// <returns>The serialized data</returns>
        public override string? Serialize(Type objectType, object data)
        {
            if (data == null || objectType == null)
                return null;
            using var Stream = new MemoryStream();
            var Serializer = new XmlSerializer(objectType);
            Serializer.Serialize(Stream, data);
            Stream.Flush();
            var ResultingArray = Stream.ToArray();
            return new UTF8Encoding(false).GetString(ResultingArray, 0, ResultingArray.Length);
        }
    }
}