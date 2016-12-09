using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Reflection;
using Xunit;

namespace SerialBox.Tests.BaseClasses
{
    [Collection("Serialization")]
    public abstract class TestBaseClass
    {
        protected TestBaseClass()
        {
            if (Canister.Builder.Bootstrapper == null)
                Canister.Builder.CreateContainer(new List<ServiceDescriptor>(), typeof(SerialBox).GetTypeInfo().Assembly);
        }
    }
}