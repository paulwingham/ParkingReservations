# Parking reservations

## Controller endpoints
It is difficult to know what endpoints to use, and what parameters to accept because there is no UI. So I have provided some that I feel may be useful to the UI.
The ones provided should be able to allow you to select and find a parking slot, and then to safely book it, without duplication.

## SQLite database
The database is stored in the `E:\HSS\REPOS\ParkingReservations\src\ParkingReservations.Infrastructure\SqlLite\CarParkEsher-SQLLite.db` file. 
It's location is specified in appsettings.json here:   
	"ParkingReservationConnectionString": "Data Source=E:\\HSS\\REPOS\\ParkingReservations\\src\\ParkingReservations.Infrastructure\\SqlLite\\CarParkEsher-SQLLite.db"
You can use a SQLite browser to view the contents of the database.

## Login and Authentication
I was unsure what to do about this, so I have a LoginController that returns a JWT token, and a UserDto object, but I have not used security for the endpoints.

## Unit Tests
I have just provided unit tests for the Core project Service folder methods, more would be provided for full test coverage.


## Testing - please use .http files
I have provided easy to run tests using the ParkingReservations.http file. Some of these end showing error messages, others return data as expected.

Please note I have added a new ParkingStructure to the database, called "Horsham", and added one parking space to this structure. The tests pick up available parking spaces for both structures.
This was done to prove the code is extensible to multiple parking structures.



