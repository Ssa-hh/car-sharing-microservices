using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);


// TODO: add dn user name and password
// var username = builder.AddParameter("sa", secret: true);
// var password = builder.AddParameter("pw", secret: true);

// Add a PostgreSQL database with pgAdmin and a data volume
var postgres = builder.AddPostgres("postgres")
    .WithPgAdmin()
    .WithDataVolume();
var postgresDb = postgres.AddDatabase("userdb");

var keycloakUserName = builder.AddParameter("keycloak-username", secret:true);
var keycloakPassword = builder.AddParameter("keycloak-password", secret: true);
var keycloak = builder.AddKeycloak("keycloak", 8001, keycloakUserName, keycloakPassword)
    .WithDataVolume()
    //.WithRealmImport("./carsharing-realms.json")
    ;

var rmqUserName = builder.AddParameter("message-broker-username", secret: true);
var rmqPassword = builder.AddParameter("message-broker-password", secret: true);
var rabbitMq = builder.AddRabbitMQ("rmq-message-broker", rmqUserName, rmqPassword)
    .WithManagementPlugin()
    .WithDataVolume();

builder.AddProject<Projects.Ssa_CarSharing_Users_API>("ssa-carsharing-users-api")
    .WithReference(postgresDb)
    .WithReference(keycloak)
    .WithReference(rabbitMq)
    .WaitFor(postgresDb)
    .WaitFor(keycloak)
    .WaitFor(rabbitMq);

builder.AddProject<Projects.Ssa_CarSharing_Rides_Api>("ssa-carsharing-rides-api")
    .WithReference(rabbitMq)
    .WaitFor(rabbitMq);

builder.Build().Run();
