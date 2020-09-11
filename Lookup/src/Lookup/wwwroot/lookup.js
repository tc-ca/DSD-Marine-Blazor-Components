// This file is to show how a library package may provide JavaScript interop features
// wrapped in a .NET API

window.lookup = {
    assemblyname: "Lookup",
    setFocus: function (element) {
        if (element) element.focus();
    },
    blurElement: function (element) {
        if (element) element.blur();
    },
    showPrompt: function (message) {
        return prompt(message, 'Type anything here');
    },
    // No need to remove the event listeners later, the browser will clean this up automagically.
    addKeyDownEventListener: function (element) {
        element.addEventListener('keydown', function (event) {
            var key = event.key;

            if (key === "Enter") {
                event.preventDefault();
            }
        });
    }
};
