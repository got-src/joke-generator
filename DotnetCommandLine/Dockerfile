FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["DotnetCommandLine/DotnetCommandLine.csproj", "DotnetCommandLine/"]
RUN dotnet restore "DotnetCommandLine/DotnetCommandLine.csproj"
COPY . .
WORKDIR "/src/DotnetCommandLine"
RUN dotnet build "DotnetCommandLine.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DotnetCommandLine.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DotnetCommandLine.dll"]
