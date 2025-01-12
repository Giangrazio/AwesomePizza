#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["AwesomePizzaAPI/AwesomePizzaAPI.csproj", "AwesomePizzaAPI/"]
COPY ["AwesomePizzaBLL/AwesomePizzaBLL.csproj", "AwesomePizzaBLL/"]
COPY ["GenericUnitOfWork/GenericUnitOfWork.csproj", "GenericUnitOfWork/"]
COPY ["AwesomePizzaDAL/AwesomePizzaDAL.csproj", "AwesomePizzaDAL/"]
RUN dotnet restore "AwesomePizzaAPI/AwesomePizzaAPI.csproj"
COPY . .
WORKDIR "/src/AwesomePizzaAPI"
RUN dotnet build "AwesomePizzaAPI.csproj" -c Development -o /app/build

FROM build AS publish
RUN dotnet publish "AwesomePizzaAPI.csproj" -c Development -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AwesomePizzaAPI.dll"]