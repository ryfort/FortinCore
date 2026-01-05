using Fortin.Infrastructure.Services;
using Fortin.Infrastructure.Interface;
using Fortin.Common;
using Fortin.Common.Dtos;
using Fortin.Infrastructure.Mappings;
using Fortin.Domain.Interface;

namespace Fortin.Domain.Implementation
{
    public class LoginService : ILoginService
    {
        private readonly IJwtService _jwtService;
        private readonly IUserEFRepository _userRepository;

        public LoginService(IJwtService jwtService, IUserEFRepository userRepository)
        {
            _jwtService = jwtService;
            _userRepository = userRepository;   
        }

        public async Task<ApiResponse<AuthResponseDto>> LoginAsync(LoginDto login)
        {
            var user = await _userRepository.GetUsersAsync(new UserResourceParameter() { Username = login.Username});
            var token = _jwtService.GenerateToken(user.FirstOrDefault());
            var authResponse = new AuthResponseDto
            {
                Token = token,
                User = user.FirstOrDefault().ToDto(),
                ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtService.GetTokenExpirationMinutes()) // Example expiration time
            };

            return ApiResponse<AuthResponseDto>.SuccessResponse(authResponse, "Login successful");
        }
    }
}
