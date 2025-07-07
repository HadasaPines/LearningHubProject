using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using BL.Exceptions;
using BL.Exceptions.UserExceptions;
using BL.Exceptions.StudentExceptions;
using BL.Exceptions.TeacherAvailabilityExceptions;
using BL.Exceptions.LessonExceptions;
using BL.Exceptions.RegistrationExceptions;
using BL.Exceptions.TeacherExceptions;
using BL.Exceptions.SubjectExceptions;

namespace WebAPI.Controllers
{
    public class ExceptionsController : Controller
    {
        private readonly ILogger<ExceptionsController> logger;

        public ExceptionsController(ILogger<ExceptionsController> _logger)
        {
            logger = _logger;
        }

        [HttpGet("/error")]
        [HttpPost("/error")]
        [HttpPut("/error")]
        [HttpDelete("/error")]
        [HttpPatch("/error")]
        public IActionResult HandleError()
        {
            var exceptionDetails = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var ex = exceptionDetails?.Error;

            if (ex != null)
            {
                logger.LogInformation("*********************************************************************");
                logger.LogError(ex, ex.Message);
                logger.LogInformation("*********************************************************************");
            }

            var exceptionMap = new Dictionary<Type, Func<Exception, IActionResult>>
            {
                [typeof(RequiredFieldsNotFilledException)] = HandleCustomException,
                [typeof(UserAlreadyExistsException)] = HandleCustomException,
                [typeof(UserNotFoundException)] = HandleCustomException,
                [typeof(WrongPasswordException)] = HandleCustomException,
                [typeof(RegisterDoesNotMatchTheStudent)] = HandleCustomException,
                [typeof(TeacherAvailabilityNotFoundException)] = HandleCustomException,
                [typeof(LessonNotFoundException)] = HandleCustomException,
                [typeof(SubjectNotFoundException)] = HandleCustomException,
                [typeof(CanNotDeleteSubject)] = HandleCustomException,
                [typeof(SubjectAlreadyExist)] = HandleCustomException,
                [typeof(RegistrationNotFoundException)] = HandleCustomException,
                [typeof(MismatchTeacherAndSubjectException)] = HandleCustomException,
                [typeof(MismatchTeacherAndLessonException)] = HandleCustomException,
                [typeof(MismatchTeacherAndAvailabilityException)] = HandleCustomException,
                [typeof(NullReferenceException)] = ex => Problem(
                    detail: "Please contact the owner of the website 0548535515",
                    title: "An error occurred",
                    statusCode: 777
                )
            };

            if (ex != null && exceptionMap.TryGetValue(ex.GetType(), out var handler))
                return handler(ex);

            return Problem(
                detail: "Please restart the website again",
                title: "An error occurred",
                statusCode: 500
            );
        }

        private IActionResult HandleCustomException(Exception ex)
        {
            logger.LogWarning(ex.Message);

            int statusCode = 400; // ברירת מחדל

            var statusCodeProperty = ex.GetType().GetProperty("StatusCode");
            if (statusCodeProperty != null)
            {
                var value = statusCodeProperty.GetValue(ex);
                if (value is int code)
                {
                    statusCode = code;
                }
            }

            return Problem(
                title: ex.Message,
                statusCode: statusCode
            );
        }

    }
}
