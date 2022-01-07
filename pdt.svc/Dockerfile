# See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build
WORKDIR /src
COPY ["pdt.svc/pdt.svc.csproj", "pdt.svc/"]
RUN dotnet restore "pdt.svc/pdt.svc.csproj"
COPY . .
WORKDIR "/src/pdt.svc"
RUN dotnet build "pdt.svc.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "pdt.svc.csproj" -c Release -o /app/publish

# takes the parms from the Github Action cmd line inside the action: docker/build-push-action@v2
# and mounts theme from cmd line, into /run/secrets directory  (automagically)
# we cat that out in a SH run command and necessarily 'export' so it is a linux shell value for the build container
RUN --mount=type=secret,id=GOOGLE_CUSTOM_SEARCH_CX \
  --mount=type=secret,id=GOOGLE_CUSTOM_SEARCH_API_KEY \
   export GOOGLE_CUSTOM_SEARCH_CX=$(cat /run/secrets/GOOGLE_CUSTOM_SEARCH_CX) && \
   export GOOGLE_CUSTOM_SEARCH_API_KEY=$(cat /run/secrets/GOOGLE_CUSTOM_SEARCH_API_KEY)
# then for .NET; we set up ENV vars with double-underscores to override appsettings override, having added Configuration.AddEnvironmentVariables() to the builder in Program.cs\
ENV SearchEngine__GoogleCustomSearchCx ${GOOGLE_CUSTOM_SEARCH_CX}
ENV SearchEngine__GoogleCustomSearchApiKey ${GOOGLE_CUSTOM_SEARCH_API_KEY}

WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "pdt.svc.dll"]