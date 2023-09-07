# My smart home REST API
Rest API repository for an IoT project, built in C# using the .NET 6.0 framework, with a MySQL database and JWT authentication. 

This repository contains a Rest API for an IoT project that will be consumed by a Kotlin app and also used with Arduino. The characteristics and functionalities of the API are detailed below.



<p align="center">
 <img src="https://img.shields.io/badge/-JSON-000000?style=for-the-badge&logo=json&logoColor=white" alt="JSON Icon" />
  <img src="https://img.shields.io/badge/-Postman-FF6C37?style=for-the-badge&logo=postman&logoColor=white" alt="Postman Icon" />
  <img src="https://img.shields.io/badge/-MySQL-4479A1?style=for-the-badge&logo=mysql&logoColor=white" alt="MySQL Icon" />
  <img src="https://img.shields.io/badge/-C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white" alt="C# Icon" />
  <img src="https://img.shields.io/badge/.NET%206.0%20Framework-512BD4?style=for-the-badge&logo=.net&logoColor=white" alt=".NET 6.0 Framework Icon" />
  <img src="https://img.shields.io/badge/-REST%20API-FF6C37?style=for-the-badge" alt="REST API Icon" />
  <img src="https://img.shields.io/badge/-JWT-000000?style=for-the-badge" alt="JWT Icon" />
  <img src="https://img.shields.io/badge/-Swagger-85EA2D?style=for-the-badge&logo=swagger&logoColor=black" alt="Swagger Icon" />
</p>

## Characteristics

* The API is built in C# using the .NET 6.0 framework.
* A MySQL database is used to store sensor data.
* JWT authentication is used to secure endpoints that require authentication.
* Swagger is used to document the API.

## Functionalities
The API has the following endpoints:
* **`/`**: Test endpoint that returns a welcome message.
* **`/login`**: Endpoint to authenticate users and obtain a JWT token.
* **`/sensors`**: Endpoint to retrieve the list of sensors stored in the database. Requires authentication.
* **`/sensors/{id}`**: Endpoint to retrieve, update, or delete a specific sensor. Requires authentication.

## Using the API
To use the API, follow these steps:
* Clone the repository to the local machine.
* Configure the database connection in the appsettings.json file.
* Compile and run the API.
* Use an HTTP client, such as Postman, to make requests to the API.


## Usage example

Below is an example of how to use the API to retrieve the list of sensors:

1. To access the API, you must send an HTTP request to the corresponding endpoint, for example: 

```curl
GET https://localhost:7129/

```

To change the port, you must modify the **launchSettings.json** file.
```json
 "profiles": {
    "SmarthomeRest": {
      "commandName": "Project",
      "dotnetRunMessages": true,
      "launchBrowser": true,
      "launchUrl": "swagger",
      "applicationUrl": "https://localhost:7129;http://localhost:5008",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    }
 }

```

2. To authenticate, you should send a POST request to the **/login** endpoint with the following body in JSON format:


```json
{
  "username": "admin",
  "password": "admin"
}
```
3. The response will contain a JWT token that should be sent in the Authorization header of subsequent requests, like this:

```token
"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYmYiOjE2OTQxMDc3OTksImV4cCI6MTY5NDExMTM5OSwiaWF0IjoxNjk0MTA3Nzk5fQ.GlJPznxoJdBskGIG8IVkLzk9m2b8ht3886I5IRL1GdU"
```

4. Send a GET request to the /sensors endpoint with the JWT token in the `Authorization` header.

5. The API will return a JSON with the list of sensors stored in the database.

```json
[
  {
    "id": 1,
    "name": "Sensor de temperatura",
    "value": 25.5,
    "date": "2023-09-07T12:00:00"
  },
  {
    "id": 2,
    "name": "Sensor de humedad",
    "value": 60.0,
    "date": "2023-09-07T12:01:00"
  }
]
```
  #### For more information about available endpoints and the parameters they accept, you can consult the documentation generated with Swagger at the path "localhost:PORT/swagger"

<details>
<summary>Swagger </summary>

```json
{
  "openapi": "3.0.1",
  "info": {
    "title": "Mi SmartHome API",
    "version": "v1"
  },
  "paths": {
    "/": {
      "get": {
        "tags": [
          "SmarthomeRest"
        ],
        "responses": {
          "200": {
            "description": "Success",
            "content": {
              "text/plain": {
                "schema": {
                  "type": "string"
                }
              }
            }
          }
        }
      }
    },
    "/login": {
      "post": {
        "tags": [
          "SmarthomeRest"
        ],
        "requestBody": {
          "content": {
            "application/json": {
              "schema": {
                "$ref": "#/components/schemas/User"
              }
            }
          },
          "required": true
        },
        "responses": {
          "200": {
            "description": "Success"
          }
        }
      }
    },
    "/sensores": {
      "get": {
        "tags": [
          "SmarthomeRest"
        ],
        "responses": {
          "200": {
            "description": "Success",
            "content": {
              "application/json": {
                "schema": {
                  "type": "array",
                  "items": {
                    "$ref": "#/components/schemas/Sensor"
                  }
                }
              }
            }
          }
        }
      },
      "post": {
        "tags": [
          "SmarthomeRest"
        ],
        "requestBody": {
          "content": {
            "application/json": {
              "schema": {
                "$ref": "#/components/schemas/Sensor"
              }
            }
          },
          "required": true
        },
        "responses": {
          "200": {
            "description": "Success"
          }
        }
      }
    },
    "/sensores/{id}": {
      "get": {
        "tags": [
          "SmarthomeRest"
        ],
        "parameters": [
          {
            "name": "id",
            "in": "path",
            "required": true,
            "schema": {
              "type": "integer",
              "format": "int32"
            }
          }
        ],
        "responses": {
          "200": {
            "description": "Success"
          }
        }
      },
      "put": {
        "tags": [
          "SmarthomeRest"
        ],
        "parameters": [
          {
            "name": "id",
            "in": "path",
            "required": true,
            "schema": {
              "type": "integer",
              "format": "int32"
            }
          }
        ],
        "requestBody": {
          "content": {
            "application/json": {
              "schema": {
                "$ref": "#/components/schemas/Sensor"
              }
            }
          },
          "required": true
        },
        "responses": {
          "200": {
            "description": "Success"
          }
        }
      },
      "delete": {
        "tags": [
          "SmarthomeRest"
        ],
        "parameters": [
          {
            "name": "id",
            "in": "path",
            "required": true,
            "schema": {
              "type": "integer",
              "format": "int32"
            }
          }
        ],
        "responses": {
          "200": {
            "description": "Success"
          }
        }
      }
    }
  },
  "components": {
    "schemas": {
      "Sensor": {
        "type": "object",
        "properties": {
          "id": {
            "type": "integer",
            "format": "int32"
          },
          "name": {
            "type": "string",
            "nullable": true
          },
          "value": {
            "type": "number",
            "format": "double"
          },
          "date": {
            "type": "string",
            "format": "date-time"
          }
        },
        "additionalProperties": false
      },
      "User": {
        "type": "object",
        "properties": {
          "username": {
            "type": "string",
            "nullable": true
          },
          "password": {
            "type": "string",
            "nullable": true
          }
        },
        "additionalProperties": false
      }
    },
    "securitySchemes": {
      "Bearer": {
        "type": "apiKey",
        "description": "JSON Web Token based security",
        "name": "Authorization",
        "in": "header"
      }
    }
  },
  "security": [
    {
      "Bearer": [ ]
    }
  ]
}
```
</details>