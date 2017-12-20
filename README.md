# Xamarin and Azure Function Sample: Coin Value

This sample shows two Azure Functions:

- A **Timer Triggered** function used to save bitcoin value every hour to an Azure Table. It also sends a notification with Azure AppCenter to all registered devices (Android, iOS and Windows 10) with the value of the Bitcoin and Ethereum coins.
- An **HTTP Triggered** function used to get the current bitcoin and ethereum value as well as the current trend (up, flat, down) calculated over the last 10 samples.

In addition you will find a Xamarin application for Android, iOS and for Windows Universal (UWP) connecting to the HTTP Triggered function and displaying the result. The client application also receives the AppCenter push notification and displays it in a local notification.

## Available content:

You can find the following content related to this sample:

* General content:
    * [Creating a trial account and a storage account in Azure](https://github.com/lbugnion/sample-azure-general/blob/master/trial-account.md)
    * [Installing and configuring the Microsoft Azure Storage Explorer](https://github.com/lbugnion/sample-azure-general/blob/master/azure-explorer.md)
    * [Opening, building and running the sample](publishing.md)

## Work in progress:

* [Screenshots](screenshots.md)

* Server-side:
    * [Building the Function application](building-function.md)
    * [Building the Saver function (Timer Triggered)](building-function-saver.md)
    * [Building the Getter function (HTTP Triggered)](building-function-getter.md)

* Client-side:
    * [Building the client data access layer](building-data.md)
    * [Building the Xamarin.Android client application](building-android.md)
    * [Building the Xamarin.iOS client application](building-ios.md)
    * [Building the Windows Universal application](building-uwp.md)