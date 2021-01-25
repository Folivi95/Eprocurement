# NuGet restore
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY eprocurement-tool/eprocurement-tool.sln .

COPY eprocurement-tool/eprocurement-tool.WebAPI/eprocurement-tool.WebAPI.csproj eprocurement-tool.WebAPI/
COPY eprocurement-tool/eprocurement-tool.Application/eprocurement-tool.Application.csproj eprocurement-tool.Application/
COPY eprocurement-tool/eprocurement-tool.Infrastructure.Data/eprocurement-tool.Infrastructure.Data.csproj eprocurement-tool.Infrastructure.Data/
COPY eprocurement-tool/eprocurement-tool.Domain/eprocurement-tool.Domain.csproj eprocurement-tool.Domain/
COPY eprocurement-tool/eprocurement-tool.Infrastructure.IoC/eprocurement-tool.Infrastructure.IoC.csproj eprocurement-tool.Infrastructure.IoC/
COPY eprocurement-tool/eprocurement-tool.Application.UniTest/eprocurement-tool.Application.UniTest.csproj eprocurement-tool.Application.UniTest/
COPY eprocurement-tool/Application.UnitTests/Application.UnitTests.csproj Application.UnitTests/


RUN dotnet restore
COPY . .
COPY eprocurement-tool/ .


# publish
FROM build AS publish
WORKDIR /src
RUN dotnet publish -c Release -o /src/publish

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime
WORKDIR /app
COPY --from=publish /src/publish .
# heroku uses the following
CMD ASPNETCORE_URLS=http://*:$PORT dotnet eprocurement-tool.WebAPI.dll
