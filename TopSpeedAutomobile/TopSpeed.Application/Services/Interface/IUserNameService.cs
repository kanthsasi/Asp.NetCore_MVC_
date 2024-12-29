namespace TopSpeed.Application.Services.Interface
{
    public interface IUserNameService
    {
        public Task<string> GetUserName(string userId);
    }
}
