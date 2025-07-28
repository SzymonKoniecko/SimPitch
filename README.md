# SimPitch

![image](https://github.com/user-attachments/assets/1d8f303f-3e7f-4285-a407-8aa5e0d1d623)


Simulation.Api
Warstwa odpowiedzialna za ekspozycję interfejsu API. Znajdziemy tu kontrolery, konfigurację usług (rozszerzenia) oraz punkt startowy aplikacji.

Simulation.Application
Zawiera logikę aplikacyjną, DTO, interfejsy oraz mapery (np. SampleMapper.cs), które służą do mapowania między warstwami.

Simulation.Domain
Warstwa domenowa z definicjami encji, wyjątków oraz interfejsów domenowych (np. agregaty).

Simulation.Infrastructure
Implementacja dostępu do danych (np. kontekst bazy danych, konfiguracje encji) oraz repozytoria. Możliwe są też rozszerzenia specyficzne dla infrastruktury.

Simulation.Tests
Zbiór testów jednostkowych lub integracyjnych w celu weryfikacji poprawności działania aplikacji.
