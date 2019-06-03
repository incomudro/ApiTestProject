

### **The project**

This test project expects a SQL-Server with connection `(localdb)\\mssqllocaldb`, but that can be changed in the `appsetting.json`.


### **The problem**

The POST and GET and DELETE works properly, but PATCH does not. I've used both Swagger `https://localhost:44393/swagger` and Postman to try to call to the API. 

In the `FruitController` I've overridden the `PatchAsync` to be able to set a breakpoint in debug. But the controller receives an empty entity. 
And when creating a constructor for the Fruit-entity and provide a default name (like Banana), the entity in the database will not get updated. 
Presumably because the `AttributesToUpdate` in the `JsonApiContext` is empty.


### **The API calls**

| Key                     | Value | 
| ----------------------- | ----- |
| Type                    | POST  |
| URL                     | https://localhost:44393/fruits |
| Content-Type            | application/json |
| Body (*first request*)  | { "name": "Strawberry" } |
| Result                  | *{ "name": "Strawberry", "id": 1, "stringId": "1" }* |
| Body (*second request*) | { "name": "Kiwi" } |
| Result                  | *{ "name": "Kiwi", "id": 2, "stringId": "2" }* |
| Body (*third request*)  | { "name": "Pineapple" } |
| Result                  | *{ "name": "Pineapple", "id": 3, "stringId": "3" }* |


| Key                 | Value | 
| ------------------- | ----- |
| Type                | GET   |
| URL (*list*)        | https://localhost:44393/fruits |
| URL (*single-item*) | https://localhost:44393/fruits/1 |
| Result              | *{ "data": { "attributes": { "name": "Strawberry" }, "type": "fruit", "id": "1" } }* |


| Key          | Value | 
| ------------ | ----- |
| Type         | PATCH |
| URL          | http://localhost:44393/fruits/1 |
| Content-Type | application/vnd.api+json |
| Body         | { "data": { "type": "fruits", "id": 1, "attributes": { "name": "Cherry" } } } |
| Result       | *{ "data": { "attributes": { "name": "Cherry" }, "type": "fruits", "id": "1" } }* |


### **SQL Code to delete the values in the database and reset the identity insert and insert seed data**

```
USE [TestProjectApi]
GO

DELETE FROM Fruit
DBCC CHECKIDENT ('Fruit', RESEED, 0); 

INSERT INTO Fruit (Name) VALUES ('Strawberry')
INSERT INTO Fruit (Name) VALUES ('Kiwi')
INSERT INTO Fruit (Name) VALUES ('Pineapple')

SELECT * FROM Fruit
```


### **Lessons learned:**

**Case-sensitivity:**

For PATCH, the attribute is case-sensitive and should be the same as defined in the model. As the attribute is not case-sensitive for POST, this mistake can be easily overlooked. 

* In the `Fruit.cs`, the atribute `[Attr("name")]` means that in the body of a PATCH, the attribute should be `"name": "Cherry"`. 
* If the attribute is defined as `[Attr("Name")]`, the attribute would be `"Name": "Cherry"`. 

Know that for a POST, correct casing is not required and both `{ "name": "Peach" }` and `{ "Name": "Peach" }` will always work.

**Type:**

The type defined in the PATCH, should be the same as the name for the `DbSet<T>`. In the example project the name of `DbSet<Fruit>` is 'Fruits', see the file `ApplicationDbContext.cs`. This means that the type provided in the body of the PATCH should be 'fruits' as well. Know that the type is not case-sensitive, thus both `"type": "fruits"` and `"type": "Fruits"` will work.
