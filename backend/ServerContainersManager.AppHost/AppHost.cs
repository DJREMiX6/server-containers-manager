using System.Net.Sockets;

var builder = DistributedApplication.CreateBuilder(args);

var serverContainerManagerApi = builder.AddContainer("server-container-manager-api", "server-container-manager-api")
    .WithDockerfile("..")
    .WithBindMount("/var/run/docker.sock", "/var/run/docker.sock")
    .WithEndpoint(port: 8080, targetPort: 8080, name: "http");

await builder.Build().RunAsync();
