const functions = require('firebase-functions');
const express = require('express');
const app=express();
// const serverrequet=	require('./lib/serverrequet');
const serverrequet2=require('./lib/serverrequet2');

// // Create and Deploy Your First Cloud Functions
// // https://firebase.google.com/docs/functions/write-firebase-functions
//



app.use('/data',serverrequet2);


app.get('/', (req, res) => {
res.send('Hello from Firebase!');
});



exports.mydatabase = functions.https.onRequest(app);



/*exports.googleads = functions.https.onRequest((request, response) => {
 response.send("Hello from Firebase!");
});*/

