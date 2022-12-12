using SerialBox.Default;
using SerialBox.Interfaces;
using SerialBox.Tests.BaseClasses;
using System.Runtime.Serialization;
using Xunit;

namespace SerialBox.Tests
{
    public class SerializerTests : TestBaseClass
    {
        [Fact]
        public void CanSerialize()
        {
            var Temp = new SerialBox(new ISerializer[] { new JSONSerializer(), new XMLSerializer() });
            Assert.True(Temp.CanSerialize("application/json"));
        }

        [Fact]
        public void Creation()
        {
            var Temp = new SerialBox(new ISerializer[] { new JSONSerializer(), new XMLSerializer() });
            Assert.NotNull(Temp);
        }

        [Fact]
        public void FileTypeToContentType()
        {
            var Temp = new SerialBox(new ISerializer[] { new JSONSerializer(), new XMLSerializer() });
            Assert.Equal("application/json", Temp.FileTypeToContentType(".json"));
        }

        [Fact]
        public void SerializeDeserialize()
        {
            var Temp = new SerialBox(new ISerializer[] { new JSONSerializer(), new XMLSerializer() });
            Assert.Equal("{\"A\":10}", Temp.Serialize<string>(new Temp { A = 10 }, typeof(Temp)));
            Assert.Equal(new Temp() { A = 10 }.A, ((Temp)Temp.Deserialize("{\"A\":10}", typeof(Temp))).A);
        }

        [DataContract]
        protected class Temp
        {
            [DataMember(Name = "A", Order = 1)]
            public int A { get; set; }
        }
    }
}