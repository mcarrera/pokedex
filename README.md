# pokedex
A REST api to retrieve Pokemon Information

## Run Locally

### Prerequisites
- .NET Core SDK 8
- Git
- A development IDE such as Visual Studio 2022 (optional)

### Steps

Clone the repository

```bash
  git clone https://github.com/mcarrera/pokedex
```

Go to the project directory
```bash
  cd Pokedex-API
```

Build with dotnet
```bash
  dotnet build
```

Start the API

```bash
  dotnet run --urls=https://localhost:7040
```

In alternative, you can use an IDE such as Visual Studio, Visual Studio Code or JetBrains Rider and open the Pokedex.sln solution.

### Troubleshooting
If you encounter any issues while running the API, check the console output for error messages. Ensure that the .NET Core SDK is correctly installed and that the specified port is not being used by another service.

### API Endpoints
The API Exposes 2 endpoints:

/api/Pokemon/{name}: Returns a Pokemon with description and additional info.
/api/Pokemon/translated/{name}: Returns a Pokemon with translated description and additional info.

which can be tested with curl

```bash
curl --location 'https://localhost:7040/api/Pokemon/mewtwo'

curl --location 'https://localhost:7040/api/Pokemon/translated/mewtwo'

```
Or simply by using the Swagger UI in a browser, from the URL: https://localhost:7040/swagger/index.html

Additionally, a postman collection is located in the Postman folder at the root of the repo.


### Azure Deployment
The API is deployed to Azure via GitHub Actions at the URL `https://pokemarco.azurewebsites.net/`. To test it, simply replace `https://localhost:7040` with the above URL.

Please note that the Azure deployment is on Free Tier, so it might slow to respond, in particular if it has been idle.

## Design Decisions and Libraries Used

### Design Decisions

- **MediatR for Mediator Pattern**: To keep the controller thin and the code more maintainable, I used the Mediator pattern via the MediatR library. This allows for loose coupling between objects and encapsulates how these objects interact.

- **Exception Handling**: Global exception handling is implemented to catch any unhandled exceptions, log it, and return a user-friendly error message.

### Libraries Used

- **MediatR**: Used for implementing the Mediator pattern, which helps in reducing the coupling between the classes and ensures that the application is easier to maintain.

- **PokeApiNet**: This library is used to fetch Pokemon data from the PokeAPI. It provide convenient methods to call the api and parse request and response objects.

- **xUnit, Moq and AutoFixture**: These libraries are used for unit testing, mocking and generate test data.

### Production vs Development considerations
While coding, I made certain decisions, mainly in the interest of time and to have a working prototype soon. I added comments to the code when that was the case. 
If this were an actual Production project, I would consider the following points:
* Gaining a clear and as complete an understanding as possible of the business domain (Pokemon), including potential licensing and copyright issues.
* Gaining a more complete understanding of the third-party API involved, including performance implications, licensing and daily usage limits. Rather than using a 3rd party library, it might be worth exploring building a service to call the pokeapi.co API Directly (similar to how it is done for the funtranslation API)
* I hard-coded several string values in the code. For example, error messages, URLs, etc. This was done in the interest of time. In a production environment, I would consider using setting files and/or environment variables.
* For a prototype with limited usage, performance appears to be adequate (usually the API returns a result in less than 1 second). For a production product, I would recommend estimating load volumes and conducting stress tests. If necessary, caching strategies (for example, a key/value database) should be considered.
* The unit tests included in the solution are not comprehensive. Each layer of the application (controller, handlers, services) is covered, but definitely to a lesser extent than what would be expected in a production environment.