FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Colabora.WebAPI/Colabora.WebAPI.csproj", "Colabora.WebAPI/"]
RUN dotnet restore "src/Colabora.WebAPI/Colabora.WebAPI.csproj"
COPY . .
WORKDIR "/src/Colabora.WebAPI"
RUN dotnet build "Colabora.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Colabora.WebAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Colabora.WebAPI.dll"]
