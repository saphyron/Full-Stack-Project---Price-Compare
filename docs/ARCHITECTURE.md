



### Historik: Domain-lag og Entity Framework

Projektet startede med et klassisk domain-lag (`Brand`, `Category`, `Price`, `Money` osv.)
designet til at blive brugt sammen med Entity Framework Core.

Undervejs valgte jeg at gå væk fra EF Core og i stedet bruge Dapper direkte mod MySQL
for at have:

- Fuld kontrol over SQL-queries
- Simpel performance-fejlretning
- Mindre “magisk” infrastruktur i et undervisnings-/portfolio-projekt

Det oprindelige domain-lag er derfor flyttet til mappen:

`Kode der ikke længere bliver brugt/Domain`

Koden bliver ikke længere kompileret og er kun bevaret som historik/arbejdsproces,
ikke som aktiv del af løsningen.
