# Creating the Coin Value Saver (Timer Triggered function)

We will now implement the CoinValueSaver function. [The full code can be found here](TODO_GITHUB_CLASS).

1. In the Solution Explorer, right click on the LbCoinValue project and select Add, New Item.
2. In the Add New Item dialog, select Azure Function.
3. Enter the name "CoinValueSaver.cs" and press OK
4. In the New Azure Function dialog, select Timer Trigger.
5. Select the Schedule parameter and enter the following expression:

```
0 0 */1 * * *
```

> Note: This CRON expression means "Every time that the seconds and minutes are 0, every 1 hour, on any day, any month, any year, execute the function. [More details on CRON expressions can be found here](TODO).

6. Press OK to create the function.

## Adding the constants and parametering them

7. Add the following constants on top of the class:

```CS
private const string Symbol = "btc";
private const string Url = "https://api.coinmarketcap.com/v1/ticker/";
public const string ConnectionString = "[CONNECTION STRING HERE]";
public const string TableName = "coins";
```

* The ```Symbol``` ```"btc"``` is for Bitcoin. Of course the function could easily be modified to save other currencies such as Ethereum etc.

* The ```Url``` points to an external service returning the value of various cryptocurrencies such as Bitcoin.

* The ```ConnectionString``` will be handled a little below.

* The ```TableName``` constant is the name of the Azure Table in which we will save the value of Bitcoin.

### Getting the connection string

At this point, we need to create a storage account in Azure to store the values, and get its Connection String so that we can continue with the implementation. The following links will help you with these steps:

* [Creating a trial account and a storage account in Azure](TODO)
* [Installing and configuring the Microsoft Azure Storage Explorer](TODO)

8. Open the Microsoft Azure Storage Explorer and locate the subscription in which you created the storage account.

9. Expand the subscription, expand Storage Accounts, then select the storage account that you created for this function.

10. In the Properties at the bottom of the pane, find the Primary Connection String and copy this value.

![Finding the Primary Connection String](TODO)

11. Paste the connection string that you just copied in the code above, replacing the `[CONNECTION STRING HERE]` string.

## Creating the Account, the Client and the Table

12. In the CoinValueSaver class, just below the call to ```log.Info```, enter the following lines of code:

```CS
// Create account, client and table
var account = CloudStorageAccount.Parse(ConnectionString);
var tableClient = account.CreateCloudTableClient();
var table = tableClient.GetTableReference(TableName);
await table.CreateIfNotExistsAsync();
```

First, we use the connection string that we just copied and stored as a constant to create an instance of a [CloudStorageAccount](TODO). Then we create a [CloudTable](TODO) by using the account's [CreateCloudTableClient method](TODO). Note that at this point we could also create a Blob client, a Queue client, etc.

The next line gets a reference to the CloudTable corresponding to the ```TableName``` constant. As a reminder, the table name that we will use is ```"coins"```. The last line above will create the table in Azure in case this table didn't already exist. This is very convenient and allows the function to run without errors even if changes are made in the storage account, for example using Azure Storage Explorer.

At this point, you will get an error if you attempt to build the application. This is because the ```CreateIfNotExistsAsync``` method is called asynchronously with the ```await``` keyword. You must therefore add the ```async``` keyword to the ```Run``` method signature, and make it return a ```Task``` as follows:

```CS
[FunctionName("CoinValueSaver")]
public static async Task Run(
    [TimerTrigger("0 0 */1 * * *")]
    TimerInfo myTimer, 
    TraceWriter log)
```

## Getting the coin value from the JSON API

13. We use a third party service to get the value of Bitcoin. I am not going into the details of the JSON parsing here, which is irrelevant to the Azure Function itself. I am sure that you can figure the code below out! Here we use the well-known Nuegt package [Json.NET](TODO).

```CS
// Get coin value (JSON)
var client = new HttpClient();
var json = await client.GetStringAsync(Url);

var price = 0.0;

try
{
    var array = JArray.Parse(json);

    var priceString = array.Children<JObject>()
        .FirstOrDefault(c => c.Property("symbol")
            .Value
            .ToString()
            .ToLower() == Symbol)?
        .Property("price_usd").Value.ToString();

    if (priceString != null)
    {
        double.TryParse(priceString, out price);
    }
}
catch
{
    // Do nothing here for demo purposes
}

if (price < 0.1)
{
    log.Info("Something went wrong");
    return; // Do some logging here
}
```

> Note: We are using LINQ to parse the JSON array, by using the ```FirstOrDefault``` method. In order to use this method, you must make sure to add the following namespace directive on top of the class:

```CS
using System.Linq;
```

## Creating the TableEntity

At this point, if everything went well with the JSON service, we should have the current value of Bitcoin in US dollars. Now we can create the table entry itself in Azure.

Azure Tables are what we call "schema-free", which means that you can store any object of (almost) any type, and the values will be stored in the corresponding columns automatically. The only constraint, really, is that the instance that is stored must implement the [ITableEntity interface](TODO). There is already a base class available implementing this interface, called [TableEntity](TODO). This is what we will use here.

> Note: This flexibility is very powerful but can also cause some unwanted side effects if you don't take precautions. For instance, see [the following blog article](TODO_GALASOFT_BLOG).

14. Create a new class called ```CoinEntity``` with the following implementation:

```CS
public class CoinEntity : TableEntity
{
    public double PriceUsd { get; set; }

    public string Symbol { get; set; }

    public DateTime TimeOfReading { get; set; }
}
```
As you can see, there is nothing special about the columns that we are using to store the information. Note that the columns will be created dynamically if needed when we insert the entity in the table. 

15. In the ```CoinValueSaver.Run``` method, at the bottom, add the following code:

```CS
var coin = new CoinEntity
{
    Symbol = Symbol,
    TimeOfReading = DateTime.Now,
    RowKey = "row" + DateTime.Now.Ticks,
    PartitionKey = "partition",
    PriceUsd = price
};

// Insert new value in table
table.Execute(TableOperation.Insert(coin));
```

The call to [TableOperation.Insert](TODO) will force the creation of a new row every time that it is called.

> Note: In cases where you need to modify an existing entity, you would rather use the [TableOperation.InsertOrMerge method](TODO). This will either edit an existing entity (identified by the unique Partition/RowKey combination) or create a new entity if no existing entity is found.

## Testing the Function

The implementation is complete and we can test the function now. There are more details about testing Timer functions locally in the following articles:

* [Creating and testing Azure Functions in Visual Studio](TODO)
* [Creating and testing a Timer triggered function](TODO)

16. To make it easier to debug, modify the CRON expression in the ```TimerTrigger``` attribute on top of the function code. We can set it up to run the function every 5 seconds with the following expression:

```
*/5 * * * * *
```

17. Please a breakpoint at the beginning of the ```CoinValueSaver.Run``` method.

18. Select Debug, Start Debugging (F5). After a few seconds, the breakpoint should get hit and you can step through the code.

19. After you reach the place where ```TableOperation.Insert``` is called, you can use the Microsoft Azure Storage Explorer to check if the table and the new row have been created correctly.


