#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/UpBlazor.Web/UpBlazor.Web.csproj", "src/UpBlazor.Web/"]
COPY ["src/UpBlazor.Infrastructure/UpBlazor.Infrastructure.csproj", "src/UpBlazor.Infrastructure/"]
COPY ["src/UpBlazor.Core/UpBlazor.Core.csproj", "src/UpBlazor.Core/"]
RUN dotnet restore "src/UpBlazor.Web/UpBlazor.Web.csproj"
COPY . .
WORKDIR "/src/src/UpBlazor.Web"
RUN dotnet build "UpBlazor.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UpBlazor.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UpBlazor.Web.dll"]