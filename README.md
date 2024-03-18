# Vani Garage API ⚙️🛞

This project is a RESTful API for the Vani Garage application. It is built with C# using .NET 8.0.

## Features

- User Authentication and Registration
- Email Confirmation
- Role-based Authorization
- JWT Token Generation
- Caching
- Swagger Documentation
- Hangfire for Background Jobs

## Some Endpoints

### AuthController
* `POST /login`: Authenticate a user and return a JWT token.
* `POST /register`: Register a new user.
* `POST /ConfirmEmail`: Confirm a user's email address.

### CarsController
* `GET /cars`: Get all cars.
* `GET /cars/{id}`: Get a car by ID.
* `POST /cars`: Add a new car.
* `DELETE /cars/{id}`: Delete a car by ID.

### MakesController
* `GET /makes`: Get all makes.
* `GET /makes/{id}`: Get a make by ID.
* `POST /makes`: Add a new make.


## Technologies Used

- C#
- .NET 8.0
- Entity Framework Core
- Redis
- MySQL
- Hangfire
- MailKit
- SQLite
- Swagger


## License
This project is licensed under 
the MIT License - see the `LICENSE` file for details.
