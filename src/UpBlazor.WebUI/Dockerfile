FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/UpBlazor.WebUI/UpBlazor.WebUI.csproj", "UpBlazor.WebUI/"]
COPY ["src/UpBlazor.ApiClient/UpBlazor.ApiClient.csproj", "UpBlazor.ApiClient/"]
COPY ["src/UpBlazor.Domain/UpBlazor.Domain.csproj", "UpBlazor.Domain/"]
RUN dotnet restore "UpBlazor.WebUI/UpBlazor.WebUI.csproj"
COPY . .
WORKDIR "/src/src/UpBlazor.WebUI"
RUN dotnet build "UpBlazor.WebUI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UpBlazor.WebUI.csproj" -c Release -o /app/publish

FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html
COPY --from=publish /app/publish/wwwroot .
COPY /src/UpBlazor.WebUI/nginx.conf /etc/nginx/nginx.conf