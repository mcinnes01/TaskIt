using Microsoft.AspNetCore.Http;
using System;

namespace TaskIt.Helpers
{
    public static class AjaxHelpers
    {
        public static bool IsAjaxRequest(this HttpRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }
            return request != null && request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }
    }
}
