using Dapper.FastCrud;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Todo.Core.Entities;
using Todo.Core.ViewModels.Membership;
using Todo.DataAccess.Infrastructure;
using Todo.Domain;

namespace Todo.DataAccess.CustomRepositories
{
    public class UserService : IGenericRepository<User>, IUserRepository
    {
        private IConnectionFactory _connectionFactory;

        public UserService(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public User Get(int id)
        {
            return _connectionFactory.GetConnection.Get(new User { Id = id });
        }

        public async Task<User> GetAsync(int id)
        {
            return await _connectionFactory.GetConnection.GetAsync(new User { Id = id });
        }

        public IEnumerable<User> GetAll()
        {
            return _connectionFactory.GetConnection.Find<User>();
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _connectionFactory.GetConnection.FindAsync<User>();
        }

        public IEnumerable<User> Filter()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<User>> FilterAsync()
        {
            throw new NotImplementedException();
        }

        public User Add(User entity)
        {
            _connectionFactory.GetConnection.Insert<User>(entity);
            return entity;
        }

        public async Task<User> AddAsync(User entity)
        {
            await _connectionFactory.GetConnection.InsertAsync(entity);
            return entity;
        }

        public bool Delete(User entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(User entity)
        {
            throw new NotImplementedException();
        }

        public bool Update(User entity)
        {
            return _connectionFactory.GetConnection.Update(entity);
        }

        public async Task<bool> UpdateAsync(User entity)
        {
            return await _connectionFactory.GetConnection.UpdateAsync(entity);
        }

        public async Task<ServiceResult> SignInWithFormAsync(UserSignInWithFormVM userSignInWithFormVm)
        {
            var serviceResult = new ServiceResult();

            var userList = await _connectionFactory.GetConnection.FindAsync<User>(x => x
                                                .Where($"{nameof(User.Username):C}=@Username")
                                                .WithParameters(new { Username = userSignInWithFormVm.Username }));

            if (!userList.Any())
            {
                serviceResult.MessageType = Core.Enums.EMessageType.Error;
                serviceResult.Message = "Böyle bir kullanıcı bulunamadı.";
                return serviceResult;
            }

            var user = userList.FirstOrDefault();
            
            if (user.Password != userSignInWithFormVm.Password)
            {
                serviceResult.MessageType = Core.Enums.EMessageType.Error;
                serviceResult.Message = "Giriş bilgileri hatalı.";
                return serviceResult;
            }
                       
            serviceResult.Result = user;
            serviceResult.MessageType = Core.Enums.EMessageType.Success;
            serviceResult.Message = "Giriş Başarılı";

            return serviceResult;
        }

        public async void RemoveOldRefreshTokensAsync(int userId)
        {
            var result = await _connectionFactory.GetConnection.BulkDeleteAsync<UserRefreshToken>(x => x.Where($"{nameof(UserRefreshToken.UserId):C}=@Email")
                                                 .WithParameters(new { Email = userId }));
        }

        public async Task<ServiceResult> AddRefreshTokenAsync(UserRefreshToken userRefreshToken)
        {
            await _connectionFactory.GetConnection.InsertAsync<UserRefreshToken>(userRefreshToken);
            return new ServiceResult
            {
                Result = userRefreshToken
            };
        }

        public async Task<UserRefreshToken> GetUserRefreshTokensAsync(UserRefreshTokenVM refreshTokenVm)
        {
            var refreshToken = await _connectionFactory.GetConnection.FindAsync<UserRefreshToken>(x => x
                     .Include<User>(join => join
                             .InnerJoin())
                     .Where($"{nameof(UserRefreshToken.Token):C} = @tokenGuid")
                     .WithParameters(new { tokenGuid = refreshTokenVm.Token }));
            return !refreshToken.Any() ? null : refreshToken.FirstOrDefault();
        }
    }
}

