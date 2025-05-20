FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
RUN useradd -m appuser 
USER appuser
WORKDIR /app

EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY Monitoring ./Monitoring
COPY FeatureToggle ./FeatureToggle
COPY Converter ./Converter
COPY ConverterUnitTest ./ConverterUnitTest
COPY ConverterAPI ./ConverterAPI
WORKDIR /src/ConverterAPI
RUN dotnet restore
RUN dotnet build "ConverterAPI.csproj" -c "$BUILD_CONFIGURATION" -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ConverterAPI.csproj" -c "$BUILD_CONFIGURATION" -o /app/publish /p:UseAppHost=false

FROM base AS final
USER appuser
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ConverterAPI.dll"]