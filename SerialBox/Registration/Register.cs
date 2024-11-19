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
using Microsoft.Extensions.DependencyInjection;
using SerialBox.Interfaces;
using System.Reflection;

namespace SerialBox.Registration
{
    /// <summary>
    /// Registration extension methods
    /// </summary>
    public static class Registration
    {
        /// <summary>
        /// Registers the library with the bootstrapper.
        /// </summary>
        /// <param name="bootstrapper">The bootstrapper.</param>
        /// <returns>The bootstrapper</returns>
        public static ICanisterConfiguration? RegisterSerialBox(this ICanisterConfiguration? bootstrapper) => bootstrapper?.AddAssembly(typeof(Registration).GetTypeInfo().Assembly);

        /// <summary>
        /// Registers the SerialBox services with the specified IServiceCollection.
        /// </summary>
        /// <param name="services">The IServiceCollection to add the services to.</param>
        /// <returns>The IServiceCollection with the registered services.</returns>
        public static IServiceCollection? RegisterSerialBox(this IServiceCollection? services) => services?.AddAllTransient<ISerializer>()?.AddSingleton<SerialBox>();
    }
}