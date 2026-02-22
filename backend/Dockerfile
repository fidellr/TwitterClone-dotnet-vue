FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["TwitterClone.slnx", "./"]
COPY ["TwitterClone.API/TwitterClone.API.csproj", "TwitterClone.API/"]
COPY ["TwitterClone.Application/TwitterClone.Application.csproj", "TwitterClone.Application/"]
COPY ["TwitterClone.Domain/TwitterClone.Domain.csproj", "TwitterClone.Domain/"]
COPY ["TwitterClone.Infrastructure/TwitterClone.Infrastructure.csproj", "TwitterClone.Infrastructure/"]
RUN dotnet restore "TwitterClone.API/TwitterClone.API.csproj"

COPY . .
WORKDIR "/src/TwitterClone.API"
RUN dotnet publish "TwitterClone.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "TwitterClone.API.dll"]