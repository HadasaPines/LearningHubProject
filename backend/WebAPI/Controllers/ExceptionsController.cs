using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BL.Exceptions;
using System.Threading;
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

       

            if (exceptionDetails?.Error is TeacherAvailabilityNotFoundException teacherAvailabilityNotFoundException)
            {
                logger.LogInformation("*********************************************************************");
                logger.LogWarning(teacherAvailabilityNotFoundException.Message);
                logger.LogInformation("*********************************************************************");
                return Problem(

                title: teacherAvailabilityNotFoundException.Message,
                statusCode: teacherAvailabilityNotFoundException.StatusCode
                );
            }
            if (exceptionDetails?.Error is LessonNotFoundException lessonNotFoundException)
            {
                logger.LogInformation("*********************************************************************");
                logger.LogWarning(lessonNotFoundException.Message);
                logger.LogInformation("*********************************************************************");
                return Problem(

                title: lessonNotFoundException.Message,
                statusCode: lessonNotFoundException.StatusCode
                );
            }
            if (exceptionDetails?.Error is SubjectNotFoundException subjectNotFoundException)
            {
                logger.LogInformation("*********************************************************************");
                logger.LogWarning(subjectNotFoundException.Message);
                logger.LogInformation("*********************************************************************");
                return Problem(

                title: subjectNotFoundException.Message,
                statusCode: subjectNotFoundException.StatusCode
                );
            }


            if (exceptionDetails?.Error is RegistrationNotFoundException registrationNotFoundException)
            {
                logger.LogInformation("*********************************************************************");
                logger.LogWarning(registrationNotFoundException.Message);
                logger.LogInformation("*********************************************************************");
                return Problem(

                title: registrationNotFoundException.Message,
                statusCode: registrationNotFoundException.StatusCode
                );
            }
          
            if (exceptionDetails?.Error is MismatchTeacherAndSubjectException mismatchTeacherAndSubjectException)
            {
                logger.LogInformation("*********************************************************************");
                logger.LogWarning(mismatchTeacherAndSubjectException.Message);
                logger.LogInformation("*********************************************************************");
                return Problem(
                    title: mismatchTeacherAndSubjectException.Message,
                    statusCode: mismatchTeacherAndSubjectException.StatusCode
                );
            }
            if (exceptionDetails?.Error is MismatchTeacherAndLessonException mismatchTeacherAndLessonException)
            {
                logger.LogInformation("*********************************************************************");
                logger.LogWarning(mismatchTeacherAndLessonException.Message);
                logger.LogInformation("*********************************************************************");
                return Problem(
                    title: mismatchTeacherAndLessonException.Message,
                    statusCode: mismatchTeacherAndLessonException.StatusCode
                );
            }

            if (exceptionDetails?.Error is MismatchTeacherAndAvailabilityException mismatchTeacherAndAvailabilityException)
            {
                logger.LogInformation("*********************************************************************");
                logger.LogWarning(mismatchTeacherAndAvailabilityException.Message);
                logger.LogInformation("*********************************************************************");
                return Problem(
                    title: mismatchTeacherAndAvailabilityException.Message,
                    statusCode: mismatchTeacherAndAvailabilityException.StatusCode
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


