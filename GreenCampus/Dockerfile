# ---------- BUILD ----------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Kopiraj csproj i restore
COPY *.csproj ./
RUN dotnet restore

# Kopiraj ostatak projekta i build
COPY . .
RUN dotnet publish -c Release -o /app/publish

# ---------- RUNTIME ----------
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Render port i environment
ENV ASPNETCORE_URLS=http://+:${PORT}
ENV ASPNETCORE_ENVIRONMENT=Production

# SQLite folder
RUN mkdir /app/data

# Kopiraj publish folder iz build stage-a
COPY --from=build /app/publish .

# Pokreni aplikaciju (DLL = ime tvog .csproj)
ENTRYPOINT ["dotnet", "GreenCampus.dll"]
