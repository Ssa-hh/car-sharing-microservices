var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Ssa_CarSharing_Users_API>("ssa-carsharing-users-api");

builder.Build().Run();
