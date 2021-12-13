mergeInto(LibraryManager.library, {

  Connect_JS: function () {
    const client = new WebSocket("ws://localhost:80");
  
    client.addEventListener("open", function() {
       console.log("client opened");
    });
  
    client.addEventListener("close", function() {
      console.log("client closed");
    });
  
    client.addEventListener("message", function(data) {
    console.log('%c received' + data, 'color: #bada55');
    });
    
    return client;
  },

});