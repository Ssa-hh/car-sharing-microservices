# Carsharing Application — Microservices Demo with .NET Aspire ( work-in-progress)

## 1. Introduction

The main purpose of this repository is to build a carsharing application to demonstrate microservices architecture, Clean Architecture, and the CQRS pattern, using the .NET Aspire development stack.
The application consists of:
- Two microservices: Users and Rides
- A web frontend developed with Angular 19
- Asynchronous communication between services via RabbitMQ
- Identity and access management handled through Keycloak

Each service exposes REST APIs, and there are example requests in the Postman collection: Docs/Postman collection/Car sharing.postman_collection.json.

![](Docs/Images/resources_graph.png)

## Run The Project
You will need the following tools:

* [.Net Core 9](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
* [Docker Desktop](https://www.docker.com/products/docker-desktop)
* [Node.js 18.19.1 or later](https://nodejs.org/en/download)
* [Angular 19](https://angular.dev/installation)
* [Visual Studio 2022 (optionel)](https://visualstudio.microsoft.com/downloads/)

### Run the project using .NET CLI

1. Start Docker Desktop.
3. Open a command prompt.
4. Navigate to the CarSharing\src\Aspire\AppHost directory (this folder contains the AppHost.csproj file).
5. Run the command: dotnet run
   
   ![](Docs/Images/run_dotnet_command.png)
   
7. Once the application starts, the dashboard URL will be displayed at the end of the logs.

![](Docs/Images/dashboard_url.png)

![](Docs/Images/resources_table.png)

## Service APIs and Postman request Examples

Users and Rides services expose REST APIs. Example requests for each endpoint are included in the Postman collection located at: Docs/Postman collection/Car sharing.postman_collection.json.
Most endpoints require authentication via a Bearer Token, except for the following endpoints:
- Register User
- Login User
- Search Rides

To obtain an access token, use the Login User endpoint. See the example below.

### Register a user

![](Docs/Images/register_user_api.png)

### Login and get access token

![](Docs/Images/get_access_token.png)

### Add a car to a user

Set access token
![](Docs/Images/set_bearer_token.png)

Add a car
![](Docs/Images/add_car_to_user.png)

### Add a ride

Set access token
![](Docs/Images/set_bearer_token.png)

Add a ride
![](Docs/Images/add_a_ride.png)

### Search rides

![](Docs/Images/search_for_rides.png)

## FrontEnd application

Note: Even though the angular-app state is marked as started, the application may take some time to fully initialize.

### Features screenshots

#### Register user

![](Docs/Images/register_user_step_1.png)

![](Docs/Images/register_user_step_2.png)

#### Add car to user

![](Docs/Images/add_a_car_to_user.png)

#### Add ride

Step 1
![](Docs/Images/publish_a_ride_step_1.png)

Step 2
![](Docs/Images/publish_a_ride_step_2.png)

Step 3
![](Docs/Images/publish_a_ride_step_3.png)

Step 4
![](Docs/Images/publish_a_ride_step_4.png)

Step 5
![](Docs/Images/publish_a_ride_step_5.png)

Step 6
![](Docs/Images/publish_a_ride_step_6.png)

Step 7
![](Docs/Images/publish_a_ride_step_7.png)

Step 8
![](Docs/Images/publish_a_ride_step_8.png)

#### Search rides

![](Docs/Images/search_a_ride.png)

