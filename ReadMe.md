# ClientInformation

`ClientInformation` is a service to retrieve information of the own HTTP-client, for example geo-information.
For this purposes `ClientInformation` has an internal database and no dependencies to other services.

## Background

There are some usecases where you want to know which country a specific IP-address belongs to for example.
This is not trivial.
But there are some [existing databases](https://github.com/sapics/ip-location-db) for this with a utilisable license.
And if the data itself are available for free then there also exist a simple, free and self-hostable server which uses this data and provide it through a Rest-API.
For this purpose you can use `ClientInformation`.
With this service you can host this service on your own.
To see an example of the result-data see [clientinformation.anion327.de](https://clientinformation.anion327.de/API/v1/ClientInformationBackendController/Information).
Due to server-capacity-reasons it is not recommended to use this link for a productive environment.
You are supposed to host this service on your own.
An example how to run this service in a container is explained in the [reference](ClientInformation/Other/Reference/ReferenceContent/index.md).

## Build

`ClientInformation` uses the [CommonProjectStructure](https://projects.aniondev.de/PublicProjects/Common/ProjectTemplates/-/blob/main/Conventions/RepositoryStructure/CommonProjectStructure/CommonProjectStructure.md) as repository-structure and requires to use `scbuildcodeunits` implemented/provided by [ScriptCollection](https://github.com/anionDev/ScriptCollection) to build the project.


## Deployment

The image can be pulled from [DockerHub](https://hub.docker.com/r/aniondev/clientinformation).

## License

See [License.txt](./License.txt) for license-information.
