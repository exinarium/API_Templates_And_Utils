using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Full_Application_Blazor.Common.Requests;
using Full_Application_Blazor.Common.Responses;
using Full_Application_Blazor.Domain.Aggregates;
using Full_Application_Blazor.Utils.Helpers.Constants;
using Full_Application_Blazor.Utils.Helpers.Enums;
using Full_Application_Blazor.Utils.Models;
using Full_Application_Blazor.Utils.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MongoDB.Bson;

namespace Full_Application_Blazor.Controllers
{
    [ExcludeFromCodeCoverage]
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController : ControllerBase
    {
        private readonly ILoggerService _loggerService;
        private readonly ILoginAggregate _loginAggregate;
        private readonly ISignupAggregate _signupAggregate;
        private readonly IVerifyAggregate _verifyAggregate;
        private readonly IResetPasswordAggregate _resetPasswordAggregate;

        public AuthController(ILoggerService loggerService, ILoginAggregate loginAggregate, ISignupAggregate signupAggregate, IVerifyAggregate verifyAggregate, IResetPasswordAggregate resetPasswordAggregate)
        {
            _loggerService = loggerService;
            _loginAggregate = loginAggregate;
            _signupAggregate = signupAggregate;
            _verifyAggregate = verifyAggregate;
            _resetPasswordAggregate = resetPasswordAggregate;
        }

        /// <summary>
        /// The login method for authentication
        /// </summary>
        /// <param name="request">The login request object</param>
        [HttpPost("login")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType(typeof(LoginResponse))]
        [ProducesErrorResponseType(typeof(ErrorResponse))]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            try
            {
                var loginResponse = await _loginAggregate.Login(request);

                if(loginResponse == null)
                {
                    return Unauthorized(new ErrorResponse
                    {
                        Message = "Invalid credentials"
                    });
                }

                return Ok(loginResponse);
            }
            catch(Exception e)
            {
                var log = new Log
                {
                    LogPriority = LogPriority.HIGH,
                    CustomMessage = e.Message,
                    SystemMessage = e.StackTrace,
                    ClassName = "AuthContoller.Login",
                    LogType = LogType.ERROR
                };

                await _loggerService.LogAsync(log);
                return StatusCode(500, new ErrorResponse
                {
                    Message = e.Message,
                    InternalExceptionMessage = e.InnerException?.Message
                }) ;
            }
        }

        /// <summary>
        /// The signup method for new users
        /// </summary>
        /// <param name="request">The signup request object</param>
        [HttpPost("signup")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType(typeof(SignupResponse))]
        [ProducesErrorResponseType(typeof(ErrorResponse))]
        [AllowAnonymous]
        public async Task<IActionResult> Signup(SignupRequest request)
        {
            try
            {
                var signupResponse = await _signupAggregate.Signup(request);

                if (signupResponse == null)
                {
                    return BadRequest(new ErrorResponse
                    {
                        Message = APIConstants.EMAIL_ALREADY_REGISTERED_ERROR
                    });
                }

                return Ok(signupResponse);
            }
            catch (Exception e)
            {
                var log = new Log
                {
                    LogPriority = LogPriority.HIGH,
                    CustomMessage = e.Message,
                    SystemMessage = e.StackTrace,
                    ClassName = "AuthContoller.Signup",
                    LogType = LogType.ERROR
                };

                await _loggerService.LogAsync(log);
                return StatusCode(500, new ErrorResponse
                {
                    Message = e.Message,
                    InternalExceptionMessage = e.InnerException?.Message
                });
            }
        }

        /// <summary>
        /// Request a new email verification token
        /// </summary>
        [HttpGet("request-email-verification")]
        [Produces("application/json")]
        [Authorize(Roles = "Unverified")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType(typeof(VerificationResponse))]
        [ProducesErrorResponseType(typeof(ErrorResponse))]
        public async Task<IActionResult> RequestEmailVerificationToken()
        {
            try
            {
                await _verifyAggregate.SendVerificationEmail(HttpContext.User);
                return Ok(new VerificationResponse
                {
                    Success = true
                });
            }
            catch (Exception e)
            {
                var log = new Log
                {
                    LogPriority = LogPriority.HIGH,
                    CustomMessage = e.Message,
                    SystemMessage = e.StackTrace,
                    ClassName = "AuthContoller.RequestEmailVerificationToken",
                    LogType = LogType.ERROR
                };

                await _loggerService.LogAsync(log);
                return StatusCode(500, new ErrorResponse
                {
                    Message = e.Message,
                    InternalExceptionMessage = e.InnerException?.Message
                });
            }
        }

        /// <summary>
        /// Request a new phone number verification token
        /// </summary>
        [HttpGet("request-phone-verification")]
        [Produces("application/json")]
        [Authorize(Roles = "Unverified")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType(typeof(VerificationResponse))]
        [ProducesErrorResponseType(typeof(ErrorResponse))]
        public async Task<IActionResult> RequestPhoneNumberVerificationToken()
        {
            try
            {
                await _verifyAggregate.SendVerificationPhoneNumber(HttpContext.User);
                return Ok(new VerificationResponse
                {
                    Success = true
                });
            }
            catch (Exception e)
            {
                var log = new Log
                {
                    LogPriority = LogPriority.HIGH,
                    CustomMessage = e.Message,
                    SystemMessage = e.StackTrace,
                    ClassName = "AuthContoller.RequestPhoneNumberVerificationToken",
                    LogType = LogType.ERROR
                };

                await _loggerService.LogAsync(log);
                return StatusCode(500, new ErrorResponse
                {
                    Message = e.Message,
                    InternalExceptionMessage = e.InnerException?.Message
                });
            }
        }

        /// <summary>
        /// Verify the user's email address
        /// </summary>
        /// <param name="request">The email verification request object</param>
        [HttpPost("verify-email")]
        [Produces("application/json")]
        [Authorize(Roles = "Unverified")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType(typeof(VerificationResponse))]
        [ProducesErrorResponseType(typeof(ErrorResponse))]
        public async Task<IActionResult> VerifyEmailToken(VerifyEmailAddressRequest request)
        {
            try
            {
                var verificationResponse = await _verifyAggregate.VerifyEmailAddress(request, HttpContext.User);

                if (verificationResponse.Success)
                {
                    return Ok(verificationResponse);
                }

                return BadRequest(verificationResponse);
            }
            catch (Exception e)
            {
                var log = new Log
                {
                    LogPriority = LogPriority.HIGH,
                    CustomMessage = e.Message,
                    SystemMessage = e.StackTrace,
                    ClassName = "AuthContoller.VerifyEmailToken",
                    LogType = LogType.ERROR
                };

                await _loggerService.LogAsync(log);
                return StatusCode(500, new ErrorResponse
                {
                    Message = e.Message,
                    InternalExceptionMessage = e.InnerException?.Message
                });
            }
        }

        /// <summary>
        /// Verify the user's phone number
        /// </summary>
        /// <param name="request">The phone number verification request object</param>
        [HttpPost("verify-phone")]
        [Produces("application/json")]
        [Authorize(Roles = "Unverified")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType(typeof(VerificationResponse))]
        [ProducesErrorResponseType(typeof(ErrorResponse))]
        public async Task<IActionResult> VerifyPhoneNumberToken(VerifyPhoneNumberRequest request)
        {
            try
            {
                var verificationResponse = await _verifyAggregate.VerifyPhoneNumber(request, HttpContext.User);

                if (verificationResponse.Success)
                {
                    return Ok(verificationResponse);
                }

                return BadRequest(verificationResponse);
            }
            catch (Exception e)
            {
                var log = new Log
                {
                    LogPriority = LogPriority.HIGH,
                    CustomMessage = e.Message,
                    SystemMessage = e.StackTrace,
                    ClassName = "AuthContoller.VerifyPhoneNumberToken",
                    LogType = LogType.ERROR
                };

                await _loggerService.LogAsync(log);
                return StatusCode(500, new ErrorResponse
                {
                    Message = e.Message,
                    InternalExceptionMessage = e.InnerException?.Message
                });
            }
        }

        /// <summary>
        /// Request a new password reset token
        /// </summary>
        /// <param name="request">The reset password request object</param>
        [HttpPost("request-password-reset")]
        [Produces("application/json")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType(typeof(ResetPasswordResponse))]
        [ProducesErrorResponseType(typeof(ErrorResponse))]
        public async Task<IActionResult> RequestPasswordResetToken(ResetPasswordRequest request)
        {
            try
            {
                await _resetPasswordAggregate.SendResetPasswordEmail(request);
                return Ok(new VerificationResponse
                {
                    Success = true
                });
            }
            catch (Exception e)
            {
                var log = new Log
                {
                    LogPriority = LogPriority.HIGH,
                    CustomMessage = e.Message,
                    SystemMessage = e.StackTrace,
                    ClassName = "AuthContoller.RequestPasswordResetToken",
                    LogType = LogType.ERROR
                };

                await _loggerService.LogAsync(log);
                return StatusCode(500, new ErrorResponse
                {
                    Message = e.Message,
                    InternalExceptionMessage = e.InnerException?.Message
                });
            }
        }

        /// <summary>
        /// Reset the user's password
        /// </summary>
        /// <param name="request">The password reset verification request object</param>
        [HttpPost("verify-password-reset")]
        [Produces("application/json")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType(typeof(VerificationResponse))]
        [ProducesErrorResponseType(typeof(ErrorResponse))]
        public async Task<IActionResult> VerifyPasswordResetToken(VerifyResetPasswordRequest request)
        {
            try
            {
                var resetPasswordResult = await _resetPasswordAggregate.VerifyResetPasswordToken(request);

                if (resetPasswordResult.Success)
                {
                    return Ok(resetPasswordResult);
                }

                return BadRequest(resetPasswordResult);
            }
            catch (Exception e)
            {
                var log = new Log
                {
                    LogPriority = LogPriority.HIGH,
                    CustomMessage = e.Message,
                    SystemMessage = e.StackTrace,
                    ClassName = "AuthContoller.VerifyPasswordResetToken",
                    LogType = LogType.ERROR
                };

                await _loggerService.LogAsync(log);
                return StatusCode(500, new ErrorResponse
                {
                    Message = e.Message,
                    InternalExceptionMessage = e.InnerException?.Message
                });
            }
        }
    }
}

