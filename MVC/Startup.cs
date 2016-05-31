public void ConfigureServices(IServiceCollection services)
        {
            var settings = Settings.New();

            services.AddMvc(config =>
            {
                config.Filters.Add(new ServiceFilterAttribute(typeof(WebApiExceptionAttribute)));
                config.Filters.Add(new ProducesAttribute(typeof(ApiResult<object>)));
            });

            services.AddCors(options =>
                options.AddPolicy("AllowSpecificOrigin", builder =>
                {
                    builder.WithOrigins(settings.AllowedOrigins)
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials();
                })
            );

            //add the custom attributes that require dependency injection
            services.AddScoped<WebApiExceptionAttribute>();

            //add all dependency injection for reuse
            services.AddSingleton<ILog, Log>();
            services.AddInstance<ISettings>(settings);
            
            //add all of your repositories below
            //services.AddSingleton<IUserUnitOfWork, UserUnitOfWorkEF>();
            services.AddSingleton<IUserUnitOfWork, UserUnitOfWorkAdo>();
        }
