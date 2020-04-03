# ToolsToLive.JsonSourceLocalizer
Localizer that uses json files (just like ngx-translate in angular).
Designed for use in the same way as NGX translate in Angular applications. It is much more convenient to use the same (with the same structure) files for the backend and frontend.

## How to use
An example with dotnet core. For dotnet framework you can use it the same way but most likely you will have to set up dependency injetion manually.

### Prerequisite
 - Reference to ToolsToLive.JsonSourceLocalizer (you can found it in nuget);
 - JSON file (or files) with translations (if you use ngx-translate, you definitely familiar with structure of such files);
 - Set up dependency injection by calling "serivces.AddJsonSourceLocalizer(localizationOptionsSection)" in your Startup.cs file.
 - Inject "IUserManagamentService" wherever you need localization.

 file en.json:
 ```json
    {
      "some namespace": {
        "internal namespace": {
          "one more namespace": {
            "term 1": "value 1 in current language",
            "term 2": "value 2 in current language",
            "term 3": "value 3 in current language"
          }
        }
      },
      "sign-up": {
        "validation-errors": {
          "No username": "Enter user name",
          "Too long": "User name too long, please enter no more then 50 chars"
        }
      }
    }
```
Note: file with russian language probably will be named as "ru.json". Anyway, you should use the same file names, as you send culture to localize() method. For example if you pass "de" as culture, you should have "de.json" file, otherwise an error will be thrown.

 ```csharp
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var localizationOptionsSection = Configuration.GetSection("Localization");
            services.AddJsonSourceLocalizer(localizationOptionsSection);

            //// or
            //services.AddJsonSourceLocalizer(settings =>
            //{
            //    settings.ResourceDirectoryPath = "PathWithFiles";
            //});

            // other code
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // other code
        }
    }
```
Note: if you pass configuration section to AddJsonSourceLocalizer() method, this section must contain "ResourceDirectoryPath" key.

 ```csharp
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IJsonSourceLocalizer _toolsToLiveLocalizer;

        public AccountController(
            IJsonSourceLocalizer toolsToLiveLocalizer)
        {
            _toolsToLiveLocalizer = toolsToLiveLocalizer;
        }

        [HttpGet]
        public async Task<string> GetMessage([FromQuery]string userName)
        {
            return  await _toolsToLiveLocalizer.Localize("sign-up.validation-errors.Too long", "en");
        }
    }
```

Note: Mehods to localize are async.