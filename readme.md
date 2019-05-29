

#### **The project**

This test project expects a SQL-Server with connection `(localdb)\\mssqllocaldb`, but that can be changed in the `appsetting.json`.


#### **The problem**

The POST and GET and DELETE works properly, but PATCH does not. I've used both Swagger `https://localhost:44393/swagger` and Postman to try to call to the API. 

In the `FruitController` I've overridden the `PatchAsync` to be able to set a breakpoint in debug. But the controller receives an empty entity. 
And when creating a constructor for the Fruit-entity and provide a default name (like Banana), the entity in the database will not get updated. 
Presumably because the `AttributesToUpdate` in the `JsonApiContext` is empty.


#### **The API calls**

| Key                     | Value | 
| ----------------------- | ----- |
| Type                    | POST  |
| URL                     | http://localhost:44393/fruit |
| Content-Type            | application/json |
| Body (*first request*)  | { "name": "Strawberry" } |
| Body (*second request*) | { "Name": "Kiwi" } |
| Body (*third request*)  | { "Name": "Pineapple" } |


| Key                 | Value | 
| ------------------- | ----- |
| Type                | GET   |
| URL (*list*)        | https://localhost:44393/fruit |
| URL (*single-item*) | https://localhost:44393/fruit/1 |
| Result              | { "data": { "attributes": { "Name": "Strawberry" }, "type": "fruits", "id": "1" } } |


| Key          | Value | 
| ------------ | ----- |
| Type         | PATCH |
| URL          | http://localhost:44393/fruit/1 |
| Content-Type | application/vnd.api+json |
| Body         | { "data": { "type": "fruit", "id": 1, "attributes": { "text": "Cherry" } } } |
| Result       | { "name": "Strawberry", "id": 1, "stringId": "1" } |


#### **SQL Code to delete the values in the database and reset the identity insert**

```
USE [TestProjectApi]
GO

DELETE FROM Fruits
DBCC CHECKIDENT ('Fruits', RESEED, 0); 

SELECT * FROM Fruits
```
