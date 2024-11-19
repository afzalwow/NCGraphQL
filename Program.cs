using GettingStarted.interfaces;
using NCGraphQL.Services;

var config = new ConfigurationBuilder()
.SetBasePath(Directory.GetCurrentDirectory())
.AddJsonFile("appsettings.json").Build();

var builder = WebApplication.CreateBuilder(args);
builder
    .Services
        .AddHttpClient("nc", HttpClient =>{
            HttpClient.BaseAddress = new Uri(config["NC_API_BASE_URL"]!); // ! is used to avoid the nullable reference 
        });

builder.Services
.AddSingleton<IMessageService, MessageService>();

builder
.AddGraphQL()
.AddTypes()
.AddMutationType<Mutation>();

var app = builder.Build();

app.MapGraphQL();

app.RunWithGraphQLCommands(args);


