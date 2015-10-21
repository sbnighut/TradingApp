# TradingApp
This is a C# .net application that mimics the characteristics of a trading application. There is price update service that raises price update event 20 times in a second. This gives a real time feel to the application. Each and every time a new price about an instrument is updated the GUI shows the same using coloring scheme. If the new price of the stock is higher than previous then Blue color will blink in the background of that cell or else Red. If the price is not changed then a neutral color i.e. LightGray would be set to background.
In order to run the application set the WPFTradeApp project as the Startup project by right clicking on it(inside Visual Studio 2013/15). Once this step is done then run your application.

Note: The price of the instruments are dummy values which is calculated randomly using the Random function of Math library
