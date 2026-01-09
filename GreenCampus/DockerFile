# ---------- BUILD ----------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY *.csproj ./
RUN dotnet restore

COPY . .
RUN dotnet publish -c Release -o /app/publish

# ---------- RUNTIME ----------
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:${PORT}
ENV ASPNETCORE_ENVIRONMENT=Production

RUN mkdir /app/data

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "GreenCampus.csproj"]
