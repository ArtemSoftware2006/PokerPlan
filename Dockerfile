FROM mcr.microsoft.com/dotnet/aspnet:7.0.13-alpine3.18 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine3.18 AS build
WORKDIR /src
COPY CodeCup/CodeCup.csproj CodeCup/
COPY DAL/DAL.csproj DAL/
COPY Domain/Domain.csproj Domain/
COPY Service/Service.csproj Service/
#dotnet restore — восстанавливает зависимости и средства проекта.
RUN dotnet restore CodeCup/CodeCup.csproj
#А точно нужна эта строка?
COPY . .
WORKDIR /src/CodeCup
RUN dotnet build CodeCup.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish CodeCup.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
#--from=publish - взять из слоя Publish /app/publish и поместить в текущую директорию слоя final
COPY --from=publish /app/publish .
#запуск приложения
ENTRYPOINT ["dotnet", "CodeCup.dll"]