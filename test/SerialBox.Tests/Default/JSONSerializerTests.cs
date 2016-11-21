using SerialBox.Default;
using SerialBox.Tests.BaseClasses;
using System.Runtime.Serialization;
using Xunit;

namespace SerialBox.Tests.Default
{
    public class JSONSerializerTests : TestBaseClass
    {
        [Fact]
        public void Creation()
        {
            var Temp = new JSONSerializer(Canister.Builder.Bootstrapper);
            Assert.NotNull(Temp);
        }

        [Fact]
        public void SerializeDeserialize()
        {
            var Temp = new JSONSerializer(Canister.Builder.Bootstrapper);
            Assert.Equal(new Temp() { A = 10 }.A, ((Temp)Temp.Deserialize(typeof(Temp), Temp.Serialize(typeof(Temp), new Temp() { A = 10 }))).A);
        }

        [DataContract]
        protected class Temp
        {
            [DataMember(Name = "A", Order = 1)]
            public int A { get; set; }
        }
    }
}