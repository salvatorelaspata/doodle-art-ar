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

## Distribuzione

I dettagli di ogni voce sono in [RELEASE.md](RELEASE.md).

- [ ] **Android:** creare il keystore di firma e conservarlo fuori dal repository, con un
      backup.
- [ ] **Android:** registrarsi come sviluppatore verificato nell'Android Developer Console,
      necessario per distribuire APK fuori dal Play Store (obbligatorio in Italia dal 2027).
- [ ] **iOS:** iscrizione all'Apple Developer Program e prima build su iPhone da Xcode, poi
      TestFlight.
- [ ] **Landing:** attivare GitHub Pages con sorgente "GitHub Actions" e fare il primo
      deploy di `landing/`. Verificare che l'indirizzo coincida con `LinkApp.PaginaDownload`
      (vedi RELEASE.md, "Landing").
- [ ] **Landing:** compilare i link di App Store e Google Play in `landing/index.html` (oggetto
      `LINK`) e, a pubblicazione avvenuta, sostituire i pulsanti con i badge ufficiali.
- [ ] **Landing:** aggiornare la schermata della galleria quando ci saranno le opere reali.
      Oggi mostra le opere segnaposto.
- [ ] **Norme sulla privacy:** pagina richiesta da entrambi gli store, per via della
      fotocamera. Può stare nella landing, ad esempio `landing/privacy.html`.
- [ ] **Web:** dopo la pubblicazione in `landing/gioca/`, provare la galleria dai browser dei
      telefoni (Safari su iPhone, Chrome su Android). Verificare anche l'avviso "La realtà
      aumentata è nell'app" e l'apertura della landing dal pulsante "Scarica l'app".
- [ ] **Web:** template personalizzato con `icona_app.png` come favicon.
- [ ] **Android 12+:** la schermata di avvio di sistema mostra l'icona su sfondo del tema;
      valutare un tema personalizzato color crema per coerenza con lo splash.
- [ ] **Store:** norme sulla privacy (l'app usa la fotocamera), screenshot di telefono e
      tablet, testi delle schede.

## Progetto

- [ ] Recuperare `Packages/com.ptc.vuforia.engine-11.4.4.tgz`: oggi il progetto si apre solo
      grazie alla copia estratta in `Library/`.
- [ ] `interazione_cube.cs`: sostituire `FindObjectOfType` (deprecato, warning CS0618) con
      `FindAnyObjectByType` e limitare l'ingrandimento del 10% a ogni tocco, che oggi non ha
      un tetto.
