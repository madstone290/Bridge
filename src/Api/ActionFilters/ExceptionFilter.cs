using Bridge.Application.Common;
using Bridge.Domain.Common;
using Bridge.Shared.ApiContract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Bridge.Api.ActionFilters
{
    public class ExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<ExceptionFilter> _logger;

        public ExceptionFilter(ILogger<ExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            if (context.Exception is DomainException domainException)
            {
                _logger.LogInformation(domainException, "BadRequest");

                context.Result = new BadRequestObjectResult(new ErrorContent(domainException.Message));
                return;
            }

            if (context.Exception is AppException appException)
            {
                _logger.LogInformation(appException, "BadRequest");

                context.Result = new BadRequestObjectResult(new ErrorContent(appException.Message, appException.Code));
                return;
            }

            if (context.Exception is DbUpdateException updateException)
            {
                if (updateException.InnerException is PostgresException postgresException)
                {
                    if (postgresException.SqlState == PostgresErrorCodes.ForeignKeyViolation)
                    {
                        context.Result = new BadRequestObjectResult(new ErrorContent("외래키 제약 조건을 위반하였습니다", ErrorCodes.FOREIGN_KEY_VIOLATION));
                        _logger.LogInformation(context.Exception, "BadRequest");
                        return;
                    }
                }

                _logger.LogError(context.Exception, "InternalServerError");
                context.Result = new StatusCodeResult(StatusCodes.Status500InternalServerError);
                return;
            }

            _logger.LogError(context.Exception, "InternalServerError");
            context.Result = new StatusCodeResult(StatusCodes.Status500InternalServerError);


        }
    }
}
