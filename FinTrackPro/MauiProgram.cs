﻿using FinTrackPro.Services;
using FinTrackPro.Services.Interface;
using Microcharts.Maui;
using Microsoft.Extensions.Logging;
using MudBlazor.Services;

namespace FinTrackPro
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddSingleton<UserService>();   
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddSingleton<TransactionService>();
            builder.Services.AddSingleton<DebtService>();



#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
            builder.Services.AddMudServices();



#endif

            return builder.Build();
        }
    }
}
