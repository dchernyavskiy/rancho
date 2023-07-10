FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["tests/Services/Managements/Rancho.Services.Management.DBBanchMarks/Rancho.Services.Management.DBBanchMarks.csproj", "tests/Services/Managements/Rancho.Services.Management.DBBanchMarks/"]
RUN dotnet restore "tests/Services/Managements/Rancho.Services.Management.DBBanchMarks/Rancho.Services.Management.DBBanchMarks.csproj"
COPY . .
WORKDIR "/src/tests/Services/Managements/Rancho.Services.Management.DBBanchMarks"
RUN dotnet build "Rancho.Services.Management.DBBanchMarks.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Rancho.Services.Management.DBBanchMarks.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Rancho.Services.Management.DBBanchMarks.dll"]
