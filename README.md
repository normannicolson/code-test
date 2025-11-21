# Code test booking api ticketing api

## Backend Developer Challenge

### Introduction

This technical task is designed to see how you approach a complex problem. We are
looking to understand how you break down your solution, what you consider when you
are making your decisions, how you write and structure code.

We are not necessarily looking for a fully working or a bug free solution. After you
have submitted you will have the opportunity to talk through your approach, what you
found challenging and why you made the decisions you did.

We think that a reasonable solution for discussion could be produced within a few
hours but feel free to spend as long or as short as you like on this.

### Brief

Create a hotel room booking API using ASP.NET Core and Entity Framework (EF) Core,
your solution must be written in C# following RESTful principles.

The solution should be committed to an online repository and access shared with us. 

If you have any supporting documentation, please include this in the repository.

If possible, it should be hosted in an Azure environment (free trials are available),
please note this is not a critical requirement.

Use the database of your choosing.

### Business Rules

- Hotels have 3 room types: single, double, deluxe.
- Hotels have 6 rooms.
- A room cannot be double booked for any given night.
- Any booking at the hotel must not require guests to change rooms at any point
during their stay.
- Booking numbers should be unique. There should not be overlapping at any
given time.
- A room cannot be occupied by more people than its capacity.

### Requirements

The system should provide the following functionality through a well designed API.

#### Business Functionality

Your solution must allow an API consumer to perform the following:

- Find a hotel based on its name.
- Find available rooms between two dates for a given number of people.
- Book a room.
- Find booking details based on a booking reference.

### Technical Requirements

- The API must be testable.
    - OpenAPI / Swagger documentation should be made available for testing.
    - For testing purposes the API should expose functionality to allow for
    seeding and resetting the data:
        - Seeding: Populate database with just enough data for testing.
        - Resetting: Remove all data ready for seeding.
    - Consideration could be given to automated testing but is not essential to
    the deliverable.
- The API requires no authentication.

## Run Application 

from ```src``` folder

```
dotnet run --project Reserve.Presentation.Api
```

```
curl -i -X GET http://localhost:5140
```

Get Booking

```
curl -i -X GET http://localhost:5140/bookings/00000000-0000-0000-0000-000000000000
```

Response 
```
HTTP/1.1 200 OK
Content-Type: application/json; charset=utf-8
Date: Fri, 21 Nov 2025 15:36:56 GMT
Server: Kestrel
Transfer-Encoding: chunked

{"id":"bc32c120-0b06-47fb-88fb-61a6742ada1e","name":"Booking Name"}%     
```

Get available rooms

```
curl -i -X GET "http://localhost:5140/rooms/search?from=2026-06-05T15:00:00Z&to=2026-06-06T11:00:00Z&numberofpeople=2"
```

One rooms available 

```
curl -X GET "http://localhost:5140/rooms/search?from=2026-06-05T15:00:00Z&to=2026-06-07T11:00:00Z&numberofpeople=2" | jq
```

Both rooms available 

```
curl -X GET "http://localhost:5140/rooms/search?from=2026-06-06T15:00:00Z&to=2026-06-07T11:00:00Z&numberofpeople=2" | jq
```



Open API Swagger APIs 

http://localhost:5140/openapi/v1.json

http://localhost:5140/swagger/index.html

### Run tests 

from ```src``` folder

```
dotnet test Reserve.Core.Test
dotnet test Reserve.Application.Test
dotnet test Reserve.Infrastructure.Data.Test
```

or from repo root 

```
dotnet test
```

## How Application was created 

### Steps 

```
mkdir src
cd src
dotnet new classlib -n Reserve.Core
dotnet new mstest -n Reserve.Core.Test
```

Create Entities classes and starter tests 

Create Api 

```
dotnet new web -n Reserve.Presentation.Api
dotnet new mstest -n Reserve.Presentation.Api.Test 
```

Create Data project 

```
dotnet new classlib -n Reserve.Infrastructure.Data
dotnet new mstest -n Reserve.Infrastructure.Data.Test
```

Create Application project

```
dotnet new classlib -n Reserve.Application
dotnet new mstest -n Reserve.Application.Test
```

Add OpenApi & Swagger to ```Reserve.Presentation.Api``` project 

```
  <ItemGroup>
    <PackageReference Include="Microsoft.AspNetCore.OpenApi" Version="10.0.0" />
    <PackageReference Include="Swashbuckle.AspNetCore" Version="10.0.1" />
  </ItemGroup>
```

### Data structure 

Class Diagram 

```mermaid

classDiagram

Schedule --> Resource  
Schedule --> Slot 

Booking --> Resource  
Booking --> Slot 
```

Create Entities 

```mermaid

erDiagram

  Schedule ||--|{ Resource : "Rooms"

  Schedule ||--o{ Slot : "Slots/nights"

  Booking ||--|{ Slot : "Booking slots timespan"

  Booking ||--|{ Resource : "Booking rooms"

```

Database Entities 

```mermaid

erDiagram

  Schedule ||--|{ ScheduleResource : Resources
  Resource ||--|{ ScheduleResource : Resources

  Schedule ||--o{ ScheduleSlot : Slots
  Slot ||--o{ ScheduleSlot : Slots

  Booking ||--|{ BookingResource : Resources
  Resource  ||--|{ ScheduleResource : Resources

  Booking ||--|{ BookingSlot : Slots
  Slot ||--o{ BookingSlot : Slots

```

### Create CQRS classes in Application project 


### Implement Database 


### Implement Api 


### Create Database 

```

```


## Design 

### Using Fhir as a starting point

Resource - This is normally a HealthcareService, Practitioner, Location or Device. In the case where a single resource can provide different services, potentially at different location, then the schedulable resource is considered the composite of the actors. 

Schedule - A container for slots of time that may be available for booking appointments. A schedule controls the dates and times available for the performance of a service and/or the use of a resource.

Slot - A slot of time on a schedule that may be available for booking appointments.

Booking/Appointment - A booking of a healthcare event among patient(s), practitioner(s), related person(s) and/or device(s) for a specific date/time.

Resource
https://build.fhir.org/location.html
https://build.fhir.org/device.html

Schedule
https://build.fhir.org/schedule.html
https://build.fhir.org/schedule-definitions.html#Schedule.actor

Slot 
https://build.fhir.org/slot.html

Appointment
https://build.fhir.org/appointment.html

Workflow 
https://build.fhir.org/workflow-module.html

### Data Model

```mermaid

erDiagram

Resource ||--o{ Resource : "Parent resource"

Schedule ||--|{ Resource : "Resources"

Schedule ||--o{ Slot : "Slots"

Booking ||--|{ Slot : "Booking slots timespan"

Booking ||--|{ Resource : "Booking resource products"

```
