FROM mcr.microsoft.com/dotnet/aspnet:9.0

RUN mkdir /Workspace \
  && mkdir /Workspace/Data \
  && mkdir /Workspace/Configuration \
  && mkdir /Workspace/Logs

ADD ./Other/Resources/DependentCodeUnits/ClientInformationBackend/BuildResult_DotNet_win-x64 /Workspace/Application

WORKDIR /Workspace/Application
HEALTHCHECK CMD curl --insecure --fail https://localhost:443/API/Other/Maintenance/HealthCheck
ENTRYPOINT ["dotnet", "/Workspace/Application/ClientInformationBackend.dll"]
