# API-Usage

`ClientInformation` provides a REST-API.
You can query information about a specific ip-address by using the route `/API/v1/ClientInformationBackendController/Information/<IP-address>`.

Example: `curl -s https://clientinformation.anion327.de/API/v1/ClientInformationBackendController/Information/8.8.8.8`

You can also omit the IP-address and then the ip of the currently requesting client will be used.
Since this is probably the default-usecase the name of the project is `ClientInformation`.
