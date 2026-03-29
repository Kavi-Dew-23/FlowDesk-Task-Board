FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY FlowDesk.API/FlowDesk.API.csproj FlowDesk.API/
COPY FlowDesk.Application/FlowDesk.Application.csproj FlowDesk.Application/
COPY FlowDesk.Domain/FlowDesk.Domain.csproj FlowDesk.Domain/
COPY FlowDesk.Infrastructure/FlowDesk.Infrastructure.csproj FlowDesk.Infrastructure/

RUN dotnet restore FlowDesk.API/FlowDesk.API.csproj

COPY . .
WORKDIR /src/FlowDesk.API
RUN dotnet publish FlowDesk.API.csproj -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "FlowDesk.API.dll"]