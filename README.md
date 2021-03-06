# Clean Architecture and Domain Driven Design

Simple project showing casing Clean Architecture and Domain-Driven Design projects.

Repos that inspired this one

- https://github.com/jasontaylordev/CleanArchitecture
- https://github.com/ardalis/CleanArchitecture
- https://github.com/thombergs/buckpal

## Learn about Clean architecture and Domain Driven Design

- [My clean architecture resources](https://github.com/dgomezs/awesome-clean-architecture)
- [My Domain Driven Design resources](https://github.com/dgomezs/awesome-ddd)

## Give a Star! :star:

If you like or are using this project to learn or start your solution, please give it a star. Thanks!

## Versions

The master branch is using .NET 5.

# Goals

The aim of the repo is to provide with a simple but complete set of features that show case the main concepts behind
clean architecture and Domain-Driven design.

# Solution structure

## Relation between Clean Architecture and the project structure

![Clean Architecture Diagram](https://blog.cleancoder.com/uncle-bob/images/2012-08-13-the-clean-architecture/CleanArchitecture.jpg "Clean Architecture ")
*[From The Clean Architecture Blog Post](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)*

- Core folder (Entities, Use Cases)
    - Domain project contains the Entities, Aggregates and other elements of the domain
    - Application.Services project contains the Use cases of the application
- Infrastructure folder (Controllers, Gateways, Presenters)
    - Persistence project contains the persistence and retrieval mechanisms for our entities, aggregates and Read models
    - WebApi project contains the Http Controllers that expose the use cases of the application
- The App project is the only project that knows about all projects in the solution. It composes all the dependencies
  and runs the application.

## Relation between projects

As the rule of Clean Architecture indicates layers point inwards in the diagram. This means that:

- Domain project does not know about any other projects
- Application.Services only knows about the Domain
- WebApi knows about the Application.Services
- Persistence knows about the Application.Services

![Project structure](https://docs.google.com/drawings/d/e/2PACX-1vQF_JYDw08PLfhVI16qDfn6vNFJePFReCFCAi5Vv1Jgy_1K4IBWeUgtxHpzXeUH3UECZqVPiIMnn8mN/pub?w=960&h=720)

## Secrets

The following secrets have to be configured per project. If you are not familiar with the concept of user secrets find
more information [here](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets)

### App project

In production DB users for DBUP and MSSQL should be different with different permissions

```
{
    "DBUP_USERID": "SA", // ID of the user for DBUP
    "DBUP_PASSWD": "TodoList@Demo", // password for DBUP
    "MSSQL_USERID": "SA", // ID of the user for MSSQL
    "MSSQL_PASSWD": "TodoList@Demo", // password for MSSQL
    "AUTH_ISSUER": "https://[YOUR_AUTH0_DOMAIN].auth0.com/", // replace with your auth0 domain 
    "AUTH_AUDIENCE": "XXX",  // replace with your audience example: https://todo-list-api
    "AUTH0_CLIENT_ID":  "XXX",  // replace with your auth0 client id
    "AUTH0_CLIENT_SECRET" : "XXX" // replace with your auth0 client secret
}
```

### Persistence.Tests

```
{
  "DBUP_USERID": "SA",
  "DBUP_PASSWD": "TodoList@Demo",
  "MSSQL_USERID": "SA",
  "MSSQL_PASSWD": "TodoList@Demo"
}
```

### Auth0.Tests

```
{
  "AUTH_ISSUER": "https://[YOUR_AUTH0_DOMAIN].auth0.com/",
   "AUTH_AUDIENCE": "XXX", 
  "AUTH0_CLIENT_ID":  "XXX",  
  "AUTH0_CLIENT_SECRET" : "XXX",
  "AUTH0_TEST_EMAIL_ADDRESS" : "XXX" // replace with an email adddress of a user that must exist on auth0
}

```
