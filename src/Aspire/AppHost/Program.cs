var builder = DistributedApplication.CreateBuilder(args);


// TODO: add dn user name and password
// var username = builder.AddParameter("sa", secret: true);
// var password = builder.AddParameter("pw", secret: true);

var postgres = builder.AddPostgres("postgres")
    .WithPgAdmin()
    .WithDataVolume();
var postgresDb = postgres.AddDatabase("userdb");

builder.AddProject<Projects.Ssa_CarSharing_Users_API>("ssa-carsharing-users-api")
    .WithReference(postgresDb)
    .WaitFor(postgresDb);

builder.Build().Run();
