# ShooterUnity

## A játék leírása
Egy rendőr figurát irányítunk. A cél, hogy megtaláljuk a kulcsot, amivel bejutunk az arénába, ahol legyőzhetjük a főellenséget. Ha ez sikerül, a játéknak vége. Három-féle ellenség van: zombi, férfi- és női karakter. A játék közben power-upokat is fel lehet szedni: lőszert és medkit-et.

## Scriptek

#### [CameraController](https://github.com/meviktor/ShooterUnity/blob/master/Assets/Scripts/CameraController.cs)
Megadott offsettel követi a játékost.
Beállítható: offset (offsetFromPlayer).

#### [GameCharacterControllerBase](https://github.com/meviktor/ShooterUnity/blob/master/Assets/Scripts/GameCharacterControllerBase.cs)
Itt van definiálva az a viselkedés, ami közös a játékosban, illetve az ellenségekben is. Ilyen pl. hogy mi történjen, ha az adott karakter meghal, hogyan kezeljük, ha eltalálják... azon kívül a közös Update() "viselkedés" is itt található - a származtatott osztályok az Update-ot nem, csak a DoWhileAlive() és a DoWhenDying() metódusokat implementálják.
Beálltható: mozgás sebessége (moveSpeed), erősség (strength), golyó tpus (bullet).

#### [PlayerController](https://github.com/meviktor/ShooterUnity/blob/master/Assets/Scripts/PlayerController.cs)
A játékost vezérlő script. Itt találhatók a csak játékosra vonatkozó tulajdonságok (pl. lőszermennyiség, megtalálta-e a kulcsot...), valamint viselkedés (pl. lövés).
Beállítható: ami az ősosztályban is.

#### [EnemyController](https://github.com/meviktor/ShooterUnity/blob/master/Assets/Scripts/EnemyController.cs)
Az ellenségeket vezérlő script. A legfontosabb itt implementált viselkedések: mit csináljon az ellenség amíg "tétlen", illetve a játékos érzékelése, és "támadása". Ez a következőképpen néz ki: alapesetben az ellenség le-fel járkál egy megadott hosszúságú szakaszon (ha valamivel eközben ütközik, háthra arc és az ellenkező irányban folytatja a sétát). Eközben ellenőrzi, hogy "rálát-e" a játékosra (a játékos adott sugarű körön belülre kerül-e, illetve nincs-e valamilyen objektum közte és a játékos között). Amennyiben igen, akkor elkezdi üldözni a játékost. A zombi típusú ellenség "lövedékkel" támad, míg a férfi- és női karakter pedig a játékoshoz akar hozzáérni, hogy életerőt szedjen le tőle.
Beálltható: séta távolság a megjelenés helyétől (walkDistance), játékos keresési környezet mérete (playerScanRadius), lövési intervallum (shootIntervalInSeconds) - zombinál, játékos követési sebesség (attackRunSpeed).

#### [EnemyBossController](https://github.com/meviktor/ShooterUnity/blob/master/Assets/Scripts/EnemyBossController.cs)
A főellenséget irányító script. Az itt megvalóstott viselkedés: új ellenségek létrehozása, illetve lövedékzápor meghatározott időközönként.
Beálltható: hol jöjjenek létre az új ellenségek (possibleSpawnPositions), ellenségek száma (numberOfSpawningEnemies), lövési időköz (shootInterval), ellenség létrehozási időköz (enemySpawnInterval).

#### [BossGateController](https://github.com/meviktor/ShooterUnity/blob/master/Assets/Scripts/BossGateController.cs)
A végső harcteret elzáró kaput nyitja ki, amennyiben meghívták az OpenGate() metódusát.
Beálltható: -

#### [GateCollisionController](https://github.com/meviktor/ShooterUnity/blob/master/Assets/Scripts/GateCollisionController.cs)
A kapu kinyitását indítja el, amennyiben a játékosnál van a kulcs.
Beállítható: -

#### [LockTriggerController](https://github.com/meviktor/ShooterUnity/blob/master/Assets/Scripts/LockTriggerController.cs)
Ha a játékos belép az arénába, akkor már nem hagyhatja el.
Beálltható: -

#### [CrazyHorseController](https://github.com/meviktor/ShooterUnity/blob/master/Assets/Scripts/CrazyHorseController.cs)
A lószobor materialjának átlátszóságát változatatja (alfa csatorna). Átlátszóból átlátszatlanba és vissza.
Beállítható: intervallum hossz (interval).

#### [BulletController](https://github.com/meviktor/ShooterUnity/blob/master/Assets/Scripts/BulletController.cs)
A golyó ütközését kezeli (mi történjen, ha az ellenség lövedéke eltalálja a játékost vagy fordítva, mi van ha valamilyen tereptárgyba csapódik).
Beállítható: golyó sebesség (bulletSpeed), golyó típus (bulletType). 

#### [PowerUpController](https://github.com/meviktor/ShooterUnity/blob/master/Assets/Scripts/PowerUpController.cs)
Power-upok viselkedését definiálja: mi történik ha felszedik őket, animáció. Három típusa van: medkit, lőszer, kulcs.
Beálltható: erő (power) - mennyi lőszert, életerőt kapjon a játékos, ha felszedi.

#### [GameController](https://github.com/meviktor/ShooterUnity/blob/master/Assets/Scripts/GameController.cs)
Eseményvezérelten vezérli a UI-t (üzenetek a képernyőn, élet- és lőszermennyiség számon tartása, megvan-e a kulcs), megjeleníti a főellenséget.
Beállítható: -

## Unity verzió
2020.3.2f1
