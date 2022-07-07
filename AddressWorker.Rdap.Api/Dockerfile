#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["AddressWorker.Rdap.Api/AddressWorker.Rdap.Api.csproj", "AddressWorker.Rdap.Api/"]
COPY ["Api.Abstractions/Api.Abstractions.csproj", "Api.Abstractions/"]
RUN dotnet restore "AddressWorker.Rdap.Api/AddressWorker.Rdap.Api.csproj"
COPY . .
WORKDIR "/src/AddressWorker.Rdap.Api"
RUN dotnet build "AddressWorker.Rdap.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AddressWorker.Rdap.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AddressWorker.Rdap.Api.dll"]