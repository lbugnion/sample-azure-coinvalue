# Creating the Coin Trend Getter (HTTP Triggered function)

Now that the values are saved in Azure Tables, we can build an API endpoint to get the value and the trend (up, down, flat) of Bitcoin. [You can see the full code here](TODO_GITHUB_CLASS).

1. In the Solution Explorer, right click on the LbCoinValue project and select Add, New Item.
2. In the Add New Item dialog, select Azure Function.
3. Enter the name "CoinTrendGetter.cs" and press OK
4. In the New Azure Function dialog, select Http Trigger and press OK.

