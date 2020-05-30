# Payment Gateway Application

This repository contains a simple API application that simulates a payment done from a merchant to a specific payment gateway. Also, a mock service will simulate the acquiring bank response based on some application parameters.


# Pre Requisites

- NET Core 3.1 SDK
- Docker

# Application Settings

The following application settings need to be setup before running the application:

- PaymentGatewayDatabase:PaymentsCollectionName - The MongoDB collection name to store and retrieve the payment entities.
- PaymentGatewayDatabase:ConnectionString - The MongoDB connection string.
- PaymentGatewayDatabase:DatabaseName - The MongoDB database name.
___
- Authentication:JwtKey - The JWT key (256bit) for API authentication.
- Authentication:JwtIssuer - The principal that requested the JWT.
- Authentication:JwtAudience - The recipients that JWT is intended for.
___
- AcquiringBankParameters:BankTotalBalance - The total bank balance that the acquiring bank contains when the application starts.
- AcquiringBankParameters:TransactionMinTimeMs - The minimum time (in MS) that a transaction will take.
- AcquiringBankParameters:TransactionMaxTimeMs - The maximum time (in MS) that a transaction will take.
- AcquiringBankParameters:TransactionTimeoutTimeMs - The time (in MS) that makes a transaction fail because of timeout.
___
- Encryption:Key - The encryption key (32 byte key on base 64). Can be generated with command 

> openssl rand -base64 32

- Encryption:IV - The encryption IV (16 byte key on base 64). Can be generated with command 

> openssl rand -base64 16
---

## Example Application Settings

    {  
      "Logging": {  
        "LogLevel": {  
          "Default": "Information",  
          "Microsoft": "Warning",  
          "Microsoft.Hosting.Lifetime": "Information"  
      }  
      },  
      "AllowedHosts": "*",  
      "PaymentGatewayDatabase": {  
        "PaymentsCollectionName": "Payments",  
        "ConnectionString": "mongodb://localhost:27017",  
        "DatabaseName": "local"  
      },  
      "Authentication": {  
        "JwtKey": "qwertyuiopasdfghjklzxcvbnm123456",  
        "JwtIssuer": "PaymentGatewayService",  
        "JwtAudience": "www.example.com"  
      },  
      "AcquiringBankParameters": {  
        "BankTotalBalance": "100",  
        "TransactionMinTimeMs": "15000",  
        "TransactionMaxTimeMs": "20000",  
        "TransactionTimeoutTimeMs": "19000"  
      },  
      "Encryption": {  
        "Key": "ZzDu+3X3Bkm2j3RxqHB6uswBSmoirqmyjMlOZlI6hT8=",  
        "Iv": "Hdm1k5ekS98d9M8/YaVscQ=="  
      }  
    }

 
 # Booting up the application
On the project root folder, head to the **docker** sub folder and run the following command

> docker-compose up

This will pull and start a docker container for a MongoDB that will start running on port 27017.

On the project root folder, head to the **config** sub folder. In here we can duplicate the appsettings.json file and call it appsettings.Development.json. Add the desired configurations to the newly created file. This is done so we can start the API on Development mode.

Go to the project root folder and head to **src/PaymentGateway.Api** subfolder and run:

> export ASPNETCORE_ENVIRONMENT=Development

This will set our NetCore environment to the development one (Mac and Linux).

> dotnet run

This will start the API application (with development environment settings) on https://localhost:5001 by default.


# Using the application

Now that the MongoDB is running on our container and the API is also set up, we can use the application by going to the following page https://localhost:5001/swagger.

This will open the Swagger UI and will let us do the requests we want to test. There are three different endpoints:

- Login - Is a mocked authentication endpoint that will generate a JWT token. We can call it with the user **checkout** to get a valid bearer token. Afterwards, still on swagger, we can add this token to the next calls (On the Authorize button on top right).
- CreatePayment - (Needs Authentication) - Creates a payment. After the payment is created, the mocked acquiring bank service will process it.
- GetPayment - (Needs Authentication) - Fetches an existing payment.
