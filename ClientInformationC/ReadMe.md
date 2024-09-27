# ClientInformation

`ClientInformation` is a service to retrieve information of the own HTTP-client, for example geo-information.
For this purposes `ClientInformation` has an internal database and no dependencies to other services.

## Background

There are some usecases where you want to know which country a specific IP-address belongs to.
This is not trivial.
But there are some [existing databases](https://github.com/sapics/ip-location-db) for this with a utilisable license.
And if the data itself are available for free then there also exist a simple, free and self-hostable server which uses this data and provide it through a Rest-API.
For this purpose you can use `ClientInformation`.

## Deployment

The image can be pulled from [DockerHub](https://hub.docker.com/r/aniondev/clientinformationc).
