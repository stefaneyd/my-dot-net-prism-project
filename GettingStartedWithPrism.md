# 1. Getting started with Prism #

Exercise in Prism pattern library
How to architect your projects so they will evolve and stand the test of time and don't break the second you change something. That is what Prism can give you.

  * What is Prism?
  * Benefits
  * Get Prism
  * How it works


---

## WHAT IS PRISM? ##
Framework for developing [composite applications](http://en.wikipedia.org/wiki/Composite_application).
Take large applications and break it down to smaller more manageable pieces.
It is specific to WPF, Silverlight and Windows Phone 7.
Relies on design patterns to help promote [loose coupling](http://en.wikipedia.org/wiki/Loose_coupling) and [separation  of concern](http://en.wikipedia.org/wiki/Separation_of_concerns).<br />
some patterns are:
  * [dependency injection pattern](http://en.wikipedia.org/wiki/Dependency_injection)
  * [Inversion of control pattern](http://en.wikipedia.org/wiki/Inversion_of_control)
  * [Command pattern](http://en.wikipedia.org/wiki/Command_pattern)
  * [Model View ViewModel MVVM pattern](http://en.wikipedia.org/wiki/Model_View_ViewModel)
just to name a few.


---

## BENEFITS ##
  * **[REUSE](http://en.wikipedia.org/wiki/Code_reuse)** - It was designed around architectural design patterns such as [separation of concerns](http://en.wikipedia.org/wiki/Separation_of_concerns) and [loose coupling](http://en.wikipedia.org/wiki/Loose_coupling) this allows Prism to provide you with many benefits. Most common benefit when using a framework is reuse, where you can build a component once and use it multiple times in a single application or cross applications and with Prism you take it a step further where you can build a component in WPF and use it in Silverlight so it is cross platforms.
  * **[EXTENSIBLE](http://en.wikipedia.org/wiki/Extensible_programming)** - allows you to add new capabilityes and more easely intergrate them to your system
  * **FLEXIBLE** - can have components replaced with alternative implementations at runtime
  * **[TEAM DEVELOPMENT](http://en.wikipedia.org/wiki/Team_programming)** - with larger projects broken down too smaller components it allows for multiple group members to work in the project at the same time
  * **[QUALITY](http://en.wikipedia.org/wiki/Software_quality)** - Increases the quality of the code, because the application is broken down into smaller more manageable peaces, it allows common services and components to be fully tested.


---

## GET PRISM ##
You can get the Prism library from Microsoft patterns & practices in the following link

http://compositewpf.codeplex.com/

What you need:
  * Microsoft Windows 7, Windows Vista or Windows Server 2008
  * Microsoft .NET Framework 4.0 (installed with Visual Studio 2010)
  * Microsoft Visual Studio 2010 Express, Professional, Premium or Ultimate editions
  * Microsoft Silverlight 4 Tools for Visual Studio 2010
  * Optional but recommended
    * Microsoft Expression Blend 4
    * Windows Phone Developer Tools SDK


---

## DEMO INSTALLING PRISM ##
just go through the wizard and unpack to a preferred location
the binary's are located in the Bin folder and they are separated into they're associated platform Desktop, Phone, Silverlight, and you can reference them from there in your project.
You can also run the RegisterPrismBinaries.bin file in the root directory to register the binary's in Visual Studio so you don't have to browse for the bins. There are also great documentation that follows the install. There are also great examples and quickstarts that come with prism. Prism also ships with its source, the source code to prism library in the PrismLibrary folder.


---

## BUILDING BLOCKS ##
  * **Shell** - a template to define the structure of the UI, it contains Regions.
  * **Regions** - Specify certain areas of the Shell as elements in which you are going to inject a View at runtime.
  * **Modules** - A single major functional area at you application, ideally this module would not depend on other module to function properly. Modules will contain a number of Views.
  * **Views** - Just a simple UI that the user interacts with. In Prism they are constructed using the MVVM design pattern.
  * **Bootstrapper** - A class that initializes the Prism Application. Create and configure Module catalogs your container any region adapter mappings and where you create and initialize the Shell.


---

## APPLICATION DESIGN ##

<img src='http://s17.postimg.org/brbnpypjz/app.png' width='400px'></img>
<img src='http://s23.postimg.org/b910ma8ez/prism.png' width='400px' />


---

## DEMO PRISM WALKTHROUGH ##

**Shell.xaml**
```
<Window x:Class="HelloWorld.Shell"
	xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
	xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
	xmlns:cal="http://www.codeplex.com/prism"
	Title="Hello World" Height="300" Width="300">
	<ItemsControl Name="MainRegion" cal:RegionManager.RegionName="MainRegion" />
</Window>
```

**HelloWorldModule.cs**
```
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;

namespace HelloWorldModule
{
  public class HelloWorldModule : IModule
  {
    private readonly IRegionViewRegistry regionViewRegistry;
    
    public HelloWorldModule(IRegionViewRegistry registry)
    {
      this.regionViewRegistry = registry;
    }

    public Initialize()
    {
      regionViewRegistry.RegisterViewWithRegion("MainRegion", typeof(Views.HelloWorldView)); // #3 Register the HelloWorldView with the MainRegion, and that is the mechanism that injects the View to the MainRegion in the Shell.
    }
  }
}
```

**HelloWorldView.xaml**
```
<UserControl x:Class="HelloWorldModule.Views.helloWorldView"
	xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
	xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml">
	<Grid>
	  <TextBlock Text="Hello World" Foreground="Green" HorizontalAlignment="Center" VerticalAlignment="Center" FontFamily="Calibri" FontSize="24" FontWeight="Bold"></TextBlock>
	</Grid>
</UserControl>
```

**Bootstrapper.cs**
```
using System.Windows;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Prism.UnityExtensins;

namespace HelloWorld
{
  class Bootstrapper : UnityBootstrapper
  {
    protected override DependencyObject CreateShell()
    {
      return this.Container.Resolve<Shell>();
    }

    protected override void InitializeShell()
    {
      base.InitializeShell();
      App.Current.MainWindow = (Window)this.Shell;
      App.Current.MainWindow.Show();
    }

    protected override void ConfigureModuleCatalog()
    {
      base.ConfigureModuleCatalog();

      ModuleCatalog moduleCatalog = (ModuleCatalog)this.ModuleCatalog;
      moduleCatalog.addModule(typeof(HelloWorldModule.HelloWorldModule)); // #2 This is where we add our HelloWorld module to the module catalog
    }
  }
}
```

**App.xaml.cs**

```
using System.Windows;

namespace HelloWorld
{
  public partial class App : Application
  {
    protected override void OnStartup(StartupEventArgs e)
    {
      base.OnStartup(e);
      Bootstrapper bootstrapper = new Bootstrapper();
      bootstrapper.Run();  // #1 this is the first thing that happens in the Application
    }
  }
}
```