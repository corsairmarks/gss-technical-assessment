# Global Shop Solutions Technical Assessment

This repository is a technical assessment for Global Shop Solutions. It contains a partially-scaffolded application that is designed to correlate items (delivered by another technical team) in bins defined by an external, third-party system.

# Design

The application is designed as a microservice. Based on the mock requirements, it will have its own RDBMS in the fictional production environment.

## Technical Assumptions

* No CI/CD has been set up
    * In the real world, this application would be configured with build/test/deploy through existing infrastructure (Jenkins, Azure DevOps, TeamCity, and so on)
* The application could be containerized if that fits existing or desired delivery or operations strategies
* The application is coded with some thought to a JWT bearer-token-based authorization solution
    * In the real world, the authorization/authentication mechanism(s) should tie in to existing infrastructure used for communicating between applications or unified user logins (e.g. Active Directory)
* The mock requirements strongly suggested a REST API based on the potential for other integrating services and/or a UI
* A focus was placed on defining the external interface to the application - in the real world, that would allow other teams (such as UI engineers) to begin work with the underlying functionality is coded
* Ideally, the MVC logger would be wired up to log sinks that write to existing centralized logging, with some configuration for local file-based logging during development

## Areas for Improvement

* If the application should support cancellation, then methods should be edited/added that accept cancellation tokens, and the IoC configured to have a unique TokenCancellationSource for each request
* Register everything in the IoC container - I ran out of time doing implementation details
* File IO is notorious for being hard to test, and I didn't devote time to testing my PoC code for a hacked-together json-file-based repository