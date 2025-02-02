﻿using SerialBox.Enums;
using SerialBox.Tests.BaseClasses;
using System.Runtime.Serialization;
using Xunit;

namespace SerialBox.Tests
{
    public class SerializerExtensionTests : TestBaseClass
    {
        [Fact]
        public void SerializeDeserializeJson()
        {
            var TestObj = new Temp() { A = 100 };
            var Value = TestObj.Serialize<string, Temp>();
            var TestObj2 = Value.Deserialize<Temp, string>();
            Assert.Equal("{\"A\":100}", Value);
            Assert.Equal(TestObj.A, TestObj2.A);
        }

        [Fact]
        public void SerializeDeserializeJSON2()
        {
            var TestObj = new Temp() { A = 100 };
            var Value = TestObj.Serialize<string, Temp>(SerializationType.JSON);
            var TestObj2 = Value.Deserialize<Temp, string>(SerializationType.JSON);
            Assert.Equal("{\"A\":100}", Value);
            Assert.Equal(TestObj.A, TestObj2.A);
        }

        [Fact]
        public void SerializeDeserializeXml()
        {
            var TestObj = new Temp() { A = 100 };
            var Value = TestObj.Serialize<string, Temp>("text/xml");
            var TestObj2 = Value.Deserialize<Temp, string>("text/xml");
            Assert.Equal(TestObj.A, TestObj2.A);
        }

        [Fact]
        public void SerializeDeserializeXML2()
        {
            var TestObj = new Temp() { A = 100 };
            var Value = TestObj.Serialize<string, Temp>(SerializationType.XML);
            var TestObj2 = Value.Deserialize<Temp, string>(SerializationType.XML);
            Assert.Equal(TestObj.A, TestObj2.A);
        }

        [DataContract]
        public class Temp
        {
            [DataMember(Name = "A", Order = 1)]
            public int A { get; set; }
        }
    }
}