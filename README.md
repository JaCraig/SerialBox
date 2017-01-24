# SerialBox

[![Build status](https://ci.appveyor.com/api/projects/status/3q7rqlaju498yw7s?svg=true)](https://ci.appveyor.com/project/JaCraig/serialbox)

SerialBox is a library designed to simplify serialization in .Net. By default it supports XML and JSON but can be expanded upon to support other serialization targets as well.

## Basic Usage

The system relies on an IoC wrapper called [Canister](https://github.com/JaCraig/Canister). While Canister has a built in IoC container, it's purpose is to actually wrap your container of choice in a way that simplifies setup and usage for other libraries that don't want to be tied to a specific IoC container. SerialBox uses it to detect and pull in serialization providers. As such you must set up Canister in order to use SerialBox:

    Canister.Builder.CreateContainer(new List<ServiceDescriptor>())
                    .RegisterSerialBox()
                    .Build();
	
This line is required prior to using the extension methods for the first time. Once Canister is set up, you can call the extension methods provided:

    [DataContract]
    public class Temp
    {
        [DataMember(Name = "A", Order = 1)]
        public int A { get; set; }
    }
    
    ...
    
    var TestObj = new Temp() { A = 100 };
    string Value = TestObj.Serialize<string, Temp>();
    Temp TestObj2 = Value.Deserialize<Temp, string>();
	
The Serialize function takes the serialization type as a parameter. If one is not passed in, it defaults to JSON. This parameter can either be the MIME type for the serialization type as a string or it can be a SerializationType object. The Deserialize function acts in the same manner.

## Adding Serialization Types

The system comes with JSON and XML serialization, however you may wish to add other targets such as binary. In order to do this all that you need to do is create a class that inherits from ISerializer<T>:

    public class MySerializer : ISerializer<byte[]>
    {
        public string ContentType => "application/octet-stream";
		
		public string FileType => ".blob";
		
		public string Name => "Binary";
		
		public object Deserialize(Type objectType, byte[] data) { ... }
		
		public byte[] Serialize(Type objectType, object data) { ... }
    }
	
After the class is created, you must tell Canister where to look for it. So modify the initialization line accordingly:

    Canister.Builder.CreateContainer(new List<ServiceDescriptor>())
                    .RegisterSerialBox()
					.AddAssembly(typeof(MySerializer).GetTypeInfo().Assembly)
                    .Build();
	
From there the system will find the new provider and use it when called.

## Overriding Serialization Types

By default the system uses the built in JSON and XML providers in .Net. However it is possible to override these by simply creating a class that inherits from ISerializer<T> and setting the correct ContentType to match the one that you wish to override. For instance to override the JSON provider with your own you would do the following:

    public class MySerializer : ISerializer<string>
    {
        public string ContentType => "application/json";
		
		public string FileType => ".json";
		
		public string Name => "JSON";
		
		public object Deserialize(Type objectType, string data) { ... }
		
		public string Serialize(Type objectType, object data) { ... }
    }
	
After the class is created, you must tell Canister where to look for it. So modify the initialization line accordingly:

    Canister.Builder.CreateContainer(new List<ServiceDescriptor>())
                    .RegisterSerialBox()
					.AddAssembly(typeof(MySerializer).GetTypeInfo().Assembly)
                    .Build();
	
From there the system will override the default JSON provider with your own.

## Installation

The library is available via Nuget with the package name "SerialBox". To install it run the following command in the Package Manager Console:

Install-Package SerialBox

## Build Process

In order to build the library you will require the following as a minimum:

1. Visual Studio 2015 with Update 3
2. .Net Core 1.0 SDK

Other than that, just clone the project and you should be able to load the solution and build without too much effort.