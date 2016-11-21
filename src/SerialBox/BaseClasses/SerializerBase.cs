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
using SerialBox.Interfaces;
using System;

namespace SerialBox.BaseClasses
{
    /// <summary>
    /// Serializer base class
    /// </summary>
    /// <typeparam name="T">Serialized data type</typeparam>
    public abstract class SerializerBase<T> : ISerializer<T>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="bootstrapper">The bootstrapper.</param>
        protected SerializerBase(IBootstrapper bootstrapper)
        {
            Bootstrapper = bootstrapper;
        }

        /// <summary>
        /// Content type (MIME type)
        /// </summary>
        public abstract string ContentType { get; }

        /// <summary>
        /// Common file type (extension)
        /// </summary>
        public abstract string FileType { get; }

        /// <summary>
        /// Name of the serializer
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Return type
        /// </summary>
        public Type ReturnType => typeof(T);

        /// <summary>
        /// Gets the bootstrapper.
        /// </summary>
        /// <value>The bootstrapper.</value>
        protected IBootstrapper Bootstrapper { get; private set; }

        /// <summary>
        /// Deserializes the data
        /// </summary>
        /// <param name="objectType">Object type</param>
        /// <param name="data">Data to deserialize</param>
        /// <returns>The deserialized data</returns>
        public abstract object Deserialize(Type objectType, T data);

        /// <summary>
        /// Serializes the object
        /// </summary>
        /// <param name="objectType">Object type</param>
        /// <param name="data">Data to serialize</param>
        /// <returns>The serialized data</returns>
        public abstract T Serialize(Type objectType, object data);
    }
}