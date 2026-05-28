## Proiect MovieLog
#### Descriere
MovieLog este o platforma web care permite gestionarea colectiilor personale de filme, acordarea de note si scrierea recenziilor. Proiectul este dezvoltat pentru a demonstra integrarea unui API RESTful cu un frontend interactiv.

#### Tehnologii Utilizate
- [ ] Backend: ASP.NET Core Web API

- [ ] Frontend: HTML5, CSS3 (Bootstrap 5), JavaScript (Vanilla)

- [ ] Baza de date: Entity Framework Core

- [ ] Autentificare: ASP.NET Identity (Roles: Admin, User)

#### Functionalitati Principale
- [ ] Gestionare filme: Adminii pot adauga, edita si sterge filme din catalog.

- [ ] Watchlist: Utilizatorii pot salva filmele preferate pentru vizionare ulterioara.

- [ ] Recenzii si Rating: Utilizatorii pot acorda note (1-10) si pot scrie opinii despre filmele vizionate.

- [ ] Validari: Validare de date atat pe partea de server (DTO-uri), cat si pe partea de client (Browser).

- [ ] Securitate: Rute protejate in functie de rolul utilizatorului (Admin vs User).

#### Arhitectura
Proiectul utilizeaza un model hibrid, unde partea de MVC se ocupa de randarea paginilor, in timp ce interactiunea cu datele se face asincron prin apeluri de tip fetch catre un API dedicat. Aceasta abordare asigura o experienta fluida a utilizatorului si o separare clara a responsabilitatilor.
