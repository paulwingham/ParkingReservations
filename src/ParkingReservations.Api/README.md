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
I was unsure what to do about this, so I have a LoginController and try to return a JWT token, but I have not used security for the endpoints.

## Unit Tests
I have just provided unit tests for the Core project Service folder methods, more would be provided for full test coverage.
