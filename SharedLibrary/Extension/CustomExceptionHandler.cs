using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using SharedLibrary.Dtos;
using SharedLibrary.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SharedLibrary.Extension
{
    public static class CustomExceptionHandler
    {
        public static void UseCustomException(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(config =>
            {
                //Run, sonlandırıcı middlewaredir yani hata meydana geldiğinde request devam etmez.
                config.Run(async context =>
                {
                    context.Response.StatusCode = 500;
                    context.Response.ContentType = "application/json";

                    var errorFeature = context.Features.Get<IExceptionHandlerFeature>();

                    if(errorFeature != null)
                    {
                        var exception = errorFeature.Error; //hata geldi.

                        ErrorDto errorDto = null;

                        if (exception is CustomException)
                        { 
                            errorDto = new ErrorDto(exception.Message,true);
                        }

                        else
                        {
                            errorDto = new ErrorDto(exception.Message, false);
                        }

                        var response = Response<NoDataDto>.Fail(errorDto,500);

                        await context.Response.WriteAsync(JsonSerializer.Serialize(response));

                    }
                });
            });
        }
    }
}
