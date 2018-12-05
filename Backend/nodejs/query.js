const express = require('express');
const router = express.Router();

var exec = require('child_process').exec,child;

router.get('/', (req, res, next) => {
    exec('date', function(err, stdout, stderr) {
       	if(err) {
            output = stderr;
       	}
        else {
       	    output = stdout;
	    res.status(200).json({
	        message: "Hello",
       	        date: output
            });
       	}
   });
});

router.get('/:URL/:UA/:DELETECOOKIES/:LOCATION', (req, res, next) => {
    const curURL           = req.params.URL;
    const curUA            = req.params.UA;
    const curDELETECOOKIES = req.params.DELETECOOKIES;
    const curLOCATION      = req.params.LOCATION;

    exec(`./callQuery.sh ${curURL} ${curUA} ${curDELETECOOKIES} ${curLOCATION}`, function(err, stdout, stderr) {
        if (err) {
            res.status(400).json({
            error: stderr
        });
        }
        else {
            output = stdout;
            res.status(200).json({
                message:    "Result from backend",
                URL:        curURL,
                UA:         curUA,
                Cookies:    curDELETECOOKIES,
                location:   curLOCATION,
                result:     output
            });
        }
    });
});

module.exports = router;
