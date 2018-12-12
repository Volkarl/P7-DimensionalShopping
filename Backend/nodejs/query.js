const express = require('express');
const router = express.Router();

var exec = require('child_process').exec,child;

router.get('/', (req, res, next) => {
    exec('date', function(err, stdout, stderr) {
       	if(err) {
            res.status(400).json({
                error: stderr
            });
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

    exec(`./P7-DimensionalShopping/Backend/nodejs/callQuery.sh ${curURL} ${curUA} ${curDELETECOOKIES} ${curLOCATION}`, function(err, stdout, stderr) {
        if (err) {
            res.status(400).json({
                error: stderr
            });
        }
        else {
            output = stdout;
            resultRegex = /RESULT:<([^>]*)>:RESULT/gm;
            outputResult = resultRegex.exec(output)[1]; // Match the text within RESULT:<HERE>:RESULT and allows newlines
            res.status(200).json({
                message:        "Result from backend",
                url:            curURL,
                ua:             curUA,
                cookies:        curDELETECOOKIES,
                location:       curLOCATION,
                result:         outputResult,
                log:            output,
                nodeName:       process.env.MY_NODE_NAME,
                podName:        process.env.MY_POD_NAME,
                podNamespace:   process.env.MY_POD_NAMESPACE,
                podIP:          process.env.MY_NODE_NAME
            });
        }
    });
});

module.exports = router;
