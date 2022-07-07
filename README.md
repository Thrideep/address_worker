# Address Worker

Address Worker is an API to query multiple endpoints to retrieve information about a domain/IP address. 
This service is divided into microservices. Each micro service is responsible for communicating with the intended service that it is retrieving data for.

The main AddressWorker.Api project is responsible for aggregating this data, and returning it to the caller in JSON format.
The API allows the caller to select a set of services that the caller wants to retrieve data from.
The API is a simple `HTTP GET` endpoint which will retrieve data from a predefined set of services &mdash; Ping, RDAP, GeoIP, and ReverseDns.

## API Usage


### AddressServiceAnalyze

- `GET /api/v1/analyze/{address}` -> Returns JSON from the default set of services - Ping, RDAP, GeoIP, and ReverseDns.
	
	**Parameter** - `address`
	
	- Type `{string}` - IP Address or Domain
		- Example: `8.8.8.8` or `google.com`
		
- `GET /api/v1/analyze/{servicelist}/{address}` ->  Returns JSON from the specified set of services in the `servicelist` parameter.

   **Parameter** - `servicelist`

  - Type `{string}` - Comma separated list of strings
    - Accepatable Values:
      - `vt`   - Virus Total
      - `rdap` - RDAP
      - `rdns` - Reverse DNS
      - `ping` - Ping
      - `geoip`  - GeoIP

    - Example: `vt,rdap,geoip`

  **Parameter** - `address`

  - Type `{string}` - IP Address or Domain
	- Example: `8.8.8.8` or `google.com`
	
## Running the App

### Prerequisites

- .NET 5
- Visual Studio 2019

### Building and running the App

#### First, run microservices using docker

```shell
cd <project root dir>

docker-compose up --build
``` 

### Run AddressWorker.Api 

Since facade api (AddressWorker.Api) is not included in docker compose, it needs to be run separately.

```shell
cd <project root dir>
dotnet run --project AddressWorker.Api
```

Now, API can be tested using this URl - - http://localhost:5000/api/v1/analyze/8.8.8.8

### To test API using Swagger

  - Make `AddressWorker.Api` as a startup project and run it from VS 2019, Or,
 
  - Start the instance of the project using `Project -> Debug -> Start New Instance` option.

  - The swagger URL will be - https://localhost:5001/swagger/index.html
 

### Testing GET endpoint `/api/v{version}/analyze/{address}` from swagger.
 
  **Parameter** - `address` => `8.8.8.8` (OR) `google.com`

  **Parameter** - `version` => `1`

### Testing GET endpoint `/api/v{version}/analyze/{servicelist}/{address}` from swagger

  **Parameter** - `servicelist` => `rdap,rdns,ping,geoip`
	
  **Parameter** - `address` => `8.8.8.8` (OR) `google.com`

  **Parameter** - `version` => `1`
  
  
### More improvements 

I still feel there is a scope to improve this project; may be, in terms of sending exceptions/errors to the client party,
implementing action filters to validate the input address and so returning particular type in action methods instead of returning `IActionResult`.
Since this is a kind of POC, I didn't get a chance to implement all of those.

