using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.UserProfiles
{
    public static class RequestHandler
    {
        public static async Task<IActionResult> HandleRequest<T>
            (T request, Func<T, Task> hander, ILogger log)
        {
            try
            {
                log.Debug("Handling Htttp Request of type{type}", typeof(T).Name);
                await hander(request);
                return new OkResult();
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error Handling the request");
                return new BadRequestObjectResult(new
                {
                    error =
                    ex.Message,
                    stackTrace = ex.StackTrace
                });
            }

        }
        public static async Task<IActionResult> HandleQuery<TModel>(
            Func<Task<TModel>> query, ILogger log)
        {
            try
            {
                return new OkObjectResult(await query());
            }
            catch (Exception e)
            {
                log.Error(e, "Error handling the query");
                return new BadRequestObjectResult(new
                {
                    error = e.Message,
                    stackTrace = e.StackTrace
                });
            }
        }
    }
}
