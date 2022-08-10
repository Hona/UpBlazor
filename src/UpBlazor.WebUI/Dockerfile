FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/UpBlazor.WebUI/UpBlazor.WebUI.csproj", "UpBlazor.WebUI/"]
COPY ["src/UpBlazor.ApiClient/UpBlazor.ApiClient.csproj", "UpBlazor.ApiClient/"]
COPY ["src/UpBlazor.Core/UpBlazor.Core.csproj", "UpBlazor.Core/"]
RUN dotnet restore "src/UpBlazor.WebUI/UpBlazor.WebUI.csproj"
COPY . .
WORKDIR "/src/src/UpBlazor.WebUI"
RUN dotnet build "UpBlazor.WebUI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UpBlazor.WebUI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UpBlazor.WebUI.dll"]
