#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
EXPOSE 5000
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src/MedicalUnitSystem
COPY ["./MedicalUnitSystem/MedicalUnitSystem.csproj", "MedicalUnitSystem/"]
RUN dotnet restore "./MedicalUnitSystem/MedicalUnitSystem.csproj"
COPY . .
RUN dotnet build "./MedicalUnitSystem/MedicalUnitSystem.csproj" -c $BUILD_CONFIGURATION -o /app/build /p:UseAppHost=false

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MedicalUnitSystem/MedicalUnitSystem.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MedicalUnitSystem.dll"]