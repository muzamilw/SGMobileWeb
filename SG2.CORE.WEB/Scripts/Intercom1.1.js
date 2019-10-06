//Insert Basic JS code here
//Set your APP_ID
//var APP_ID = "xby7ad23";
var APP_ID = glb_intercomKey;

// Set Show Event to be false initiall
var SHOW_EVENT = false;

window.intercomSettings = {
    app_id: APP_ID
  };
(function(){var w=window;var ic=w.Intercom;if(typeof ic==="function")
{ic('reattach_activator');ic('update',intercomSettings);}else
{var d=document;var i=function(){i.c(arguments)};i.q=[];i.c=function(args)
{i.q.push(args)};w.Intercom=i;function l()
{var s=d.createElement('script');s.type='text/javascript';s.async=true;
s.src='https://widget.intercom.io/widget/' + APP_ID;
var x=d.getElementsByTagName('script')
[0];x.parentNode.insertBefore(s,x);}if(w.attachEvent)
{w.attachEvent('onload',l);}else{w.addEventListener('load',l,false);}}})()


function messenger(display) {
  //Insert sign out code here
  Intercom(display)
  return(true) //check to see if code was correctly copied
}


// The code below here is only related to the demo
// i.e. it has nothing to do with installing Intercom

// Code to show snippet code on toggle
function toggle(element) {
    document.getElementById(element).style.display = (document.getElementById(element).style.display == "none") ? "" : "none";
}

//Code to check that launcher is loading
var retries = 0;
var launcherExist = setInterval(function() {
  if ($('.intercom-launcher-frame').length) {
    $('.checkmark').css({'display':'inline-block'});
    $('.still-no-messenger').hide();
    clearInterval(launcherExist);
  }
  retries += 1;
  if (retries >= 120) {
    clearInterval(launcherExist);
  }
}, 500);
  
$('#myCheck').click(function() {
  if ($('#display').is(":visbile")) {
    $('#display').hide();
  } else {
    $('#display').show();
  }
});