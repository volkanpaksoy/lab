using System;

namespace Sample.Core
{
    public interface IUserService
    {
        int GetTestId();
    }

    public class UserService : IUserService
    {
        public int GetTestId()
        {
            return 2;
        }
    }
}
