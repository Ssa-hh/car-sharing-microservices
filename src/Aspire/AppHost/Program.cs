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

var keycloakUsername = builder.AddParameter("keycloak-username", secret:true);
var keycloakPassword = builder.AddParameter("keycloak-password", secret: true);
var keycloak = builder.AddKeycloak("keycloak", 8001, keycloakUsername, keycloakPassword)
    .WithDataVolume()
    .WithRealmImport("./carsharing-realms.json")
    ;

var rmqUsername = builder.AddParameter("message-broker-username", secret: true);
var rmqPassword = builder.AddParameter("message-broker-password", secret: true);
var rabbitMq = builder.AddRabbitMQ("rmq-message-broker", rmqUsername, rmqPassword)
    .WithManagementPlugin()
    .WithDataVolume();

var mongoUsername = builder.AddParameter("mongo-username", secret: true);
var mongoPassword = builder.AddParameter("mongo-password", secret: true);
var mongo = builder.AddMongoDB("ride-mongodb", null, mongoUsername, mongoPassword)
    .WithMongoExpress()
    .WithDataVolume();

var mongoDb = mongo.AddDatabase("ridedb");

builder.AddProject<Projects.Ssa_CarSharing_Users_API>("ssa-carsharing-users-api")
    .WithReference(postgresDb)
    .WithReference(keycloak)
    .WithReference(rabbitMq)
    .WaitFor(postgresDb)
    .WaitFor(keycloak)
    .WaitFor(rabbitMq);

builder.AddProject<Projects.Ssa_CarSharing_Rides_Api>("ssa-carsharing-rides-api")
    .WithReference(rabbitMq)
    .WithReference(mongoDb)
    .WaitFor(rabbitMq)
    .WaitFor(mongoDb);

builder.Build().Run();
