## POS-PointOfSalesTeamProject
POS Point Of Sales - FastFood Restaurant

### Project Description
The application is indented to respond to the needs of a small fast-food restaurant as well as of a chain of restaurants. It includes the features to manage daily sales operations as well as a customized view for managers and authorized staff to track statistics, update prices and products. It has a user-friendly interface, mainly oriented to using screen and minimum keyboard input, very easy to use and It provides a lot of flexibility and maintainability. It is designed as that expansion might happen. It provides two - way relationship between the user and database.

### Technologies Used:
* WPF with MVVM (Multiple Views)
* Azure - SQL Database
* Charts, Graphs
* Unit Testing

### Special Features:
* User Authentication
* Passing data between different views
* MVVM data Binding
* Price Control
* Printing receipts 
* Exporting statistics to PDF and sending via Email
* Using buttons instead of keyboard input

### Additional Libraries:
 - Modern UI for WPF (Charts and Graphs)
 - iTextSharp Library (PDF generation)

### Application Overview:
The Application is built with two modules: Managers App for tracking statistics and control products and Sellers App for registering daily sales operations, payments and printing receipts.
The two modules are secured by an authentication feature -  a login view

![Login View](https://cloud.githubusercontent.com/assets/12819018/19778958/06d023ac-9c4d-11e6-96eb-1d696f0d8c13.png)

Following the authentication the Seller's Module provides a user-friendly interface which is mainly designed toward using buttons on the screen rather than keyboard input. So we tried to provide functionality from the screen for all the needs. The seller has the products list (dynamically generated from the database) organized in different categories to choose from and populate the order view

![Sellers View](https://cloud.githubusercontent.com/assets/12819018/19779172/ebe1a9e8-9c4d-11e6-800d-f2b2bf183b4c.png)


The application computes the amount to be paid, registers the payment in the database and prints the receipt.

![Receipt Printed](https://cloud.githubusercontent.com/assets/12819018/19779995/7c27537e-9c51-11e6-9928-1de64266568f.png)

 OK, so this application solves one big problem - registering daily sales operations. But how about statistics? So we decided to develop a second Module of the application which would respond to basic managers needs. It has a customized view to track statistics by month in a nice graphical way and generate PDF to be sent by email.

![Statistics View](https://cloud.githubusercontent.com/assets/12819018/19780081/d7ce6e24-9c51-11e6-8083-6b8c099665fe.png)

 Also, we Implemented a view to update the products in the database. This way we provide functionality to maintain usual fluctuations in prices and menus. 

![Add or Edit Products](https://cloud.githubusercontent.com/assets/12819018/19780202/1f1ff540-9c52-11e6-821d-02673d18525a.png)

### Authors and Contributors
This project is the result of two member team work: Valentina Migalatii and Olga Racu.


