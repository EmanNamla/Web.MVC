using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Application.DTOs.Identity;
using Web.Application.Interfaces;
using Web.Core.Interfaces.Identity;

namespace Web.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IApplicationUserRepository _userRepository;
        private readonly ILogger<AuthService> _logger;

        public AuthService(IApplicationUserRepository userRepository, ILogger<AuthService> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<IdentityResult> RegisterUserAsync(RegisterDto registerDto)
        {
            try
            {
                var existingUser = await _userRepository.GetByEmailAsync(registerDto.Email);
                if (existingUser != null)
                {
                    _logger.LogWarning("Registration failed: Email {Email} is already in use.", registerDto.Email);

                    // Create a failed IdentityResult with custom error message
                    return IdentityResult.Failed(new IdentityError
                    {
                        Code = "DuplicateEmail",
                        Description = "This email is already in use."
                    });
                }

                // Check if Password and ConfirmPassword match
                if (registerDto.Password != registerDto.ConfirmPassword)
                {
                    _logger.LogWarning("Registration failed: Passwords do not match for email {Email}.", registerDto.Email);

                    return IdentityResult.Failed(new IdentityError
                    {
                        Code = "PasswordMismatch",
                        Description = "Password and Confirm Password do not match."
                    });
                }

                // Map DTO to ApplicationUser
                var user = registerDto.Adapt<ApplicationUser>();

                // Attempt to create the user
                var result = await _userRepository.CreateUserAsync(user, registerDto.Password);

                // Log the outcome
                if (result.Succeeded)
                {
                    _logger.LogInformation("User registered successfully: {Email}", user.Email);
                }
                else
                {
                    _logger.LogWarning("User registration failed for {Email}: {Errors}", user.Email,
                        string.Join(", ", result.Errors.Select(e => e.Description)));
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while registering user: {Email}", registerDto.Email);
                throw new ApplicationException("An unexpected error occurred during user registration.", ex);
            }
        }

        public async Task<string?> LoginUserAsync(LoginDto loginDto)
        {
            try
            {
                // Retrieve the user by email
                var user = await _userRepository.GetByEmailAsync(loginDto.Email);

                if (user == null)
                {
                    _logger.LogWarning("Login failed: User with email {Email} not found.", loginDto.Email);
                    return null; // User not found
                }

                // Check the password
                if (!await _userRepository.CheckPassword(user, loginDto.Password))
                {
                    _logger.LogWarning("Login failed: Invalid password for user {Email}.", loginDto.Email);
                    return null; // Invalid password
                }

                // Sign in the user
                var result = await _userRepository.PasswordSignInAsync(user, loginDto.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User {Email} logged in successfully.", loginDto.Email);
                    return user.Id;
                }
                else
                {
                    _logger.LogWarning("Login failed for user {Email}: SignInResult not succeeded.", loginDto.Email);
                    return null; // Sign-in failed
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during login for user {Email}.", loginDto.Email);
                throw new ApplicationException("An unexpected error occurred during login.", ex);
            }
        }
    }


}
