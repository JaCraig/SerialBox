using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace SerialBox.Tests.BaseClasses
{
    [Collection("Serialization")]
    public abstract class TestBaseClass
    {
        protected TestBaseClass()
        {
            if (Canister.Builder.Bootstrapper == null)
            {
                new ServiceCollection().AddCanisterModules(configure => configure.RegisterSerialBox());
            }
        }
    }
}