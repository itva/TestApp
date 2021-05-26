# TestApp

## При запуске стартуют два проекта (ServiceA, ServiceB), при этом окно браузера открывается только для ServiceB

### TestDbModel     БД
### IDbContext      Интерфейсы для работы с БД и файлами
### DbContext       Имплементация интерфейсов

### ServiceA        Статусы подразделений. Статический экземпляр Dictionary<long, bool> - id подразделения -> статус. По таймеру меняются случайным образом каждые 3 секунды.
### ServiceB        Загружает сведения о подразделениях от IDbContext, передает id подразделения и получает данные от ServiceA. Формирует интерфейс. Из фильтрации исключены статусы.


## 1.json          Пример файла для загрузки
## 1_cycle.json    Пример некорректного файла для загрузки
