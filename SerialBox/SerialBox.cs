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
            serializers ??= [];
            Serializers = serializers.Where(x => !x.GetType().Namespace?.StartsWith("SERIALBOX", StringComparison.OrdinalIgnoreCase) ?? false)
                                          .ToDictionary(x => x.ContentType);
            foreach (ISerializer? Serializer in serializers.Where(x => x.GetType()
                                                                       .Namespace
                                                                       ?.StartsWith("SERIALBOX", StringComparison.OrdinalIgnoreCase) ?? false))
            {
                if (!Serializers.ContainsKey(Serializer.ContentType))
                    Serializers.Add(Serializer.ContentType, Serializer);
            }
        }

        /// <summary>
        /// Serializers
        /// </summary>
        protected IDictionary<string, ISerializer> Serializers { get; }

        /// <summary>
        /// Determines if the system can serialize/deserialize the content type
        /// </summary>
        /// <param name="contentType">Content type</param>
        /// <returns>True if it can, false otherwise</returns>
        public bool CanSerialize(string contentType) => !string.IsNullOrEmpty(contentType) && Serializers.ContainsKey(contentType.Split(';')[0]);

        /// <summary>
        /// Deserializes the data to an object
        /// </summary>
        /// <typeparam name="T">Data type</typeparam>
        /// <typeparam name="TR">Return object type</typeparam>
        /// <param name="data">Data to deserialize</param>
        /// <param name="contentType">Content type (MIME type)</param>
        /// <returns>The deserialized object</returns>
        public TR? Deserialize<T, TR>(T data, SerializationType? contentType = null)
        {
            contentType ??= SerializationType.JSON;
            return (TR)Deserialize(data, typeof(TR), contentType)!;
        }

        /// <summary>
        /// Deserializes the data to an object
        /// </summary>
        /// <typeparam name="T">Type of the data</typeparam>
        /// <param name="data">Data to deserialize</param>
        /// <param name="objectType">Object type requested</param>
        /// <param name="contentType">Content type (MIME type)</param>
        /// <returns>The deserialized object</returns>
        public object? Deserialize<T>(T data, Type objectType, SerializationType? contentType = null)
        {
            contentType ??= SerializationType.JSON;
            if (string.IsNullOrEmpty(contentType.ToString()) || objectType == null)
                return null;
            contentType = (SerializationType)contentType.ToString().Split(';')[0];
            return !Serializers.ContainsKey(contentType) || Serializers[contentType].ReturnType != typeof(T)
                ? null
                : ((ISerializer<T>)Serializers[contentType]).Deserialize(objectType, data);
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
        /// <typeparam name="TR">Return type</typeparam>
        /// <returns>The serialized object as a string</returns>
        public TR? Serialize<T, TR>(T data, SerializationType? contentType = null)
        {
            if (data == null)
                return default!;
            contentType ??= SerializationType.JSON;
            return Serialize<TR>(data, typeof(T), contentType);
        }

        /// <summary>
        /// Serializes the object based on the content type specified
        /// </summary>
        /// <typeparam name="T">Return type</typeparam>
        /// <param name="data">Object to serialize</param>
        /// <param name="objectType">Object type</param>
        /// <param name="contentType">Content type (MIME type)</param>
        /// <returns>The serialized object as a string</returns>
        public T? Serialize<T>(object data, Type? objectType, SerializationType? contentType = null)
        {
            contentType ??= SerializationType.JSON;
            if (string.IsNullOrEmpty(contentType) || objectType == null)
                return default!;
            contentType = (SerializationType)contentType.ToString().Split(';')[0];
            return !Serializers.ContainsKey(contentType) || Serializers[contentType].ReturnType != typeof(T)
                ? default
                : ((ISerializer<T>)Serializers[contentType]).Serialize(objectType, data);
        }

        /// <summary>
        /// Outputs information about the serializers the system is using
        /// </summary>
        /// <returns>String version of the object</returns>
        public override string ToString()
        {
            var Builder = new StringBuilder();
            _ = Builder.Append("Serializers: ");
            var Separator = "";
            foreach (var Key in Serializers.Keys.Order())
            {
                _ = Builder.AppendFormat("{0}{1}", Separator, Serializers[Key].Name);
                Separator = ",";
            }
            return Builder.ToString();
        }
    }
}