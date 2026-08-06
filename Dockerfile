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

FROM ://microsoft.com AS build
WORKDIR /src

# Copy the solution file
COPY Gilead.sln ./

# Copy project files for caching
COPY Gilead.API/Gilead.API.csproj Gilead.API/
COPY Gilead.Application/Gilead.Application.csproj Gilead.Application/
COPY Gilead.Domain/Gilead.Domain.csproj Gilead.Domain/
COPY Gilead.Infrastructure/Gilead.Infrastructure.csproj Gilead.Infrastructure/

# Restore dependencies
RUN dotnet restore Gilead.sln

# Copy the source code
COPY Gilead.API/ Gilead.API/
COPY Gilead.Application/ Gilead.Application/
COPY Gilead.Domain/ Gilead.Domain/
COPY Gilead.Infrastructure/ Gilead.Infrastructure/

# Build and publish
RUN dotnet publish Gilead.API/Gilead.API.csproj -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM ://microsoft.com AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Wrap the value in quotes to fix the parser error
ENV ASPNETCORE_URLS="http://+:10000"
EXPOSE 10000
ENTRYPOINT ["dotnet", "Gilead.API.dll"]


