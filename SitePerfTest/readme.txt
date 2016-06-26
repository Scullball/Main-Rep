Simple code. Main application logic written on javaScript. Server side(c#) used
	only for storing to database and render page.
	Perfectly working on “firefox” with CORSE(cross-origin-requests-enabled),
	rendering beautifully 3d graphs in real-time, as needed.

	 Warning! required Firefox and https://addons.mozilla.org/ru/firefox/addon/cors-everywhere/

	


	All the basic logic of the program is in two files: 
	~/Views/Shared/_ScriptsPartial.cshtml
	~/Scripts/project-functions.js

	~/Scripts/project-graphics.js, project-tables.js - implementation Schedule

	Cross-domain requests are prohibited on the server Global.asax(Application_BeginRequest);
	If you have a browser will not block them, it will work. 
	Otherwise: Firefox + https://addons.mozilla.org/ru/firefox/addon/cors-everywhere/.
	
	
	Charts used: highcharts http://www.highcharts.com/demo/3d-column-stacking-grouping


									Best Regards
	
	