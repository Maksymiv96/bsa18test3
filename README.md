# bsa18test3
## Cars
-Список всіх машин (GET) - 
route [/api/cars/]

-Деталі по одній машині (GET) - 
route [/api/cars/{idname}]

-Видалити машину (DELETE) - 
route [/api/cars/{idname}]

-Додати машину (POST) - 
body [car { string ident, double balance, string type}]

## Parking

-Кількість вільних місць (GET) - [api/parking/free]

-Кількість зайнятих місць (GET) - [api/parking/total]

-Загальний дохід (GET) - [api/parking/money]

## Transactions

-Вивести Transactions.log (GET) - [api/transactions/log]

-Вивести транзакції за останню хвилину (GET) - [api/transactions]

-Вивести транзакції за останню хвилину по одній конкретній машині (GET) - [api/transactions/{idstring}]

-Поповнити баланс машини (PUT) - [api/transactions/{id}] + body [int]
