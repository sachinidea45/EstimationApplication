using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Net;
using EstimationApplication.Entities;

namespace EstimationApplication.API.Models
{
    public class EstimationApplicationAPIExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            HttpStatusCode status;
            string message;

            var exceptionType = context.Exception.GetType();
            if (exceptionType == typeof(UnauthorizedAccessException))
            {
                message = EstimationApplicationConstant.UnauthorizedAccessExceptionMessage;
                status = HttpStatusCode.Unauthorized;
            }
            else if (exceptionType == typeof(NotImplementedException))
            {
                message = EstimationApplicationConstant.NotImplementedExceptionMessage;
                status = HttpStatusCode.NotImplemented;
            }
            else if (exceptionType == typeof(ExstimationApplicationBusinessException))
            {
                message = EstimationApplicationConstant.BusinessLayerExceptionMessage + context.Exception.ToString();
                status = HttpStatusCode.InternalServerError;
            }
            else if (exceptionType == typeof(ExstimationApplicationDataException))
            {
                message = EstimationApplicationConstant.DataLayerExceptionMessage + context.Exception.ToString();
                status = HttpStatusCode.InternalServerError;
            }
            else
            {
                message = context.Exception.Message;
                status = HttpStatusCode.NotFound;
            }
            context.ExceptionHandled = true;

            HttpResponse response = context.HttpContext.Response;
            response.StatusCode = (int)status;
            response.ContentType = "application/json";
            var err = message + " " + context.Exception.StackTrace;
            response.WriteAsync(err);
        }
    }
}
