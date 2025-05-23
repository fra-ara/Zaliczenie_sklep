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

### 2. Budowanie i uruchamianie

``` 
dotnet build
dotnet run --project Shop.Server
dotnet run --project Shop.Client

```
### 📁 Struktura projektu

ShopApp/
├── Shop.Server/         
│   ├── Consumers/       
│   ├── Messages/        
│   ├── Services/        
├── Shop.Client/         
│   ├── Models/
│   ├── ViewModels/
│   ├── Views/
