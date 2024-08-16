namespace SelenMebel.Api.Middlewares
{
    public class TokenRedirectMiddleware
    {
        private readonly RequestDelegate _next;
        public TokenRedirectMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task InvokeAsync(HttpContext httpContext)
        {
            if (httpContext.Request.Cookies.TryGetValue("X-Access-Token", out var accessToken))
            {
                if (!string.IsNullOrEmpty(accessToken))
                {
                    string bearerToken = string.Format("Bearer {0}", accessToken);
                    httpContext.Request.Headers.Append("Authorization", bearerToken);
                }
            }
            return _next(httpContext);
        }
    }
}
