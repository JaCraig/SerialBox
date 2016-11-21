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

using SerialBox.Enums;
using SerialBox.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SerialBox
{
    /// <summary>
    /// SerialBox's main class
    /// </summary>
    public class SerialBox
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="serializers">The serializers.</param>
        public SerialBox(IEnumerable<ISerializer> serializers)
        {
            serializers = serializers ?? new List<ISerializer>();
            Serializers = serializers.Where(x => !x.GetType().Namespace.StartsWith("SERIALBOX", StringComparison.OrdinalIgnoreCase))
                                          .ToDictionary(x => x.ContentType);
            foreach (ISerializer Serializer in serializers.Where(x => x.GetType()
                                                                       .Namespace
                                                                       .StartsWith("SERIALBOX", StringComparison.OrdinalIgnoreCase)))
            {
                if (!Serializers.ContainsKey(Serializer.ContentType))
                    Serializers.Add(Serializer.ContentType, Serializer);
            }
        }

        /// <summary>
        /// Serializers
        /// </summary>
        protected IDictionary<string, ISerializer> Serializers { get; private set; }

        /// <summary>
        /// Determines if the system can serialize/deserialize the content type
        /// </summary>
        /// <param name="contentType">Content type</param>
        /// <returns>True if it can, false otherwise</returns>
        public bool CanSerialize(string contentType)
        {
            if (string.IsNullOrEmpty(contentType))
                return false;
            return Serializers.ContainsKey(contentType.Split(';')[0]);
        }

        /// <summary>
        /// Deserializes the data to an object
        /// </summary>
        /// <typeparam name="T">Data type</typeparam>
        /// <typeparam name="R">Return object type</typeparam>
        /// <param name="data">Data to deserialize</param>
        /// <param name="contentType">Content type (MIME type)</param>
        /// <returns>The deserialized object</returns>
        public R Deserialize<T, R>(T data, SerializationType contentType = null)
        {
            contentType = contentType ?? SerializationType.JSON;
            return (R)Deserialize<T>(data, typeof(R), contentType);
        }

        /// <summary>
        /// Deserializes the data to an object
        /// </summary>
        /// <typeparam name="T">Type of the data</typeparam>
        /// <param name="data">Data to deserialize</param>
        /// <param name="objectType">Object type requested</param>
        /// <param name="contentType">Content type (MIME type)</param>
        /// <returns>The deserialized object</returns>
        public object Deserialize<T>(T data, Type objectType, SerializationType contentType = null)
        {
            contentType = contentType ?? SerializationType.JSON;
            if (string.IsNullOrEmpty(contentType.ToString()) || objectType == null)
                return null;
            contentType = (SerializationType)contentType.ToString().Split(';')[0];
            if (!Serializers.ContainsKey(contentType) || Serializers[contentType].ReturnType != typeof(T))
                return null;
            return ((ISerializer<T>)Serializers[contentType]).Deserialize(objectType, data);
        }

        /// <summary>
        /// File type to content type
        /// </summary>
        /// <param name="fileType">File type</param>
        /// <returns>Content type</returns>
        public string FileTypeToContentType(string fileType)
        {
            return Serializers.FirstOrDefault(x => string.Equals(x.Value.FileType, fileType, StringComparison.OrdinalIgnoreCase))
                              .Value
                              ?.ContentType ?? "";
        }

        /// <summary>
        /// Serializes the object based on the content type specified
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="data">Object to serialize</param>
        /// <param name="contentType">Content type (MIME type)</param>
        /// <typeparam name="R">Return type</typeparam>
        /// <returns>The serialized object as a string</returns>
        public R Serialize<T, R>(T data, SerializationType contentType = null)
        {
            contentType = contentType ?? SerializationType.JSON;
            return Serialize<R>(data, typeof(T), contentType);
        }

        /// <summary>
        /// Serializes the object based on the content type specified
        /// </summary>
        /// <param name="objectType">Object type</param>
        /// <param name="data">Object to serialize</param>
        /// <param name="contentType">Content type (MIME type)</param>
        /// <typeparam name="T">Return type</typeparam>
        /// <returns>The serialized object as a string</returns>
        public T Serialize<T>(object data, Type objectType, SerializationType contentType = null)
        {
            contentType = contentType ?? SerializationType.JSON;
            if (string.IsNullOrEmpty(contentType) || objectType == null)
                return default(T);
            contentType = (SerializationType)contentType.ToString().Split(';')[0];
            if (!Serializers.ContainsKey(contentType) || Serializers[contentType].ReturnType != typeof(T))
                return default(T);
            return ((ISerializer<T>)Serializers[contentType]).Serialize(objectType, data);
        }

        /// <summary>
        /// Outputs information about the serializers the system is using
        /// </summary>
        /// <returns>String version of the object</returns>
        public override string ToString()
        {
            var Builder = new StringBuilder();
            Builder.Append("Serializers: ");
            string Separator = "";
            foreach (string key in Serializers.Keys.OrderBy(x => x))
            {
                Builder.AppendFormat("{0}{1}", Separator, Serializers[key].Name);
                Separator = ",";
            }
            return Builder.ToString();
        }
    }
}