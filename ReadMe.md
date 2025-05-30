# ClientInformation

`ClientInformation` is a service to retrieve information of the own HTTP-client, for example geo-information.
For this purposes `ClientInformation` has an internal database and no dependencies to other services.

## Background

There are some usecases where you want to know which country a specific IP-address belongs to for example.
This is not trivial.
But there are some [existing databases](https://github.com/sapics/ip-location-db) for this with a utilisable license.
And if the data itself are available for free then there should also exist a simple, free and self-hostable server which uses this data and provide it through a Rest-API.
For this purpose you can use `ClientInformation`.
With `ClientInformation` you can host this service on your own.
The last update-date of the geo-ip-data is 2025-05-25.

## Try out

To see an example of the result-data see [clientinformation.anion327.de](https://clientinformation.anion327.de/API/v1/ClientInformationBackendController/Information).
Due to server-capacity-reasons it is not recommended to use this link for a productive environment.
You are supposed to host this service on your own.
An example how to run this service in a container is explained in the [reference](ClientInformation/Other/Reference/ReferenceContent/index.md).

You can just run `docker run -p 443:443 aniondev/clientinformation` to try out the service locally and then run `curl -k -s https://127.0.0.1:443/API/v1/ClientInformationBackendController/Information/8.8.8.8` to retrieve an example-response.

## Build

This product requires to use `scbuildcodeunits` implemented/provided by [ScriptCollection](https://github.com/anionDev/ScriptCollection) to build the project.

`ClientInformation` has 2 codeunits:

- [ClientInformation](./ClientInformation/Other/Reference/ReferenceContent/index.md)
- [ClientInformationBackend](./ClientInformationBackend/Other/Reference/ReferenceContent/index.md)

Please read also the "Get real ip-address of a client"-topic in the "Known issues"-section if you are running `ClientInformation` behind a reverse-proxy.

## Deployment

`ClientInformation` does not have any dependencies to external services.
The geo-data-database is stored internally.

The usual deployment-method is by running `ClientInformation` in a container.
To do so the official `ClientInformation`-image can be pulled from [DockerHub](https://hub.docker.com/r/aniondev/clientinformation).
The configuration-file will be generated/created on the first run.

## Known issues

### Get real ip-address of a client

When you deploy `ClientInformation` there are 2 possible cases:

1. The service is "directly" connected to the internet.
2. The service is behind a reverse-proxy.

For the first case you do not have to do anything, the service should work out of the box.

For the second case you have to change the value of `TrustForwardedHeader` in the `ServerConfiguration`-section from `false` to `true`.
Otherwise the server will assume the ip-address of the reverse-proxy as client-ip and ignore the [`X-Forwarded-For`](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/X-Forwarded-For)-header.
Furthermore you must configure your reverse-proxy to set the `X-Forwarded-For`-header with the real client-ip-address as value so that `ClientInformation` is finally able to retrieve the client-ip-address.
For [nginx](https://nginx.org/en/docs/http/ngx_http_realip_module.html) as reverse-proxy for example you must add `proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;` to the nginx-configuration.

The reason why the default-value of `TrustForwardedHeader` can not be `true` is that when you run `ClientInformation` without a reverseproxy, a client can simply set the `X-Forwarded-For`-header to an arbitrary value and then `ClientInformation` can not trust this header-value anymore. If a client would be able to pretend a wrong client-ip-address then also the client-ip-addresses in the access-log would be wrong.

### IPv6

IPv6 is currently not supported.
Only IPv4-addresses are supported by `ClientInformation`.

## Contribue

Contributions are always welcome.

This product has the contribution-requirements defines by [DefaultOpenSourceContributionProcess](https://projects.aniondev.de/PublicProjects/Common/ProjectTemplates/-/blob/main/Conventions/Contributing/DefaultOpenSourceContributionProcess/DefaultOpenSourceContributionProcess.md).

## Repository-structure

This product uses the [CommonProjectStructure](https://projects.aniondev.de/PublicProjects/Common/ProjectTemplates/-/blob/main/Conventions/RepositoryStructure/CommonProjectStructure/CommonProjectStructure.md) as repository-structure.

## Branching-system

This product follows the [GitFlowSimplified](https://projects.aniondev.de/PublicProjects/Common/ProjectTemplates/-/blob/main/Conventions/BranchingSystem/GitFlowSimplified/GitFlowSimplified.md)-branching-system.

## Image-properties

The image-artifacts of this product must fulfill the image-properties defined by [DefaultImageUsabilityRequirements](https://projects.aniondev.de/PublicProjects/Common/ProjectTemplates/-/blob/main/Conventions/ImageProperties/DefaultImageUsabilityRequirements/DefaultImageUsabilityRequirements.md).

## Versioning

This product follows the [SemVerPractise](https://projects.aniondev.de/PublicProjects/Common/ProjectTemplates/-/blob/main/Conventions/Versioning/SemVerPractise/SemVerPractise.md)-versioning-system.

## License

See [License.txt](./License.txt) for license-information.
