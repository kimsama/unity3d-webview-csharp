unity3d-webview-csharp
======================

### Overview

C# port of unity3d-webviw-integration sample on [unity-webview-integration](https://github.com/keijiro/unity-webview-integration).

### Howto

#### Unity Side

Also see the original sample of keijiro's if you want to use Unity javascript version as the following:

See [Assets/Plugins/WebMediator.cs](https://github.com/kimsama/unity3d-webview-csharp/blob/master/unity/Assets/Plugins/WebMediator.cs) which does all the magic. It loads specified URL and can be specified page margin and so on. It also does pick message which is sent from webview. 
You can see for more details on [TestInterface.cs](https://github.com/kimsama/unity3d-webview-csharp/blob/master/unity/Assets/Scripts/TestInterface.cs)

#### Webview side

See the [index.html](https://github.com/keijiro/unity-webview-integration/blob/gh-pages/index.html) for the details.

### Test

Tested on the following devices:

#### iPhone

* iPhone4s on iOS 6.1 (built with Xcode 4.6 on OSX 10.8.2)

#### Android

* Samsung Galuxy S2
* Samsung Galuxy Note II
(all built on OSX 10.8.2)

### License

See the included LICENSE.txt. (same as  [unity-webview-integration](https://github.com/keijiro/unity-webview-integration))



