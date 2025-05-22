# Shop Saga - Zaawansowane Programowanie Obiektowe

## Opis projektu

Projekt implementuje aplikację sklepu z magazynem, wykorzystującą zaawansowane wzorce programistyczne i architektoniczne w technologii .NET (C#) z użyciem MassTransit i RabbitMQ. 

### Główne zastosowane wzorce i technologie:
- **Maszyna stanów (State Machine)** - realizowana przez `OrderSagaStateMachine` z MassTransit
- **Saga** - zarządzanie procesem zamówienia jako transakcją rozproszoną
- **Wzorzec wydawca-abonent (Pub/Sub)** - komunikacja pomiędzy klientem, sklepem i magazynem za pomocą MassTransit
- **Broker wiadomości** - RabbitMQ
- **Architektura GUI** - w wersji konsolowej (można rozbudować do WPF i MVVM)
- **Zasady SOLID** - kod jest modularny i zgodny z najlepszymi praktykami

## Struktura systemu

- **Shop.Server** - serwer zarządzający zamówieniami i magazynem, implementuje sagę i konsumentów MassTransit
- **Shop.Client** - klient wysyłający zamówienia i odbierający potwierdzenia
- **Messages** - definicje komunikatów (StartOrder, ConfirmOrder, RejectOrder itd.)

## Proces zamówienia

1. Klient wysyła zamówienie (StartOrder) do sklepu.
2. Sklep uruchamia sagę, wysyłając zapytania o potwierdzenie do magazynu (CheckStock) i klienta.
3. Magazyn sprawdza dostępność i rezerwuje zasoby, odsyła potwierdzenie lub odmowę.
4. Klient potwierdza lub odrzuca zamówienie.
5. Sklep kończy proces zamówienia sukcesem (potwierdzenie) lub porażką (odrzucenie).
6. Zamówienie niepotwierdzone w ciągu 20 sekund jest anulowane (timeout).

## Uruchomienie

1. Uruchom RabbitMQ (localhost, domyślne konto guest/guest)
2. Uruchom Shop.Server
3. Uruchom jeden lub więcej klientów Shop.Client
4. Klienci mogą składać zamówienia przez konsolę

---

## Technologie

- .NET 6+
- MassTransit 7+
- RabbitMQ
- C#
- Wzorce projektowe: Saga, State Machine, Pub/Sub
