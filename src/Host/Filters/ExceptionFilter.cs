using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace IdentityServer4.Filters
{
    /// <summary>
    /// Handle all exceptions that are not caught and returned.
    /// AspNetCore will throw 500 but here we can log those.
    /// </summary>
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.ExceptionHandled == false)
            {
                //TODO:ba way to DI logger???
                // logger.Error("Exception thrown", context.Exception);

                context.ExceptionHandled = true;
                context.Result = new StatusCodeResult(500);
            }
        }
    }
}
