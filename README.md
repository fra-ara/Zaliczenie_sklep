# 🛒 ShopApp – Distributed Order System

Projekt zaliczeniowy z przedmiotu **Zaawansowane Programowanie Obiektowe**  

---

## 📌 Opis projektu

System rozproszony typu "Sklep z magazynem" zrealizowany w oparciu o:
- 🧠 **Maszynę stanów (State Machine)** – zamówienia obsługiwane przez sagi
- 🌀 **Sagi (Saga Pattern)** – kontrola przepływu komunikatów
- 📣 **Wydawca–abonent (Pub/Sub)** – RabbitMQ + MassTransit
- 📦 **Broker wiadomości (RabbitMQ)** – do komunikacji między procesami
- 🖥️ **GUI w MVVM** – konsolowa aplikacja działająca w architekturze MVVM

---

## 🏗️ Technologia

- [.NET 8](https://dotnet.microsoft.com/)
- [MassTransit](https://masstransit.io/)
- [RabbitMQ](https://www.rabbitmq.com/)
- Docker (do uruchomienia RabbitMQ)
- C# (.NET console) + MVVM

---

## 🔧 Uruchamianie

### 1. Wymagania
- Docker
- .NET 8 SDK
- RabbitMQ (uruchom przez Docker)

```bash
docker run -d --hostname rabbitmq --name rabbitmq \
  -p 5672:5672 -p 15672:15672 rabbitmq:3-management
```

Panel RabbitMQ: http://localhost:15672
Login: guest, Hasło: guest

## 🚀 Uruchomienie

1. Upewnij się, że RabbitMQ działa lokalnie.
2. W terminalu, uruchom serwer:

``` 
```bash
cd Shop.Server
dotnet run
```

3. W drugim terminalu. uruchom klienta:

```bash
cd Shop.Client
dotnet run
```

4. W konsoli klienta wpisuj ilość do zamówienia (np. 1, 2, itd.).

- System losowo potwierdza lub odrzuca zamówienia.

- Serwer i klient logują przebieg całego procesu.

### 🧠 Architektura
MassTransit z RabbitMQ jako message broker

Saga (state machine) w OrderSagaStateMachine.cs śledzi cykl życia zamówienia

ClientService.cs reaguje na prośby o potwierdzenie

### 🧪 Funkcjonalność
Sprawdzenie dostępności towaru

Potwierdzenie lub odrzucenie przez klienta

Obsługa timeoutu

Finalizacja lub anulowanie zamówienia

     
