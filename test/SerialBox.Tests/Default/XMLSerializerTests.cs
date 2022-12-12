using SerialBox.Default;
using SerialBox.Tests.BaseClasses;
using System.Runtime.Serialization;
using Xunit;

namespace SerialBox.Tests.Default
{
    public class XMLSerializerTests : TestBaseClass
    {
        [Fact]
        public void Creation()
        {
            var Temp = new XMLSerializer();
            Assert.NotNull(Temp);
        }

        [Fact]
        public void SerializeDeserialize()
        {
            var Temp = new XMLSerializer();
            Assert.Equal(new Temp() { A = 10 }.A, ((Temp)Temp.Deserialize(typeof(Temp), Temp.Serialize(typeof(Temp), new Temp() { A = 10 }))).A);
        }

        [DataContract]
        public class Temp
        {
            [DataMember(Name = "A", Order = 1)]
            public int A { get; set; }
        }
    }
}