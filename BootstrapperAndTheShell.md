# 2. Bootstrapper and the Shell #

## WHAT IS THE BOOTSTRAPPER? ##

  * Initializes application - The bootstrapper is a class that is responsible for initializing the application.
  * Core Services - the first thing that is initialized. (non application specific services that are from prism the library)
    * IModuleManager - Initialize and retrieve the applications modules.
    * IModuleCatalog - Registers the modules the application is going to load.
    * IModuleInitializer - Initializes the modules.
    * IRegionManager - The visual containers for the UI.
    * IEventAggregator - Create events that are loosely coupled between the publisher and the subscriber.
    * ILoggerFacade - Rapper for logging mechanism, to implement your own logging mechanism.
    * IServiceLocator - Gives access to the container.
  * Application specific services - Services that are specific to the application and common amongst all application modules.


---

## BOOTSTRAPPER PROCESS ##

<img src='http://s12.postimg.org/f5m45v5ml/bootstrapper_process.png' />


---

## Demo Organize your Solution ##
  * Create new blank solution. Name "PrismDemo"
  * Open the project directory and create two folders
    * Libs -  Holds all the libraries that are going to be used in the application
      * Create a folder Prism v4 for the Prism libraries and copy the Prism files that you need into that folder.
        * Microsoft.Practices.Prism.dll
        * Microsoft.Practices.Prism.Interactivity.dll
        * Microsoft.Practices.Prism.MefExtensions.dll
        * Microsoft.Practices.Prism.UnityExtensions.dll
        * Microsoft.Practices.ServiceLocation.dll
        * Microsoft.Practices.Unity.dll
    * Src
      * Create Business folder - holds project that deal with the domain
      * Create Modules folder that contains all the Prism modules
  * Add a new WPF Application project to the solution  and put it in the Src folder. Name "Demo".
  * Add a new WPF User Control Library to the solution and put it in the Src folder to. This is referred to as the Infrastructure project and that is the code that is shared and common across the application and modules. Name "Demo.Infrastructure". Delete the user control. We use the WPF User Control Library because when we add to the project we see the object that we use in WPF projects instead of Windows.Forms objects that come with the class library.
  * Add two solution folders to match the folder solution, helps to keep the solution organized when there are many projects in the solution.
    * Create Modules folder
    * Create Business folder


---

## Demo Create a Unity Bootstrapper ##

Add a reference to to the Prism library bins;
  * Microsoft.Practices.Prism.dll
  * Microsoft.Practices.Prism.UnityExtensions.dll
  * Microsoft.Practices.Unity.dll
Next add a class to the Demo project, this is going to be the bootstrapper class.
  * make it public
  * add a using Microsoft.Practices.Prism.UnityExtensions
  * let it inherit the UnityBootstrapper
  * implement the abstract members
  * delete the MainWindow from the Demo project
  * in App.Xaml remove the ` StartupUri="MainWindow.xaml" ` line
  * in App.xaml.cs we  override the OnStartup method
```
protected override void Onstartup(StartupEventArgs e)
{
	base.OnStartup(e);
	Bootstrapper bootstrapper = new Bootstrapper();
	bootstrapper.Run();
}
```


---

## What is the Shell? ##
  * Main window/Page - Where the primary user interface content is contained
  * "Master Page" - Similar to Master Page in ASP.NET, used as a template to define the overall appearance for the application.
  * Contains Regions - Shell contains regions in which views will be injected to at runtime


---

## Demo Create a Unity shell ##

  * add Window (WPF) to the Demo project name it Shell
  * add ` <TextBlock Text="Hello World" /> ` to the Shell
  * remove the height and width settings from the Shell
  * add ` using Microsoft.Practises.Unity ` statement to the Bootstrapper
  * replace the code in the CreateShell method ` return Container.Resolve<Shell>(); `
    * ` Resolve<T> ` comes from the Microsoft.Practises.Unity
  * override the InitializeShell method in the Bootstrapper
```
protected override void InitializeShell()
{
	base.InitializeShell();
	App.Current.MainWindow = (Window)Shell; // needs using System.Windows
	// the reason we have to cast to Window is because CreateShell returns DependecyObject and the MainWindow takes the type of Window

	App.Current.MainWindow.Show();
}
```