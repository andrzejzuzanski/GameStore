This application uses ASP NET Core and Minimal API.
Using Entity Framework and SQLite it shares REST endpoints with CRUD operations. 

This is simple games relational database with one relation between two tables.
Games table has relation with Genre table. 

This web application shares endpoints to 
- GET /games - get all games
- GET /games/{id} - to get specific game
- POST /games - to create new game
- PUT /games/{id} - to edit specific game
- DELETE /games/{id} - to delete specific game
- GET /genres - to get all availabe genres

  All entities from database are mapped into specific DTOs 
