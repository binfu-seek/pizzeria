# Pizzeria

## Overview
This is a Rest API backend application built using C#, .Net Core 3.1. It has adopted Dependency Injection pattern to decouple each parts of the app. And it also used InMemory DB to simplify the case, and the initial data has been seeded.

## Architecture
The solution has been separated into 5 projects and adopted popular Clear Architecture or Onion Architecture.

### Pizzeria.Core
As the core project of the whole solution, it only contains business-related definitions for the entity mode, DTOs, as well as the interfaces. 

### Pizzeria.Services
Implemented the service interface defined in the **Core** project, therefore it is dependent on the **Pizzeria.Core** project.
It contains the implementation of business logic.

### Repositories
Contains the implementation of data access logic that defined in the interfaces in the core project.

### Pizzeria.Web
Implemented the REST API endpoints. It is the surface layer that rely on all other projects including  **Pizzeria.Core**, **Pizzeria.Services**, and **Repositories** projects.

### Pizzeria.Tests
Contains unit tests for all other projects.

## Assumptions
The application is built based on below assumptions:
- Pizza prices are integers
- One pizza can only apply zero or one topping of the choice
- If several copies of one pizza is ordered, the topping for them are same
- For one customer order, only pizzas from one outlet can be ordered

## Endpoints and sample requests
- GET /pizzeria
    - Retrieve a list of all outlets and all the pizza details for sale under each outlet

- POST /pizzeria/totalPrice
    - Calculate total price based on the outlet, types and numbers of pizzas, and toppings
    - Sample request body:
```
{
    "OutletID": 1,
    "PizzaOrderList": [
        {
            "PizzaID": 1,
            "Quantity": 2,
            "ToppingID": 1
        },
        {
            "PizzaID": 2,
            "Quantity": 2,
            "ToppingID": 2
        }
    ]
}
```
- POST /pizzeria/newOutlet
    - Create new outlet by inserting new record in database
    - Insert pizzas for sale in this outlet in another table
    - Sample request body:
```
{
    "OutletName": "Berwick",
    "PizzaPriceList": [
        {
            "PizzaID": 1,
            "Price": 14
        },
        {
            "PizzaID": 4,
            "Price": 39
        }
    ]
}
```
- POST /pizzeria/updatePrice
    - Update one or more pizza price for a single outlet
    - Sample request body:
```
{
    "OutletID": 3,
    "PizzaPriceList": [
        {
            "PizzaID": 1,
            "Price": 16
        },
        {
            "PizzaID": 4,
            "Price": 31
        }
    ]
}
```

## How to run and test
- Running the application is simple on local. Just clone the repo and open it using Visual Studio 2019. Then set Pizzeria.Web as the start up project, build and run it.
- The endpoints can be tested via Postman using above sample payload when application is running.

## Things to be done and known issues
- Implement authentication/token mechanism to make sure the NewOutlet and UpdatePrice endpoints can only be accessed by authenticated and authorised users, such as business admin.
- Add logging for exceptions and other places
- InMemory DB does not support transactions. For new outlet open operation, there are two steps: insert a new record to Outlet table and insert records to OutletPizza table to define the pizzas for sale in the new outlet. So these operations should be atomic, which means they should be all successful or to be completely rolled back. It can be solved by using transactions in non-InMemory DB.
- Unit tests are only added for the controller class and two functions in the service class due to time constraint. More unit tests should be added to cover the other two functions in the service class and the repository class.
