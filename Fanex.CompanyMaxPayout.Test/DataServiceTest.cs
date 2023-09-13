using Fanex.CompanyMaxPayout.Business.Services;
using NUnit.Framework;

namespace Fanex.CompanyMaxPayout.Test;

public class LogTest
{
    [Test]
    public void PLus_Always_ReturnRightValue()
    {
        const int a = 1;
        const int b = 2;
        const int expected = 3;

        var plus = new LogImp();
        var result = plus.Plus(a, b);

        Assert.That(result, Is.EqualTo(expected));
    }
}