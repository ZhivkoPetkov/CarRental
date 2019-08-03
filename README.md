# Car Rental
Car Rental is a web app where you can rent cars in various bulgarian locations (Sofia, Plovdiv, Varna etc). Every customer can choose from which date to which date to make his order and the app will show the available cars for the locations automatically.
After every successfully finished order, the customer will receive a pop-up message to rate the service (from 1 to 5). After every successfully given feedback, the app will generate a voucher for future rents with discount from 1% to 5%.

### Demo
http://carrentalpetkov.azurewebsites.net/

Admin Email: `admin@admin.bg`
Admin Password: `123123`

### Used technology
- ASP.Net Core MVC
- JS

The app uses SignalR to notify the looged in users when someone makes an order. Pop up windows appears on the bottom of the page and automatically closes after some seconds.

![](https://i.imgur.com/y5mriEx.png)

# Features

### Not logged in user
- Access index page for searching
- View the list with all cars in Car Rental

### Registered user
- Access index page for searching
- View the list with all cars in Car Rental
- Search from Date To Date for cars in the locations
- Make orders
- Use vouchers for discount
- See his orders
- See details of every car
- Give review
- See his vouchers
- Change password and phone

### Administrator
- Access index page for searching
- View the list with all cars in Car Rental
- Search from Date To Date for cars in the locations
- Make orders
- Use vouchers for discount
- See his orders
- See details of every car
- Give feedback
- See his vouchers
- Change password and phone
- #### Access admin area
- See all orders
- See all vouchers
- See all reviews
- Add and remove cars
- Add and remove locations
- Delete orders, cancel them or finish them
- Delete reviews
- Delete vouchers or generate custom for user
