using NetArchTest.Rules;
using NetArchTest.Rules.Extensions;

namespace amorphie.template.test;

public class DependencyCheck
{
    [Fact]
    public void DataToCoreCheck()
    {
        /*
        var result = Types.InCurrentDomain()
            .That()
            .ResideInNamespace("amorphie.template.core")
            .Should()
            .BeSealed()
            .GetResult()
            .IsSuccessful;

            Assert.True(result);
            */
    }

    //core uygulamasının data ve application tarafına bağımlı olmaması clean code arch. açısından önemlidir. GErek duyulursa kullanılabilir
    [Fact]
    public void CoreShouldNotReferenceDataOrApplication()
    {
        /*
        var result = Types.InCurrentDomain()
            .That()
            .ResideInNamespace("amorphie.template.core")
            .Should()
            .NotHaveDependencyOn("amorphie.template.data")
            .And()
            .NotHaveDependencyOn("amorphie.template.application")
            .GetResult()
            .IsSuccessful;

        Assert.True(result);
        */
    }
}