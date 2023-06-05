FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["TCP-Chat-SocketClass/TCP-Chat-SocketClass.csproj", "TCP-Chat-SocketClass/"]
RUN dotnet restore "TCP-Chat-SocketClass/TCP-Chat-SocketClass.csproj"
COPY . .
WORKDIR "/src/TCP-Chat-SocketClass"
RUN dotnet build "TCP-Chat-SocketClass.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TCP-Chat-SocketClass.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TCP-Chat-SocketClass.dll"]
