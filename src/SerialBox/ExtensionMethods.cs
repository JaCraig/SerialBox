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

using Microsoft.Extensions.DependencyInjection;
using SerialBox.Enums;
using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace SerialBox
{
    /// <summary>
    /// Extension methods dealing with serialization
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class ExtensionMethods
    {
        /// <summary>
        /// The service provider lock
        /// </summary>
        private static readonly object ServiceProviderLock = new object();

        /// <summary>
        /// The service provider
        /// </summary>
        private static IServiceProvider ServiceProvider;

        /// <summary>
        /// Deserializes the data based on the MIME content type specified (defaults to json)
        /// </summary>
        /// <typeparam name="R">Return type expected</typeparam>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="data">Data to deserialize</param>
        /// <param name="contentType">Content type (MIME type)</param>
        /// <returns>The deserialized object</returns>
        public static R Deserialize<R, T>(this T data, string contentType = "application/json")
        {
            return data.Deserialize<R, T>((SerializationType)contentType);
        }

        /// <summary>
        /// Deserializes the data based on the content type specified (defaults to json)
        /// </summary>
        /// <typeparam name="R">Return type expected</typeparam>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="data">Data to deserialize</param>
        /// <param name="contentType">Content type</param>
        /// <returns>The deserialized object</returns>
        [return: MaybeNull]
        public static R Deserialize<R, T>(this T data, SerializationType contentType)
        {
            contentType ??= SerializationType.JSON;
            var TempManager = GetServiceProvider()?.GetService<SerialBox>();
            return (R)TempManager?.Deserialize(data, typeof(R), contentType)! ?? default;
        }

        /// <summary>
        /// Serializes the data based on the MIME content type specified (defaults to json)
        /// </summary>
        /// <typeparam name="R">Return type expected</typeparam>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="serializationObject">Object to serialize</param>
        /// <param name="contentType">Content type (MIME type)</param>
        /// <returns>The serialized object</returns>
        public static R Serialize<R, T>(this T serializationObject, string contentType = "application/json")
        {
            return serializationObject.Serialize<R, T>((SerializationType)contentType);
        }

        /// <summary>
        /// Serializes the data based on the type specified (defaults to json)
        /// </summary>
        /// <typeparam name="R">Return type expected</typeparam>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="serializationObject">Object to serialize</param>
        /// <param name="contentType">Content type</param>
        /// <returns>The serialized object</returns>
        public static R Serialize<R, T>(this T serializationObject, SerializationType contentType)
        {
            contentType ??= SerializationType.JSON;
            var TempManager = GetServiceProvider()?.GetService<SerialBox>();
            return TempManager == null ? default : TempManager.Serialize<T, R>(serializationObject, contentType);
        }

        /// <summary>
        /// Gets the service provider.
        /// </summary>
        /// <returns></returns>
        private static IServiceProvider GetServiceProvider()
        {
            if (ServiceProvider is not null)
                return ServiceProvider;
            lock (ServiceProviderLock)
            {
                if (ServiceProvider is not null)
                    return ServiceProvider;
                ServiceProvider = new ServiceCollection().AddCanisterModules()?.BuildServiceProvider();
            }
            return ServiceProvider;
        }
    }
}