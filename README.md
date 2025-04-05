# Members API

![Build Status](https://github.com/Palatino/MembersAPI/actions/workflows/master_membersapiapp.yml/badge.svg)
[![codecov](https://codecov.io/gh/Palatino/MembersAPI/graph/badge.svg?token=QHK8TOE0BS)](https://codecov.io/gh/Palatino/MembersAPI)

Simple web API built using .NET8, ASP.NET, and Entity Framework Core.

It contains five endpoints to manage memberships:

![image](https://github.com/user-attachments/assets/5d17089a-659d-46b7-b3f5-fca1f80bedcc)


## Code Structure
The solution is broken in five different projects:
![image](https://github.com/user-attachments/assets/1eac901e-fb33-4978-b5c8-074616beb0a1)

**Domain** : Contains the definition of the different entities that will stored in the DB.

**Infrastructure**: Contains all the DB related files (Migrations, configurations etc.)

**Contracts**: All domain objects are mapped to equivalent DTO, these DTOs are the different inputs and outputs of the API. All DTOs are defined in this project. This project is published as a Nuget package using a custom pipeline (https://www.nuget.org/packages/Palatino.MembersAPI.Contracts). The nuget can used by any client application, avoiding the need for redefining the types. This project is a .NET Standard 2.0 so it can be used even by old .Net Framework 4.8 applications.

**Application**: This project contains all the business logic

**Api**: Contains the API itself, including controller, end points, and so on.

## Unit Testing 

Unit testing is implemented using XUnit, NSubstitute and Fluent Assertions. Tests run automatically when the master branch changed, if tests fail deployment is cancelled. Deployment status and test coverage are reported at the top of this README.
