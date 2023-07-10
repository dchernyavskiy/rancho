# 🛍️ ECommerece Microservices Sample

[![Coverage Status](https://coveralls.io/repos/github/mehdihadeli/ecommerce-microservices/badge.svg?branch=develop&style=flat-square)](https://coveralls.io/github/mehdihadeli/ecommerce-microservices?branch=develop)
[![semantic-release](https://img.shields.io/badge/%20%20%F0%9F%93%A6%F0%9F%9A%80-semantic--release-e10079.svg?&style=flat-square)](https://github.com/semantic-release/semantic-release)
[![Commitizen friendly](https://img.shields.io/badge/commitizen-friendly-brightgreen.svg?&style=flat-square)](http://commitizen.github.io/cz-cli/)

[![Open in Gitpod](https://gitpod.io/button/open-in-gitpod.svg)](https://gitpod.io/https://github.com/mehdihadeli/ecommerce-microservices)

[![Open in GitHub Codespaces](https://github.com/codespaces/badge.svg)](https://mehdihadeli-humble-space-couscous-5x5pqwwjx5c7664.github.dev)

> `ECommerece Microservices` is a fictional ecommerce sample, built with .Net Core and different software architecture and technologies like **Microservices Architecture**, **Vertical Slice Architecture** , **CQRS Pattern**, **Domain Driven Design (DDD)**, **Event Driven Architecture**. For communication between independent services, we use asynchronous messaging with using rabbitmq on top of [MassTransit](https://github.com/MassTransit/MassTransit) library, and sometimes we use synchronous communication for real-time communications with using REST and gRPC calls.

💡 This application is not business oriented and my focus is mostly on technical part, I just want to implement a sample with using different technologies, software architecture design, principles and all the thing we need for creating a microservices app.

> **Warning**
> This project is in progress. I add new features over the time. You can check the [Release Notes](https://github.com/mehdihadeli/ecommerce-microservices/releases) and follow the progress on Twitter [@mehdi_hedli](https://twitter.com/shadcn) and Linkedin [mehdihadeli](https://www.linkedin.com/in/mehdihadeli/).

🎯 This Application ported to `modular monolith` approach in [ecommerce-modular-monolith](https://github.com/mehdihadeli/ecommerce-modular-monolith) repository, we can choose best fit architecture for our projects based on production needs.

# ⭐ Support

If you like feel free to ⭐ this repository, It helps out :)

Thanks a bunch for supporting me!

# Table of Contents

- [Features](#features)
- [Plan](#plan)
- [Setup](#setup)
  - [Dev Certificate](#dev-certificate)
  - [Conventional Commit](#conventional-commit)
  - [Formatting](#formatting)
  - [Analizers](#analizers)
- [Technologies - Libraries](#technologies---libraries)
- [The Domain and Bounded Context - Service Boundary](#the-domain-and-bounded-context---service-boundary)
- [Application Architecture](#application-architecture)
- [Application Structure](#application-structure)
- [Vertical Slice Flow](#vertical-slice-flow)
- [Prerequisites](#prerequisites)
- [How to Run](#how-to-run)
  - [Using PM2](#using-pm2)
  - [Using Docker-Compose](#using-docker-compose)
  - [Using Tye](#using-tye)
  - [Using Kubernetes](#using-kubernetes)
- [Contribution](#contribution)
- [Project References](#project-references)
- [License](#license)

## Features

- ✅ Using `Vertical Slice Architecture` as a high level architecture
- ✅ Using `Event Driven Architecture` on top of RabbitMQ Message Broker and MassTransit
- ✅ Using `Domain Driven Design`in most of services like Customers, Catalogs, ...
- ✅ Using `Event Sourcing` in `Audit Based` services like Orders, Payment
- ✅ Using `Data Centeric Architecture` based on `CRUD` in Identity Service
- ✅ Using `CQRS Pattern` on top of `MediatR` library
- ✅ Using `Outbox Pattern` for all microservices for [Guaranteed Delivery](https://www.enterpriseintegrationpatterns.com/GuaranteedMessaging.html) or [At-least-once Delivery](https://www.cloudcomputingpatterns.org/at_least_once_delivery/)
- ✅ Using `Inbox Pattern` for handling [Idempotency](https://www.cloudcomputingpatterns.org/idempotent_processor/) in reciver side and [Exactly-once Delivery](https://www.cloudcomputingpatterns.org/exactly_once_delivery/)
- ✅ Using `Minimal APIs` for handling requests
- ✅ Using `Fluent Validation` and a [Validation Pipeline Behaviour](./src/BuildingBlocks/BuildingBlocks.Validation/RequestValidationBehavior.cs) on top of MediatR
- ✅ Using `Postgres` for write database as relational DB and `MongoDB` and `Elasric Search` for read database
- ✅ Using docker and `docker-compose` for deployment
- ✅ Using [Microsoft Tye](https://github.com/dotnet/tye) for deployment
- 🚧 Using `Helm` and `Kubernetes` for deployment
- 🚧 Using `OpenTelemetry` for collection `Metrics` and `Distributed Tracing`

## Plan

> This project is in progress, New features will be added over time.

| Feature          | Architecture Pattern                  | Status         | CI-CD                                                                                                                                                                                                                                                  |
| ---------------- | ------------------------------------- | -------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| API Gateway      | Microsoft YARP Reverse Proxy          | Completed ✔️   | [![Gateway-CI-CD](https://github.com/mehdihadeli/ecommerce-microservices/actions/workflows/gateway.yml/badge.svg?branch=develop&style=flat-square)](https://github.com/mehdihadeli/ecommerce-microservices/actions/workflows/gateway.yml)              |
| Identity Service | Data Centeric Architecture (CRUD)     | Completed ✔️   | [![Identity-CI-CD](https://github.com/mehdihadeli/ecommerce-microservices/actions/workflows/identity.yml/badge.svg?branch=develop&style=flat-square)](https://github.com/mehdihadeli/ecommerce-microservices/actions/workflows/identity.yml)           |
| Customer Service | Domain Driven Design                  | Completed ✔️   | [![Customers-CI-CD](https://github.com/mehdihadeli/ecommerce-microservices/actions/workflows/customers.yml/badge.svg?branch=develop&style=flat-square)](https://github.com/mehdihadeli/ecommerce-microservices-sample/actions/workflows/customers.yml) |
| Catalog Service  | Domain Driven Design                  | Completed ✔️   | [![Catalogs-CI-CD](https://github.com/mehdihadeli/ecommerce-microservices/actions/workflows/catalogs.yml/badge.svg?branch=develop&style=flat-square)](https://github.com/mehdihadeli/ecommerce-microservices/actions/workflows/catalogs.yml)           |
| Order Service    | Event Sourccing, Domain Driven Design | In Progress👷  | -                                                                                                                                                                                                                                                      |
| Shipping Service | Domain Driven Design                  | Not Started 🚩 | -                                                                                                                                                                                                                                                      |
| Payment Service  | Event Sourccing, Domain Driven Design | Not Started 🚩 | -                                                                                                                                                                                                                                                      |

## Technologies - Libraries

- ✔️ **[`.NET 7`](https://dotnet.microsoft.com/download)** - .NET Framework and .NET Core, including ASP.NET and ASP.NET Core
- ✔️ **[`Npgsql Entity Framework Core Provider`](https://www.npgsql.org/efcore/)** - Npgsql has an Entity Framework (EF) Core provider. It behaves like other EF Core providers (e.g. SQL Server), so the general EF Core docs apply here as well
- ✔️ **[`FluentValidation`](https://github.com/FluentValidation/FluentValidation)** - Popular .NET validation library for building strongly-typed validation rules
- ✔️ **[`Swagger & Swagger UI`](https://github.com/domaindrivendev/Swashbuckle.AspNetCore)** - Swagger tools for documenting API's built on ASP.NET Core
- ✔️ **[`Serilog`](https://github.com/serilog/serilog)** - Simple .NET logging with fully-structured events
- ✔️ **[`Polly`](https://github.com/App-vNext/Polly)** - Polly is a .NET resilience and transient-fault-handling library that allows developers to express policies such as Retry, Circuit Breaker, Timeout, Bulkhead Isolation, and Fallback in a fluent and thread-safe manner
- ✔️ **[`Scrutor`](https://github.com/khellang/Scrutor)** - Assembly scanning and decoration extensions for Microsoft.Extensions.DependencyInjection
- ✔️ **[`Opentelemetry-dotnet`](https://github.com/open-telemetry/opentelemetry-dotnet)** - The OpenTelemetry .NET Client
- ✔️ **[`DuendeSoftware IdentityServer`](https://github.com/DuendeSoftware/IdentityServer)** - The most flexible and standards-compliant OpenID Connect and OAuth 2.x framework for ASP.NET Core
- ✔️ **[`Newtonsoft.Json`](https://github.com/JamesNK/Newtonsoft.Json)** - Json.NET is a popular high-performance JSON framework for .NET
- ✔️ **[`Rabbitmq-dotnet-client`](https://github.com/rabbitmq/rabbitmq-dotnet-client)** - RabbitMQ .NET client for .NET Standard 2.0+ and .NET 4.6.1+
- ✔️ **[`AspNetCore.Diagnostics.HealthChecks`](https://github.com/Xabaril/AspNetCore.Diagnostics.HealthChecks)** - Enterprise HealthChecks for ASP.NET Core Diagnostics Package
- ✔️ **[`Microsoft.AspNetCore.Authentication.JwtBearer`](https://www.nuget.org/packages/Microsoft.AspNetCore.Authentication.JwtBearer)** - Handling Jwt Authentication and authorization in .Net Core
- ✔️ **[`NSubstitute`](https://github.com/nsubstitute/NSubstitute)** - A friendly substitute for .NET mocking libraries.
- ✔️ **[`StyleCopAnalyzers`](https://github.com/DotNetAnalyzers/StyleCopAnalyzers)** - An implementation of StyleCop rules using the .NET Compiler Platform
- ✔️ **[`AutoMapper`](https://github.com/AutoMapper/AutoMapper)** - Convention-based object-object mapper in .NET.
- ✔️ **[`Hellang.Middleware.ProblemDetails`](https://github.com/khellang/Middleware/tree/master/src/ProblemDetails)** - A middleware for handling exception in .Net Core
- ✔️ **[`IdGen`](https://github.com/RobThree/IdGen)** - Twitter Snowflake-alike ID generator for .Net
- ✔️ **[`MassTransit`](https://github.com/MassTransit/MassTransit)** - Distributed Application Framework for .NET

## Setup

### Dev Certificate

This application uses `Https` for hosting apis, to setup a valid certificate on your machine, you can create a [Self-Signed Certificate](https://learn.microsoft.com/en-us/aspnet/core/security/docker-https#macos-or-linux), see more about enforce certificate [here](https://learn.microsoft.com/en-us/dotnet/core/additional-tools/self-signed-certificates-guide) and [here](https://learn.microsoft.com/en-us/aspnet/core/security/enforcing-ssl).

- Setup on windows and [`powershell`](https://learn.microsoft.com/en-us/dotnet/core/additional-tools/self-signed-certificates-guide#with-dotnet-dev-certs):

```powershell
dotnet dev-certs https --clean
dotnet dev-certs https -ep $env:USERPROFILE\.aspnet\https\aspnetapp.pfx -p <CREDENTIAL_PLACEHOLDER>
dotnet dev-certs https --trust
```

- Setup in [`linux and wsl`](https://learn.microsoft.com/en-us/aspnet/core/security/docker-https?view=aspnetcore-7.0#macos-or-linux):

```bash
dotnet dev-certs https --clean
dotnet dev-certs https -ep ${HOME}/.aspnet/https/aspnetapp.pfx -p <CREDENTIAL_PLACEHOLDER>
dotnet dev-certs https --trust
```

`dotnet dev-certs https --trust` is only supported on macOS and Windows. You need to trust certs on Linux in the way that is supported by your distribution. It is likely that you need to trust the certificate in your browser(with this certificate we don't get an exception for https port because of not found certificate but browser shows us this certificate is not trusted).

### Conventional Commit

In this app I use [Conventional Commit](https://www.conventionalcommits.org/en/) and for enforcing its rule I use [conventional-changelog/commitlint](https://github.com/conventional-changelog/commitlint) and [typicode/husky](https://github.com/typicode/husky) with a pre-commit hook. For read more about its setup see [commitlint docs](https://github.com/conventional-changelog/commitlint#getting-started) and [this article](https://betterprogramming.pub/how-to-lint-commit-messages-with-husky-and-commitlint-b51d20a5e514) and [this article](https://www.code4it.dev/blog/conventional-commit-with-githooks).

Here I configured a husky hook for conventional commits:

1. Install NPM:

```bash
npm init
```

2. Install Husky:

```bash
npm install husky --save-dev
```

3. Add `prepare` and `install-dev-cert-bash` command for installing and activating `husky hooks` in the package.json file:

```bash
npm pkg set scripts.prepare="husky install && dotnet tool restore"

npm pkg set scripts.install-dev-cert-bash="curl -sSL https://aka.ms/getvsdbgsh | bash /dev/stdin -v vs2019 -l ~/vsdbg"
```

4. Install CommitLint:

```bash
npm install --save-dev @commitlint/config-conventional @commitlint/cli
```

5. Create the `commitlint.config.js` file with this content:

```js
module.exports = { extends: '@commitlint/config-conventional']};
```

6. Create the Husky folder:

```bash
mkdir .husky
```

7. Link Husky and CommitLint:

```bash
npx husky add .husky/commit-msg 'npx --no -- commitlint --edit ${1}'
```

8. Activate and installing all husky hooks with this command:

```bash
npm run prepare

# this command should run in git-bash on the windows or bash in the linux
npm run install-dev-cert-bash
```

### Formatting

For formatting I use [belav/csharpier](https://github.com/belav/csharpier) but you can also use `dotnet format`, you can integrate it with your [prefered IDE](https://csharpier.com/docs/Editors).

Here I configured a husky hook for formatting:

1. Install NPM:

```bash
npm init
```

2. Install Husky:

```bash
npm install husky --save-dev
```

2. Install Husky:

```bash
npm install husky --save-dev
```

3. Install manifest file with `dotnet new tool-manifest` because it doesn't exist at first time and then install our required packages as dependency with [dotnet tool install](https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-tool-install), that will add to [dotnet-tools.json](.config/dotnet-tools.json) file in a `.config` directory:

```bash
dotnet new tool-manifest

dotnet tool install csharpier
dotnet tool install dotnet-format
```

4. Add `prepare` command for installing and activating `husky hooks` and `restoring` our installed [dotnet tools](.config/dotnet-tools.json) in the previous step to the [package.json](package.json) file:

```bash
npm pkg set scripts.prepare="husky install && dotnet tool restore"
```

5. Create the Husky folder:

```bash
mkdir .husky
```

6. Link Husky and formatting tools:

```bash
npx husky add .husky/pre-commit "dotnet format"

# Or using csharpier
npx husky add .husky/pre-commit "dotnet csharpier ."
```

7. Activate and installing all husky hooks with this command:

```bash
npm run prepare
```

### Analizers

For roslyn analizers I use serveral analyzers and config the in `.editorconfig` file:

- [StyleCop/StyleCop](https://github.com/StyleCop/StyleCop)
- [JosefPihrt/Roslynator](https://github.com/JosefPihrt/Roslynator)
- [meziantou/Meziantou.Analyzer](https://github.com/meziantou/Meziantou.Analyzer)
- [Microsoft.VisualStudio.Threading.Analyzers](https://www.nuget.org/packages/Microsoft.VisualStudio.Threading.Analyzers)

## The Domain And Bounded Context - Service Boundary

`ECommerce Microservices` is a simple online ecommerce api sample that has the basic business scenario for online purchasing with some dedicated services. There are six possible `Bounded context` or `Service` for above business:

- `Identity Service`: the Identity Service uses to authenticate and authorize users through a token. Also, this service is responsible for creating users and their corresponding roles and permission with using [.Net Core Identity](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity) and Jwt authentication and authorization. I will add also [Identity Server](https://github.com/DuendeSoftware/IdentityServer) in future for this service. Each of `Administrator`, `Customer` and `Supplier` are a `User`, actually a `IdentityUser`. To be a User, User Registration is required. Each User is assigned one or more User Role.
  Each User Role has set of Permissions. A Permission defines whether User can invoke a particular action or not.

- `Catalog Service`: The Catalog Service presents the ability to add items to our ecommerce, It can be electronics, foods, books or anything else. Items can be grouped into categories and catalogs. A catalog is defined as a list of items that a company showcases online. the catalog is a collection of items, which can be grouped into categories. An item can be assigned to only one category or be direct child of a catalog without any category.
  Buyer can browse the products list with supported filtering and sorting by product name and price. customer can see the detail of the product on the product list and in the detail page, can see a name, description, available product in the inventory,...

- `Customers Service`: This service is responsible for managing our customers information, track the activities and subscribing to get notification for out of stock products

- `Order Service`: The Orders Service main purpose is to ecommerce order details and manage orders created by users on client side. This service is not designed to be a full order processing system like ERP but serves as storage for customer orders details and can be synchronized with different external processing systems.
  Some of this service responsibilities are `Saving orders`, `Saving order drafts`, `Ability to view and manage fulfillment, packages`, `Change discounts`

- `Payment Service`: The payment service is responsible for payment process of our customer with different payment process and managing and tracking our payment history

- `Shipping Service`: The Shipping Service provides the ability to extend shipping provider list with custom providers and also provides an interface and API for managing these shipping providers.
  Some of shipping service capabilities are `Register Shipping methods`, `Edit Shipping method`, `Shipment details`, `Shipping settings`

## Application Architecture

The bellow architecture shows that there is one public API (API Gateway) which is accessible for the clients and this is done via HTTP request/response. The API gateway then routes the HTTP request to the corresponding microservice. The HTTP request is received by the microservice that hosts its own REST API. Each microservice is running within its own `AppDomain` and has directly access to its own dependencies such as databases, files, local transaction, etc. All these dependencies are only accessible for that microservice and not to the outside world. In fact microservices are decoupled from each other and are autonomous. This also means that the microservice does not rely on other parts in the system and can run independently of other services.

![](./assets/microservices.png)

Microservices are event based which means they can publish and/or subscribe to any events occurring in the setup. By using this approach for communicating between services, each microservice does not need to know about the other services or handle errors occurred in other microservices.

In this architecture we use [CQRS Pattern](https://www.eventecommerce.com/cqrs-pattern) for separating read and write model beside of other [CQRS Advantages](https://youtu.be/dK4Yb6-LxAk?t=1029). Here for now I don't use [Event Sourcing](https://www.eventecommerce.com/blog/event-sourcing-and-cqrs) for simplicity but I will use it in future for syncing read and write side with sending streams and using [Projection Feature](https://event-driven.io/en/how_to_do_events_projections_with_entity_framework/) for some subscribers to syncing their data through sent streams and creating our [Custom Read Models](https://codeopinion.com/projections-in-event-sourcing-build-any-model-you-want/) in subscribers side.

Here I have a write model that uses a postgres database for handling better `Consistency` and `ACID Transaction` guaranty. beside o this write side I use a read side model that uses MongoDB for better performance of our read side without any joins with suing some nested document in our document also better scalability with some good scaling features of MongoDB.

For syncing our read side and write side we have 2 options with using Event Driven Architecture (without using events streams in event sourcing):

- If our `Read Sides` are in `Same Service`, during saving data in write side I save a [Internal Command](https://github.com/kgrzybek/modular-monolith-with-ddd#38-internal-processing) record in my `Command Processor` storage (like something we do in outbox pattern) and after commiting write side, our `command processor manager` reads unsent commands and sends them to their `Command Handlers` in same corresponding service and this handlers could save their read models in our MongoDb database as a read side.

- If our `Read Sides` are in `Another Services` we publish an integration event (with saving this message in the outbox) after committing our write side and all of our `Subscribers` could get this event and save it in their read models (MongoDB).

All of this is optional in the application and it is possible to only use what that the service needs. Eg. if the service does not want to Use DDD because of business is very simple and it is mostly `CRUD` we can use `Data Centric` Architecture or If our application is not `Task based` instead of CQRS and separating read side and write side again we can just use a simple `CRUD` based application.

Here I used [Outbox](http://www.kamilgrzybek.com/design/the-outbox-pattern/) for [Guaranteed Delivery](https://www.enterpriseintegrationpatterns.com/patterns/messaging/GuaranteedMessaging.html) and can be used as a landing zone for integration events before they are published to the message broker .

[Outbox pattern](https://event-driven.io/en/outbox_inbox_patterns_and_delivery_guarantees_explained/) ensures that a message was sent (e.g. to a queue) successfully at least once. With this pattern, instead of directly publishing a message to the queue, we ecommerce it in the temporary storage (e.g. database table) for preventing missing any message and some retry mechanism in any failure ([At-least-once Delivery](https://www.cloudcomputingpatterns.org/at_least_once_delivery/)). For example When we save data as part of one transaction in our service, we also save messages (Integration Events) that we later want to process in another microservices as part of the same transaction. The list of messages to be processed is called a [StoreMessage](./src/BuildingBlocks/BuildingBlocks.Abstractions/Messaging/PersistMessage/StoreMessage.cs) with [Message Delivery Type](./src/BuildingBlocks/BuildingBlocks.Abstractions/Messaging/PersistMessage/MessageDeliveryType.cs) `Outbox` that are part of our [MessagePersistence](./src/BuildingBlocks/BuildingBlocks.Core/Messaging/MessagePersistence/InMemory/InMemoryMessagePersistenceService.cs) service. This infrastructure also supports `Inbox` Message Delivery Type and `Internal` Message Delivery Type (Internal Processing).

Also we have a background service [MessagePersistenceBackgroundService](./src/BuildingBlocks/BuildingBlocks.Core/Messaging/BackgroundServices/MessagePersistenceBackgroundService.cs) that periodically checks the our [StoreMessages](./src/BuildingBlocks/BuildingBlocks.Abstractions/Messaging/PersistMessage/StoreMessage.cs) in the database and try to send the messages to the broker with using our [MessagePersistenceService](./src/BuildingBlocks/BuildingBlocks.Core/Messaging/MessagePersistence/InMemory/InMemoryMessagePersistenceService.cs) service. After it gets confirmation of publishing (e.g. ACK from the broker) it marks the message as processed to `avoid resending`.
However, it is possible that we will not be able to mark the message as processed due to communication error, for example `broker` is `unavailable`. In this case our [MessagePersistenceBackgroundService](./src/BuildingBlocks/BuildingBlocks.Core/Messaging/BackgroundServices/MessagePersistenceBackgroundService.cs) try to resend the messages that not processed and it is actually [ At-Least-Once delivery](http://www.cloudcomputingpatterns.org/at_least_once_delivery/). We can be sure that message will be sent `once`, but can be sent `multiple times` too! That’s why another name for this approach is Once-Or-More delivery. We should remember this and try to design receivers of our messages as [Idempotents](https://www.enterpriseintegrationpatterns.com/patterns/messaging/IdempotentReceiver.html), which means:

> In Messaging this concepts translates into a message that has the same effect whether it is received once or multiple times. This means that a message can safely be resent without causing any problems even if the receiver receives duplicates of the same message.

For handling [Idempotency](https://www.enterpriseintegrationpatterns.com/patterns/messaging/IdempotentReceiver.html) and [Exactly-once Delivery](https://www.cloudcomputingpatterns.org/exactly_once_delivery/) in receiver side, we could use [Inbox Pattern](https://event-driven.io/en/outbox_inbox_patterns_and_delivery_guarantees_explained/).

This pattern is similar to Outbox Pattern. It’s used to handle incoming messages (e.g. from a queue) for `unique processing` of `a single message` only `once` (even with executing multiple time). Accordingly, we have a table in which we’re storing incoming messages. Contrary to outbox pattern, we first save the messages in the database, then we’re returning ACK to queue. If save succeeded, but we didn’t return ACK to queue, then delivery will be retried. That’s why we have at-least-once delivery again. After that, an `inbox background process` runs and will process the inbox messages that not processed yet. also we can prevent executing a message with specific `MessgaeId`multiple times. after executing our inbox message for example with calling our subscribed event handlers we send a ACK to the queue when they succeeded. (Inbox part of the system is in progress, I will cover this part soon as possible)

Also here I used `RabbitMQ` as my `Message Broker` for my async communication between the microservices with using eventually consistency mechanism, for now I used [MassTransit](https://github.com/MassTransit/MassTransit) tools for doing broker communications. beside of this eventually consistency we have a synchronous call with using `REST` (in future I will use gRpc) for our immediate consistency needs.

We use a `Api Gateway` and here I used [YARP](https://microsoft.github.io/reverse-proxy/articles/getting-started.html) that is microsoft reverse proxy (we could use envoy, traefik, Ocelot, ...), in front of our services, we could also have multiple Api Gateway for reaching [BFF pattern](https://blog.bitsrc.io/bff-pattern-backend-for-frontend-an-introduction-e4fa965128bf). for example one Gateway for mobile apps, One Gateway for web apps and etc.
With using api Gateway our internal microservices are transparent and user can not access them directly and all requests will serve through this Gateway.
Also we could use gateway for load balancing, authentication and authorization, caching ,...

## Application Structure

In this project I used [vertical slice architecture](https://jimmybogard.com/vertical-slice-architecture/) or [Restructuring to a Vertical Slice Architecture](https://codeopinion.com/restructuring-to-a-vertical-slice-architecture/) also I used [feature folder structure](http://www.kamilgrzybek.com/design/feature-folders/) in this project.

- We treat each request as a distinct use case or slice, encapsulating and grouping all concerns from front-end to back.
- When We adding or changing a feature in an application in n-tire architecture, we are typically touching many different "layers" in an application. we are changing the user interface, adding fields to models, modifying validation, and so on. Instead of coupling across a layer, we couple vertically along a slice and each change affects only one slice.
- We `Minimize coupling` `between slices`, and `maximize coupling` `in a slice`.
- With this approach, each of our vertical slices can decide for itself how to best fulfill the request. New features only add code, we're not changing shared code and worrying about side effects. For implementing vertical slice architecture using cqrs pattern is a good match.

![](./assets/vertical-slice-architecture.jpg)

![](./assets/vsa2.png)

Also here I used [CQRS](https://www.eventecommerce.com/cqrs-pattern) for decompose my features to very small parts that makes our application:

- maximize performance, scalability and simplicity.
- adding new feature to this mechanism is very easy without any breaking change in other part of our codes. New features only add code, we're not changing shared code and worrying about side effects.
- easy to maintain and any changes only affect on one command or query (or a slice) and avoid any breaking changes on other parts
- it gives us better separation of concerns and cross cutting concern (with help of MediatR behavior pipelines) in our code instead of a big service class for doing a lot of things.

With using [CQRS](https://event-driven.io/en/cqrs_facts_and_myths_explained/), our code will be more aligned with [SOLID principles](https://en.wikipedia.org/wiki/SOLID), especially with:

- [Single Responsibility](https://en.wikipedia.org/wiki/Single-responsibility_principle) rule - because logic responsible for a given operation is enclosed in its own type.
- [Open-Closed](https://en.wikipedia.org/wiki/Open%E2%80%93closed_principle) rule - because to add new operation you don’t need to edit any of the existing types, instead you need to add a new file with a new type representing that operation.

Here instead of some [Technical Splitting](http://www.kamilgrzybek.com/design/feature-folders/) for example a folder or layer for our `services`, `controllers` and `data models` which increase dependencies between our technical splitting and also jump between layers or folders, We cut each business functionality into some vertical slices, and inner each of these slices we have [Technical Folders Structure](http://www.kamilgrzybek.com/design/feature-folders/) specific to that feature (command, handlers, infrastructure, repository, controllers, data models, ...).

Usually, when we work on a given functionality we need some technical things for example:

- API endpoint (Controller)
- Request Input (Dto)
- Request Output (Dto)
- Some class to handle Request, For example Command and Command Handler or Query and Query Handler
- Data Model

Now we could all of these things beside each other and it decrease jumping and dependencies between some layers or folders.

Keeping such a split works great with CQRS. It segregates our operations and slices the application code vertically instead of horizontally. In Our CQRS pattern each command/query handler is a separate slice. This is where you can reduce coupling between layers. Each handler can be a separated code unit, even copy/pasted. Thanks to that, we can tune down the specific method to not follow general conventions (e.g. use custom SQL query or even different storage). In a traditional layered architecture, when we change the core generic mechanism in one layer, it can impact all methods.

### High Level Structure

![](./assets/level-structure.png)

### Catalogs Service Structure

![](./assets/catalog-service.png)

## Vertical Slice Flow

For implementing vertical slice architecture in each microservice, I have two projects, for example in `Catalog Service` I have [ECommerce.Services.Catalogs](src/Services/Catalogs/ECommerce.Services.Catalogs/) project and [ECommerce.Services.Catalogs.Api](/src/Services/Catalogs/ECommerce.Services.Catalogs.Api/) project.

- `ECommerce.Services.Catalogs.Api` is responsible for Hosting microservice and configuring our `web api`, running the application on top of .net core and actually serving our microservices slices to outside of world.
- `ECommerce.Services.Catalogs` is responsible for putting all slices (features) based on our functionality in some slices, for example we put all [Features or Slices](src/Services/Catalogs/ECommerce.Services.Catalogs/Products/Features) related to `product` functionalities in [Products](src/Services/ECommerce.Services.Catalogs/ECommerce.Services.Catalogs/Products/) folder, also we have a [Shared Folder](src/Services/Catalogs/ECommerce.Services.Catalogs/Shared) that contains some infrastructure things will share between all slices (for example [Data-Context](src/Services/Catalogs/ECommerce.Services.Catalogs/Shared/Data/CatalogDbContext.cs), [ServiceCollectionExtensions.Persistence](src/Services/Catalogs/ECommerce.Services.Catalogs/Shared/Extensions/ServiceCollectionExtensions/ServiceCollectionExtensions.Persistence.cs)).

In vertical slice flow, we treat each request as a `slice`. For example for [CreatingProduct](src/Services/Catalogs/ECommerce.Services.Catalogs/Products/Features/CreatingProduct/) feature or slice, Our flow will start with a `Endpoint` with name [CreateProductEndpoint](src/Services/Catalogs/ECommerce.Services.Catalogs/Products/Features/CreatingProduct/CreateProductEndpoint.cs) and inner this endpoint we handle the http request from out side of world and pass our request data with a mediator gateway to corresponding handler.

```csharp
// POST api/v1/catalog/products
public static class CreateProductEndpoint
{
    internal static IEndpointRouteBuilder MapCreateProductsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost($"{ProductsConfigs.ProductsPrefixUri}", CreateProducts)
            .WithTags(ProductsConfigs.Tag)
            .RequireAuthorization()
            .Produces<CreateProductResult>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status400BadRequest)
            .WithName("CreateProduct")
            .WithDisplayName("Create a new product.");

        return endpoints;
    }

    private static async Task<IResult> CreateProducts(
        CreateProductRequest request,
        ICommandProcessor commandProcessor,
        IMapper mapper,
        CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));

        var command = mapper.Map<CreateProduct>(request);
        var result = await commandProcessor.SendAsync(command, cancellationToken);

        return Results.CreatedAtRoute("GetProductById", new { id = result.Product.Id }, result);
    }
}
```

In this endpoint we use CQRS and pass [CreateProduct](src/Services/Catalogs/ECommerce.Services.Catalogs/Products/Features/CreatingProduct/CreateProduct.cs) command to our command processor for executing and route to corresponding [CreateProductHandler](src/Services/Catalogs/ECommerce.Services.Catalogs/Products/Features/CreatingProduct/CreateProduct.cs#L88) command handler.

```csharp
public record CreateProduct(
    string Name,
    decimal Price,
    int Stock,
    int RestockThreshold,
    int MaxStockThreshold,
    ProductStatus Status,
    int Width,
    int Height,
    int Depth,
    string Size,
    ProductColor Color,
    long CategoryId,
    long SupplierId,
    long BrandId,
    string? Description = null,
    IEnumerable<CreateProductImageRequest>? Images = null) : ITxCreateCommand<CreateProductResult>
{
    public long Id { get; init; } = SnowFlakIdGenerator.NewId();
}

public class CreateProductHandler : ICommandHandler<CreateProduct, CreateProductResult>
{
    private readonly ILogger<CreateProductHandler> _logger;
    private readonly IMapper _mapper;
    private readonly ICatalogDbContext _catalogDbContext;

    public CreateProductHandler(
        ICatalogDbContext catalogDbContext,
        IMapper mapper,
        ILogger<CreateProductHandler> logger)
    {
        _logger = Guard.Against.Null(logger, nameof(logger));
        _mapper = Guard.Against.Null(mapper, nameof(mapper));
        _catalogDbContext = Guard.Against.Null(catalogDbContext, nameof(catalogDbContext));
    }

    public async Task<CreateProductResult> Handle(
        CreateProduct command,
        CancellationToken cancellationToken)
    {
        Guard.Against.Null(command, nameof(command));

        var images = command.Images?.Select(x =>
            new ProductImage(SnowFlakIdGenerator.NewId(), x.ImageUrl, x.IsMain, command.Id)).ToList();

        var category = await _catalogDbContext.FindCategoryAsync(command.CategoryId);
        Guard.Against.NotFound(category, new CategoryDomainException(command.CategoryId));

        var brand = await _catalogDbContext.FindBrandAsync(command.BrandId);
        Guard.Against.NotFound(brand, new BrandNotFoundException(command.BrandId));

        var supplier = await _catalogDbContext.FindSupplierByIdAsync(command.SupplierId);
        Guard.Against.NotFound(supplier, new SupplierNotFoundException(command.SupplierId));

        var product = Product.Create(
            command.Id,
            command.Name,
            Stock.Create(command.Stock, command.RestockThreshold, command.MaxStockThreshold),
            command.Status,
            Dimensions.Create(command.Width, command.Height, command.Depth),
            command.Size,
            command.Color,
            command.Description,
            command.Price,
            category!.Id,
            supplier!.Id,
            brand!.Id,
            images);

        await _catalogDbContext.Products.AddAsync(product, cancellationToken: cancellationToken);

        await _catalogDbContext.SaveChangesAsync(cancellationToken);

        var created = await _catalogDbContext.Products
            .Include(x => x.Brand)
            .Include(x => x.Category)
            .Include(x => x.Supplier)
            .SingleOrDefaultAsync(x => x.Id == product.Id, cancellationToken: cancellationToken);

        var productDto = _mapper.Map<ProductDto>(created);

        _logger.LogInformation("Product a with ID: '{ProductId} created.'", command.Id);

        return new CreateProductResult(productDto);
    }
}
```

This command handler will execute in a transaction with using [EfTxBehavior](src/BuildingBlocks/BuildingBlocks.Core/Persistence/EfCore/EfTxBehavior.cs) pipeline, because `CreateProduct` inherits from [ITxCreateCommand](src/BuildingBlocks/BuildingBlocks.Abstractions/CQRS/Commands/ITxCommand.cs).

And in the end of this handler before [Committing Transaction](src/BuildingBlocks/BuildingBlocks.Core/Persistence/EfCore/EfTxBehavior.cs#L74) we publish our domain events to their handlers with help of [DomainEventPublisher](src/BuildingBlocks/BuildingBlocks.Core/CQRS/Events/DomainEventPublisher.cs#L38). Also after [publishing our domain event handlers](src/BuildingBlocks/BuildingBlocks.Core/CQRS/Events/DomainEventPublisher.cs#L60), if We have a valid [EventMapper](src/BuildingBlocks/BuildingBlocks.Core/CQRS/Events/DomainEventPublisher.cs#L77) for mapping our domain events to `integration events` we can get their corresponding `Integration Events` for example [ProductEventMapper](src/Services/Catalogs/ECommerce.Services.Catalogs/Products/ProductEventMapper.cs) is a event mapping file for products functionality.

These integration events will [Save](src/BuildingBlocks/BuildingBlocks.Core/CQRS/Events/DomainEventPublisher.cs#L83) in the persistence message store, with help of [MessagePersistenceService](src/BuildingBlocks/BuildingBlocks.Core/Messaging/MessagePersistence/InMemory/InMemoryMessagePersistenceService.cs#L39) as [StoreMessage](src/BuildingBlocks/BuildingBlocks.Abstractions/Messaging/PersistMessage/StoreMessage.cs) with [MessageDeliveryType](src/BuildingBlocks/BuildingBlocks.Abstractions/Messaging/PersistMessage/MessageDeliveryType.cs) `Outbox` for guaranty delivery before committing.
After [Committing Transaction](src/BuildingBlocks/BuildingBlocks.Core/Persistence/EfCore/EfTxBehavior.cs#L77) our [MessagePersistenceBackgroundService](src/BuildingBlocks/BuildingBlocks.Core/Messaging/BackgroundServices/MessagePersistenceBackgroundService.cs#L45) will send, StoreMessage with delivery type outbox to message broker.

## Prerequisites

1. This application uses `Https` for hosting apis, to setup a valid certificate on your machine, you can create a [Self-Signed Certificate](https://learn.microsoft.com/en-us/aspnet/core/security/docker-https?view=aspnetcore-7.0#macos-or-linux), see more about enforce certificate [here](https://learn.microsoft.com/en-us/aspnet/core/security/enforcing-ssl).
2. Install git - [https://git-scm.com/downloads](https://git-scm.com/downloads).
3. Install .NET Core 7.0 - [https://dotnet.microsoft.com/download/dotnet/7.0](https://dotnet.microsoft.com/download/dotnet/7.0).
4. Install Visual Studio, Rider or VSCode.
5. Install docker - [https://docs.docker.com/docker-for-windows/install/](https://docs.docker.com/docker-for-windows/install/).
6. Make sure that you have ~10GB disk space.
7. Clone Project [https://github.com/mehdihadeli/ecommerce-microservices-sample](https://github.com/mehdihadeli/ecommerce-microservices-sample), make sure that's compiling
8. Run the [docker-compose.infrastructure.yaml](deployments/docker-compose/docker-compose.infrastructure.yaml) file, for running prerequisites infrastructures with `docker-compose -f ./deployments/docker-compose/docker-compose.infrastructure.yaml up -d` command.
9. Open [ecommerce.sln](./ECommerce.sln) solution.

## How to Run

For Running this application we could run our microservices one by one in our Dev Environment, for me, it's Rider, Or we could run it with using [Docker-Compose](#using-docker-compose) or we could use [Kubernetes](#using-kubernetes).

For testing apis I used [REST Client](https://marketplace.visualstudio.com/items?itemName=humao.rest-client) plugin of VSCode its related file scenarios are available in [\_httpclients](_httpclients) folder. also after running api you have access to `swagger open api` for all microservices in `/swagger` route path.

In this application I use a `fake email sender` with name of [ethereal](https://ethereal.email/) as a SMTP provider for sending email. after sending email by the application you can see the list of sent emails in [ethereal messages panel](https://ethereal.email/messages). My temp username and password is available inner the all of [appsettings file](./src/Services/Customers/ECommerce.Services.Customers.Api/appsettings.json).

### Using PM2

For ruining all microservices and control on their running mode we could use [PM2](https://pm2.keymetrics.io/) tools. for installing `pm2` on our system globally we should use this command:

```bash
npm install pm2 -g
```

After installing pm2 on our machine, we could run all of our microservices with running bellow command in root of the application with using [pm2.yaml](./pm2.yaml) file.

```bash
pm2 start pm2.yaml
```

Some PM2 useful commands:

```bash
pm2 -h

pm2 list

pm2 logs

pm2 monit

pm2 info pm2.yaml

pm2 stop pm2.yaml

pm2 restart pm2.yaml

pm2 delete pm2.yaml
```

### Using Docker-Compose

- First we should create a [dev-certificate](https://learn.microsoft.com/en-us/aspnet/core/security/docker-compose-https?view=aspnetcore-7.0#macos-or-linux) for our docker-compose file with this commands, see more about enforce certificate [here](https://learn.microsoft.com/en-us/aspnet/core/security/enforcing-ssl):

```powershell
dotnet dev-certs https --clean
dotnet dev-certs https -ep ${HOME}/.aspnet/https/aspnetapp.pfx -p $CREDENTIAL_PLACEHOLDER$
dotnet dev-certs https --trust
```

This local certificate will mapped to our containers in docker-compose file with setting `~/.aspnet/https:/https:ro` volume mount

- Our docker-compose files are based on linux
- Run the [docker-compose.infrastructure.yaml](./deployments/docker-compose/docker-compose.infrastructure.yaml) file, for running prerequisites infrastructures with `docker-compose -f ./deployments/docker-compose/docker-compose.infrastructure.yaml up -d` command.
- Run the [docker-compose.services.yaml](./deployments/docker-compose/docker-compose.services.yaml) with `docker-compose -f ./deployments/docker-compose/docker-compose.services.yaml` for production mode that uses pushed docker images for services or for development mode you can use [docker-compose.services.dev.yaml](./deployments/docker-compose/docker-compose.services.dev.yaml) override docker-compose file with `docker-compose -f ./deployments/docker-compose/docker-compose.services.yaml -f ${workspaceFolder}/deployments/docker-compose/docker-compose.services.dev.yaml up` command for building `dockerfiles` instead of using images in docker registry. Also for `debugging` purpose of docker-containers in vscode you can use [./deployments/docker-compose/docker-compose.services.debug.yaml](./deployments/docker-compose/docker-compose.services.debug.yaml) override docker-compose file with running `docker-compose -f ./deployments/docker-compose/docker-compose.services.yaml -f ${workspaceFolder}/deployments/docker-compose/docker-compose.services.debug.yaml up -d`, I defined some [tasks](.vscode/tasks.json) for vscode for executing this command easier. For debugging in vscode we should use [launch.json](.vscode/launch.json).
- Wait until all dockers got are downloaded and running.
- You should automatically get:
  - Postgres running
  - RabbitMQ running
  - MongoDB running
  - Microservies running and accessible:
    - Api Gateway, Available at: [http://localhost:3000](http://localhost:3000)
    - Customers Service, Available at: [http://localhost:8000](http://localhost:8000)
    - Catalogs Service, Available at: [http://localhost:4000](http://localhost:4000)
    - Identity Service, Available at: [http://localhost:7000](http://localhost:7000)

Some useful docker commands:

```powershell
// start dockers
docker-compose -f .\docker-compose.yaml up

// build without caching
docker-compose -f .\docker-compose.yaml build --no-cache

// to stop running dockers
docker-compose kill

// to clean stopped dockers
docker-compose down -v

// showing running dockers
docker ps

// to show all dockers (also stopped)
docker ps -a
```

### Using Tye

We could run our microservices with new microsoft tools with name of [Project Tye](https://devblogs.microsoft.com/dotnet/introducing-project-tye/).

Project Tye is an experimental developer tool that makes developing, testing, and deploying microservices and distributed applications easier.

For installing `Tye` globally on our machine we should use this command:

```bash
dotnet tool install -g Microsoft.Tye --version "0.11.0-alpha.22111.1"
```

OR if you already have Tye installed and want to update:

```bash
dotnet tool update -g Microsoft.Tye
```

After installing tye, we could run our microservices with following command in the root of our project:

```bash
tye run
```

One of key feature from tye run is a dashboard to view the state of your application. Navigate to [http://localhost:8000](http://localhost:8000) to see the dashboard running.

Also We could run some [docker images](https://devblogs.microsoft.com/dotnet/introducing-project-tye/#adding-external-dependencies-redis) with Tye and Tye makes the process of deploying your application to [Kubernetes](https://devblogs.microsoft.com/dotnet/introducing-project-tye/#deploying-to-kubernetes) very simple with minimal knowlege or configuration required.

### Using Kubernetes

For using kubernetes we can use multiple approach with different tools like [`plain kubernetes`](./deployments/k8s/kubernetes/), [`kustomize`](./deployments/k8s/kustomize/) and `helm` and here I will use show use of all of them.

#### Plain Kubernetes

##### Prerequisites

Here I uses plain kubernetes command and resources for applying kubernetes manifest to the cluster. If you're a Docker/Compose user new to Kubernetes, you might have noticed that you can’t `substitutes` and `replace` the `environment variables` in your kubernetes manifest files (exception is `substitutes` and `replace` environment variables in `env` attribute, with using [`environment dependent variable`](https://kubernetes.io/docs/tasks/inject-data-application/define-interdependent-environment-variables/#define-an-environment-dependent-variable-for-a-container)). we often have some personal `.env` files for projects that we use to store credentials and configurations.

For `substitutes` and `replace` environment variables in our kubernetes `manifest` or `resource` files we can use [envsubst](https://github.com/a8m/envsubst) tools, and we can even pipe it into other commands like Kubernetes `kubectl`, read more in [this](https://skofgar.ch/dev/2020/08/how-to-quickly-replace-environment-variables-in-a-file/) and [this](https://blog.8bitbuddhism.com/2022/11/12/how-to-use-environment-variables-in-a-kubernetes-manifest/) articles.

So here we should first install this tools on our OS with [this guid](https://github.com/a8m/envsubst#installation).

Now for running manifest files, firstly we should `load` our `environment variables` inner [`.env`](./deployments/k8s/kubernetes/.env) file in our `current shell session` by using `source .env` command (after closing our shell environments will destroy).

> Make sure to use `export`, otherwise our variables are considered shell variables and might not be accessible to `envsubst`

Here is a example of `.env` file:

```env
export ASPNETCORE_ENVIRONMENT=docker
export REGISTRY=ghcr.io
```

After loading `environment variables` to the our `shell session` we can run manifests with using `envsubst` and pipe envsubst output to `kubectl` command as a input like this example:

```bash
# pipe a deployment "deploy.yml" into kubectl apply
envsubst < deploy.yml | kubectl apply -f -
```

Also it is also possible to write our `substitution` to a `new file` (envsubst < input.tmpl > output.text):

```bash
envsubst < deploy.yml > compiled_deploy.yaml
```

I've create a [shell](https://medium.com/@peey/how-to-make-a-file-executable-in-linux-99f2070306b5) script [kubectl](./deployments/k8s/kubernetes/kubectl), we can call this `script` just like we would `kubectl`. This script sources any `.env` file in the manifests directory , If we're running `./kubectl apply` or `./kubectl delete` with [kubectl script](./deployments/k8s/kubernetes/kubectl), it calls `envsubst` to `swap out` your environment variables, then it passes the code on to the `real kubectl`.

```bash
#!/bin/bash
ENV_FILE=.env

source $ENV_FILE

if [[ "$1" == "apply" ]] || [[ "$1" == "delete" ]]; then
    envsubst < $3 | kubectl $1 $2 -
else
    kubectl "$@"
fi
```

##### Installation

- For installing `Infrastructure` manifests for kubernetes cluster we run bellow command with [`kubectl script`](./deployments/k8s/kubernetes/kubectl):

```bash
./kubectl apply -f ./deployments/k8s/kubernetes/infrastructure.yaml
```

## Contribution

The application is in development status. You are feel free to submit pull request or create the issue.

## Project References

- [https://github.com/oskardudycz/EventSourcing.NetCore](https://github.com/oskardudycz/EventSourcing.NetCore)
- [https://github.com/dotnet-architecture/eShopOnContainers](https://github.com/dotnet-architecture/eShopOnContainers)
- [https://github.com/jbogard/ContosoUniversityDotNetCore-Pages](https://github.com/jbogard/ContosoUniversityDotNetCore-Pages)
- [https://github.com/kgrzybek/modular-monolith-with-ddd](https://github.com/kgrzybek/modular-monolith-with-ddd)
- [https://github.com/thangchung/clean-architecture-dotnet](https://github.com/thangchung/clean-architecture-dotnet)

## License

The project is under [MIT license](https://github.com/mehdihadeli/ecommerce-microservices-sample/blob/main/LICENSE).

## Star History

[![Star History Chart](https://api.star-history.com/svg?repos=mehdihadeli/ecommerce-microservices&type=Date)](https://star-history.com/#mehdihadeli/ecommerce-microservices&Date)
