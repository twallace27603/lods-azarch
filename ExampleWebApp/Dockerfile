FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["ExampleWebApp/ExampleWebApp.csproj", "ExampleWebApp/"]
RUN dotnet restore "ExampleWebApp/ExampleWebApp.csproj"
COPY . .
WORKDIR "/src/ExampleWebApp"
RUN dotnet build "ExampleWebApp.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "ExampleWebApp.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ExampleWebApp.dll"]