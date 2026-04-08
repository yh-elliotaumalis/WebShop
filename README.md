# WebShop
Grupparbete

Definition of done
- Koden är skriven i en branch för user storyn.
- Koden är testad praktiskt och genom kollad.
- koden är dokumenterad.
- koden möter acceptanskriterierna.
- koden har blivit reviewed.
- Mergad in i dev branch och inte master.


User stories
Kund
- Som besökare vill jag se en startsida med välkomst-text så att jag förstår vad shoppen har att erbjuda
	- Välkomst text
-  Som besökare vill jag se utvalda produkter och visa de 6 mest sålda produkterna på startsidan så att jag snabbt hittar populära varor
	-  Minst 3 produkter visas
- Som besökare vill jag kunna se en tydlig meny för att jag lätt ska kunna navigera
	- Att ha en fungerande meny navigation (gå fram/tillbaka)
	
- Som kund vill jag kunna söka efter produkter via fritext så att jag snabbt kan hitta det jag söker
	- Sökfält
	- Filtreras på input
- Som kund vill jag kunna välja en produkt för att få mer information för att få bättre koll på vad man ska köpa
	- Namn, beskrivning, pris, färg, storlek? och kategori
- Som kund vill jag kunna lägga till produkter i varukorgen så att jag kan köpa produkten
	- Lägg till - knapp tryck (H/L knapparna?)
	- Produkter läggs till i varukorg
	
Varukorg
- Som kund vill jag kunna se alla tillagda varor för att kunna få en överblick
	- Lista med valda produkter visas
- Som kund vill jag få ett totalpris för att se vad allt kostar
	- Summering visas längst ner
	- Uppdateras vid ändring
	- Summera alla varor
- Som kund vill jag kunna ändra antal produkter så att jag kan justera min beställning
	- Antal kan ökas/minskas
	- Totalpriset ändras
- Som kund vill jag kunna ta bort produkter ur varukorg så att justera min beställning
	- Kunna ta bort varor från varukorg - knapp tryck (T/Delete)
	- Totalpris ändras
- Som kund vill jag ha en köp knapp för att gå vidare mot betalning
	- visuell knapp med knapp tryck (K/B)
	- Skicka vidare till betalnings menyn

Frakt
- Som kund vill jag kunna välja mellan olika frakt alternativ så att jag kan välja pris och leveranstid som passar mig bäst
	- Minst 2 alternativ
	- Pris visas per alternativ - 40kr, 90kr
	- Random mellan - 0,10
	- Fri frakt över 500kr
- Som kund måste jag ange namn och adress så att varorna kan levereras
	-  Namn och adress ska fyllas i
	-  Validering mot att det inte får vara tomma strängar

Betalning
- Som kund vill jag se sammanfattning över min order så att jag vet vad jag betalar för
	- produkter och pris visas
- Som kund vill jag se totalpris inklusive frakt samt moms
	- Räkna med moms och frakt i totalpris
- Som kund vill jag kunna välja betalningsmetod
	- 2 betalningsalternativ
- Efter genomförd betalning skall systemet tömma varukorgen så att det blir lätt för nästa kund
	- Varukorgen töms efter betalning
- Betalnings bekräftelse att köpet gått igenom så att kunden vet att det är färdigt
	- En bekräftelse som visar att det gått igenom (kvittens)

Admin
- Som admin vill jag kunna hantera produkter så att allt blir smidigt
	- Ta bort produkt
	- Lägga till produkt
	- redigera produkt
		- redigera namn, beskrivning pris
		- sätta kategori
		- sätta leverantör
		- sätta lager saldo
- Som admin vill jag skapa och hantera produktkategorier
	- Placera produkter i kategorier
	- Skapa nya kategorier
	- Ta bort kategorier - constraint (bara om den är tom)
- Som admin vill jag kunna ändra kund uppgifter
	- Kan ändra kund uppgifter
- Som admin vill jag kunna se kunders historik
	- få en lista av köp

Struktur
 - Onion Architecture 
	 - 1.Domain
		 - ENTITIES
			 - Produkt
				 - GUID Id
				 - Namn
				 - Beskrivning
				 - decimal Pris
				 - Leverantör
				 - Färg
				 - Storlek
				 - LagerAntal
				 - GUID LeverantörId
				 - Leverantör Leverantör
				 - GUID KategoriId
				 - Kategori Kategori
				
			- Leverantör
				- GUID Id
				- Namn
				- List- Produkt- Produkter

			- Kategori
				- GUID Id
				- Namn
				- List -Produkt- Produkter
				
			 - Kund
				 - GUID Id
				 - Namn - required
				 - Gata
				 - Stad
				 - Postnummer
				- Telefon - required
				- Email - required
				- list-order- Ordrar 
				
			- Order
				- GUID Id
				- DateTime OrderDatum
				- Decimal TotalPris 
				- KundId
				- Kund Kund
				- GUID FraktOmbudId
				- FraktOmbud FraktOmbud
				- List-ProduktOrder- ProduktOrdrar
				
			- ProduktOrder (JoinTabel)
				- GUID Id
				- int Antal
				- decimal PrisvidKöp
				- GUID OrderId
				- Order Order
				- GUID ProduktId
				- Produkt Produkt

			- FraktOmbud
				- GUID Id
				- Namn
				- Pris
				- List-Order- Odrar
				
	 - 2.Application
		 -  SERVICE
		 -  HELPERS
	 - 3.Infrastrucutre
		 - EF
			 - DBCONTEXT - USER SECRETS
			 - CONFIGURATIONS
	 - 4.Presentation
		 - UI
		 - MENU
		 - DISPLAYSERVICE
		 - WINDOW
		program.cs
 - LINQ
- ASYNC 
- Valfri HTTP API
- Olika collections (Queue för varukorg?)
- CLEAN CODE
