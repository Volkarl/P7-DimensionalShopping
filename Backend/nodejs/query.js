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

router.get('/:URL/:UA/:DELETECOOKIES', (req, res, next) => {
    const curURL           = req.params.URL;
    const curUA            = req.params.UA;
    const curDELETECOOKIES = req.params.DELETECOOKIES;

    exec(`./P7-DimensionalShopping/Backend/callQueryWithUser.sh ${curURL} ${curUA} ${curDELETECOOKIES} $(cat /etc/username)`, function(err, stdout, stderr) {
        if (err) {
            res.status(400).json({
                error: stderr
            });
        }
        else {
            output = stdout;
            resultRegex = /RESULT:<([^>]*)>:RESULT/gm; // Match the text within RESULT:<HERE>:RESULT and allows newlines
            outputResult = resultRegex.exec(output); 
            if (outputResult !== null && outputResult !== undefined) {
                outputResult = outputResult[1]; // Return only what we found within the capture group (ie. the parentheses)
            }
            res.status(200).json({
                message:        "Result from backend",
                url:            curURL,
                ua:             curUA,
                cookies:        curDELETECOOKIES,
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
