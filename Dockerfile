#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["DevopsWebApp/DevopsWebApp.csproj", "DevopsWebApp/"]
RUN dotnet restore "DevopsWebApp/DevopsWebApp.csproj"
COPY ./DevopsWebApp ./DevopsWebApp
WORKDIR "/src/DevopsWebApp"
RUN dotnet build "DevopsWebApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DevopsWebApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DevopsWebApp.dll"]