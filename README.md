

# SplashSharp
[**Splash**](https://scrapinghub.com/splash) is a javascript rendering service that can be ran in docker. It's a lightweight browser with an HTTP API, written by Scrapinghub.

**SplashSharp** is a C# client for talking to Splash instances.

## Getting Started

    // Create a new SplashClient for a splash instance running on localhost
	var splash = new SplashClient("http://localhost:8050", new HttpClient());
	
	// Get an HtmlAgilityPack HtmlDocument for a given Url
	var splashResponse = await splash.RenderHtmlDocumentAsync(new RenderHtmlOptions 
	{ 
		Url = "http://www.bbc.co.uk/news" 
	});
	var htmlDoc = splashResponse.Data;
	
	// Display text of all h2 elements found on the page
	var titles = htmlDoc.DocumentNode
		.SelectNodes("//h2").Select(n => n.InnerText);
