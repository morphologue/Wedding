# Wedding
A website to collect RSVPs for a wedding at Novotel Barossa Valley in January 2018.

## How to run locally
Ensure you have the .NET Core 2.0 SDK installed, as well as a recent Node, NPM and MySql Server.
Then from the root of the checkout:
```
npm install
npm run build
ASPNETCORE_ENVIRONMENT=Development dotnet run
```
Finally open http://localhost:5000/wedding in a browser.

## Building a Docker container
From the top level of the checkout:
```
./publish.sh
sudo docker build -t wedding .
```
