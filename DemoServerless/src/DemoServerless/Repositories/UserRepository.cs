using DemoServerless.Entities;

namespace DemoServerless.Repositories
{
    public interface IUserRepository
    {
        
    }
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(CoreDbContext context) : base(context) { }
    }
}
