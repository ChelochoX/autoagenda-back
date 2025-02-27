﻿using autoagenda_back.Middlewares;

namespace autoagenda_back.Configurations;

public static class AppExtensions
{
    public static void UseHandlingMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<ErrorHandlingMiddleware>();
    }
}
