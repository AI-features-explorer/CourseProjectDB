# Database UI realization

## `Tables of pawnshop DB`

`Client table` -_contains basic customer information_

| Attribute Name | Type         | NULLABLE | Description                     |
| -------------- | ------------ | -------- | ------------------------------- |
| idClient       | int          | No       | Primary key                     |
| ClientName     | nvarchar(50) | No       | Name and initials of the client |
| ClientAddress  | nvarchar(50) | Yes      | Customer's e-mail address       |
| ClientPhone    | nvarchar(50) | Yes      | Customer's phone number         |

`Employee table` - _contains basic employee information_

| Attribute Name    | Type         | NULLABLE | Description                       |
| ----------------- | ------------ | -------- | --------------------------------- |
| idEmployee        | int          | No       | Primary key                       |
| EmployeeName      | nvarchar(50) | No       | Name and initials of the employee |
| EmployeePrivilege | int          | No       | Employee privilege level          |
| EmployeePhone     | nvarchar(50) | No       | Employee's work phone number      |
| EmployeeLogin     | nvarchar(50) | No       | Employee Login                    |
| EmployeePassword  | nvarchar(50) | No       | Employee Password (Hash)          |

`Price table` - _contains basic information on all price variations of the product_

| Attribute Name      | Type    | NULLABLE | Description                                                   |
| ------------------- | ------- | -------- | ------------------------------------------------------------- |
| idPrice             | int     | No       | Primary key                                                   |
| PriceValue          | money   | No       | Numeric price value                                           |
| PriceHeveCommission | char(5) | No       | Flag indicating that a 25% commission is imposed on the price |

`Product table` - _contains basic product information_

| Attribute Name | Type         | NULLABLE | Description           |
| -------------- | ------------ | -------- | --------------------- |
| idProduct      | int          | No       | Primary key           |
| ProductName    | nvarchar(50) | No       | Product Name          |
| ProductOwner   | nvarchar(50) | No       | Name of product owner |

`Operation table` - _contains basic information on pawnshop operations_

| Attribute Name   | Type          | NULLABLE | Description                                        |
| ---------------- | ------------- | -------- | -------------------------------------------------- |
| idOperation      | int           | No       | Primary key                                        |
| fkClient         | int           | No       | Secondary key. Connection with the Client table    |
| fkProduct        | int           | No       | Secondary key. Connection with the Producttable    |
| fkEmployee       | int           | No       | Secondary key. Connection with the Employee table |
| fkPrice          | int           | No       | Secondary key. Connection with the Pricetable      |
| OperationDocLink | nvarchar(MAX) | No       | Link to the contract document                      |
| OperationDate    | date          | No       | The date on which the goods change hands           |

---

Creared in 2020 year.
