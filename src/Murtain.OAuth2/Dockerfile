FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY *.sln ./
COPY Murtain.OAuth2/Murtain.OAuth2.csproj Murtain.OAuth2/
RUN dotnet restore
COPY . .
WORKDIR /src/Murtain.OAuth2
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Murtain.OAuth2.dll"]
