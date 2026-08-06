#FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
#WORKDIR /src
#
#COPY Gilead.sln ./
#COPY Gilead.API/Gilead.API.csproj Gilead.API/
#COPY Gilead.Application/Gilead.Application.csproj Gilead.Application/
#COPY Gilead.Domain/Gilead.Domain.csproj Gilead.Domain/
#COPY Gilead.Infrastructure/Gilead.Infrastructure.csproj Gilead.Infrastructure/
#RUN dotnet restore Gilead.API/Gilead.API.csproj
#
#COPY Gilead.API/ Gilead.API/
#COPY Gilead.Application/ Gilead.Application/
#COPY Gilead.Domain/ Gilead.Domain/
#COPY Gilead.Infrastructure/ Gilead.Infrastructure/
#COPY Gilead.DB/ Gilead.DB/
#RUN dotnet publish Gilead.API/Gilead.API.csproj -c Release -o /app/publish /p:UseAppHost=false
#
#FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
#WORKDIR /app
#COPY --from=build /app/publish .
#ENV ASPNETCORE_URLS=http://+:8080
#EXPOSE 8080
#ENTRYPOINT ["dotnet", "Gilead.API.dll"]
#

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the solution file
COPY Gilead.sln ./

# Copy ALL project files first to leverage Docker layer caching
COPY Gilead.API/Gilead.API.csproj Gilead.API/
COPY Gilead.Application/Gilead.Application.csproj Gilead.Application/
COPY Gilead.Domain/Gilead.Domain.csproj Gilead.Domain/
COPY Gilead.Infrastructure/Gilead.Infrastructure.csproj Gilead.Infrastructure/
COPY Gilead.DB/Gilead.DB.csproj Gilead.DB/

# Restore dependencies using the solution file to catch all projects
RUN dotnet restore Gilead.sln

# Copy the rest of the source code
COPY Gilead.API/ Gilead.API/
COPY Gilead.Application/ Gilead.Application/
COPY Gilead.Domain/ Gilead.Domain/
COPY Gilead.Infrastructure/ Gilead.Infrastructure/
COPY Gilead.DB/ Gilead.DB/

# Build and publish the web API
RUN dotnet publish Gilead.API/Gilead.API.csproj -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Update port to 10000 for seamless Render compatibility
ENV ASPNETCORE_URLS=http://+:10000
EXPOSE 10000
ENTRYPOINT ["dotnet", "Gilead.API.dll"]
