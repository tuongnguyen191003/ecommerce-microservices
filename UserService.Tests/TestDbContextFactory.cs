using Microsoft.EntityFrameworkCore;
using UserService.Data;

namespace UserService.Tests.TestHelpers
{
    public static class TestDbContextFactory
    {
        public static UserDbContext CreateInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<UserDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Tạo database tạm
                .Options;

            return new UserDbContext(options);
        }
    }
}
