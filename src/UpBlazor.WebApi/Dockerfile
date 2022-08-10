FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/UpBlazor.WebApi/UpBlazor.WebApi.csproj", "UpBlazor.WebApi/"]
COPY ["src/UpBlazor.Application/UpBlazor.Application.csproj", "UpBlazor.Application/"]
COPY ["src/UpBlazor.Core/UpBlazor.Core.csproj", "UpBlazor.Core/"]
COPY ["src/UpBlazor.Infrastructure/UpBlazor.Infrastructure.csproj", "UpBlazor.Infrastructure/"]
RUN dotnet restore "src/UpBlazor.WebApi/UpBlazor.WebApi.csproj"
COPY . .
WORKDIR "/src/src/UpBlazor.WebApi"
RUN dotnet build "UpBlazor.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UpBlazor.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UpBlazor.WebApi.dll"]
