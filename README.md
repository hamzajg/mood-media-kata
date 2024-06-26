# Mood Media Kata
## Kata
### Given
- The database diagram in the databaseDiagram.jpeg file.   
```mermaid
erDiagram
    COMPANY {
        Id int PK "Identity"
        Name nvarchar(255) "not null"
        Code nvarchar(50) "not null UNIQUE"
        Licensing int "not null"
    }

    LOCATION {
        Id int PK "Identity"
        Name nvarchar(255) "not null"
        Address nvarchar(max) "not null"
        ParentId int FK "not null"
    }

    DEVICE {
        Id int PK "Identity"
        SerialNumber nvarchar(255) "not null UNIQUE"
        Type int "not null"
        LocationId int FK "not null"
    }

    COMPANY ||--o{ LOCATION : ""
    LOCATION ||--|| DEVICE : ""
```
- Two action/message types:
- Create Company - json payload can be found in createCompany.json  
```json
{
   "Id":"0BA545F1-64C8-487C-988F-1B466A06B30F",
   "MessageType":"NewCompany",
   "CompanyName":"My Company 1",
   "CompanyCode":"COMP-123",
   "Licensing":"Standard",
   "Devices":[
      {
         "OrderNo":"1",
         "Type":"Standard"
      },
      {
         "OrderNo":"2",
         "Type":"Custom"
      }
   ]
}
```
- Delete Devices - json payload can be found in deleteDevices.json  
```json
{
   "Id":"0BA545F1-64C8-487C-988F-1B466A06B30F",
   "MessageType": "DeleteDevices",
   "SerialNumbers": ["Serial1", "Serial2"]
}
```

### Then
- Build a .NET console application to process the two message types above and perform
the necessary actions according to the MessageType property of the payload:
- Create Company:
  -  Messages of type NewCompany.
  - A new Company entity with its associated Location(s) and Device(s)
entities must be created in the database
  - Locations will be named using the following format: Location {index}
  - For each entity within the Devices array of the message payload, an
individual location must be created.
  - E.g. the first device in the list goes to Location 1, the second
device goes to Location 2, etc.
  - Each device has a unique serial number which must be generated by the
application. Please use whatever technique you choose to achieve this.
  - The type of a device can be: Custom or Standard.
- Delete devices
  - Messages of type DeleteDevices.
  - The SerialNumbers array of the message payload contains the serial
numbers of the devices to be deleted.
### Observations
- The goal of this exercise is to show off your coding skills. Performance of the application
is not important.
- Tests are advisable but we do not require a TDD approach or specific coverage.
- No Entity Framework. We’d like to see how you write queries.

## Solution
### Design Overview
``` mermaid
graph

    subgraph MoodMediaKata.WebApp
        style MoodMediaKata.WebApp fill:#9ff,stroke:#333,stroke-width:2px
        W1[htmx Client Web App]
    end
    subgraph MoodMediaKata.APIs
        style MoodMediaKata.APIs fill:#9f9,stroke:#333,stroke-width:2px
        API1[Companies REST APIs]
        API2[MessagingPublisher]
    end

    subgraph MoodMediaKata
        style MoodMediaKata fill:#f9f,stroke:#333,stroke-width:2px
        I1[SqlRepository]
        I2[MongoDbRepository]
        I3[QueueBusMessageProcessor]
        I4[ConsoleMessageProcessor]
        I5[MessagingSubscriber]
        B1[Startup]
        B2[Program]
    end

    subgraph MoodMediaKata.Company
        style MoodMediaKata.Company fill:#f9f,stroke:#333,stroke-width:2px
        C1[AddCompanyUseCase]
        C2[AddDevicesUseCase]
        c3[DeleteDevicesUseCase]
        C4[AddDevicesUseCase]
    end

    subgraph MoodMediaKata.App
        style MoodMediaKata.App fill:#f9f,stroke:#333,stroke-width:2px
        A1[Entity]
        A2[IdGenerator]
        A3[IRepository]
        A4[InMemoryRepository]
    end
```
### Use Cases
```mermaid
graph

    CreateCompanyUseCase["Create Company Use Case"]
    DeleteDevicesUseCase["Delete Devices Use Case"]
    QueryCompanyByIdUseCase["Query Company By Id Use Case"]

    style CreateCompanyUseCase fill:#f9c,stroke:#333,stroke-width:2px
    style DeleteDevicesUseCase fill:#f96,stroke:#333,stroke-width:2px
    style QueryCompanyByIdUseCase fill:#9cf,stroke:#333,stroke-width:2px
```
### Technologies
```mermaid
graph TD
    RabbitMQ["RabbitMQ"]
    SQLServer["SQL Server"]
    Swagger["Swagger"]
    MongoDB["MongoDB"]
    Docker["Docker"]
    HTMX["htmx"]
    Dapper["Dapper"]
    EntityFramework["Entity Framework"]

    style RabbitMQ fill:#FFBF00,stroke:#333,stroke-width:2px
    style SQLServer fill:#E34C26,stroke:#333,stroke-width:2px
    style Swagger fill:#85EA2D,stroke:#333,stroke-width:2px
    style MongoDB fill:#47A248,stroke:#333,stroke-width:2px
    style Docker fill:#2496ED,stroke:#333,stroke-width:2px
    style HTMX fill:#0C4B33,stroke:#333,stroke-width:2px
    style Dapper fill:#007ACC,stroke:#333,stroke-width:2px
    style EntityFramework fill:#68217A,stroke:#333,stroke-width:2px
```

## Usage
1. Run Infrastructure:  
```sh
docker compose -f docker-compose.infra.yml up
```

2. Run MoodMediaKata.APIs:  
```sh
cd MoodMediaKata.APIs
dotnet build
dotnet run
```

3. Run MoodMediaKata 
This project support multi setup and tools at runtime, by entering the args values for `--db-orm=[none/dapper/ef]`, `--repository=[InMemory/PostgreSql/Sql/MongoDb]` and `--message-processor=[RabbitMq/Console]`.  

Build:
```sh
cd MoodMediaKata
dotnet build
```
Run:
- No ORM & InMemory Repositoty & RabbitMq MessageProcessor
```sh
dotnet run --db-orm=none --repository=InMemory --message-processor=rabbitmq
```
- Dapper ORM & PostgresSql Repositoty & RabbitMq MessageProcessor
```sh
dotnet run --db-orm=dapper --repository=PostgreSql --message-processor=rabbitmq
```
- EntityFramework ORM & SqlServer Repositoty & RabbitMq MessageProcessor
```sh
dotnet run --db-orm=ef --repository=Sql --message-processor=rabbitmq
```
- EntityFramework ORM & MongoDb Repositoty & RabbitMq MessageProcessor
```sh
dotnet run --db-orm=ef --repository=MongoDb --message-processor=rabbitmq
```
