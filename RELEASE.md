# Distribuzione

Come far arrivare **Doodle Art Experience** sui dispositivi: prima ai tester, senza store,
poi sugli store ufficiali. Il documento copre Android, iOS e Web.

> Scadenze e requisiti degli store cambiano spesso: le informazioni sono aggiornate a
> settembre 2026 (vedi [Fonti](#fonti)). Prima di ogni pubblicazione ricontrolla le pagine
> ufficiali.

## Stato della configurazione

Già impostato in *Project Settings → Player*:

| Voce | Valore |
| --- | --- |
| Nome prodotto | Doodle Art Experience |
| Azienda | Salvatore La Spata |
| Bundle ID (Android, iOS, desktop) | `com.salvatorelaspata.doodleart` |
| Versione | `0.1.0` (Android *Bundle Version Code* 1) |
| Icona | `Assets/Branding/icona_app.png`, predefinita per tutte le piattaforme e usata da iOS |
| Icona adattiva Android | Sfondo `icona_android_sfondo.png` + primo piano `icona_android_primo_piano.png` |
| Splash di Unity | Logo del titolo su sfondo crema a scarabocchi, senza "Made with Unity" |
| Launch screen iOS | Stesso logo su sfondo crema, così non c'è stacco con lo splash |
| Descrizione fotocamera (iOS) | "La fotocamera serve a riconoscere il disegno e a mostrare l'opera in realtà aumentata." |
| Android | Minimo Android 8.0 (API 26), target automatico, ARM64, Vulkan + OpenGL ES 3 |
| iOS | Minimo iOS 15.0, Metal |
| Orientamento | Rotazione automatica: menu, galleria e pulsanti si adattano a verticale e orizzontale |

Il logo "Made with Unity" è stato disattivato, cosa consentita da Unity 6 anche con la
licenza Personal. Si riattiva da *Player → Splash Image → Show Unity Logo*.

Ancora da fare, perché dipende da account e segreti personali:

- **Android:** creare il keystore di firma. Vedi [Firma dell'app](#firma-dellapp).
- **iOS:** indicare il *Team ID* Apple, oppure sceglierlo in Xcode alla prima build.

## Prima di ogni rilascio

- [ ] Incrementare la versione (`0.1.0` → `0.1.1` …) e il numero di build. Ogni caricamento
      su Play Console o App Store Connect richiede un numero di build più alto del precedente:
      su Android è il *Bundle Version Code*, su iOS il *Build*.
- [ ] Verificare che `Assets/Resources/VuforiaConfiguration.asset` contenga una license key
      valida. È solo in locale e non è versionato.
- [ ] Controllare le scene in *Build Profiles*: tutte e tre per le app, senza la scena AR per
      il Web (vedi [Web](#web)).
- [ ] Provare la build su almeno un dispositivo reale per piattaforma.

---

## Android

### Firma dell'app

Ogni APK o AAB va firmato con un **keystore**.

- Si crea da *Player → Publishing Settings → Keystore Manager*.
- File e password vanno conservati **fuori dal repository**: `.gitignore` esclude già
  `*.keystore` e `*.jks`. Tienine un backup sicuro.
- Senza Google Play, se perdi il keystore non puoi più pubblicare aggiornamenti della stessa
  app.
- Con Google Play conviene attivare **Play App Signing**: Google custodisce la chiave
  definitiva e il tuo keystore diventa solo una "chiave di caricamento", recuperabile se va
  persa.

### Opzioni di distribuzione

| Opzione | Costo | Per chi | Note |
| --- | --- | --- | --- |
| **Build and Run via USB** | gratis | Tu, in sviluppo | Attiva *Opzioni sviluppatore* e *Debug USB* sul telefono, poi *Build And Run* da Unity |
| **APK condiviso** (link, Drive, email) | gratis | Pochi tester | Il tester abilita l'installazione da fonti sconosciute. Vedi sotto la verifica degli sviluppatori |
| **Firebase App Distribution** | gratis | Gruppi di tester | Inviti via email, notifiche di nuove versioni, APK o AAB. È sempre un'installazione fuori dal Play Store |
| **Google Play – test interno** | 25 $ una tantum (account Play Console) | Fino a 100 tester | Pubblicazione quasi immediata. La strada consigliata appena ci sono tester esterni |
| **Google Play – test chiuso / aperto** | incluso | Gruppi più ampi | Il test chiuso è obbligatorio per i nuovi account personali (vedi sotto) |
| **Google Play – produzione** | incluso | Tutti | Revisione di Google, scheda dello store completa |

**Verifica degli sviluppatori Android.** Google sta rendendo obbligatoria la registrazione
degli sviluppatori anche per le app installate fuori dal Play Store:

- dal **30 settembre 2026** in Brasile, Indonesia, Singapore e Thailandia;
- **dal 2027 in tutti i paesi**, Italia compresa, sui dispositivi Android certificati.

Un'app di uno sviluppatore non registrato non si installa più per la via normale. Restano
possibili l'installazione via ADB e un "percorso avanzato" volutamente scomodo (modalità
sviluppatore, riavvio, 24 ore di attesa). Per continuare a distribuire APK fuori dallo store
conviene quindi registrarsi come sviluppatore verificato nell'Android Developer Console.

**Requisiti per la pubblicazione su Google Play:**

- Formato **AAB** (*Build Profiles → Android → Build App Bundle (Google Play)*).
- **Target API 36 (Android 16)**, obbligatorio per le app nuove e gli aggiornamenti dal
  31 agosto 2026, con proroga richiedibile fino al 1° novembre 2026. Il target è su
  "automatico" (livello più alto installato): prima di pubblicare verifica che l'AAB punti
  davvero ad API 36.
- **Test chiuso con almeno 12 tester per 14 giorni consecutivi** se l'account Play Console è
  personale e creato dopo il 13 novembre 2023. Gli account aziendali (organizzazione) ne sono
  esenti.
- **Scheda dello store:** icona 512 × 512, immagine in evidenza 1024 × 500, screenshot di
  telefono (e tablet se lo supporti), descrizioni.
- **Norme sulla privacy:** URL obbligatorio perché l'app usa la fotocamera. Vanno compilati
  anche il modulo *Sicurezza dei dati*, la classificazione dei contenuti e il pubblico di
  destinazione.

### Come fare la build

1. *File → Build Profiles → Android → Switch Platform*. La prima volta reimporta tutti gli
   asset e ci vuole qualche minuto.
2. Per i test: *Development Build* attivo, poi **Build** (APK) o **Build And Run** (telefono
   collegato).
3. Per il Play Store: *Development Build* disattivo e *Build App Bundle (Google Play)* attivo,
   poi **Build** (AAB).

---

## iOS

### Prerequisiti

- Mac con **Xcode** (già installato) e modulo **iOS** di Unity (già installato).
- Un Apple ID. Per TestFlight, Ad Hoc e App Store serve l'**Apple Developer Program**
  (99 $/anno).

### Opzioni di distribuzione

| Opzione | Costo | Per chi | Limiti |
| --- | --- | --- | --- |
| **Xcode con Apple ID gratuito** | gratis | Tu, in sviluppo | L'app scade dopo **7 giorni**, solo sui tuoi dispositivi |
| **Build di sviluppo** (account a pagamento) | 99 $/anno | Tu e il team | Dispositivi registrati, profilo valido un anno |
| **Ad Hoc** | 99 $/anno | Tester con dispositivo registrato | Fino a 100 dispositivi per tipo (iPhone, iPad…) all'anno, UDID da raccogliere. Si può distribuire con Firebase App Distribution |
| **TestFlight** (consigliata) | 99 $/anno | Tester | Fino a 100 tester interni senza revisione e fino a 10.000 esterni con una revisione beta leggera. Build valide 90 giorni. Nessun UDID da raccogliere |
| **App Store** | 99 $/anno | Tutti | Revisione Apple, scheda dello store completa |

Nell'Unione Europea esistono anche marketplace alternativi e la distribuzione dal web
previste dal Digital Markets Act. Richiedono però l'autorizzazione di Apple e condizioni
sull'account, quindi per ora non sono una via pratica.

**Requisiti per l'App Store:**

- **Descrizione della fotocamera:** già impostata. Senza, iOS chiude l'app quando Vuforia
  apre la fotocamera, e la revisione la rifiuta.
- **Dettagli sulla privacy** in App Store Connect e **URL delle norme sulla privacy**.
- **Screenshot:** iPhone e anche iPad, perché oggi l'app è abilitata anche su iPad. Se non
  vuoi supportare iPad, cambia *Target Device* in *Player → iOS*.
- **Classificazione per età** e **conformità all'esportazione**: l'app non usa crittografia
  propria, quindi rientra nell'esenzione.

### Come fare la build

1. *File → Build Profiles → iOS → Switch Platform*, poi **Build**. Unity genera un progetto
   Xcode: scegli una cartella dentro `Builds/`, che è già ignorata da git.
2. Apri `Unity-iPhone.xcodeproj` in Xcode, poi *Signing & Capabilities → Team* e seleziona
   il tuo account (firma automatica).
3. **Sul tuo iPhone:** collegalo, selezionalo come destinazione e premi *Run*.
4. **Per TestFlight o App Store:** *Product → Archive*, poi *Distribute App → App Store
   Connect*. La build compare in TestFlight dopo l'elaborazione di Apple.

---

## Web

### Cosa funziona e cosa no

| Parte | Web | Motivo |
| --- | --- | --- |
| Menu | ✅ | UGUI, TextMesh Pro e Input System sono supportati |
| Galleria 3D | ✅ | Lo shader `Doodle/Toon` è leggero (WebGL 2) e l'input `Pointer` copre mouse e touch |
| Esperienza AR | ❌ | **Vuforia non supporta il Web**: il pacchetto include plugin solo per Android, iOS e Windows/UWP |

Unity 6 supporta anche i browser dei telefoni: Chrome, Firefox, Edge e Safari su iOS 15 e
successivi, che devono avere WebGL 2. WebGPU è ancora sperimentale.

### Preparazione della build Web

- **Senza la scena AR:** crea un *Build Profile* Web con solo `01_MenuPrincipale` e
  `02_StanzaVirtuale3D`.
- **Pulsanti AR da adattare:** la scheda "Esperienza AR" del menu e "Vedi in AR" della
  galleria oggi caricano `03_EsperienzaAR`. Nel Web quella scena non c'è, quindi vanno
  nascosti o devono mostrare un invito a scaricare l'app (vedi `TODO.md`).
- **Compressione:** Brotli dà i file più piccoli, ma il server deve inviare gli header giusti
  (`Content-Encoding: br` più il `Content-Type` corretto). Se l'hosting non permette di
  configurarli, attiva *Decompression Fallback* oppure usa Gzip.
- **Formato delle texture:** ASTC per i browser dei telefoni, DXT/BC per i computer. Scegli in
  base al pubblico principale oppure prepara due build.
- **Icona del browser (favicon):** il template *Default* usa quella di Unity. Per usare
  `icona_app.png` serve un template Web personalizzato.
- **Orientamento:** nel browser non si può bloccare, ma l'interfaccia si adatta già a entrambi.
- **HTTPS:** necessario per l'accesso alla fotocamera, se in futuro aggiungi AR nel browser.

### Dove pubblicarla

| Hosting | Costo | Note |
| --- | --- | --- |
| **itch.io** | gratis | Carichi uno zip della build e la pagina la esegue nel browser. Adatto per far provare il progetto |
| **GitHub Pages** | gratis | Hosting statico dal repository, ma senza header personalizzati: serve *Decompression Fallback* o Gzip |
| **Netlify / Cloudflare Pages** | piani gratuiti | Header configurabili (file `_headers`), quindi Brotli funziona, più un dominio personalizzato |
| **Server proprio** (nginx, Apache) | variabile | Controllo completo su header, cache e dominio |

### Realtà aumentata nel browser

| Opzione | Costo | Pro | Contro |
| --- | --- | --- | --- |
| **Nessuna AR sul Web** (consigliata ora) | – | Nessun lavoro in più: menu e galleria online, AR solo nelle app | Sul Web manca l'esperienza AR |
| **Imagine WebAR** (plugin Unity) | a pagamento (Asset Store) | Image tracking dentro Unity WebGL, contenuti 3D riutilizzabili | Serve una scena AR separata per il Web |
| **MindAR** (JavaScript) | gratis, open source | Image tracking maturo nel browser | È fuori da Unity: contenuti da rifare in three.js o A-Frame |

Da evitare:

- **8th Wall:** i servizi ospitati sono stati chiusi il 28 febbraio 2026. Il codice è
  diventato open source, ma senza piattaforma.
- **WebXR:** Safari su iOS non supporta la realtà aumentata WebXR e l'image tracking non è
  standard.

---

## Costi in sintesi

| Voce | Costo |
| --- | --- |
| Vuforia Engine, piano Basic | Gratis: pubblicazione con Image Target senza watermark. A pagamento solo Model Target e Area Target, non usati |
| Google Play Console | 25 $ una tantum |
| Apple Developer Program | 99 $/anno |
| Hosting Web | Gratis con le opzioni indicate sopra |

## Percorso consigliato

1. **Android:** creare il keystore, APK di sviluppo sul tuo telefono, poi test interno su
   Google Play.
2. **iOS:** iscrizione all'Apple Developer Program, build da Xcode sul tuo iPhone, poi
   TestFlight.
3. **Web:** build con menu e galleria, pulsanti AR adattati, pubblicazione su itch.io o
   Netlify.
4. **Store:** norme sulla privacy, schede, screenshot, test chiuso di 14 giorni su Google
   Play, poi revisioni.

## Fonti

- [Google: verifica degli sviluppatori Android](https://support.google.com/android-developer-console/answer/16561738?hl=en)
- [Hacker News: scadenza del 30 settembre 2026 per la verifica degli sviluppatori](https://thehackernews.com/2026/06/google-sets-sept-30-deadline-for.html)
- [Android Authority: tempi delle nuove regole sul sideload](https://www.androidauthority.com/android-sideloading-changes-timeline-3679204/)
- [Google Play: requisiti sul livello API di destinazione](https://support.google.com/googleplay/android-developer/answer/11926878?hl=en)
- [Google Play: test obbligatori per i nuovi account personali](https://support.google.com/googleplay/android-developer/answer/14151465?hl=en)
- [Vuforia: prezzi e licenze](https://developer.vuforia.com/library/vuforia-engine/FAQ/pricing-and-licensing-options/)
- [Unity: compatibilità con i browser](https://docs.unity3d.com/Manual/webgl-browsercompatibility.html)
- [Road to VR: 8th Wall diventa open source e chiude i servizi ospitati](https://roadtovr.com/niantic-webar-platform-8th-wall-open-source/)
- [Imagine WebAR Image Tracker per Unity](https://imagine-webar.com/unity/image-tracker/)
- [MindAR con Unity WebGL (discussione Unity)](https://discussions.unity.com/t/webgl-augmented-reality-mind-ar-js/865545)
