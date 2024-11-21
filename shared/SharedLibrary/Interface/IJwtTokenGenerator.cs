namespace SharedLibrary.Security
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(string userId);
    }
}
