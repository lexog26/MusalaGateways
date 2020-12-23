# MusalaSoft Gateways project [https://github.com/lexog26/MusalaGateways.git]

# Project Structure

## MusalaGateways.Domain [src/MusalaGateways.Domain]
	- Domain entitites
## MusalaGateways.Enums [src/MusalaGateways.Enums]
	- Common enums
## MusalaGateways.DataTransferObjects [src/MusalaGateways.DataTransferObjects]
	- Data transfer objects for REST Service web exchanges or others, based on domain entities
## MusalaGateways.DataLayer [src/MusalaGateways.DataLayer]
	- Database context definitions
	- Domain entities db mapping configurations using Fluent API
	- Repository design pattern
	- Unit of Work design pattern
	- Migrations
## MusalaGateways.BusinessLogic [src/MusalaGateways.BusinessLogic]
	- Mapper definition for dto <--> entitites mapping
	- Business logic services. For instance: GatewayService
## MusalaGateways.Api [src/MusalaGateways.GatewayApi]
	- Gateways Api definition with CRUD methods
## MusalaGateways.BusinessLogicUnitTest [test/MusalaGateways.BusinessLogicUnitTest]
	- Unit tests for BusinessLogic services (GatewayService,DeviceService)

## Output project MusalaGateways.Api

#Instalation Guides

1- Clone the project [https://github.com/lexog26/MusalaGateways.git]
2- Build the solution
3- Set database in MusalaGateways.Api project
Change field InMemoryDb in appsettings.json file, set it to true if you want to use db in memory or false to use mssql db

{
	........

  "InMemoryDB": true

    .........
}

If you want to use mssql db, need to specify mssql database connection string in appsettings.json file

{
	........

  "InMemoryDB": false,
  "ConnectionStrings": {
    "MusalaConnectionString": "Data Source=DESKTOP-N8TAT47;Database=musala;Connect Timeout=30;Trusted_Connection=True;MultipleActiveResultSets=true"
  }

    .........
}

Also needs to run migrations defined in MusalaGateways.DataLayer project [src/MusalaGateways.DataLayer/Migrations]

4- Set authorization configuration in appsettings.json file [Auth section], this project uses IdentityServer4 nuget package 
as Oauth2 and OpenId implementation with client credentials flow and Bearer authorization scheme. By default it has
values defined in IdentityConfig.cs file [src/MusalaGateways.Api/IdentityConfig.cs].

Default auth configuration

{
	........

  "Auth": {
    "Authority": "http://localhost:5000",
    "ClientId": "MusalaGatewaysApiSwagger",
    "ClientSecret": "musalaGatewaysSecret",
    "Scope": "gatewaysApi"
  }

    .........
}
**For default auth configuration the field "Auth:Authority" must be equal to MusalaGateways.Api project's url

Custom Oauth2 and OpenId implementation

{
	........

  "Auth": {
    "Authority": "your authority provider url",
    "ClientId": "your client_id",
    "ClientSecret": "your client_secret",
    "Scope": "your scope"
  }

    .........
}

5- Run the MusalaGateways.Api project

# Default UI
 
SwaggerUI is the default MusalaGateways.Api project UI, using Swashbucle.AspNetCore nuget package. This tool provides
api documentation and can use for test the api endpoints. Also used authorization flow described in step 4.

# Automated build

The file azure-pipelines.yml [src/MusalaGateways.api/azure-pipelines.yml] defines an Azure DevOps pipeline for automated
build and deploy. It uses the github repository url and AzureDevops account