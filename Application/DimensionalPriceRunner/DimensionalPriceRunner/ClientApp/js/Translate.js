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
        //Has the format:XMLHttpRequest.open(method, url)
        //               XMLHttpRequest.open(method, url, async)
        //               XMLHttpRequest.open(method, url, async, user)
        //               XMLHttpRequest.open(method, url, async, user, password)
        xhrFile.open("GET", "./ClientApp/language/" + this.lang + ".json", false);// not sure if it slashes should be backslaches. 
        xhrFile.onreadystatechange = function () {
            // 4 is the flag that signals that the operation is complete. 
            //Does not say if it was a  success or failed
            if (xhrFile.readyState === 4) {
                //Status 200 denotes a successful request.
                //Before the request is complete, the value of status will be 0. 
                //Browsers report a status of 0 in case of XMLHttpRequest errors too.
                if (xhrFile.status === 200 || xhrFile.status == 0) {
                    var LangObject = JSON.parse(xhrFile.responseText);
                    console.log(LangObject["name1"]);// can be deleted
                    var allDom = document.getElementsByTagName("*");
                    for (var i = 0; i < allDom.length; i++) {
                        var elem = allDom[i];
                        var key = elem.getAttribute(_self.attribute);

                        if (key != null) {
                            console.log(key);//can be deleted
                            elem.innerHTML = LangObject[key];
                        }
                    }

                }
            }
        }
        xrhFile.send();
    }
}