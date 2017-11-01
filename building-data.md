# Building the client data access layer
 
The Azure Function application is, in essence, an API that can be consumed by various clients through HTTP. While the Timer triggered function is running independently from any client call, on a schedule, the HTTP triggered one can be called with an HTTP GET. We will now build a library to get these values and we will share the library between our Xamarin clients.

## Building the shared project

In this sample, I am using the [MVVM Light Toolkit](http://mvvmlight.net), a lightweight toolkit used to implement cross-platform applications following the Model-View-ViewModel pattern. For this very simple client, this may seem unnecessary but for more complex applications, using MVVM is a good practice that makes maintaining and extending the application much easier. Of course you can use any MVVM framework you prefer or even roll your own.

> Note: We will use a portable class library (PCL) in this project in order to maximize compatibility with older versions of the Universal Windows Platform (UWP). If however you are targeting the newest version of UWP, you can also use a .NET Standard 2.0 class library instead. The process to build the library is the same.

1. Select File, New, Project in the LbCoinValue solution.
2. In the Add New Project dialog, enter "portable" in the Search box.
3. Select a Class LIbrary (Legacy Portable) C# from the dialog.
4. Enter the name CoinValue.Data and press OK.

## Abstracting the service

It's always good practice to abstract the data service so it can be replaced by another implementation, for example for test purposes. Follow the steps:

5. Right click on the CoinValue.Data project in the Solution Explorer.
6. Select Add, New Folder from the context menu. Name this folder Model.
7. Right click on the Model folder and select Add, New Item from the context menu.
8. Select Interface and name it ICoinService.cs.
9. Define an asymchronous method called GetTrend as follows:

```CS
public interface ICoinService
{
    Task<CoinTrend> GetTrend();
}
```

## Implementing the service

Now that we have an interface, we need to implement the runtime version of the service. As you will see, this is very easy:

10. Create a new class called CoinValueService with the following code:

```CS
public class CoinService : ICoinService
{
    private const string Url = "[URL_HERE]";

    public async Task<CoinTrend> GetTrend()
    {
        var client = new HttpClient();
        var json = await client.GetStringAsync(Url);

        var trend = JsonConvert.DeserializeObject<CoinTrend>(json);
        return trend;
    }
}
```

As you can see, the service is very simple and essentially just 