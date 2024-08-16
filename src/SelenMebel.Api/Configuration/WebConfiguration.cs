﻿namespace SelenMebel.Api.Configuration
{
    public static class WebConfiguration
    {
        public static void AddWeb(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureAuth(configuration);
        }
    }
}
