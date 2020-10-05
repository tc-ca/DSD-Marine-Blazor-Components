window.stateManager = {
    save: function (key, str) {
        localStorage[key] = str;
    },
    load: function (key) {
        return localStorage[key];
    },
    updateBreadcrumb: function (href, name) {
        let breadcrumbList = document.getElementsByClassName("breadcrumb");
        if (breadcrumbList.length !== 1) return; 

        let breadcrumb = breadcrumbList[0];

        let a = document.createElement('a'); 
        let link = document.createTextNode(name); 
        a.appendChild(link); 
        a.href = href;

        let newStepItem = document.createElement("li");
        newStepItem.appendChild(a);

        let stepItem = breadcrumb.children.length === 4 ? breadcrumb.children[3] : null;
        if (stepItem) {
            breadcrumb.replaceChild(newStepItem, stepItem);
        } else {
            breadcrumb.appendChild(newStepItem);
        }
    },
    getInputValue: function (id) {
        const input = document.getElementById(id);
        const value = input ? input.value : null;
        return value;
    },
    clearLocalStorage: function () {
        localStorage.clear();
    },
    setClassByTagName: function (name, className) {
        document.getElementsByTagName(name)[0].className = className;
    },
    appendChildModalDiv: function () {
        this.modalContainer = document.createElement('div');
        this.modalContainer.className = "modal-backdrop show"; // show";
        document.body.appendChild(this.modalContainer);
    },
    setBodyModalMode: function() {
        this.setClassByTagName("body", "modal-open");
        this.appendChildModalDiv();
    },
    clearBodyModalMode: function () {
        let modalDivs = document.getElementsByClassName("modal-backdrop");

        let body = document.getElementsByTagName("body");
        if (body.length > 0) {
            if (modalDivs.length > 0) {
                body[0].removeChild(modalDivs[0]);
            }
            body[0].removeAttribute("class");
        }
    }
}

////to clear app data on browser close, uncomment the following:
//window.addEventListener("unload", function () {
//    stateManager.clearLocalStorage();
//});