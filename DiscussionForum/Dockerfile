FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /App

# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore
# Build and publish a release
RUN dotnet publish -c release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /App

ENV ASPNETCORE_URLS=http://+:5055
ENV ASPNETCORE_ENVIRONMENT=Docker

COPY --from=build /App/out .

EXPOSE 5055 

ENTRYPOINT ["dotnet", "DiscussionForum.dll"]