function Translate() {
    //Initialization
    this.init = function (attribute, lang) {
        this.attribute = attribute;
        this.lang = lang;
    }
    //Translate 
    this.process = function () {
        _self = this;
        var xhrFile = new XMLHttpRequest();
        //Load content data 
        //Has the format: XMLHttpRequest.open(method, url, async)
        xhrFile.open("GET", "./dist/language/" + this.lang + ".json", false);// not sure if it slashes should be backslaches. 
        xhrFile.onreadystatechange = function () {
            // 4 is the flag that signals that the operation is complete. 
            //Does not say if it was a  success or failed
            if (xhrFile.readyState === 4) {
                //Status 200 denotes a successful request.
                //Before the request is complete, the value of status will be 0. 
                //Browsers report a status of 0 in case of XMLHttpRequest errors too.
                if (xhrFile.status === 200) {
                    var LangObject = JSON.parse(xhrFile.responseText);
                    // finds all elements with a specific tag
                    var elementsfromtag = document.getElementsByTagName("*");
                    for (var i = 0; i < elementsfromtag.length; i++) {
                        var elem = elementsfromtag[i];
                        var key = elem.getAttribute(_self.attribute);

                        if (key != null) {
                            elem.innerHTML = LangObject[key];
                        }
                    }

                }
            }
        }
        xhrFile.send();
    }
}