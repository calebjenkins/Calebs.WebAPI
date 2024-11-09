# Caleb's WebAPI
## a basic Web API useful for testing multiple scenarios - deployed as a console app and dotnet tool.

## Install
```
dotnet tool install --global Calebs.WebAPI
```
See the latest package details on [nuget](https://nuget.org/packages/calebs.webapi)


## Usage
### To start web API app:
```
calebs.webapi
```

### Optionaly specifiy port to run on:
```
calebs.webapi --urls=http://localhost:PORT
```

## Supported Commands:

- GET /hello/
This is a hello world end point. Should return `hello world`

- GET /hello/{name}/
Hello world, with a passed token. Should return `hello {name}`

- GET /secure/ 
a `secure` end point. Will return `403 Not Authorized` if no bearer token is present as a header.

- GET /AuthToken/
will return a token that can be used for the /secure/ end point

- Echo
This is why you're here. Echo returns back everything from the request. Including `VERB` used (GET, POST, DELETE, PUT, PATCH) are currently supported. All `querystring`, `headers`, `cookies` and `body` information are all reflected back as a `JSON` object.

### Coming Soon
- A set of RESTFUL end-points for `User` objects - backed by an `IKeyValueRepo` for some minimal stateful scenarios.
- Considering: a set of "admin" end-points to allow dynamically adding new endpoints and supported RESTFUL nouns.


## Contribution
Please submit all PRs to the `develop` branch

## Version History
- 0.1.0 - intial version. `.NET 7` Installs as a dotnet global tool or run locally as a console web app.
- 1.0.0 - official release. New features coming soon! 
- 1.0.1 - added unit tests and version info on launch text
