// Filtro de escritura para Nombre y Apellido en "Crear cuenta":
// solo deja escribir letras (con tildes y ñ), espacios, guiones y apóstrofos.
// Funciona por delegación de eventos, así que sirve aunque la página se cargue después.
(function () {
    const fieldIds = ["FirstName", "LastName"];
    const notAllowed = /[^\p{L} '\-]/gu;

    function isNameField(target) {
        return target && target.tagName === "INPUT" && fieldIds.includes(target.id);
    }

    // Bloquea el carácter antes de que aparezca en el campo.
    document.addEventListener("beforeinput", function (e) {
        if (!isNameField(e.target) || typeof e.data !== "string") {
            return;
        }

        if (e.data.replace(notAllowed, "") !== e.data) {
            e.preventDefault();
        }
    });

    // Limpia lo que llegue por pegado, arrastre o autocompletado.
    document.addEventListener("input", function (e) {
        const input = e.target;
        if (!isNameField(input)) {
            return;
        }

        const clean = input.value.replace(notAllowed, "");
        if (clean !== input.value) {
            const position = input.selectionStart - (input.value.length - clean.length);
            input.value = clean;
            input.setSelectionRange(position, position);
        }
    });

    // Límite de longitud del campo.
    document.addEventListener("focusin", function (e) {
        if (isNameField(e.target)) {
            e.target.maxLength = 50;
        }
    });
})();
