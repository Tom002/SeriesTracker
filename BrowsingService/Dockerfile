#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["BrowsingService/BrowsingService.csproj", "BrowsingService/"]
COPY ["BrowsingService.Test/BrowsingService.Test.csproj", "BrowsingService.Test/"]
COPY ["Common/Common.csproj", "Common/"]
RUN dotnet restore "BrowsingService/BrowsingService.csproj"
COPY . .
WORKDIR "/src/BrowsingService"
RUN dotnet build "BrowsingService.csproj" -c Release -o /app/build

FROM build as test
WORKDIR "/src/BrowsingService.Test"
LABEL test=true
RUN dotnet tool install dotnet-reportgenerator-globaltool --tool-path /tools
RUN dotnet test --results-directory /testresults --logger "trx;LogFileName=browsing_tests.xml" /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura /p:CoverletOutput=/testresults/coverage/ /p:Exclude="[xunit.*]*%2c[StackExchange.*]*"
RUN /tools/reportgenerator "-reports:/testresults/coverage/coverage.cobertura.xml" "-targetdir:/testresults/coverage/reports" "-reporttypes:HTMLInline;HTMLChart"
RUN ls -la /testresults/coverage/reports

FROM build AS publish
WORKDIR "/src/BrowsingService"
RUN dotnet publish "BrowsingService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BrowsingService.dll"]