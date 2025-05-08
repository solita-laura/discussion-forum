FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /App

ENV ASPNETCORE_URLS=http://+:5055
ENV ASPNETCORE_ENVIRONMENT=Docker

# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore

# Build and publish a release
RUN dotnet publish -c release -o out

# create migrations
FROM build AS usermigrations
RUN dotnet tool install --global dotnet-ef
RUN /root/.dotnet/tools/dotnet-ef migrations add InitialUser --context UserContext

ENTRYPOINT ["/root/.dotnet/tools/dotnet-ef", "database", "update", "--context", "UserContext"]

FROM build AS messagemigrations
RUN dotnet tool install --global dotnet-ef
RUN /root/.dotnet/tools/dotnet-ef migrations add InitialMessage --context MessageContext

ENTRYPOINT ["/root/.dotnet/tools/dotnet-ef", "database", "update", "--context", "MessageContext"]

FROM build AS topicmigrations
RUN dotnet tool install --global dotnet-ef
RUN /root/.dotnet/tools/dotnet-ef migrations add InitialTopic --context TopicContext

ENTRYPOINT ["/root/.dotnet/tools/dotnet-ef", "database", "update", "--context", "TopicContext"]

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /App

ENV ASPNETCORE_URLS=http://+:5055
ENV ASPNETCORE_ENVIRONMENT=Docker

COPY --from=build /App/out .

EXPOSE 5055 

ENTRYPOINT ["dotnet", "DiscussionForum.dll"]

