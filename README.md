# SerialBox

[![.NET Publish](https://github.com/JaCraig/SerialBox/actions/workflows/dotnet-publish.yml/badge.svg)](https://github.com/JaCraig/SerialBox/actions/workflows/dotnet-publish.yml)

SerialBox is a library designed to simplify serialization in .Net. By default it supports XML and JSON but can be expanded upon to support other serialization targets as well.

## Basic Usage

The library can be initialized by registering it with your IoC container during startup. Example code:

```csharp
ServiceProvider? ServiceProvider = new ServiceCollection().RegisterAspectus()?.BuildServiceProvider();
```

or

```csharp
ServiceProvider? ServiceProvider = new ServiceCollection().AddCanisterModules()?.BuildServiceProvider();
```

As the library supports [Canister Modules](https://github.com/JaCraig/Canister).
	
This line is required prior to using the extension methods for the first time. Once it is set up, you can call the extension methods provided:

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
	
After the class is created, the system will automatically pick it up and use it.

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
	
After the class is created, the system will automatically pick it up and use it.

## Installation

The library is available via Nuget with the package name "SerialBox". To install it run the following command in the Package Manager Console:

Install-Package SerialBox

## Build Process

In order to build the library you will require the following as a minimum:

1. Visual Studio 2022

Other than that, just clone the project and you should be able to load the solution and build without too much effort.
