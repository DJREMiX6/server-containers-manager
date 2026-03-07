using Projects;

var builder = DistributedApplication.CreateBuilder(args);

// Uncomment the following code to run the API project as a container in development environment.
/*var aspNetCoreDevEnvironment = builder.AddParameter("aspnetcoreDevEnvironment", Environments.Development);
var dotNetDevEnvironment = builder.AddParameter("dotNetDevEnvironment", Environments.Development);

var serverContainerManagerApi = builder.AddContainer(
        name: "server-container-manager-api", 
        image: "server-container-manager-api", 
        tag: "dev")
    .WithDockerfile("..")
    .WithBindMount("/var/run/docker.sock", "/var/run/docker.sock")
    .WithEndpoint(port: 8080, targetPort: 8080, name: "http", scheme: "http")
    .WithEnvironment("ASPNETCORE_ENVIRONMENT", aspNetCoreDevEnvironment)
    .WithEnvironment("DOTNET_ENVIRONMENT", dotNetDevEnvironment);*/

/*var sqliteDb = builder.AddSqlite("AppDb", databaseFileName: "data.sqlite")
    .WithSqliteWeb();*/

var serverContainerManagerApi = builder.AddProject<ServerContainerManager_API>("server-container-manager-api")
    /*.WithReference(sqliteDb)
    .WaitFor(sqliteDb)*/;

await builder.Build().RunAsync();
