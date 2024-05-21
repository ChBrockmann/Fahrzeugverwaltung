using DataAccess.BaseService;
using Model.User;

namespace DataAccess.UserService;

public interface IUserService : IBaseService<UserModel, Guid> { }