var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.ServerContainerManager_API>("servercontainermanager-api");

builder.Build().Run();
