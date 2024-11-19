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

using System;

namespace SerialBox.Interfaces
{
    /// <summary>
    /// Serializer interface
    /// </summary>
    public interface ISerializer
    {
        /// <summary>
        /// Content type associated with this serializer (MIME type)
        /// </summary>
        string ContentType { get; }

        /// <summary>
        /// File ending associated with the serializer
        /// </summary>
        string FileType { get; }

        /// <summary>
        /// Name of the serializer
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Return type for the serialized data
        /// </summary>
        Type ReturnType { get; }
    }

    /// <summary>
    /// Serializer interface
    /// </summary>
    /// <typeparam name="T">Object type returned</typeparam>
    public interface ISerializer<T> : ISerializer
    {
        /// <summary>
        /// Deserializes the data
        /// </summary>
        /// <param name="objectType">Object type</param>
        /// <param name="data">Data to deserialize</param>
        /// <returns>The deserialized data</returns>
        object? Deserialize(Type objectType, T data);

        /// <summary>
        /// Serializes the object
        /// </summary>
        /// <param name="objectType">Object type</param>
        /// <param name="data">Data to serialize</param>
        /// <returns>The serialized data</returns>
        T? Serialize(Type objectType, object data);
    }
}