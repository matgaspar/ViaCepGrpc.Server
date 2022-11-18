#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["ViaCepGrpc.Application/ViaCepGrpc.Application.csproj", "ViaCepGrpc.Application/"]
COPY ["ViaCepGrpc.Grpcs/ViaCepGrpc.Grpcs.csproj", "ViaCepGrpc.Grpcs/"]
COPY ["ViaCepGrpc.IoC/ViaCepGrpc.IoC.csproj", "ViaCepGrpc.IoC/"]
COPY ["ViaCepGrpc.Services/ViaCepGrpc.Services.csproj", "ViaCepGrpc.Services/"]
RUN dotnet restore "ViaCepGrpc.Application/ViaCepGrpc.Application.csproj" -p:PublishReadyToRun=true
COPY . .
WORKDIR "/src/ViaCepGrpc.Application"
RUN dotnet build "ViaCepGrpc.Application.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ViaCepGrpc.Application.csproj" -c Release -r linux-x64 -o /app/publish --no-restore -p:PublishReadyToRun=true --self-contained

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ViaCepGrpc.Application.dll"]