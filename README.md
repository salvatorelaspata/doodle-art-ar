# Doodle Art AR

Applicazione Unity che affianca a un menu di navigazione due esperienze: una stanza
virtuale 3D e un'esperienza in realtà aumentata basata su Vuforia, attivata dal
riconoscimento di un Image Target (un disegno / "doodle" stampato).

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
`Assets/Resources/VuforiaConfiguration.asset`.

> **Attenzione:** l'asset è tracciato da git e il commit iniziale è già stato pubblicato
> sul repository GitHub pubblico. Le chiavi presenti in questa versione del progetto
> devono quindi essere considerate **compromesse**: chiunque può leggerle dalla storia
> del repository, e riscrivere la storia non basterebbe a renderle di nuovo segrete
> (fork, cache e mirror restano).

Il progetto è in fase di sviluppo e le chiavi attuali sono di prova, quindi la
situazione è tollerata. Prima di un uso reale — pubblicazione, condivisione con altri,
o passaggio a una licenza a pagamento:

1. Revoca la chiave compromessa dal
   [License Manager Vuforia](https://developer.vuforia.com/vui/develop/licenses)
   e generane una nuova.
2. Verifica che `Assets/Resources/VuforiaConfiguration.asset` sia ignorato da git
   (la regola è già presente in `.gitignore`, vedi sotto).
3. Inserisci la nuova chiave localmente da *Window → Vuforia Configuration*, senza
   committare l'asset.

#### Se ricrei il repository da zero

`.gitignore` contiene già la regola che esclude `VuforiaConfiguration.asset`. La regola
**non ha effetto sul repository attuale**, perché git continua a tracciare i file già
aggiunti all'indice: vale solo a partire da un repository nuovo. Quindi, ricreando il
repo (`rm -rf .git` + nuovo repo GitHub), l'asset resterà automaticamente fuori dal
primo commit — a patto di aver prima rigenerato la chiave, dato che quella vecchia
resta pubblica nel repo precedente finché non lo elimini.

Per chi clona un repo in cui l'asset non è versionato: Unity rigenera
`VuforiaConfiguration.asset` con valori vuoti alla prima importazione di Vuforia, ed è
sufficiente incollare la propria chiave da *Window → Vuforia Configuration*.

## Struttura del progetto

```
Assets/
├── Scenes/
│   ├── 01_MenuPrincipale.unity      # Menu di ingresso, navigazione tra le esperienze
│   ├── 02_StanzaVirtuale3D.unity    # Ambiente 3D non-AR
│   └── 03_EsperienzaAR.unity        # Esperienza AR con Vuforia Image Target
├── Resources/
│   └── VuforiaConfiguration.asset   # Configurazione e license key Vuforia
├── StreamingAssets/Vuforia/
│   ├── doodle-art.xml               # Database target: ImageTarget "test" (21.0 × 26.1 cm)
│   └── doodle-art.dat
├── Editor/Vuforia/ImageTargetTextures/doodle-art/
│   └── test_scaled.jpg              # Immagine da stampare / inquadrare
├── RouterController.cs
├── rotate_cube.cs
└── interazione_cube.cs
```

Tutte e tre le scene sono già incluse in *Build Settings*.

## Script

| Script | Ruolo |
| --- | --- |
| `RouterController.cs` | Navigazione tra le scene (`ApriMenuPrincipale`, `ApriStanzaVirtuale`, `ApriEsperienzaAR`) e uscita dall'app. I metodi sono pensati per essere agganciati agli `onClick` dei bottoni UI. |
| `rotate_cube.cs` | Rotazione continua sull'asse Y più oscillazione verticale sinusoidale. Parametri esposti: `velocitaRotazione`, `velocitaFluttuazione`, `altezzaFluttuazione`. |
| `interazione_cube.cs` | Raycast su tocco/clic tramite il nuovo Input System: al colpo sull'oggetto ne randomizza il colore e ne aumenta la scala del 10%. |

## Come provare l'esperienza AR

1. Stampa (o apri su un secondo schermo) `Assets/Editor/Vuforia/ImageTargetTextures/doodle-art/test_scaled.jpg`.
   Il target è calibrato su un formato di 21.0 × 26.1 cm.
2. Apri `Assets/Scenes/03_EsperienzaAR.unity` e avvia il Play Mode: con una webcam
   collegata Vuforia usa la *Play Mode* via webcam.
3. Inquadra il target: il contenuto agganciato all'Image Target compare in overlay.

Per il test su dispositivo: *File → Build Settings*, seleziona Android o iOS e compila.
Su Android l'architettura target è ARM64 e il minimo SDK richiesto è il 26 (Android 8.0).

## Note tecniche

- Render pipeline: **URP 17.6.0**
- Input: **Input System 1.20.0** (nuovo sistema; gli script usano `Touchscreen`/`Mouse`,
  non le API legacy `Input.*`)
