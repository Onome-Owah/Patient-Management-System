# Use an official .NET runtime as a parent image
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Use the SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["PMS.Web/PMS.Web.csproj", "PMS.Web/"]
COPY ["PMS.Data/PMS.Data.csproj", "PMS.Data/"]
RUN dotnet restore "PMS.Web/PMS.Web.csproj"

COPY . .
WORKDIR "/src/PMS.Web"
RUN dotnet publish "PMS.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "PMS.Web.dll"]
