/// <reference path="../reactLib/react.js" />
/// <reference path="../reactLib/react-dom.js" />

var VideoChatModal = React.createClass({

    render: function () {
        return (

          <div className="modal fade" role="dialog">
            <div className="modal-dialog modal-lg">
              <div className="modal-content">
                <div className="modal-header">
                  <button type="button" className="close" data-dismiss="modal">&times;</button>
                  <h4 className="modal-title">Video Chat</h4>
                </div>
                <div className="modal-body">
                   <table>
                       <tr>
                           <th width="40%">My Video</th>
                           <th width="40%">Agent Video</th>
                           <th width="20%">Action</th>
                       </tr>
                       <tr>
                           <td>
                           <video id="my-video" ref="callerVideo" autoplay></video>
                           </td>
                           <td>
                             <video id="their-video" ref="receiverVideo" autoplay class="their-video"></video>
                           </td>
                           <td>
                                    <div id="step2">
	                                    <p>Your id: <span id="my-id">...</span></p>
	                                    <p>Share this id with others so they can call you.</p>
                                        <audio id="chatAudio" ref="chatAudio">
                                             <source src="images/ringtone.mp3" type="audio/mpeg"></source>
                                        </audio>
	                                    <p>
	                                    <span id="subhead">Make a call</span><br />
		                                    <input type="text" placeholder="Call user id..." id="callto-id" />
		                                    <a href="#" id="make-call">Call</a>
	                                    </p>
                                    </div>
                               <div id="step3">
	                                    <p>Currently in call with <span id="their-id">...</span></p>
	                                    <p><a href="#" id="end-call">End call</a></p>
                               </div>
                           <div id="step1">
		                                <p>Please click 'allow' on the top of the screen so we can access your webcam and microphone for calls</p>
		                                <div id="step1-error">
			                                <p>Failed to access the webcam and microphone. Make sure to run this demo on an http server and click allow when asked for permission by the browser.</p>
			                                <a href="#" id="step1-retry" class="button">Try again</a>
		                                </div>
                           </div>
                           </td>
                       </tr>

                   </table>



                </div>
                <div className="modal-footer">
                  <button type="button" class="btn btn-default" data-dismiss="modal">End Chat</button>
                </div>
              </div>
            </div>
          </div>
            );
                             },
                                 getInitialState: function () {

                                 return (
                                     {
                                         callerKey: '',
                                         diallerKey: ''
                                     }
                                     );
                             },
                                 //componenentWillMount: function () {
                                 // //   this.initializeApp();
                                 //    //this.state.ChatHub.client.newMessage = this.pushNewMeesage;
                                 //    //$.connection.hub.url = 'http://localhost/OmniChannel.VideoChatConnector.APIs/signalr;'
                                 //    //$.connection.hub.logging = true;
                                 //    //$.connection.hub.start().done(function () {
                                 //    //    console.log("Connected, transport = " + $.connection.hub.transport.name);
                                 //    //}).fail(function (e) {
                                 //    //    console.log('Connection Error ' + e);
                                 //    //});

                                 ////},
                                 //pushNewMeesage: function (message) {
                                 //    this.setState({ callerKey: message });

                                 //    if (this.state.diallerKey != this.state.callerKey) {
                                 //        var call = peer.call($('#callto-id').val(), window.localStream);
                                 //        step3(call);
                                 //    }


                                 //    //var call = peer.call($('#callto-id').val(), window.localStream);
                                 //    //step3(call);


                                 //},
                                 initializeApp: function () {

                             },
                                 componentDidMount: function () {
                                 //  var video = document.getElementById('my-video');
                                 var peer = new Peer({
                                     key: '2p9ffp7ol6p3nmi',
                                     debug: 3,
                                     config: {
                                         'iceServers': [
                                         { url: 'stun:stun.l.google.com:19302' },
                                         { url: 'stun:stun1.l.google.com:19302' },
                                         { url: 'turn:numb.viagenie.ca', username: "anuj.sethi@cgi.com", credential: "cgi@2017" }
                                         ]
                                     }
                                 });

                                 var videoChatHub = $.connection.videoChatHub;

                                 $.connection.hub.logging = true;

                                 var self = this;
                                 videoChatHub.client.newMessage = function (message) {
                                     self.setState({ callerKey: message });



                                     //your code to be executed after 1 second
                                     console.log(message + " push message received ");
                                     if (self.state.diallerKey != self.state.callerKey) {

                                         self.refs.chatAudio.play();
                                         var delay=5000; //1 second
                                         setTimeout(function() {
                                             k
                                             var call = peer.call(message, window.localStream);
                                             step3(call);
                                             self.refs.chatAudio.pause();
                                         }   , delay);
                                     }



                                 }
                                 $.connection.hub.start().done(function () {
                                     console.log("Connected, transport = " + $.connection.hub.transport.name);
                                     peer.on('open', function () {
                                         $('#my-id').text(peer.id);
                                         self.setState({ diallerKey: peer.id });
                                         sendMessage(peer.id);
                                     });
                                 }).fail(function (e) {
                                     console.log('Connection Error ' + e);
                                 });

                                 videoChatHub.client.checkForAgents = function (message) {
                                     if(message==true)
                                     {
                                         $('#divImg img').css({ 'display': 'inline-block' });
                                         $('#divTxt h4').css({ 'display': 'none' });
                                     }
                                     else
                                     {
                                         $('#divTxt h4').css({ 'display': 'inline-block' });
                                         $('#divImg img').css({ 'display': 'none' });
                                     }
                                 }


                                 if (videoChatHub) {
                                     console.log("SignalR hub initialized.");
                                 }

                                 function sendMessage(localPeerId) {
                                     if (videoChatHub.server) {
                                         videoChatHub.server.send(localPeerId);
                                         console.log(localPeerId + " diallerkey shared");
                                     }
                                 }

                                 peer.on('call', function (call) {
                                     // Answer automatically for demo
                                     call.answer(window.localStream);
                                     step3(call);
                                 });
                                 // On open, set the peer id
                                 //peer.on('open', function () {
                                 //    $('#my-id').text(peer.id);
                                 //    self.setState({ diallerKey: peer.id });
                                 //    sendMessage(peer.id);
                                 //});
                                 $(function () {
                                     $('#make-call').click(function () {
                                         //Initiate a call!
                                         var call = peer.call($('#callto-id').val(), window.localStream);
                                         step3(call);

                                     });
                                     $('#end-call').click(function () {
                                         if (window.existingCall) {
                                             window.existingCall.close();
                                         }
                                         step2();
                                     });

                                     // Retry if getUserMedia fails
                                     $('#step1-retry').click(function () {
                                         $('#step1-error').hide();
                                         step1();
                                     });

                                     // Get things started
                                     step1();
                                 });
                                 function step1() {
                                     if (navigator.mediaDevices && navigator.mediaDevices.getUserMedia) {
                                         navigator.mediaDevices.getUserMedia({ audio: true, video: true }).then(function (stream) {
                                             self.refs.callerVideo.src = window.URL.createObjectURL(stream);
                                             self.refs.callerVideo.play();
                                             window.localStream = stream;
                                             step2();
                                         });
                                     }
                                     //navigator.mediaDevices.getUserMedia({
                                     //    audio: true,
                                     //    video: true
                                     //})
                                     //.then(function (stream) {
                                     //    // Display the video stream in the video object
                                     //    $('#my-video').prop('src', URL.createObjectURL(stream));
                                     //    window.localStream = stream;
                                     //    step2();
                                     //}
                                     //)
                                     //.catch(function (e) {
                                     //    $('#step1-error').show();
                                     //    console.log(e);
                                     //});
                                 }
                                 function step2() { //Adjust the UI
                                     $('#step1', '#step3').hide();
                                     $('#step2').show();
                                 }
                                 function step3(call) {
                                     // Hang up on an existing call if present
                                     if (window.existingCall) {
                                         window.existingCall.close();
                                     }

                                     // Wait for stream on the call, then setup peer video
                                     call.on('stream', function (stream) {

                                         self.refs.receiverVideo.src = URL.createObjectURL(stream);

                                         //   $('#their-video').prop('src', URL.createObjectURL(stream));
                                         //   $('#their-video').play();
                                     });
                                     window.existingCall = call;
                                     call.on('close', step2);
                                     //  $('#step1', '#step2').hide();
                                     $('#step3').show();
                                 }
                             }//end of component did mount

                             });
        //ReactDOM.render(
        //   <VideoChatModal />,
        //    document.getElementById('chatDiv')
        //);
        var VideoChatModalId = 'sample-modal-container';
        var VideoChatComponent = React.createClass({
            handleShowVideoChatModal: function () {
                var modal = React.cloneElement(<VideoChatModal></VideoChatModal>);
                var modalContainer = document.createElement('div');
                modalContainer.id = VideoChatModalId;
                document.body.appendChild(modalContainer);
                ReactDOM.render(modal, modalContainer, function () {
                    var modalObj = $('#' + VideoChatModalId + '>.modal');
                    modalObj.modal('show');
                    modalObj.on('hidden.bs.modal', this.handleHideVideoChatModal);
                }.bind(this));
            },
            handleHideVideoChatModal: function () {
                $('#' + VideoChatModalId).remove();
            },
            render: function () {
                return (
                          <div>
                              <img src="images/vc2.png" class="btn " href="javascript:;" onClick={this.handleShowVideoChatModal} data-toggle="modal" data-target="#myModal" />
                          </div>
    )
    }
});

ReactDOM.render(
   <VideoChatComponent />,
    document.getElementById('chatDiv')
);