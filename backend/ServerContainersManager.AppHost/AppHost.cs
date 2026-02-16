using Microsoft.Extensions.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

var aspNetCoreDevEnvironment = builder.AddParameter("aspnetcoreDevEnvironment", Environments.Development);
var dotNetDevEnvironment = builder.AddParameter("dotNetDevEnvironment", Environments.Development);

var serverContainerManagerApi = builder.AddContainer(
        name: "server-container-manager-api", 
        image: "server-container-manager-api", 
        tag: "dev")
    .WithDockerfile("..")
    .WithBindMount("/var/run/docker.sock", "/var/run/docker.sock")
    .WithEndpoint(port: 8080, targetPort: 8080, name: "http", scheme: "http")
    .WithEnvironment("ASPNETCORE_ENVIRONMENT", aspNetCoreDevEnvironment)
    .WithEnvironment("DOTNET_ENVIRONMENT", dotNetDevEnvironment);

await builder.Build().RunAsync();
