# TODO

Attività aperte su Doodle Art AR, raggruppate per area. Dentro ogni area le voci sono
in ordine di priorità indicativo.

## Galleria 3D

- [ ] **Verifica su dispositivo.** Provare la galleria in Play mode con il Device Simulator
      e su un telefono reale, poi tarare i gesti (`InputGalleria`: `sogliaTrascinamento`,
      `lunghezzaMinimaSwipe`, `durataMassimaSwipe`) e la rotazione dello sguardo
      (`CameraGalleria.gradiPerSchermo`).
- [ ] **Opere reali.** Sostituire le sei opere segnaposto in `Assets/Opere/` con immagini vere
      (circa 2048 px sul lato lungo) e testi definitivi. Vedi "Aggiungere o sostituire
      un'opera" nel README.
- [ ] **Camminata libera.** Implementare la modalità `CamminataLibera`, già prevista in
      `GalleriaController`: il pulsante "Cammina" esiste, ma oggi mostra solo l'avviso
      "in arrivo". Da decidere se usare joystick virtuale e trascinamento per lo sguardo,
      oppure un tocco sul pavimento per spostarsi. Servono collisioni con pareti e arredi.
- [ ] **Stanze tematiche.** Poter creare nuove sale, ciascuna con un tema e le proprie opere.
  - [ ] Dati della sala: un asset `StanzaDati` con nome, tema, elenco di `OperaDati` e
        materiali di pareti, pavimento e cornici.
  - [ ] Cornice come prefab, così una sala nuova si allestisce senza copiare oggetti a mano.
  - [ ] Primo passo semplice: una scena per sala, partendo da una copia di
        `02_StanzaVirtuale3D` e cambiando materiali, adesivi e opere.
  - [ ] Passo successivo: costruire la sala dai dati, con le cornici disposte automaticamente
        lungo le pareti in base al numero e alle dimensioni delle opere.
  - [ ] Passaggio tra le sale: porta della galleria cliccabile e/o schermata "Scegli la sala"
        nel menu principale.
  - [ ] Varianti grafiche per tema: carta da parati, pavimento, colori delle cornici, adesivi.
- [ ] **Targhette** accanto alle cornici, con titolo e autore.
- [ ] **Rifiniture.** Animazione d'ingresso nella sala, sguardo che torna al centro dopo il
      trascinamento, suoni al tocco.

## Esperienza AR

- [ ] "Vedi in AR" oggi apre la scena AR generica: passare l'opera scelta nella galleria e
      mostrare il contenuto associato.
- [ ] Catalogo opere condiviso tra galleria e AR: collegare ogni `OperaDati` al proprio
      Image Target nel database Vuforia.
- [ ] Target definitivo: `test_scaled.jpg` è 480 × 597 px e sta in una cartella `Editor`
      (non entra nella build). Sostituirlo con un'immagine ad alta risoluzione.

## Menu e interfaccia

- [ ] Font a fumetto al posto di LiberationSans (ad esempio Luckiest Guy o Fredoka, entrambi
      a licenza libera), creando il relativo font asset TextMesh Pro.
- [ ] Menu in orizzontale con le due schede affiancate: oggi resta una colonna centrale piccola.

## Grafica e prestazioni

- [ ] Valutare MSAA 4x in `Mobile_RPAsset`: oggi è disattivato e il render scale è 0.8.
      Bordi e tratti a inchiostro sarebbero più netti, ma il costo ricade anche sulla scena AR.
- [ ] Galleria: disattivare le ombre della Directional Light, perché lo shader `Doodle/Toon`
      non usa luci in tempo reale.

## Progetto e pubblicazione

- [ ] Revocare e rigenerare la license key Vuforia, compromessa (vedi README), prima di
      qualsiasi uso reale.
- [ ] Recuperare `Packages/com.ptc.vuforia.engine-11.4.4.tgz`: oggi il progetto si apre solo
      grazie alla copia estratta in `Library/`.
- [ ] Player Settings: nome prodotto (`My project`), azienda (`DefaultCompany`) e bundle id
      sono ancora quelli del template URP.
- [ ] `interazione_cube.cs`: sostituire `FindObjectOfType` (deprecato, warning CS0618) con
      `FindAnyObjectByType` e limitare l'ingrandimento del 10% a ogni tocco, che oggi non ha
      un tetto.
