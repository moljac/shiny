## Shiny Samples

These samples make use of:
* Prism
* ReactiveUI & RX Principles
* Heavy use of dependency injection

If you are looking to just use one part of Shiny, such as notifications, this probably isn't for you.  Shiny brings:
* Handles all of the cruft like Permissions, main thread traversal, and app restarts
* Your infrastructure to the background
* Gives a clean & testable API surface for your code

## Compiling on iOS
NFC & Push Notifications are enabled in the info.plist which means you need a custom provisioning profile (or you have to disable these before deploying to your device)

## To Run:

Android
* Copy your own google-services.json and ensure its build action is set to GoogleServicesJson

iOS 
* For firebase testing, copy in your own GoogleService-Info.plist
* Ensure your provisioning profile is assigned push notification permissions

## Secrets.json
Create a secrets.json in the solution root with the following values:

* AzureNotificationHubFullConnectionString - full endpoint that allows sending messages
* AzureNotificationHubListenerConnectionString - a listen only connection string
* AzureNotificationHubName - your test hub name
* GoogleCredentialProjectId - Firebase project ID
* GoogleCredentialPrivateKeyId - for testing firebase (leave blank otherwise)
* GoogleCredentialPrivateKey - for testing firebase (leave blank otherwise)
* GoogleCredentialClientId - for testing firebase (leave blank otherwise)
* GoogleCredentialClientEmail - for testing firebase (leave blank otherwise)
* GoogleCredentialClientCertUrl - for testing firebase (leave blank otherwise)
* AppCenterKey - Your appcenter key for android/ios (leave blank if not using)