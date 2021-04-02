# Clean Architecture and Domain Driven Design

Starting point for Clean Architecture and Domain-Driven Design projects.

Repos that inspired this template

- https://github.com/jasontaylordev/CleanArchitecture
- https://github.com/ardalis/CleanArchitecture
- https://github.com/thombergs/buckpal

## Learn about Clean architecture and Domain Driven Design

### Books

- [Clean Architecture: A Craftsman's Guide to Software Structure and Design](https://www.amazon.com/Clean-Architecture-Craftsmans-Software-Structure-ebook/dp/B075LRM681/ref=sr_1_1?dchild=1&keywords=clean+architecture&qid=1616938659&sr=8-1)
- [Get Your Hands Dirty on Clean Architecture: A hands-on guide to creating clean web applications with code examples in Java](https://www.amazon.com/Hands-Dirty-Clean-Architecture-hands-ebook/dp/B07YFS3DNF/ref=sr_1_12?dchild=1&keywords=clean+architecture&qid=1616938690&sr=8-12)
- [Domain-Driven Design: Tackling Complexity in the Heart of Software (a.k.a The Blue Book)](https://www.amazon.com/Domain-Driven-Design-Tackling-Complexity-Software-ebook-dp-B00794TAUG/dp/B00794TAUG/ref=mt_other?_encoding=UTF8&me=&qid=1616938355)
- [Implementing Domain-Driven Design (a.k.a The Red Book)](https://www.amazon.com/Implementing-Domain-Driven-Design-Vaughn-Vernon-ebook/dp/B00BCLEBN8/ref=sr_1_3?crid=2KU25HXMXASLH&dchild=1&keywords=domain+driven+design&qid=1616938447&s=digital-text&sprefix=domain+dr%2Cdigital-text%2C228&sr=1-3)
- [Patterns, Principles, and Practices of Domain-Driven Design](https://www.amazon.com/Patterns-Principles-Practices-Domain-Driven-Design-ebook/dp/B00XLYUA0W/ref=sr_1_4?crid=2KU25HXMXASLH&dchild=1&keywords=domain+driven+design&qid=1616938497&s=digital-text&sprefix=domain+dr%2Cdigital-text%2C228&sr=1-4)
- [Secure by Design](https://www.manning.com/books/secure-by-design)

### Talks

- [GOTO 2019 • Clean Architecture with ASP.NET Core 3.0 • Jason Taylor](https://www.youtube.com/watch?v=dK4Yb6-LxAk&t=600s)
- [Robert C Martin - Clean Architecture](https://www.youtube.com/watch?v=Nltqi7ODZTM)

## Give a Star! :star:

If you like or are using this project to learn or start your solution, please give it a star. Thanks!

## Versions

The master branch is using .NET 5. 

# Goals

The goal of this repository is to provide a starting point to implement Clean Architecture and Domain-Driven design. 
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
- The App project is the only project that knows about all projects in the solution. It composes all the dependencies and runs the application.    

## Relation between projects

As the rule of Clean Architecture indicates layers point inwards in the diagram. This means that:

- Domain project does not know about any other projects
- Application.Services only knows about the Domain
- WebApi knows about the Application.Services
- Persistence knows about the Application.Services

![Project structure](https://docs.google.com/drawings/d/e/2PACX-1vQF_JYDw08PLfhVI16qDfn6vNFJePFReCFCAi5Vv1Jgy_1K4IBWeUgtxHpzXeUH3UECZqVPiIMnn8mN/pub?w=960&h=720)

