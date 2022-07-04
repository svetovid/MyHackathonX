FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG beijernugetsource
ARG solutionpath
ARG configuration
ARG projectfilepath
COPY ["Proto/tagHub.proto", "Proto/"]
WORKDIR /src
COPY ${solutionpath} .
RUN dotnet restore ${projectfilepath} -s "https://api.nuget.org/v3/index.json"
RUN dotnet build ${projectfilepath} -c ${configuration} -o /app/build

FROM build AS publish
ARG configuration
ARG projectfilepath
RUN dotnet publish ${projectfilepath} -c ${configuration} -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HackathonX.Service.dll"]