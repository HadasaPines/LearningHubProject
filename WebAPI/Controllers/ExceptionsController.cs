using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BL.Exceptions;
using System.Threading;
using BL.Exceptions.UserExceptions;
using BL.Exceptions.StudentExceptions;
namespace WebAPI.Controllers
{
    public class ExceptionsController : Controller
    {
        private readonly ILogger logger;
        public ExceptionsController(ILogger<ExceptionsController> _logger)
        {
            logger = _logger;
        }
        [HttpGet("/error")]
        [HttpPost("/error")]
        [HttpPut("/error")]
        [HttpDelete("/error")]
        public IActionResult HandleError()
        {
            var exceptionDetails = HttpContext.Features.Get<IExceptionHandlerFeature>();
            if (exceptionDetails != null)
            {
                logger.LogInformation("*********************************************************************");
                logger.LogError(exceptionDetails.Error.Message, "error was throwed");
                logger.LogDebug(exceptionDetails.Error, "");
                logger.LogInformation("*********************************************************************");
            }


            //if (exceptionDetails?.Error is InvalidEmailException invalidEmailException)
            //{
            //    logger.LogInformation("*********************************************************************");
            //    logger.LogWarning(invalidEmailException.Message);
            //    logger.LogInformation("*********************************************************************");
            //    return Problem(

            //    title: invalidEmailException.Message,
            //    statusCode: invalidEmailException.StatusCode
            //    );

            //}

           //if(exceptionDetails ?.Error is InvalidPasswordException invalidPasswordException)
           // {
           //     logger.LogInformation("*********************************************************************");
           //     logger.LogWarning(invalidPasswordException.Message);
           //     logger.LogInformation("*********************************************************************");
           //     return Problem(

           //     title: invalidPasswordException.Message,
           //     statusCode: invalidPasswordException.StatusCode
           //     );
           // }
 

            if (exceptionDetails?.Error is RequiredFieldsNotFilledException requiredFieldsNotFilledException)
            {
                logger.LogInformation("*********************************************************************");
                logger.LogWarning(requiredFieldsNotFilledException.Message);
                logger.LogInformation("*********************************************************************");
                return Problem(

                title: requiredFieldsNotFilledException.Message,
                statusCode: requiredFieldsNotFilledException.StatusCode
                );
            }

            if (exceptionDetails?.Error is UserAlreadyExistsException userAlreadyExistsException)
            {
                logger.LogInformation("*********************************************************************");
                logger.LogWarning(userAlreadyExistsException.Message);
                logger.LogInformation("*********************************************************************");
                return Problem(

                title: userAlreadyExistsException.Message,
                statusCode: userAlreadyExistsException.StatusCode
                );
            }

            if (exceptionDetails?.Error is UserNotFoundException userNotFoundException)
            {
                logger.LogInformation("*********************************************************************");
                logger.LogWarning(userNotFoundException.Message);
                logger.LogInformation("*********************************************************************");
                return Problem(

                title: userNotFoundException.Message,
                statusCode: userNotFoundException.StatusCode
                );
            }

            if (exceptionDetails?.Error is NameIdMismatchException nameIdMismatchException)
            {
                logger.LogInformation("*********************************************************************");
                logger.LogWarning(nameIdMismatchException.Message);
                logger.LogInformation("*********************************************************************");
                return Problem(

                title: nameIdMismatchException.Message,
                statusCode: nameIdMismatchException.StatusCode
                );
            }

            if (exceptionDetails?.Error is WrongPasswordException wrongPasswordException)
            {
                logger.LogInformation("*********************************************************************");
                logger.LogWarning(wrongPasswordException.Message);
                logger.LogInformation("*********************************************************************");
                return Problem(

                title: wrongPasswordException.Message,
                statusCode: wrongPasswordException.StatusCode
                );
            }

            if (exceptionDetails?.Error is RegisterDoesNotMatchTheStudent registerDoesNotMatchTheStudent)
            {
                logger.LogInformation("*********************************************************************");
                logger.LogWarning(registerDoesNotMatchTheStudent.Message);
                logger.LogInformation("*********************************************************************");
                return Problem(

                title: registerDoesNotMatchTheStudent.Message,
                statusCode: registerDoesNotMatchTheStudent.StatusCode
                );
            }

            if (exceptionDetails?.Error is StudentNotFoundException studentNotFoundException)
            {
                logger.LogInformation("*********************************************************************");
                logger.LogWarning(studentNotFoundException.Message);
                logger.LogInformation("*********************************************************************");
                return Problem(

                title: studentNotFoundException.Message,
                statusCode: studentNotFoundException.StatusCode
                );
            }


            if (exceptionDetails?.Error is NullReferenceException)
                {
                    return Problem(
                    detail: "Please conncet the owner of the website 0548535515",
                    title: "An error occurred",
                    statusCode: 777
                    );

                }
                return Problem(
                    detail: "Please restart the website again",
                    title: "An error occurred",
                    statusCode: 500
                );


            }


        }
    }


