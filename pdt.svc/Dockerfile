#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["pdt.svc/pdt.svc.csproj", "pdt.svc/"]
RUN dotnet restore "pdt.svc/pdt.svc.csproj"
COPY . .
WORKDIR "/src/pdt.svc"
RUN dotnet build "pdt.svc.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "pdt.svc.csproj" -c Release -o /app/publish

RUN --mount=type=secret, id=GOOGLE_CUSTOM_SEARCH_CX \
--mount=type=secret, id=GOOGLE_CUSTOM_SEARCH_API_KEY \
export GOOGLE_CUSTOM_SEARCH_CX=$(cat /run/secrets/GOOGLE_CUSTOM_SEARCH_CX) && \
export GOOGLE_CUSTOM_SEARCH_API_KEY=$(cat /run/secrets/GOOGLE_CUSTOM_SEARCH_API_KEY) && \
yarn gen

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "pdt.svc.dll"]