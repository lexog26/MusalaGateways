# This project defines Musala Soft Gateways test project

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
## MusalaGateways.GatewayApi [src/MusalaGateways.GatewayApi]
	- Gateways Api definition with CRUD methods
## MusalaGateways.BusinessLogicUnitTest [test/MusalaGateways.BusinessLogicUnitTest]
	- Unit tests for BusinessLogic services (GatewayService,DeviceService)

#Instalation Guides
