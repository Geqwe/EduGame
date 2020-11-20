var firebase = require("firebase/app");
require("firebase/firestore");
require("firebase/database");
require("firebase/auth");
const express = require('express');
var router=express.Router();

var config = {
  appId:"1:278236233379:android:69b93f6a6f9e41acf78916",
  apiKey: "AIzaSyAEVejCMF0s3Fm1fU0XfMf3FhwQM4y95n0",
  databaseURL: "https://edugame-7353d.firebaseio.com",
  storageBucket: "edugame-7353d.appspot.com"
};

var secondApp = firebase.initializeApp(config,"mysecondapp");
  
// Get a reference to the database service
var database = firebase.database();

function sendtoserver(response)
{
	writeUserData(response.id,response);
}
function writeUserData(userId, response) {
  firebase.database().ref('users/' + userId).set(response);
}


router.post('/', (req, res) => {
	var a=req.body;
	console.log(a);
  if(a.option=="save") {
    secondApp.database().ref('users').child(a.username).set(a.data);
  }
  else if(a.option=="saveLevel") {
    secondApp.database().ref('users').child(a.username).child(a.game).child(a.levelReached).set(a.data);
  }
  else if(a.option=="saveFinishing") {
    secondApp.database().ref('users').child(a.username).child(a.game).set(a.data);
  }
  else if(a.option=="load")
  {
    var username;
    var i=0;
    try {
      secondApp.database().ref('users/'+a.username).once('value').then(function(snapshot) {
      if(snapshot.val()===null)
        res.send("noResult");
      else
        res.send(snapshot.val());      
    });
    } catch(error) {
      res.send("noResult");	
    }
  }
});

module.exports =router;
