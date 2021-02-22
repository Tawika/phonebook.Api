using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PhonebookApi.Models.Models.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace PhonebookApi.Extensions
{
  public static class ExceptionMiddlewareManager
  {
    public static void ConfigureExceptionHandler(this IApplicationBuilder app, ILogger logger)
    {
      app.UseExceptionHandler(appError =>
      {
        appError.Run(async context =>
        {
          context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
          context.Response.ContentType = "application/json";
          var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
          if (contextFeature != null)
          {
            logger.LogError($"Something went wrong: {contextFeature.Error}");
            await context.Response.WriteAsync(new Error()
            {
              StatusCode = context.Response.StatusCode,
              Message = "Internal Server Error."
            }.ToString());
          }
        });
      });
    }
  }
}
