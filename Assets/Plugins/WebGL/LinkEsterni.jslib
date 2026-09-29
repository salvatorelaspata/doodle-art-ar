// Apertura di link esterni dalla build Web.
// I browser aprono una nuova scheda solo durante un evento generato dall'utente, mentre Unity
// elabora i clic un frame dopo: il link viene quindi preparato alla pressione del pulsante e
// aperto dal browser stesso al rilascio del dito o del mouse.
var LinkEsterni = {
  $statoLink: {
    apri: null,
    annulla: function () {
      if (statoLink.apri) {
        document.removeEventListener("mouseup", statoLink.apri, true);
        document.removeEventListener("touchend", statoLink.apri, true);
        statoLink.apri = null;
      }
    }
  },

  PreparaApriLink: function (indirizzo) {
    var url = UTF8ToString(indirizzo);
    statoLink.annulla();
    statoLink.apri = function () {
      statoLink.annulla();
      var finestra = window.open(url, "_blank");
      if (finestra) {
        finestra.opener = null;
      } else {
        // Popup bloccato comunque: si apre la pagina nella scheda corrente
        window.location.href = url;
      }
    };
    document.addEventListener("mouseup", statoLink.apri, true);
    document.addEventListener("touchend", statoLink.apri, true);
  },

  AnnullaApriLink: function () {
    statoLink.annulla();
  }
};

autoAddDeps(LinkEsterni, "$statoLink");
mergeInto(LibraryManager.library, LinkEsterni);
