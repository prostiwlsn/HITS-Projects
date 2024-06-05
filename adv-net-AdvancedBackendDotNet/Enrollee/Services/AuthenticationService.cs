using Auth.Data;
using Auth.Data.Models;
using Auth.Interfaces;
using Auth.Models;
using Common.Messages;
using Common.Models;
using Common.Result;
using EasyNetQ;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Auth.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private IBus _bus;
        private AuthDbContext _dbContext;
        private UserManager<IdentityUser> _userManager;
        private SignInManager<IdentityUser> _signInManager;
        private IConfiguration _config;
        public AuthenticationService(IBus bus, AuthDbContext dbContext, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IConfiguration config)
        {
            _bus = bus;
            _dbContext = dbContext;
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
        }

        public async Task<Result<TokenResponseModel>> Login(string email, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(email, password, false, false);

            if (result.Succeeded) 
            {
                Console.WriteLine("xdd");
                var user = await _userManager.FindByEmailAsync(email);

                Guid sessionId = Guid.NewGuid();
                var accessToken = await GenerateAccessToken(email, user.Id, sessionId);
                var refreshToken = await GenerateRefreshToken(sessionId);

                Session newSession = new Session
                {
                    Id = sessionId,
                    UserId = new Guid(user.Id),
                    IsActive = true,
                    Expires = DateTime.UtcNow.AddMonths(1),
                };

                _dbContext.Sessions.Add(newSession);
                _dbContext.SaveChanges();

                return new Result<TokenResponseModel> 
                { 
                    ResultMessage = ResultMessage.Success,
                    Success = true,
                    Load = new TokenResponseModel
                    {
                        AccessToken = accessToken,
                        RefreshToken = refreshToken,
                        ExpiresIn = 3600
                    },
                };
            }
            else
            {
                Console.WriteLine(result);
            }

            return new Result<TokenResponseModel> 
            {
                ResultMessage = ResultMessage.WrongCredentials,
                Success = false,
                Message = "Wrong credentials"
            };
        }

        public async Task<Result<TokenResponseModel>> Refresh(Guid sessionId)
        {
            var session = await _dbContext.Sessions.FindAsync(sessionId);

            if (session == null)
                return new Result<TokenResponseModel>
                {
                    ResultMessage = ResultMessage.NotFound,
                    Success = false,
                    Message = "Session not found"
                };
            else if (session.Expires < DateTime.UtcNow)
                return new Result<TokenResponseModel>
                {
                    ResultMessage = ResultMessage.Expired,
                    Success = false,
                    Message = "Session expired"
                };

            var user = await _userManager.FindByIdAsync(session.UserId.ToString());

            if (user == null)
                return new Result<TokenResponseModel>
                {
                    ResultMessage = ResultMessage.Fail,
                    Success = false,
                    Message = "User not found"
                };

            Guid newSessionId = Guid.NewGuid();
            var accessToken = await GenerateAccessToken(user.Email, user.Id, newSessionId);
            var refreshToken = await GenerateRefreshToken(newSessionId);

            Session newSession = new Session
            {
                Id = newSessionId,
                UserId = new Guid(user.Id),
                IsActive = true,
                Expires = DateTime.UtcNow.AddMonths(1),
            };

            _dbContext.Sessions.Remove(session);
            _dbContext.Sessions.Add(newSession);
            _dbContext.SaveChanges();

            return new Result<TokenResponseModel>
            {
                ResultMessage = ResultMessage.Success,
                Success = true,
                Load = new TokenResponseModel
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    ExpiresIn = 3600
                },
            };
        }

        public async Task<Result> Logout()
        {
            try
            {
                await _signInManager.SignOutAsync();
                return new Result { Message = "Success", ResultMessage = ResultMessage.Success, Success = true };
            }
            catch (Exception ex)
            {
                return new Result { Message = "Something went wrong", ResultMessage = ResultMessage.Fail, Success = false };
            }
        }

        public async Task<Result> Register(UserRegistrationModel model)
        {
            Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(model));
            IdentityUser user = new IdentityUser
            {
                Email = model.Email,
                UserName = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                var newUser = await _userManager.FindByEmailAsync(model.Email);
                var id = newUser.Id;

                var request = new RegistrationFormRequest
                {
                    UserId = new Guid(id),
                    Name = model.Name,
                    Surname = model.Surname,
                    Secondname = model.Secondname,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    Gender = model.Gender,
                    Citizenship = model.Citizenship
                };
                var profileCreationResult = await _bus.Rpc.RequestAsync<RegistrationFormRequest, Result>(request);

                Console.WriteLine(profileCreationResult.Message);

                if(!profileCreationResult.Success)
                    await _userManager.DeleteAsync(user);

                return profileCreationResult;
            }
            else
            {
                Console.WriteLine(result.Errors);
                return new Result { ResultMessage = ResultMessage.Fail, Message = "Something went wrong", Success = false, Errors = result.Errors };
            }
        }

        public async Task<Guid?> GetUserId(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
                return null;

            return new Guid(user.Id);
        }

        private async Task<string> GenerateAccessToken(string email, string id, Guid sessionId)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, email),
                new Claim("id", id),
                new Claim("type", "access"),
                new Claim("sessionId", sessionId.ToString())
            };

            var role = _bus.Rpc.Request<GetRoleRequest, Result<Roles>>(new GetRoleRequest
            {
                UserId = new Guid(id)
            });

            if (role != null && role.Success)
                claims.Add(new Claim(ClaimTypes.Role, role.Load.RoleToString()));

            return await GenerateToken(claims, DateTime.Now.AddHours(1));
        }

        private async Task<string> GenerateRefreshToken(Guid sessionId)
        {
            var claims = new List<Claim>
            {
                new Claim("type", "refresh"),
                new Claim("sessionId", sessionId.ToString())
            };

            return await GenerateToken(claims, DateTime.Now.AddMonths(1));
        }

        private async Task<string> GenerateToken(List<Claim> claims, DateTime expires)
        {
            var securutyKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("Jwt:Key").Value));

            var credentials = new SigningCredentials(securutyKey, SecurityAlgorithms.HmacSha512Signature);

            var securityToken = new JwtSecurityToken(
                claims: claims,
                expires: expires,
                signingCredentials: credentials,
                issuer: _config.GetSection("Jwt:Issuer").Value,
                audience: _config.GetSection("Jwt:Audience").Value
                );

            var token = new JwtSecurityTokenHandler().WriteToken(securityToken);
            return token;
        }
    }
}
