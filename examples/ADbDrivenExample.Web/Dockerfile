FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["examples/ADbDrivenExample.Web/ADbDrivenExample.Web.csproj", "ADbDrivenExample.Web/"]
RUN dotnet restore "examples/ADbDrivenExample.Web/ADbDrivenExample.Web.csproj"
COPY . .
WORKDIR "/src/ADbDrivenExample.Web"
RUN dotnet build "ADbDrivenExample.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ADbDrivenExample.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ADbDrivenExample.Web.dll"]
