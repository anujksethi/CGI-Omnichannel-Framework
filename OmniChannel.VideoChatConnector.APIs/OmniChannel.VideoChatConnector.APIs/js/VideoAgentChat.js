/*
 ps-webrtc-peerjs.js
 Author: Lisa Larson-Kelley (http://learnfromlisa.com)
 WebRTC Fundamentals - Pluralsight.com
 Version 1.0.0
 -
 The simplest WebRTC 2-person chat, with manual signalling
 Adapted from peerjs documentation
*/

navigator.getWebcam = (navigator.getUserMedia ||
                         navigator.webkitGetUserMedia ||
                         navigator.mozGetUserMedia ||
                         navigator.msGetUserMedia);

// PeerJS object ** FOR PRODUCTION, GET YOUR OWN KEY at http://peerjs.com/peerserver **
var peer = new Peer({
    key: '2p9ffp7ol6p3nmi',
    debug: 3,
    config: {
        'iceServers': [
        { url: 'stun:stun.l.google.com:19302' },
        { url: 'stun:stun1.l.google.com:19302' },
        { url: 'turn:numb.viagenie.ca', username: "lisa@learnfromlisa.com", credential: "22paris22" }
        ]
    }
});

var videoChatHub = $.connection.videoChatHub;

$.connection.hub.logging = true;
var callerKey, diallerKey;
var self = this;
var hubConnectionId
videoChatHub.client.newMessage = function (message) {
    // self.setState({ callerKey: message });

    callerKey = message;

    //your code to be executed after 1 second
    console.log(message + " push message received ");
    //if (diallerKey != callerKey) {

    //    $('#chatAudio')[0].play();
    //    var delay = 5000; //1 second
    //    setTimeout(function () {

    //        var call = peer.call(message, window.localStream);
    //        step3(call);
    //        $('#chatAudio')[0].pause();
    //    }, delay);
    //}



}
//$.connection.hub.start().done(function () {
//    console.log("Connected, transport = " + $.connection.hub.transport.name);
//    peer.on('open', function () {
//        $('#my-id').text(peer.id);
//        diallerKey = peer.id;
//        sendMessage(peer.id);
//    });
//}).fail(function (e) {
//    console.log('Connection Error ' + e);
//});



if (videoChatHub) {
    console.log("SignalR jquery hub initialized.");
}

function sendMessage(localPeerId) {
    if (videoChatHub.server) {
        videoChatHub.server.connectAgent(localPeerId);
        console.log(localPeerId + " jquery connectagent called");
    }
}


//function sendMessage(localPeerId) {
//    if (videoChatHub.server) {

//        videoChatHub.server.connectAgent('John', localPeerId, 'Order');

//        videoChatHub.server.send(localPeerId);
//        console.log(localPeerId + " diallerkey shared");
//    }
//}





// On open, set the peer id
peer.on('open', function () {
    $('#my-id').text(peer.id);
    diallerKey = peer.id;
});

peer.on('call', function (call) {
    // Answer automatically for demo
    call.answer(window.localStream);
    step3(call);
});

// Click handlers setup
$(function () {
    //$("input[id=hidCustomerDiallerKey]").change(function () {
    //    //fire your ajax call  
    //    alert('value changed');
    //});
    $("input[id=hidCustomerDiallerKey]").on('change', function() {
        alert($("input[id=hidCustomerDiallerKey]").val());
        $('#myModal').modal({
            show: 'true'
        });
        step1();
            $('#chatAudio')[0].play();
        var delay = 5000; //1 second
        setTimeout(function () {

            initiateFeed();
            $('#chatAudio')[0].pause();
        }, delay);
      

    });
 

    $('#make-call').click(function () {



        window.localStream.getTracks().forEach(t =>  t.stop());
        window.localStream.getAudioTracks().forEach(t =>  t.stop());
        //Initiate a call!
        //var call = peer.call($('#callto-id').val(), window.localStream);
        //step3(call);
    });
    $('#end-call').click(function () {
        window.existingCall.close();
        step2();


    });


    $("#btnShowModal").click(function() {
        
        $.connection.hub.start().done(function () {
            console.log("Connected, transport = " + $.connection.hub.transport.name);
            if (diallerKey != undefined && diallerKey.length > 0) {
                sendMessage(diallerKey);
            }
        }).fail(function (e) {
            console.log('Connection Error ' + e);
        });
        step1();

    });
    // Retry if getUserMedia fails
    $('#step1-retry').click(function () {
        $('#step1-error').hide();
        step();
    });
    $("#myModal").on("hidden.bs.modal", function () {
      
          if (window.existingCall) {
              window.existingCall.close();
              step2();
          }
          window.localStream.getTracks().forEach(t =>  t.stop());
          window.localStream.getAudioTracks().forEach(t =>  t.stop());

    });

    // Get things started
    //step1();
});
function initiateFeed() {
    $.connection.hub.start().done(function () {
        console.log("Connected, transport = " + $.connection.hub.transport.name);
        if (diallerKey != undefined && diallerKey.length > 0) {
            
                var call = peer.call($("input[id=hidCustomerDiallerKey]").val(), window.localStream);
                alert(diallerKey + " customer key " + $("input[id=hidCustomerDiallerKey]").val());
                step3(call);
            
        }
    }).fail(function (e) {
        console.log('Connection Error ' + e);
    });
   // step1();
}
function step1() {

    navigator.mediaDevices.getUserMedia({
        audio: true,
        video: true
    })
    .then(function (stream) {
        // Display the video stream in the video object
        $('#my-video').prop('src', URL.createObjectURL(stream));
        window.localStream = stream;
        step2();
    }
    )
    .catch(function (e) {
        $('#step1-error').show();
        console.log(e);
    });


}

function step2() { //Adjust the UI
    $('#step2').show();
}

function step3(call) {
    // Hang up on an existing call if present
    if (window.existingCall) {
        window.existingCall.close();
    }

    // Wait for stream on the call, then setup peer video
    call.on('stream', function (stream) {
        $('#their-video').prop('src', URL.createObjectURL(stream));
    });
    window.existingCall = call;
    $('#step1', '#step2').hide();
    call.on('close', step2);
    $('#step3').show();
}






