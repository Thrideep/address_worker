#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["AddressWorker.VirusTotal.Api/AddressWorker.VirusTotal.Api.csproj", "AddressWorker.VirusTotal.Api/"]
COPY ["Api.Abstractions/Api.Abstractions.csproj", "Api.Abstractions/"]
RUN dotnet restore "AddressWorker.VirusTotal.Api/AddressWorker.VirusTotal.Api.csproj"
COPY . .
WORKDIR "/src/AddressWorker.VirusTotal.Api"
RUN dotnet build "AddressWorker.VirusTotal.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AddressWorker.VirusTotal.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AddressWorker.VirusTotal.Api.dll"]