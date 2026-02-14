var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.ServerContainersManager>("servercontainersmanager");

builder.Build().Run();
