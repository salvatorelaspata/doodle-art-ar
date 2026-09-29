# Doodle Art AR

Applicazione Unity che affianca a un menu di navigazione due esperienze: una galleria
3D da visitare e un'esperienza in realtà aumentata basata su Vuforia, attivata dal
riconoscimento di un Image Target (un disegno / "doodle" stampato).

Interfaccia e galleria seguono uno stile doodle/cartoon: contorni neri spessi, ombre
nette e colori pop, ripresi dall'opera usata come target.

Il nome dell'app è **Doodle Art Experience** (bundle ID `com.salvatorelaspata.doodleart`).

- Le attività aperte sono elencate in [TODO.md](TODO.md).
- Le opzioni per distribuire l'app (Android, iOS, Web) sono descritte in
  [RELEASE.md](RELEASE.md).
- La landing con i link agli store è in `landing/` e si pubblica su GitHub Pages con il
  workflow `.github/workflows/landing.yml` (vedi RELEASE.md, "Landing").

## Requisiti

- **Unity 6000.6.3f1** (Unity 6.6) — versione esatta indicata in `ProjectSettings/ProjectVersion.txt`
- **Vuforia Engine 11.4.4** distribuito come tarball locale (vedi [Setup](#setup))
- Piattaforme target: **Android** (min SDK 26, ARM64) e **iOS**
- Git LFS, usato per gli asset binari (`.png`, `.jpg`, `.fbx`, `.obj`, `.psd`, `.wav`, `.mp3`, `.zip`)

## Setup

```bash
git lfs install
git clone <url-del-repo>
cd doodle-art-ar
```

### Pacchetto Vuforia (passaggio obbligatorio)

`Packages/manifest.json` referenzia Vuforia come tarball locale:

```json
"com.ptc.vuforia.engine": "file:com.ptc.vuforia.engine-11.4.4.tgz"
```

**Il file `Packages/com.ptc.vuforia.engine-11.4.4.tgz` non è presente nel repository
né, allo stato attuale, sulla macchina di sviluppo.** Il progetto si apre solo perché
Unity conserva la copia già estratta in
`Library/PackageCache/com.ptc.vuforia.engine@58134fb7b422`: cancellando `Library/`
(operazione ordinaria per forzare una reimportazione) la dipendenza non è più
risolvibile e il progetto va in errore.

Per ripristinare la dipendenza:

1. Scarica `com.ptc.vuforia.engine-11.4.4.tgz` dal
   [portale sviluppatori Vuforia](https://developer.vuforia.com/downloads/sdk)
   (richiede un account PTC gratuito).
2. Copialo in `Packages/`, accanto a `manifest.json`.
3. Riapri il progetto: il Package Manager risolve `file:` contro il tarball locale.

Il tarball resta escluso dal versionamento — `.gitignore` ignora `*.tgz` e l'EULA PTC
limita la redistribuzione dell'SDK — quindi questo passaggio va ripetuto su ogni nuova
macchina o dopo ogni clone pulito.

### License key Vuforia

La configurazione Vuforia, incluse `vuforiaLicenseKey` e `ufoLicenseKey`, è salvata in
`Assets/Resources/VuforiaConfiguration.asset`. L'asset è escluso da `.gitignore` e non è
mai stato versionato in questo repository: la chiave resta solo sulla macchina di
sviluppo.

Su un clone nuovo Unity rigenera `VuforiaConfiguration.asset` con valori vuoti alla prima
importazione di Vuforia. Basta incollare la propria chiave da *Window → Vuforia
Configuration*, generandola dal
[License Manager Vuforia](https://developer.vuforia.com/vui/develop/licenses) se serve.
Il piano Basic è gratuito e permette di pubblicare app con Image Target, senza watermark
(vedi [RELEASE.md](RELEASE.md)).

## Struttura del progetto

```
Assets/
├── Scenes/
│   ├── 01_MenuPrincipale.unity      # Menu di ingresso, navigazione tra le esperienze
│   ├── 02_StanzaVirtuale3D.unity    # Galleria 3D a postazioni
│   └── 03_EsperienzaAR.unity        # Esperienza AR con Vuforia Image Target
├── Scripts/
│   ├── RouterController.cs          # Navigazione tra le scene
│   ├── BottoneDoodle.cs             # Interfaccia: effetto "adesivo premuto"
│   ├── DecorazioneOndeggiante.cs    # Interfaccia: animazione delle decorazioni
│   ├── AreaSicura.cs                # Interfaccia: adattamento a notch e angoli arrotondati
│   ├── AvvisoSoloApp.cs             # Web: avviso "la realtà aumentata è nell'app"
│   ├── PulsanteLinkEsterno.cs       # Web: apertura di link esterni senza blocchi popup
│   ├── LinkApp.cs                   # Indirizzi esterni (landing di download)
│   ├── rotate_cube.cs               # Oggetti interattivi
│   ├── interazione_cube.cs
│   └── Galleria/                    # Logica della galleria 3D
├── Opere/                           # Dati delle opere (OperaDati), immagini in Immagini/
├── Shaders/
│   └── DoodleToon.shader            # Shader cartoon della galleria
├── Materials/
│   ├── LiberationSans SDF - Titolo Doodle.mat   # Contorno del titolo del menu
│   └── Galleria/                    # Materiali della sala (shader Doodle/Toon)
├── Sprites/
│   ├── Menu/                        # Pannelli, icone e decorazioni dell'interfaccia
│   └── Galleria/                    # Icone dei pulsanti della galleria
├── Textures/Galleria/               # Carta da parati e parquet (ripetibili)
├── Branding/                        # Icona dell'app (anche adattiva Android) e splash screen
├── Plugins/WebGL/LinkEsterni.jslib  # Apertura dei link nel browser (solo build Web)
├── Settings/Build Profiles/         # Profili di build: Android - Test, Android - Google Play, iOS, Web
├── Resources/
│   └── VuforiaConfiguration.asset   # Configurazione e license key Vuforia
├── StreamingAssets/Vuforia/
│   ├── doodle-art.xml               # Database target: ImageTarget "test" (21.0 × 26.1 cm)
│   └── doodle-art.dat
└── Editor/Vuforia/ImageTargetTextures/doodle-art/
    └── test_scaled.jpg              # Immagine da stampare / inquadrare
```

Tutte e tre le scene sono incluse nella lista globale delle scene, usata dalle build Android
e iOS. Il profilo `Web` esclude la scena AR, perché Vuforia non supporta il browser.

## Script

### Navigazione e interfaccia

| Script | Ruolo |
| --- | --- |
| `RouterController.cs` | Navigazione tra le scene (`ApriMenuPrincipale`, `ApriStanzaVirtuale`, `ApriEsperienzaAR`) e uscita dall'app. I metodi sono pensati per essere agganciati agli `onClick` dei bottoni UI. Nella build Web `ApriEsperienzaAR` mostra `avvisoSoloApp` invece di caricare la scena AR. |
| `AvvisoSoloApp.cs` | Avviso animato "La realtà aumentata è nell'app", presente nel menu e nella galleria. Si vede solo nella build Web. |
| `PulsanteLinkEsterno.cs` | Apre un indirizzo web al tocco. Nel browser usa `Plugins/WebGL/LinkEsterni.jslib` per aprire la nuova scheda al rilascio del dito, così non viene bloccata come popup. Senza indirizzo usa `LinkApp.PaginaDownload`. |
| `LinkApp.cs` | Indirizzi esterni in un solo punto: `PaginaDownload` è la landing con i link agli store (oggi un segnaposto). |
| `BottoneDoodle.cs` | Alla pressione sposta la "faccia" del pulsante sulla sua ombra, come un adesivo schiacciato. Campi: `faccia`, `spostamentoPremuto`, `velocita`. |
| `DecorazioneOndeggiante.cs` | Oscillazione di rotazione e leggera fluttuazione per le decorazioni del menu. |
| `AreaSicura.cs` | Adatta un `RectTransform` a `Screen.safeArea` e si aggiorna quando il telefono ruota. Contiene i pulsanti delle scene 02 e 03. |

### Galleria 3D (`Scripts/Galleria/`)

| Script | Ruolo |
| --- | --- |
| `OperaDati.cs` | ScriptableObject di un'opera: titolo, autore, anno, descrizione, immagine e dimensioni reali in cm. Si crea da *Create → Doodle Art → Opera*. |
| `CorniceOpera.cs` | Adatta tela, passe-partout e bordi della cornice alle dimensioni dell'opera e applica l'immagine. Funziona anche in editor (`ExecuteAlways`). |
| `GalleriaController.cs` | Regia della visita: postazioni, vista di dettaglio, testi dell'interfaccia e avvisi. Prevede la futura modalità `CamminataLibera`. |
| `CameraGalleria.cs` | Spostamenti morbidi della camera, inquadratura frontale delle opere nella parte libera dello schermo, sguardo col trascinamento, campo visivo adattato all'orientamento. |
| `InputGalleria.cs` | Riconosce tocco, trascinamento e swipe con mouse o touch (`Pointer` dell'Input System) e ignora i gesti che partono sull'interfaccia. |
| `PannelloOpera.cs` | Scheda di dettaglio animata: in basso in verticale, a destra in orizzontale. Calcola la parte di schermo lasciata libera per l'opera. |

### Oggetti interattivi

| Script | Ruolo |
| --- | --- |
| `rotate_cube.cs` | Rotazione continua sull'asse Y più oscillazione verticale sinusoidale. Parametri esposti: `velocitaRotazione`, `velocitaFluttuazione`, `altezzaFluttuazione`. Usato dal cubo sul piedistallo della galleria. |
| `interazione_cube.cs` | Raycast su tocco/clic tramite il nuovo Input System: al colpo sull'oggetto ne randomizza il colore e ne aumenta la scala del 10%. Usato dal cubo sul piedistallo della galleria. |

## Galleria 3D

La scena `02_StanzaVirtuale3D` è una sala di 10 × 7 m con sei opere, una panca, due
lampade, due piante e un piedistallo con un cubo interattivo. Le opere attuali sono
**segnaposto**: portano l'etichetta "SEGNAPOSTO".

- Si parte dalla vista dell'ingresso e si visitano le opere in giro continuo: parete
  sinistra, parete di fondo, parete destra, di nuovo ingresso.
- Le frecce ‹ › o lo swipe passano all'opera successiva o precedente.
- Toccando un'opera ci si sposta davanti a lei. Toccando l'opera corrente, o l'etichetta col
  titolo, si apre la scheda di dettaglio con titolo, autore, anno, dimensioni, descrizione e
  il pulsante "Vedi in AR".
- Trascinando il dito ci si guarda intorno, entro certi limiti.
- Nel dettaglio lo swipe sfoglia le schede delle altre opere e un tocco sulla stanza la chiude.
- Il pulsante "Cammina" è predisposto per la futura camminata libera: per ora mostra solo
  un avviso.

### Aggiungere o sostituire un'opera

- **Sostituire un segnaposto:** apri il suo asset in `Assets/Opere/`, cambia immagine e testi
  e imposta larghezza e altezza reali in cm. La cornice si ridimensiona da sola, anche in
  editor.
- **Aggiungere un'opera nuova:**
  1. Importa l'immagine in `Assets/Opere/Immagini/`, circa 2048 px sul lato lungo.
  2. Crea l'asset con *Create → Doodle Art → Opera* e compila i campi.
  3. Nella scena duplica una `Galleria/Opere/Cornice_0X` e spostala sulla parete. L'asse Z
     (blu) della cornice deve puntare **dentro** il muro. Assegna il nuovo asset al campo `dati`.
  4. Aggiungi la cornice all'array `opere` di `GalleriaController`, sull'oggetto `Galleria`.
     L'ordine dell'array è l'ordine della visita.

## Grafica e interfaccia

- **Scala dell'interfaccia:** tutti i Canvas usano *Scale With Screen Size* in modalità
  *Expand*.
  - Menu: riferimento 1080 × 1920, così la colonna del menu entra sempre nello schermo.
  - Scene 02 e 03: riferimento 1080 × 1080, così i pulsanti hanno la stessa dimensione in
    verticale e in orizzontale.
- **Sprite dell'interfaccia** (`Sprites/Menu`): pannelli arrotondati con bordo nero, in
  9-slice. Il riempimento è bianco e si colora con il `Color` dell'`Image`, mentre il bordo
  resta nero. Sono importati senza compressione.
- **Titolo del menu:** usa un materiale TextMesh Pro con contorno. L'ombra netta è una copia
  nera del testo spostata in basso a destra: l'underlay SDF creava artefatti.
- **Shader `Doodle/Toon`**, usato da tutti i materiali della galleria. È unlit: non usa luci
  in tempo reale né lightmap. Parametri principali:
  - `_SpessoreBordi`: tratto a inchiostro lungo gli spigoli, in pixel. Funziona sui cubi,
    dove ogni faccia ha UV da 0 a 1. Sulle sfere va lasciato a 0.
  - `_SogliaContorno`: contorno per gli oggetti curvi (sfere).
  - `_ScalaMondo`: metri per ripetizione della texture proiettata dal mondo, per avere la
    stessa densità su muri e pavimento. Con 0 usa le UV della mesh.
  - `_ColoreOmbra`, `_DirezioneLuce`, `_SogliaOmbra`: luce a due toni. Le facce rivolte
    dall'altra parte prendono il colore d'ombra.
  - `_Cutoff`: ritaglio per gli adesivi sui muri.

## Come provare la galleria

Apri `Assets/Scenes/02_StanzaVirtuale3D.unity` e avvia il Play Mode. In editor il mouse fa
le veci del dito: clic per toccare, trascinamento per guardarsi intorno, trascinamento
veloce in orizzontale per lo swipe. Per le proporzioni di un telefono usa il
*Device Simulator* (*Window → General → Device Simulator*).

## Come provare l'esperienza AR

1. Stampa (o apri su un secondo schermo) `Assets/Editor/Vuforia/ImageTargetTextures/doodle-art/test_scaled.jpg`.
   Il target è calibrato su un formato di 21.0 × 26.1 cm.
2. Apri `Assets/Scenes/03_EsperienzaAR.unity` e avvia il Play Mode: con una webcam
   collegata Vuforia usa la *Play Mode* via webcam.
3. Inquadra il target: il contenuto agganciato all'Image Target compare in overlay.

Per il test su dispositivo: *File → Build Profiles*, seleziona il profilo `Android - Test` o
`iOS`, premi *Switch Profile* e compila.
Su Android l'architettura target è ARM64 e il minimo SDK richiesto è il 26 (Android 8.0).
I passaggi completi (firma, TestFlight, Google Play, Web) sono in [RELEASE.md](RELEASE.md).

## Note tecniche

- Render pipeline: **URP 17.6.0**. Su Android e iOS è attivo `Mobile_RPAsset`: renderer
  Forward, render scale 0.8, MSAA disattivato.
- Input: **Input System 1.20.0** (nuovo sistema; gli script usano `Touchscreen`/`Mouse`/`Pointer`,
  non le API legacy `Input.*`)
- Gli asset binari (`.png`, `.jpg`…) passano da Git LFS: prima del commit verifica che
  `git lfs install` sia stato eseguito.
