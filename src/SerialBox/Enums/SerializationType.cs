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

namespace SerialBox.Enums
{
    /// <summary>
    /// Defines the serialization types in an enum like static class
    /// </summary>
    public class SerializationType
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SerializationType"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        protected SerializationType(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Gets the json serialization type.
        /// </summary>
        /// <value>The json serialization type.</value>
        public static SerializationType JSON => new SerializationType("application/json");

        /// <summary>
        /// Gets the XML serialization type.
        /// </summary>
        /// <value>The XML serialization type.</value>
        public static SerializationType XML => new SerializationType("text/xml");

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        private string Name { get; set; }

        /// <summary>
        /// Performs an explicit conversion from <see cref="string"/> to <see cref="SerializationType"/>.
        /// </summary>
        /// <param name="serializationType">Type of the serialization.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator SerializationType(string serializationType)
        {
            return new SerializationType(serializationType);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="SerializationType"/> to <see cref="string"/>.
        /// </summary>
        /// <param name="serializationType">Type of the serialization.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator string(SerializationType serializationType)
        {
            return serializationType.ToString();
        }

        /// <summary>
        /// Returns a <see cref="string"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="string"/> that represents this instance.</returns>
        public override string ToString()
        {
            return Name;
        }
    }
}