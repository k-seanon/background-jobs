FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["examples/TheSimplestExample.Web/TheSimplestExample.Web.csproj", "TheSimplestExample.Web/"]
RUN dotnet restore "examples/TheSimplestExample.Web/TheSimplestExample.Web.csproj"
COPY . .
WORKDIR "/src/TheSimplestExample.Web"
RUN dotnet build "TheSimplestExample.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TheSimplestExample.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TheSimplestExample.Web.dll"]
