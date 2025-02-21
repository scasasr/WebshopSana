# WebshopSana
This is the solution to the technical test. I created an API using ASP.NET. The solution consists of two projects: a Web API and a class library. They are separated to modularize data access and the API implementation.
I had used GraphQL in a university project, where we used it as an API Gateway to access multiple microservices. However, I had not used it directly in .NET before. I tried to use it following tutorials on the HotChocolate package, but when I attempted to modularize queries and mutations, I encountered an error with the context. Due to time constraints, I decided to proceed with the REST API.
For the frontend, I used the code from this repository as a base:
 https://github.com/chaoocharles/complete-shopping-cart
I made the necessary changes, but you can see in the repository how Redux is used to manage state, as well as the use of a memory storage method other than a database—in this case, localStorage.
Instructions:
-To set up the database, there is a folder named Database. Inside, you will find the database diagram and a script to create the database, tables, and some records.
-To run the .NET program, you need to clean the solution, select the "Restore NuGet Packages" option, and then execute it. This will display an interface for the endpoints using Swagger at  the following address:
   https://localhost:7270/swagger/index.html
 You can access some methods using GraphQL at:
   https://localhost:7270/graphql/
 However, they are not fully functional.
 
-To run the frontend, install the dependencies using npm install, and then execute it with npm start.

Additional Information:
The user created with the database script has the following credentials:
 Email: test@gmail.com
 Password: test123*
