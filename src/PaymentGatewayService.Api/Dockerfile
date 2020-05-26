FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.sln .
COPY ./src/ ./src/
COPY ./configs/ ./configs/
RUN dotnet restore

# Copy everything else and build
WORKDIR /app/src/PaymentGatewayService.Api
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "api/PaymentGatewayService.Api.dll"]