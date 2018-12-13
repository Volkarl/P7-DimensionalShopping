FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 13458
EXPOSE 44339

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
RUN apt-get install -y curl \
    && curl -sL https://deb.nodesource.com/setup_9.x | bash - \
    && apt-get install -y nodejs \
	&& curl -L https://www.npmjs.com/install.sh | sh
COPY ["DimensionalPriceRunner/DimensionalPriceRunner.csproj", "DimensionalPriceRunner/"]
RUN dotnet restore "DimensionalPriceRunner/DimensionalPriceRunner.csproj"
COPY . .
WORKDIR "/src/DimensionalPriceRunner"
RUN dotnet build "DimensionalPriceRunner.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "DimensionalPriceRunner.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "DimensionalPriceRunner.dll"]