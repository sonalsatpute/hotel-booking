﻿FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Set OTEL environment variables
ENV OTEL_DOTNET_AUTO_HOME="/otel"
ENV OTEL_SERVICE_NAME="eshop-orders-api"
ENV OTEL_RESOURCE_ATTRIBUTES="deployment.environment=staging,service.version=1.0.0"
ENV OTEL_EXPORTER_OTLP_ENDPOINT="http://host.docker.internal:4318"
ENV OTEL_TRACES_EXPORTER="otlp"
ENV OTEL_LOGS_EXPORTER="otlp"
ENV OTEL_METRICS_EXPORTER="otlp"

# Disable auto-instrumentation
ENV OTEL_DOTNET_AUTO_LOGS_ENABLED="true"
ENV OTEL_DOTNET_AUTO_TRACES_ENABLED="true"

# Enable auto-instrumentation
ENV OTEL_DOTNET_AUTO_METRICS_ENABLED="true"

RUN apt-get update && apt-get install -y curl unzip
RUN mkdir "/otel"
RUN curl -L -o /otel/otel-dotnet-install.sh https://github.com/open-telemetry/opentelemetry-dotnet-instrumentation/releases/download/v1.9.0/otel-dotnet-auto-install.sh -O
RUN chmod +x /otel/otel-dotnet-install.sh

RUN /bin/bash /otel/otel-dotnet-install.sh
RUN chmod +x /otel/instrument.sh


COPY start.sh /start.sh
RUN chmod +x /start.sh

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["eShop.Orders.Api/eShop.Orders.Api.csproj", "eShop.Orders.Api/"]
RUN dotnet restore "eShop.Orders.Api/eShop.Orders.Api.csproj"
COPY . .
WORKDIR "/src/eShop.Orders.Api"
RUN dotnet build "eShop.Orders.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "eShop.Orders.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

RUN ls -la /app

#ENTRYPOINT ["/bin/bash", "-c", "source /otel/instrument.sh && dotnet eShop.Orders.Api.dll"]
ENTRYPOINT ["/start.sh", "dotnet"]
CMD ["eShop.Orders.Api.dll"]





#build:
# docker build --no-cache --progress=plain -t eshop-order-api . -f ./eShop.Orders.Api/Dockerfile  

#RUN:
# docker run -p 8080:80 -e OTEL_EXPORTER_OTLP_ENDPOINT="http://host.docker.internal:4318" eshop-order-api 
# docker run -p 8080:80  -e OTEL_METRICS_EXPORTER="console" eshop-order-api 