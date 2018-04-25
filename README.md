
# SplashSharp
[**Splash**](https://scrapinghub.com/splash) is a javascript rendering service. It's a lightweight browser with an HTTP API, written by Scrapinghub.
**SplashSharp** is a C# client for talking to Splash instances.

## Getting Started
    // Create a new SplashClient for a splash instance running on localhost
	var splash = new SplashClient("http://localhost:8050", new HttpClient());
	
	// Get an HtmlAgilityPack HtmlDocument for a given Url
	var htmlDoc = await splash.GetHtmlDocumentAsync(new RenderHtmlOptions { Url = "https://www.bbc.co.uk" });
	
	// Display text of all h2 elements found on the page
	var titles = htmlDoc.DocumentNode.SelectNodes("//h2").Select(n => n.InnerText);
