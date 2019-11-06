using System;
using Xunit;

namespace Sample.Core.UnitTests
{
    public class UserServiceTests
    {
        [Fact]
        public void Test_Should_Return_Test_Value()
        {
            int expected = 1;

            UserService userService = new UserService();
            int actual = userService.GetTestId();

            Assert.Equal<int>(expected, actual);
        }
    }
}
