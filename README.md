# ShotTheShip
Domyślny stosunek wymiarów ekranu dla gry ShotTheShip wynosi 3:4.

Do wykonania gwiazd poruszających się w tle, oraz tarczy statku bohatera wykorzystano elementy graficzne udostępnione przez autora książki "Projektowanie gier przy użyciu środowiska Unity i języka C# - J. G. Bonda. Autor udostepnił również klasę Utils, jako narzędzie pomocnicze do opracowania projektu.
Do wykoniania części modeli wrogich statków wykorzystałem program blender.

Założenia gry: celem gry jest zniszczenie jak największej ilości wrogich statków. Statek bohatera wyposażony jest w tarczę, która chroni go przed zniszczeniem w przypadku zderzenia z wrogim statkiem. Poziom tarczy na początku gry jest równy 1. Każde zderzenie z wrogim statkiem zmniejsza poziom tarczy o 1. Gdy statek 
gracza zderzy się z wrogim statkiem nie posiadając tarczy (poziom 0), nastąpi jego zniszczenie oraz restart gry. Pokonani wrogowie upuszczają obiekty wzmacniające.
O - element zwiększający poziom tarczy o 1;
S - element dodający dodatkową broń typu "spread" (maks. 5);
B - element dodający dodatkową broń typu "blaster" (maks. 5);

Sterowanie: sterowanie statkiem bohatera odbywa się przy użyciu kalwiszy WSDA, lub strzałek kierunkowych. Za oddanie strzału odpowiada przycisk spacji.

Skrypty:
BoundsCheck - odpowiada za utrzymywanie na ekranie wybranych obiektów;
Enemy - klasa nadrzędna wroga;
Enemy1, Enemy2, Enemy3, Enemy4 - klasy dziedziczące po klasie Enemy, odpowiadające za rodzaj wrogów;
Hero - klasa odpowiada za poruszanie się statkiem bogatera, oraz podnoszenie obiektów wzmacniających;
Main - klasa zarządzająca grą, odpowiada za tworzenie losowego wroga nad krawędzią ekranu, oraz tworzenie obiektów wzmacniających;
Parallax - odpowiada za ruch tła w grze;
PowerUp - klasa obiektów wzmacniających;
Projectile - klasa pocisku bohatera, definiująca typ broni, z której oddano strzał;
Shield - odpowiada za zmianę wyglądu tarczy bohatera, w zależności od jej poziomu;
Utils - klasa udostępniona przez autora wyżej wymienionej książki jako narzędzie. Wykorzystana do wykonania płynnego ruchu statków w oparciu o krzywe Beziera;
Weapon - klasa która definiuje rodzaje broni;
