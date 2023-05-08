# WowDin: A Complete Online Food Ordering Platform Developed with ASP.NET Core MVC

## Table of Content

## Summary of WowDin's Development and Achievements
WowDin is an **online food ordering platform project that replicates a real-life website**, which was completed within three-month in a seven-month microsoft funded bootcamp. The project includes a platform administrator, store management, and ordering web pages, providing a complete shopping process. This article will summarize the important technical details of the **Order module** and showcase the achievements.

The project was developed using **ASP.NET Core MVC**, with a layered architecture of **Repository Pattern + Service Layer**. The front-end utilizes JavaScript and Vue, with data retrieved through API requests using Fetch/Axios, and the back-end utilizes LINQ queries for CRUD operations. Notion was used as a project management tool to enhance schedule management and team collaboration efficiency. Additionally, GitHub and Azure DevOps were used for version control and CI/CD pipeline to ensure application quality.

**Selenium** was used for web testing, with test methods written to simulate user behavior, increasing testing efficiency and reducing the likelihood of human error. Finally, Azure DevOps **blue-green deployment** was utilized to enhance rollback capability and reduce risk when adding or modifying the application.

## The design logic of Software Layered Architecture Pattern
![Software Layered Architecture](https://github.com/MollyH1391/MollyH1391.github.io/blob/14e647e9f14598559f6cddb898e06eba6d07f434/GUI/layered_architecture.PNG)
This project adopts a layered architecture pattern that follows the principles of single responsibility and separation of concerns, which increases code reusability. and improves team collaboration efficiency. It consists of four layers: 
  - Presentation Layer (Controller): Handles client requests, calls the Service Layer, and returns ViewModels.
  - Business Layer (Service): Focuses on processing business logic, calls the Repository Layer, and returns Data Transfer Objects (DTOs) to the Controller.
  - Data Layer (Repository): Specializes in handling database operations.
  - Common Layer: Includes shared Enums and Exception Filters across modules
This architecture greatly facilitates the separation of concerns in the development process, minimizes conflicts in collaboration, and demonstrates significant benefits in multi-person projects.

## Order Placement Process
![Order Placement Process](https://github.com/MollyH1391/MollyH1391.github.io/blob/14e647e9f14598559f6cddb898e06eba6d07f434/GUI/order_process.PNG)
The order process includes obtaining items from the shopping cart, filling out ordering information, choosing payment method (credit card or cash), completing the order, and customer feedback.

## Ensuring Successful Multi-Table CUD Operations with Transactions and Rollback on Failure


## Using Redis to Improve Website Data Access Performance

## Using partial views to reduce duplicate code and enhance user experience

## Using refactoring and the AsNoTracking method to improve web page performance

## Streamlining Website Testing with Automated Selenium Testing

## Ensuring Collaborative Efficiency and Application Quality with Azure DevOps



