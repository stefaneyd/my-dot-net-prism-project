# Introduction to Prism #

This is a exercise in .NET Prism pattern library, where we follow the [pluralsight tutorial](http://pluralsight.com/training/courses/TableOfContents?courseName=prism-introduction&highlight=brian-lagunas_prism1-01-intro*1,3,4,7,0,2,8,5,6!brian-lagunas_prism1-02-bootstrapper-shell*0,9,1,2,6,3,4,5,7,8!brian-lagunas_prism1-03-regions*0,1,2,4,6,3,5!brian-lagunas_prism1-04-modules*0,1,4,10,11,2,3,5,6,7,8,9!brian-lagunas_prism1-05-views*0,1,5,6,8,11,2,7,9,3,10,4!brian-lagunas_prism1-6-communication*0,1,2,4,6,8,10,12,3,5,7,11,9!brian-lagunas_prism1-07-state-based-navigation*0,5,4,1,2,3!brian-lagunas_prism1-08-view-based-navigation*0,13,1,3,5,6,7,9,11,2,4,8,10,12#prism1-01-intro) with with Brian Lagunas


## 1. Getting started with Prism ##

### INTRODUCTION ###
Exercise in Prism pattern library
How to architect your projects so they will evolve and stand the test of time and don't break the second you change something. That is what Prism can give you.

  * What is Prism?
  * Benefits
  * Get Prism
  * How it works



&lt;hr /&gt;


### WHAT IS PRISM? ###
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



&lt;hr /&gt;


### BENEFITS ###
  * **[REUSE](http://en.wikipedia.org/wiki/Code_reuse)** - It was designed around architectural design patterns such as [separation of concerns](http://en.wikipedia.org/wiki/Separation_of_concerns) and [loose coupling](http://en.wikipedia.org/wiki/Loose_coupling) this allows Prism to provide you with many benefits. Most common benefit when using a framework is reuse, where you can build a component once and use it multiple times in a single application or cross applications and with Prism you take it a step further where you can build a component in WPF and use it in Silverlight so it is cross platforms.
  * **[EXTENSIBLE](http://en.wikipedia.org/wiki/Extensible_programming)** - allows you to add new capabilityes and more easely intergrate them to your system
  * **FLEXIBLE** - can have components replaced with alternative implementations at runtime
  * **[TEAM DEVELOPMENT](http://en.wikipedia.org/wiki/Team_programming)** - with larger projects broken down too smaller components it allows for multiple group members to work in the project at the same time
  * **[QUALITY](http://en.wikipedia.org/wiki/Software_quality)** - Increases the quality of the code, because the application is broken down into smaller more manageable peaces, it allows common services and components to be fully tested.



&lt;hr /&gt;


### GET PRISM ###
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



&lt;hr /&gt;


### DEMO INSTALLING PRISM ###
just go through the wizard and unpack to a preferred location
the binary's are located in the Bin folder and they are separated into they're associated platform Desktop, Phone, Silverlight, and you can reference them from there in your project.
You can also run the RegisterPrismBinaries.bin file in the root directory to register the binary's in Visual Studio so you don't have to browse for the bins. There are also great documentation that follows the install. There are also great examples and quickstarts that come with prism. Prism also ships with its source, the source code to prism library in the PrismLibrary folder.



&lt;hr /&gt;


### BUILDING BLOCKS ###
  * **Shell** - a template to define the structure of the UI, it contains Regions.
  * **Regions** - Specify certain areas of the Shell as elements in which you are going to inject a View at runtime.
  * **Modules** - A single major functional area at you application, ideally this module would not depend on other module to function properly. Modules will contain a number of Views.
  * **Views** - Just a simple UI that the user interacts with. In Prism they are constructed using the MVVM design pattern.
  * **Bootstrapper** - A class that initializes the Prism Application. Create and configure Module catalogs your container any region adapter mappings and where you create and initialize the Shell.



&lt;hr /&gt;


### APPLICATION DESIGN ###

<img src='http://s17.postimg.org/brbnpypjz/app.png' width='400px'></img>
<img src='http://s23.postimg.org/b910ma8ez/prism.png' width='400px' />



&lt;hr /&gt;


### DEMO PRISM WALKTROUGH ###

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

**Bootstrapeper.cs**
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



&lt;hr /&gt;


## 2. Bootstrapper and the Shell ##

### WHAT IS THE BOOTSTRAPPER? ###

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



&lt;hr /&gt;


### BOOTSTRAPPER PROCESS ###

<img src='http://s12.postimg.org/f5m45v5ml/bootstrapper_process.png' />



&lt;hr /&gt;


### Demo Organize your Solution ###
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



&lt;hr /&gt;


### Demo Create a Unity Bootstrapper ###

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



&lt;hr /&gt;


### What is the Shell? ###
  * Main window/Page - Where the primary user interface content is contained
  * "Master Page" - Similar to Master Page in ASP.NET, used as a template to define the overall appearance for the application.
  * Contains Regions - Shell contains regions in which views will be injected to at runtime



&lt;hr /&gt;


### Demo Create a Unity shell ###

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



&lt;hr /&gt;


## 3. Regions ##

### Introduction ###
These are the discussion in this section.
  * What are Regions? - What are they and what role do they play in our application
  * RegionManager - how it manages the regions in our Prism application
  * RegionAdapter -  the relationship between it and RegionManager
  * Create a Custom Region -  how to create a custom region, these are needed for regions that Prism does not support



&lt;hr /&gt;


### What are Regions? ###
  * "Placeholder" for dynamic content that is going to be presented in the UI or in the Shell.Named location that you can define where a View will appear.

<img src='http://s16.postimg.org/z7wwjn9j9/region.png' width='400' /><br>
These regions define areas of the UI where we're gonna inject a view. Each region should be specifically named that describes the type of content that we are going to inject into that region<br>
<br>
<ul><li>No knowledge of views - they have no knowledge of views<br>
<img src='http://s12.postimg.org/4r2jlczel/region2.png' width='400' />
<br>We can redesign our Shell and it will have no impact on the region injection, no need to modify the infrastructure or module code.</li></ul>

<ul><li>Create in code or in XAML -  region is not a control, its apply to a control via the region name attach property on the region manager.</li></ul>

<ul><li>implements IRegion - Region implements the IRegion interface</li></ul>

<br>
<br>
<hr /><br>
<br>
<br>
<h3>Region Manager</h3>
is responsible for managing the regions in the application.<br> It does it by:<br>
<ul><li>Maintains collection of regions - creates new regions for controls<br>
</li><li>Provides a RegionName attached property - used to create regions by applying the attach property to the host control, can be done through XAML or code.<br>
</li><li>Maps RegionAdapter to controls - responsible for associating region with the host control. in order to expose a UI control as a region it must have a region adapter, and each region adapter adapts a specific type of UI control.<br> <b>Prism Region Adapters</b>
<ul><li>ContentControlRegionAdapter<br>
</li><li>ItemsControlRegionAdapter<br>
</li><li>SelectorRegionAdaptor<br>
</li><li>TabControlRegionAdapter (Silverlight only)<br>
</li></ul></li><li>Provides a RegionContext attached property -  similar to dataContext in XAML, it allows you to propagate its data down the visual tree. Technik used to share data between a parent view and a child view that are hosted within a region.</li></ul>

<br>
<br>
<hr /><br>
<br>
<br>
<h3>Demo Creating Regions</h3>

Create a new project in the Modules folder Name it ModuleA, in that module there are two Views.<br>
<ul><li>ContentView.xaml<br>
<pre><code>&lt;UserControl x:Class="ModuleA.ContentView"<br>
             xmlns="http://schemas.microsoft.com/winfx/2006/presentation"<br>
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"<br>
             xmlns:mc="http//schemas.openxmlformats.org/markup-compatibility/2006"<br>
             xmlns:d="http://schemas.micosoft.com/expression/blend/2008"<br>
             mc:Ignorable="d"<br>
             d:DesignHeignt="300" d:DesingWidth="300"&gt;<br>
  &lt;Grid background="LightCoral"&gt;<br>
    &lt;TextBlock Text="Content" VerticalAlignment="Center" HorizontalAlignment="Center /&gt;<br>
  &lt;/Grid&gt;<br>
&lt;/UserControl&gt;<br>
</code></pre>
</li><li>toolbarview.xaml<br>
<pre><code>&lt;UserControl x:Class="ModuleA.ToolbarView"<br>
             xmlns="http://schemas.microsoft.com/winfx/2006/presentation"<br>
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"<br>
             xmlns:mc="http//schemas.openxmlformats.org/markup-compatibility/2006"<br>
             xmlns:d="http://schemas.micosoft.com/expression/blend/2008"<br>
             mc:Ignorable="d"<br>
             d:DesignHeignt="300" d:DesingWidth="300"&gt;<br>
  &lt;Button Content="Toolbar Item" /&gt;<br>
&lt;/UserControl&gt;<br>
</code></pre>
</li><li>open Shell.xaml and replace the grid with DockPanel and with in the DockPanel add a ContentControl because the items we are going to inject into the Views are single instance. Thats why we use ContentControl as our region host.<br>
<pre><code>// Add a namespace to the Shell<br>
xmlns:prism="http://www.codeplex.com/prism"<br>
 <br>
&lt;DockPanel LastChildfill="True"&gt;<br>
  &lt;ContentControl DockPanel.Dock="Top" prism:RegionManager.RegionName="ToolbarRegion" /&gt;  // we user the prism RegionName to identify the content controls as regions.<br>
  &lt;ContentControl prism:RegionManager.RegionName="ContentRegion" /&gt;<br>
&lt;/Dockpanel&gt;<br>
</code></pre>
</li><li>In the ModuleA project Create ModuleAModule.cs class and put the below code in it:<br>
<pre><code>using Microsoft.Practices.Prism.Modularity;<br>
using Microsoft.Practices.Unity;<br>
using Microsoft.Practices.Prism.Regions,<br>
<br>
namespace ModuleA<br>
{<br>
  public class ModuleAModule : IModule<br>
  {<br>
    IUnityContainer _container;<br>
    IregionManager _regionManager;<br>
<br>
    public ModuleAModule(IUnityContainer container, IRegionMaganer regionManager)<br>
    {<br>
      _container = container;<br>
      _regionManager = regionManager;<br>
    }<br>
<br>
    public void Initialize()<br>
    {<br>
      _regionManager.RegisterViewWithRegion("ToolbarRegion", typeof(ToolbarView));<br>
      _regionManager.RegisterViewWithRegion("ContentRegion", typeof(ContentView));<br>
    }<br>
  }<br>
}<br>
</code></pre>
</li><li>Now lets change this a little bit to get rid of the "magic string" like "ToolbarRegion" and "ContentRegion" and create constans instead<br>
</li><li>Add a class the the Demo.Infrastructure project, Name RegionNames.cs<br>
<pre><code>namespace Demo.Infrastructure<br>
{<br>
  public class RegionNames<br>
  {<br>
    public static string ToolbarRegion = "ToolbarRegion";<br>
    public static string ContentRegion = "ContentRegion";<br>
  }<br>
}<br>
</code></pre>
</li><li>Open the Shell.xaml and add a namespace<br>
<pre><code>xmlns:inf="Demo.Infrastructure;assembly=Demo.Infrastructure"<br>
</code></pre>
</li><li>And change how we set the region name<br>
<pre><code>&lt;ContentControl DockPanel.Dock="Top" prism:RegionManager.RegionName="{x:Static inf:RegionNames.ToolbarRegion}" /&gt;<br>
</code></pre>
</li><li>And do the same for ContentRegion<br>
</li><li>Open the ModuleAModule.cs and change the region names there to.<br>
<pre><code>using Demo.Infrastructure;<br>
<br>
_regionManager.RegisterViewWithRegion(RegionNames.ToolbarRegion, typeof(ToolbarView));<br>
</code></pre>
</li><li>Now we can change the layout of the Shell without having to change any thing else.<br>
<pre><code>DockPanel.Dock="Bottom"<br>
</code></pre>
</li><li>Now we are going add multiple item in the ToolbarRegion so we need to change from ContentControl to ItemsControl<br>
</li><li>To simulate this go to ModuleAModule and change how we register the ToolbarRegion with the manager.<br>
<pre><code>IRegion region = _regionManager.Regions[RegionNames.ToolbarRegion];<br>
region.add(_container.Resolve&lt;ToolbarView&gt;());<br>
region.add(_container.Resolve&lt;ToolbarView&gt;());<br>
region.add(_container.Resolve&lt;ToolbarView&gt;());<br>
region.add(_container.Resolve&lt;ToolbarView&gt;());<br>
region.add(_container.Resolve&lt;ToolbarView&gt;());<br>
</code></pre></li></ul>

<br>
<br>
<hr /><br>
<br>
<br>
<h3>Custom Region</h3>

When you have a third party control or a custom control as your region host, but it has to support one of the region adapters provided by prism but if that is not the case then we can create our own custom region adapter.<br>
<br>
how to create custom region:<br>
<ul><li>Derive from <code>RegionAdapterBase&lt;T&gt;</code> - Create a class that derives from region adapter base<br>
</li><li>Implement CreateRegion method -  and return one of the three objects<br>
<ul><li>SingleActiveRegion -  allows one active view, used for content controls<br>
</li><li>AllActiveRegion - keeps all the views in it active, deactivation of views are not allowed, used for items controls<br>
</li><li>Region - allows multiple active views, used for selector control<br>
</li></ul></li><li>Implement Adapt method -  the code that adapt the control<br>
</li><li>Register your adapter</li></ul>

<br>
<br>
<hr /><br>
<br>
<br>
<h3>Custom Region</h3>

When you have a third party control or a custom control as your region host, but it has to support one of the region adapters provided by prism but if that is not the case then we can create our own custom region adapter.<br>
<br>
how to create custom region:<br>
<ul><li>Derive from <code>RegionAdapterBase&lt;T&gt;</code> - Create a class that derives from region adapter base<br>
</li><li>Implement CreateRegion method -  and return one of the three objects<br>
<ul><li>SingleActiveRegion -  allows one active view, used for content controls<br>
</li><li>AllActiveRegion - keeps all the views in it active, deactivation of views are not allowed, used for items controls<br>
</li><li>Region - allows multiple active views, used for selector control<br>
</li></ul></li><li>Implement Adapt method -  the code that will adapt the control<br>
</li><li>Register your adapter - register the adapter with bootstrapper</li></ul>

<br>
<br>
<hr /><br>
<br>
<br>
<h3>Demo Create a custom Region</h3>

<ul><li>Add a class to the Demo.Infrastructure project and name it StackPanelRegionAdapter<br>
<pre><code>using Microsoft.Practices.Prism.Regions;<br>
using System.Windows.Controls;<br>
<br>
namespace Demo.Infrastructure<br>
{<br>
  public class StackPanelRegionAdapter : RegionAdapterBase&lt;StackPanel&gt;<br>
  {<br>
    public StackPanelRegionAdapter(IRegionBehaviorFactory regionFactory)<br>
	: base(regionBehaviorFactory)<br>
    {<br>
      <br>
    }<br>
    protected override void Adapt(IRegion region, StackPanel regionTarget)<br>
    {<br>
      region.Views.CollectionChanged += (s, e) =&gt;<br>
	{<br>
	  if(e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)<br>
	  {<br>
	    foreach(FramworkElement element in e.NewItems)<br>
	    {<br>
	      regionTarget.Children.Add(element);<br>
	    }<br>
	  }<br>
          // TODO handle remove<br>
	};<br>
    }<br>
<br>
    protected override Iregion CreateRegion()<br>
    {<br>
      return new AllActiveRegion();<br>
    }<br>
  }<br>
}<br>
</code></pre></li></ul>

<ul><li>Open Bootstrapper and override method ConfigureRegionAdapterMappings<br>
<pre><code>using Microsoft.Practices.Prism.Regions;<br>
using Demo.Infrastructure;<br>
<br>
protected override Microsoft.Practices.Prism.Regions.RegionAdapterMappings ConfigureRegionAdapterMappings()<br>
{<br>
  RegionAdapterMappings mappings = base.Configure.RegionAdapterMappings();<br>
  mappings.RegisterMappings(typeof(StackPanel), Containter.Resolve&lt;StackPanelRegionAdapter&gt;());<br>
  return mappings;<br>
}<br>
</code></pre>
</li><li>Change the content control in the Shell from DockPanel to StackPanel</li></ul>

Steps involved in creating custom region<br>
<ol><li>change the control host to StackPanel<br>
</li><li>Create StackPanelRegionAdapter<br>
</li><li>Register the adapter in Bootstrapper</li></ol>

<br>
<br>
<hr /><br>
<br>
<br>
<h2>4. Modules</h2>

<h3>Introduction</h3>

This section will explain how to take the views and logic out of a large application and break them down into a smaller peaces called Modules and use the in the Prism application.<br>
<ul><li>What is a Module?<br>
</li><li>Registering Modules<br>
</li><li>Loading Modules<br>
</li><li>Initializing Modules</li></ul>

<br>
<br>
<hr /><br>
<br>
<br>
<h3>What is a Module?</h3>

<ul><li>Building block of a Prism application, a package that bundles up all the functionality and resources for a portion of the overall application.</li></ul>

Current architecture that we want to change:<br>
<ul><li>Solution that has a Single Main Project with<br>
<ul><li>all the views<br>
</li><li>all services<br>
</li><li>ll business logic<br>
</li><li>and every single code that has to do with this application is in a single project<br>
<img src='http://s17.postimg.org/brbnpypjz/app.png' width='400px'></img></li></ul></li></ul>

We want to identify the major functional areas of our application and start breaking them down into smaller more manageable peaces, and those peaces are called Modules<br>
<img src='http://s23.postimg.org/b910ma8ez/prism.png' width='400px' />

<ul><li>Class library/XAP - we have broken the project down into modules and the result is a class library that can be used in other applications.</li></ul>

<ul><li>Class that implements IModule - each module has a class that implements the IModule interface and that is what identifies it as a module to the Prism library, it has a single method in it called Initialize, which is responsible for initializing the module and integrating it into the Prism application.</li></ul>

<br>
<br>
<hr /><br>
<br>
<br>
<h3>Demo Creating a Module</h3>

<ul><li>Add a WPF User Control Library into the solutions module folder, call it ModuleA and save it to the module folder in our directory structure.<br>
</li><li>delete the UserControl<br>
</li><li>add a reference Microsoft.Practices.Prism.dll<br>
</li><li>add a class and name it ModuleAModule<br>
</li><li>it has to be public and implement the IModule and the Initialize method</li></ul>

<br>
<br>
<hr /><br>
<br>
<br>
<h3>Registering Discovering Modules</h3>

Module Lifetime - first we start by:<br>
<ul><li>Register Modules<br>
</li><li>Discover Modules</li></ul>

All the modules that need to be loaded at runtime are defined in the ModuleCatalog, it contains information about all the modules to be loaded, their location, order to be loaded and if a module is dependent on another module.<br>
<br>
There are some options on how to register the modules with the ModuleCatalog:<br>
<ul><li>Code - the shell will require a reference to the module<br>
</li><li>XAML - Then the application does not require that reference to the module<br>
</li><li>Configuration File (WPF) - Then the application does not require that reference to the module<br>
</li><li>Disk (WPF) - Use a directory on a disk and the application discovers the modules at runtime and we don't need to specify them in a file</li></ul>

<br>
<br>
<hr /><br>
<br>
<br>
<h3>Loading Modules</h3>

the next process in the module lifetime<br>
<ul><li><del>Register Modules</del> - done<br>
</li><li><del>Discover Modules</del> - done<br>
</li><li>Load Modules - All of the assemblies that contain the modules need to be loaded into memory.<br>
This may require the modules to be retrieved from disk<br>
<ul><li>From disk (WPF) - local directory or remote directory (Shared directory on a network)<br>
</li><li>Downloaded from web (Silverlight)<br>
</li><li>Control when to load - Prism allows us to control when to load the modules:<br>
<ul><li>When available - as soon as possible<br>
</li><li>On-demand - when the application needs them<br>
</li><li>download or download in background (Silverlight)</li></ul></li></ul></li></ul>

Guidelines for Loading Modules<br>
<ul><li>Required to run? - is it required to run the application, then it needs to be downloaded and initialized when the application runs<br>
</li><li>Always used? - is it always used or almost always used then it can be downloaded in the background and initialized when it becomes available.<br>
</li><li>Rarely used? - download in the background and initialize on-demand</li></ul>

When we are designing our modules consider how we are breaking the application up, consider common usage scenarios or application startup times, that will help us how to configure our module for downloading and initialization<br>
<br>
<br>
<br>
<hr /><br>
<br>
<br>
<h3>Demo Register Load Modules in Code</h3>

Here we are going to override the ConfigureModuleCatalog method in the Bootstrapper, and we need to add a reference to the module and a using statement in the Bootstrapper.<br>
<br>
<pre><code>protected override void ConfigureModuleCatalog()<br>
{<br>
  Type moduleAType = typeof(ModuleAModule);<br>
  ModuleCatalog.AddModule(new ModuleInfo()<br>
  {<br>
    ModuleName = moduleAType.Name,<br>
    ModuleType = moduleAtype.AssemblyQualifiedName,<br>
    InitializationMode = InitializationMode.WhenAvailable<br>
  });<br>
}<br>
</code></pre>

<br>
<br>
<hr /><br>
<br>
<br>
<h3>Demo Register Load Modules from Directory</h3>

To setup the Prism application to load the modules from a directory, we modify the Bootstrapper by overriding the CreateModuleCatalog method.<br>
<br>
<br>
<pre><code>protected override void CreateModuleCatalog()<br>
{<br>
  return new DirectoryModuleCatalog() { ModulePath = @".\Modules" };<br>
}<br>
</code></pre>

We need to make sure that we are pointing to a valid directory.<br>
If we want to see it work then copy a module dll to that directory.<br>
To define the ModuleName and Initialization method we use attributes.<br>
<pre><code>[Module(ModuleName="ModuleA", OnDemand=true)]<br>
public class ModuleAModule : IModule<br>
</code></pre>

By default the initialization method is going to be when available but we can change it to OnDemand in that Module attribute. If that module has some dependencies then we use this code.<br>
<pre><code>[ModuleDependency("")]<br>
public class ModuleAModule : IModule<br>
</code></pre>

<br>
<br>
<hr /><br>
<br>
<br>
<h3>Demo Register Load Modules from XAML</h3>

Add a Resource Dictionary (WPF) to the shell project name it XamlCatalog. In the properties for the file set the "Build Action" to Resource. Add a namespace<br>
<pre><code>xmlns:Modularity="Microsoft.Practices.Prism.Modularity;assembly=Microsoft.Practices.Prism"<br>
</code></pre>

Replace the ResourceDictionary tags with Modularity:ModuleCatalog.<br>
Next we add a new ModuleInfo declaration<br>
<pre><code>&lt;Modularity:ModuleInfo Ref="file://ModuleA.dll" ModuleName="ModuleA" ModuleType="ModuleA.ModuleAModule, ModuleA, Version=1.0.0.0" InitializationMode="WhenAvailable" /&gt;<br>
</code></pre>

The end result<br>
<pre><code>&lt;Modularity:ModuleCatalog xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"<br>
	xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"<br>
	xmlns:Modularity="Microsoft.Practices.Prism.Modularity;assembly=Microsoft.Practices.Prism"&gt;<br>
  &lt;Modularity:ModuleInfo Ref="file://ModuleA.dll" ModuleName="ModuleA" ModuleType="ModuleA.ModuleAModule, ModuleA, Version=1.0.0.0" InitializationMode="WhenAvailable" /&gt;<br>
&lt;/Modularity:ModuleCatalog&gt;<br>
</code></pre>

Next we open the Bootstrapper and override the CreateModuleCatalog method.<br>
<pre><code>protected override IModuleCatalog CreateModuleCatalog()<br>
{<br>
  return Microsoft.Practices.Prism.Modularity.ModuleCatalog.CreateFromXaml(new Uri("/Demo;component/XamlCatalog.xaml", UriKind.Relative));<br>
}<br>
</code></pre>

We need to copy the module to the root of the application for this to work. Would be a good idea to create a build action to copy the modules to the application root folder<br>
<br>
<br>
<br>
<hr /><br>
<br>
<br>
<h3>Demo Register Load Modules from App.config File</h3>

We start by overriding the CreateModuleCatalog method in the Bootstrapper.<br>
<pre><code>protected override IModuleCatalog CreateModuleCatalog()<br>
{<br>
  return new ConfigurationModuleCatalog();<br>
}<br>
</code></pre>

Add App.config file to the shell project. Add configSection tag to the App.config, create a new section and give it the name "modules" and the type "Microsoft.Practices.Prism.Modularity.ModulesConfigurationSection, Microsoft.Practices.Prism".<br>
Now we can define the modules<br>
<pre><code>&lt;modules&gt;<br>
  &lt;module assemblyFile="Modules/ModuleA.dll" moduleType="ModuleA.ModuleAModule, ModuleA, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" moduleName="ModuleA" startupLoaded="true" /&gt;<br>
&lt;/modules&gt;<br>
</code></pre>

end result:<br>
<pre><code>&lt;?xml version="1.0" encoding="utf-8" ?&gt;<br>
&lt;configuration&gt;<br>
  &lt;configuration&gt;<br>
    &lt;section name="modules" type="Microsoft.Practices.Prism.Modularity.ModulesConfigurationSection, Microsoft.Practices.Prism" /&gt;<br>
  &lt;/configuration&gt;<br>
  &lt;modules&gt;<br>
    &lt;module assemblyFile="Modules/ModuleA.dll" moduleType="ModuleA.ModuleAModule, ModuleA, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" moduleName="ModuleA" startupLoaded="true" /&gt;<br>
  &lt;/modules&gt;<br>
&lt;/configuration&gt;<br>
</code></pre>

Open ModuleAModule and add a code to show MessageBox when the module initializes.<br>
<pre><code>MessageBox.Show("ModuleA Loaded");<br>
</code></pre>

And lets create a post build event and go to ModuleA project properties, select the Build Events tab and put this command in the Post-build event command line: box.<br>
<pre><code>xcopy "$(TargetDir)*.*" "$(SolutionDir)Src\Demo\bin\$(ConfigurationName)\Modules\" /Y<br>
</code></pre>

This command is going to copy the output of the ModuleA project into the Modules folder of our shell project.<br>
<br>
<br>
<br>
<hr /><br>
<br>
<br>
<h3>Initializing Modules</h3>

The last thing in the Module Lifetime is the Initialization.<br>
<ul><li><del>Register Modules</del>
</li><li><del>Discover Modules</del>
</li><li><del>Load Modules</del>
</li><li>Initialize Modules<br>
<img src='http://s13.postimg.org/6l1fhqlif/module_lifetime.png' width='300px' /></li></ul>

<b>Initializing Modules</b>
<ul><li>IModule.Initialize()<br>
</li><li>Register types<br>
</li><li>Subscribe to services or events<br>
</li><li>Register shared services<br>
</li><li>Compose views into shell</li></ul>

When modules are being initialized, instances of the central module class are being created and the Initialize() method in them are being called via the IModule interface.<br>
The Initialize method is where we write the code that is going to Register our types, we may subscribe to services or events, we can register shared services or we can compose our views into the shell. The initialize method is where we put all our code that does all the work to get our module ready for consumptions in our Prism application.<br>
<br>
<br>
<br>
<hr /><br>
<br>
<br>
<br>
<h2>5. Views</h2>
In this section we go over how Views work.<br>
<ul><li>What is a View?<br>
</li><li>View Composition<br>
</li><li>View Discovery<br>
</li><li>View Injection</li></ul>

<br>
<br>
<hr /><br>
<br>
<br>
<h3>What is a View?</h3>

<ul><li>Portion of the user interface -  smaller unit of the user interface which encapsulates a piece of functionality and decoupled from other parts of the User Interface.</li></ul>

Lets use Outlook as an example, when we first look at it you get the impression that you only have a single View to use and interact with the application. If we analyse it more closely we can identify sections of functionality that does not have any dependencies on other parts on the View:<br>
<ul><li>Toolbar - probably its own view, has no dependencies on other parts of the application, its main function is to send messages of actions that need to be performed. Other parts of the application listen for specific messages and perform actions based on the message it receives<br>
</li><li>Navigation area<br>
</li><li>Email list Content area<br>
</li><li>Statusbar</li></ul>

<img src='http://s10.postimg.org/4o8igle9l/outlook_Views.png' width='600px' />

Even though the user sees one cohesive view it is composed of many views.<br>
<br>
<ul><li>Can be made of multiple views (Composite view)<br>
</li><li>UserContol, Page, DataTemplate, etc...<br>
</li><li>Multiple instances - we can have tab control with number of taps in it showing the same view but they all show different data.<br>
</li><li>Patterns not required</li></ul>

<br>
<br>
<hr /><br>
<br>
<br>
<h3>Demo Creating a View</h3>

Now we are going to create toolbarView and contentView. We begin by adding these views to the module "ModuleA".<br>
<br>
<ul><li>Right click on the project and add user control and name it "ToolbarA", delete the grid from the XAML and add a button instead.<br>
</li><li>Add another user control and name it "ContentA" and add a TextBlock to the XAML grid.<br>
</li><li>We are not using any type of pattern to create these Views. Not in this section anyway.<br>
</li><li>Next we register our Views with our Container.<br>
</li><li>Open ModulAModule, in the Initialize method we need to register our views with our container.<br>
</li><li>Add a reference to Microsoft.Practices.Unity.dll<br>
</li><li>Add the namespace using Microsoft.Practices.unity<br>
</li><li>Add a private variable of IUnityContainer to the class.<br>
</li><li>Now create a public constructor that takes in IUnityContainer parameter and let the class variable of IUnityContainer take the value of the constructor variable.<br>
</li><li>Now we can register our views with the container in the Initialize method.</li></ul>

<pre><code>using Microsoft.Practices.Unity;<br>
<br>
namespace ModuleA<br>
{<br>
  public class ModuleAModule : IModule<br>
  {<br>
    IUnityContainer _container;<br>
    public ModuleAModule(IUnityContainer container)<br>
    {<br>
      _container = container;<br>
    }<br>
<br>
    public void Initialize()<br>
    {<br>
      _container.RegisterType&lt;ToolbarA&gt;();<br>
      _container.RegisterType&lt;ContentA&gt;();<br>
    }<br>
  }<br>
}<br>
</code></pre>

Whats happening here is when the module is created, Prism recocnizes that we are asking for IUnityContainer and provides us with one, once we have that container the Initialize mothod is going to be called and we start registering our Types with the container, once the types are registered with our container we can start using them with our module.<br>
<br>
<br>
<br>
<hr /><br>
<br>
<br>
<h3>Demo Creating a View (MVVM)</h3>

Now we are going to change the views in this module to use the MVVM design pattern.<br>
<br>
<ul><li>Add a new Interface to the Demo.Infrastructure and name it IView.<br>
</li><li>Add another Interface to Demo.Infrastructure and name it IViewModel<br>
</li><li>Make sure they are both public<br>
</li><li>IView is going to have IViewModel instance property<br>
</li><li>IViewModel is going to have IView property, here the reference is to an interface not directly to the view<br>
</li><li>Normally we would create a base class but we are going to skip that step right now<br>
</li><li>Add a new interface to the ModuleA project and name it IContentAView<br>
</li><li>Add another interface to the ModuleA project and name it IContentViewViewModel<br>
</li><li>make them public<br>
</li><li>Add reference to the Demo.Infrastructure project and the namespace<br>
</li><li>Now let the IContentAView inherit the IView<br>
</li><li>Open the ContentA.xaml.cs code behind file and let it inherit the IContentAView and implement it<br>
<pre><code>public Demo.Infrastructure.IViewModel ViewModel<br>
{<br>
  get<br>
  {<br>
    return (IContentAViewViewModel)DataContext;<br>
  }<br>
  set<br>
  {<br>
    DataContext = value;<br>
  }<br>
}<br>
</code></pre></li></ul>

<ul><li>open IContentViewViewModel and let it inherit the IViewModel and implement it<br>
</li><li>Now lets create our ViewModel add a new class and name it ContentAViewViewModel, make sure its public and let it implement the IContentAViewViewModel<br>
</li><li>Now we create a constructor<br>
<pre><code>public Demo.Infrastructure.IView View { get; set; }<br>
<br>
public ContentAViewViewModel(IContentAView view)<br>
{<br>
  View = view;<br>
  View.ViewModel = this;<br>
}<br>
</code></pre></li></ul>


So what's going to happen is when this ViewModel is resolved for our container it is going to see that we are asking for a view of Type IContentAView, when we get the view we are going to set the ViewModel to the current instance and when that happens the setter on the ViewModel property of the ContentA view will set the DataContext to that instance of the ViewModel, that is what binds the View and the ViewModel. But we have to register our types with our container.<br>
<br>
Open ModuleAModule class and change the initialize method<br>
<pre><code>_container.RegisterType&lt;IContentAView, ContentA&gt;();<br>
_container.RegisterType&lt;IContentAViewViewModel, ContentAViewViewModel&gt;();<br>
</code></pre>

So whenever we ask for IContentAViewViewModel the container will resolve an instance of ContentAViewViewModel for us<br>
<br>
<br>
<br>
<hr /><br>
<br>
<br>
<h3>Demo Creating a View (MVVM) - View First</h3>

In the previous demo we implemented the MVVM pattern in our Prism application using a ViewModel first approach, that is where the ViewModel is responsible for instantiating the View. Now lets take a look at more common View first approach that is when the View is responsible for instantiating the ViewModel.<br>
<br>
<ul><li>Add a new interface to Demo.Infrastructure and name it IViewModel and make it public.<br>
</li><li>Add a new interface to Demo.Infrastructure and name it IView and make it public<br>
</li><li>IView should have IViewModel property<br>
</li><li>But the IView will not have IViewModel property.<br>
</li><li>Add a new interface to ModelA Project and name it IContentAViewViewModel make it public and implement IViewModel so we need a reference to Demo.Infrastructure and a using namespace.<br>
</li><li>Add a new class to ModelA and name it ContentAViewViewModel, should be public and implement IContentAViewViewModel<br>
</li><li>Add a constructor to the class<br>
</li><li>Open the code behind for the ContentA view and implement the IView interface<br>
<pre><code>public IViewModel ViewModel<br>
{<br>
  get<br>
  {<br>
    return (IViewModel)DataContext;<br>
  }<br>
  set<br>
  {<br>
    DataContext = value;<br>
  }<br>
}<br>
</code></pre>
</li><li>Add a parameter of the type IContentAViewViewModel to the constructor and set the ViewModel to that parameter.</li></ul>

So what's going to happen is when we create an instance of this View, the view will create an instance of the IContentAViewViewModel and when that instance is created we are assigning that instance to the ViewModel property which is setting the DataContext to that instance. Thats where the binding between the ViewModel and the View is occurring<br>
<br>
<ul><li>Lastly we need to register our types<br>
<pre><code>_container.RegisterType&lt;ToolbarA&gt;();<br>
_container.RegisterType&lt;ContentA&gt;();<br>
_container.RegisterType&lt;IContentAViewViewModel, ContentAViewViewModel&gt;();<br>
</code></pre></li></ul>

In the implementation we don't need to register our interface for the views.<br>
<br>
<br>
<br>
<hr /><br>
<br>
<br>
<h3>View Composition</h3>
<ul><li>Constructing of a view - simply the process of constructing a view<br>
</li><li>Made up of many visual elements - made up of many visual elements that are loosely coupled and are often contained within the module. As these elements are created and loaded they will be displayed in regions.<br>
</li><li>Displayed in Regions - which is normally hosted by the Shell and you can create and display these elements either automatically thru discovery or programmatically trug View injection<br>
</li><li>View Discovery<br>
</li><li>View Injection</li></ul>

<br>
<br>
<hr /><br>
<br>
<br>
<h3>View Discovery</h3>

<ul><li>Views added automatically - With view discovery views are added to the region automatically. To enable view discovery we need to setup relationship in the region view registry between a region name and the type of view. To do this we call the RegionManager.RegisterViewWithRegion(name, type) pass in the name of the region and pass in the type.<br>
</li><li>RegionManager.RegisterViewWithRegion(name, type) -  typically this is done when the module initializes or when a user performs an action.<br>
</li><li>Region looks for view types - when the region is created it looks for all the view types that are associated with that region and automatically instantiates and displays thous views.<br>
</li><li>No explicit control -  Side effect of this behaviour it we don't have explicit control of when to load and display a view inside a region</li></ul>

<br>
<br>
<hr /><br>
<br>
<br>
<h3>Demo View Discovery</h3>

How to implement View discovery:<br>
<ul><li>Open ModuleAModule class and we need to get a reference to the regionManager so we add an IRegionManager parameter to the constructor<br>
</li><li>add a private class variable of the type IRegionManager<br>
</li><li>And the class variable takes the value of the constructor variable<br>
</li><li>now we enable view discovery for the toolbarView<br>
<pre><code>_container.RegisterType&lt;ToolbarA&gt;();<br>
_container.RegisterType&lt;IContentAView, ContentA&gt;();<br>
_container.RegisterType&lt;IContentAViewViewModel, ContentAViewViewModel&gt;();<br>
<br>
_manager.RegisterViewWithRegion(RegionName.ToolbarRegion, typeof(ToolbarA));<br>
</code></pre></li></ul>

Now when ever the region is created the region will automatically initialize and instantiate our Toolbar item. As you can see we have very limited control on how and when the toolbar is instantiated and displayed.<br>
<br>
<hr />
<h3>View Injection</h3>

<ul><li>View added programmatically - with the view injection views are added programmatically. This can also be done when a module initializes or as a result of user action. We can achieve view injection couple of different ways.<br>
</li><li>RegionManager.Region["Name"].Add(view, name) -  ["Name"] is the index of region name and pass in a view and the name of the view.<br>
</li><li>IRegion.Add(view, name) - Or we can get an instance of the IRegion from the RegionManager and work with the region directly.<br>
</li><li>Activate/Deactivate - when adding views programmatically we may need to activate and deactivate views. For example, if we have a content control as a region which already contains a view and we add a new view to it we will need to deactivate the current view before the newly injected view is shown.</li></ul>

<ul><li>More control - As you can see view injection gives you more control over when views are loaded and displayed, we even have the ability to remove views from the region.</li></ul>

<ul><li>Can't add View to Region that hasn't been created - keep in mind that we can't add a view to a region that hasn't been created, with view discovery a view is automatically created when a region is created, but with view injection we have to be more aware of what regions have been created before we try to inject our view into it.</li></ul>

<hr />
<h3>Demo View Injection</h3>

View injection gives us much more control on how and when views are created and displayed.<br>
<br>
<ul><li>Open ModuleAModule class</li></ul>

<pre><code>public void Initialize()<br>
{<br>
  _container.RegionType&lt;ToolbarA&gt;();<br>
  _container.RegionType&lt;IContentAView, ContentA&gt;();<br>
  _container.RegionType&lt;IContentAViewViewModel, ContentAViewViewModel&gt;();<br>
<br>
  _manager.RegisterViewWithRegion(RegionName.ToolbarRegion, typeof(ToolbarA));<br>
<br>
  // Now lets create a view injection<br>
  // Get an instance of the viewModel<br>
  var vm = _container.Resolve&lt;IContentAViewViewModel&gt;();<br>
  // now we use the viewModel to inject the view into the contentRegion<br>
  _manager.Regions[RegionNames.ContentRegion].Add(vm.View);<br>
  IRegion region = _manager.Regions[RegionName.ContentRegion];<br>
  region.add (vm.View);<br>
  // And with the instance of the region we can add View<br>
  // and Activate the view, getting the activeViews, Behaviors, Deactivation. So we have much more control over the region and the views with in that region<br>
}<br>
</code></pre>

<ul><li>Open IContentViewViewModel interface and add a string property named Message<br>
</li><li>Open ContentAViewViewModel and implement that change. Ideally we want our ViewModels to implement the INotifyPropertyChange interface and on the setter call the IPropertyChangeEvent.<br>
</li><li>Open the ContentA.XAML and set the text to bind to the message.<br>
<pre><code>&lt;TextBlock Text="{Binding Message}" FontSize="38" /&gt;<br>
</code></pre>
</li><li>Now in the ModuleAModule we can sett the ViewModel Message</li></ul>

<pre><code>var vm = _container.Resolve&lt;IContentAViewViewModel&gt;();<br>
vm.Message = "First View";<br>
IRegion region = _manager.Regions[RegionNames.ContentRegion];<br>
region.Add(vm.View);<br>
</code></pre>

<ul><li>Now lets create another View and add it to the same region<br>
<pre><code>var vm2 = _container.Resolve&lt;IContentAViewViewModel&gt;();<br>
vm2.Message = "Second View";<br>
<br>
region.Deactivate(vm.View);<br>
region.add(vm2.View);<br>
</code></pre></li></ul>

<hr />
<h2>6. Commands</h2>
When building a Prism application we divide the application into individual, loosely coupled modules and sometimes it is necessary to communicate between those modules. To send data, user service or correspond to an action.<br>
In this section we go over how to communicate between modules and still maintain loose coupling.<br>
When we need to communicate between modules there are a couple of approaches that we can take:<br>
<ul><li>Commanding - supports delegate commands, composite commands<br>
</li><li>Event Aggregation -<br>
</li><li>Shared Services -<br>
</li><li>Region Context -</li></ul>

<hr />
<h3>Commanding Overview</h3>

The most common method of communication in a Prism is Commanding.<br>
What is Commanding:<br>
<ul><li>Binds a UI gesture to action - such as Button Click to action that needs to be performed.<br>
</li><li>Execute - each command has an execute method, this method is called when the method is invoked.<br>
</li><li>CanExcute - command has also a CanExecute method and this method determines whether or not the command can be executed, and the element that is bound to the command will be either enabled or disabled based on the result of the CanExcute method.<br>
</li><li>RoutedCommand - The most common way of creating commands is either to use the RoutedCommand or CustomCommand. RoutedCommands deliver their command messages thru UI elements in the visual tree. That means that any element outside the tree will not receive these messages. RoutedCommands require to create the handlers in the codeBehind, which is where we don't want to be placing our logic.<br>
</li><li>CustomCommand - This involves creating a custom class that derives from ICommand and implements the ICommand interface. Often creating custom commands involves more work. We must provide ways for command handlers to hook up and do the routing when the command is provoked. We must also decide what criteria we will use for determining when to raise the CanExcuteChangedEvent.</li></ul>

In a Prism application the command handler is often a ViewModel and doesn't have any association with any element in the VisualTree. Prism provides two classes that makes commanding easier and provides more functionality:<br>
<ul><li>DelegateCommand -  which allows you to call a delegate method when the command is executed.<br>
</li><li>CompositeCommand - which allows us to combine multiple commands.</li></ul>

<hr />
<h3>DelegateCommand</h3>

<ul><li>Uses delegates - A delegate command is a command that allows you to supply methods as delegates that will be invoked when the command is invoked.<br>
</li><li>Doesn't require a handler - Event handler is not required in the code behind.<br>
</li><li>Usually local - delegates are normally locally scoped, meaning they are created within the ViewModel, and the concerns of the delegate methods are within the context of that ViewModel.<br>
</li><li>DelegateCommand or DelegateCommand<code>&lt;T&gt;</code> - Prism provides us with two delegate commands. the difference is that the Execute and CanExcute delegate methods for the delegate command will not accept a parameter for as the delegate command of <code>&lt;T&gt;</code> allows us to specify the type of parameter that the Execute and CanExcute methods parameter will be.</li></ul>

<hr />
<h3>Demo Creating a DelegateCommand</h3>

Lets take a look at how this application looks.<br>
<br>
<ul><li>The Shell has two regions in it, StatusBarRegion and ContentRegion.<br>
</li><li>The Infrastructure project has a new ViewModelBase class, it implements the INotifyPropertyChanged and the IViewModel interfaces.</li></ul>

<pre><code>public class : ViewModelBase : IViewModel, INotifyPropertyChanged<br>
{<br>
  public IView View { get; set; }<br>
<br>
  public ViewModelBase(IView view)<br>
  {<br>
    View = view;<br>
    View.ViewModel = this;<br>
  }<br>
<br>
  public event PropertyChangedEventHandler PropertyChanged;<br>
  protected void OnPropertyChanged(string propertyName)<br>
  {<br>
    if(PropertyChanged != null)<br>
      PropertyChanged(this, new PropertyChangedEventArgs(propertyName));<br>
  }<br>
}<br>
</code></pre>

It has a basic constructor which wires up the view and the Views viewModel.<br>
<br>
<ul><li>The application has two modules<br>
</li><li>the StatusBar Module<br>
</li><li>and the People Module - it represents anything that has to do with people or a person. It has a single view in it which is a form which will be used to edit a single instance of a person class.</li></ul>

<pre><code>&lt;Grid xName="LayoutRoot" Background="White"&gt;<br>
  &lt;Grid.ColumnDefinitions&gt;<br>
    &lt;ColumnDefinition Width="Auto" /&gt;<br>
    &lt;ColumnDefinition /&gt;<br>
  &lt;/Grid.ColumnDefinitions&gt;<br>
  &lt;Grid.RowDefinitions&gt;<br>
    &lt;RowDefinition Height="Auto" /&gt;<br>
    &lt;RowDefinition Height="Auto" /&gt;<br>
    &lt;RowDefinition Height="Auto" /&gt;<br>
    &lt;RowDefinition Height="Auto" /&gt;<br>
    &lt;RowDefinition Height="Auto" /&gt;<br>
  &lt;/Grid.RowDefinitions&gt;<br>
  <br>
  &lt;TextBlock Text="First Name:" Margin="5" /&gt;<br>
  &lt;TextBox Grid.Column="1" Text="{Binding Person.FirstName, Mode=TwoWay}" Margin="5" /&gt;<br>
  &lt;TextBlock Grid.Row="1" Text="Last Name:" Margin="5" /&gt;<br>
  &lt;TextBox Grid.Row="1" Grid.Column="1" Text="{Binding Person.LastName, Mode=TwoWay}" Margin="5" /&gt;<br>
  &lt;TextBlock Gfid.Row="2" Text="Age:" Margin="5" /&gt;<br>
  &lt;TextBox Grid.Row="2" Grid.Column="1" Text="{Binding Person.Age, Mode=TwoWay}" Margin="5" /&gt;<br>
  &lt;TextBlock Grid.Row="3" Text="Last Updated:" Margin="5" /&gt;<br>
  &lt;TextBox Grid.Row="3" Grid.Column="1" Text="{Binding Person.LastUpdatedDate, Mode=TwoWay}" Margin="5" /&gt;<br>
<br>
  &lt;Button Grid.Row="4" Grid.ColumnSpan="2" Content="Save" Margin"10" /&gt;<br>
&lt;/Grid&gt;<br>
</code></pre>

<ul><li>That person class has been defined in our Business project.</li></ul>

<pre><code>public class Person : INotifyPropertyChanged, IDataErrorInfo<br>
{<br>
  private string _firstName;<br>
  public string FirstName<br>
  {<br>
    get { return _firstName; }<br>
    set<br>
    {<br>
      _firtsName = value;<br>
      OnPropertyChanged("FirstName");<br>
    }<br>
  }<br>
<br>
  private string _lastName;<br>
  publivc string LastName<br>
  {<br>
    get { return _lastName; }<br>
    set<br>
    {<br>
      _lastName = value;<br>
      OnPropertyChanged("LastName");<br>
    }<br>
  }<br>
  <br>
  private int _age;<br>
  public int Age<br>
  {<br>
    get { return _age; }<br>
    set<br>
    {<br>
      _age = value;<br>
      OnPropertyChanged("Age");<br>
    }<br>
  }<br>
<br>
  private DateTime? _lastUpdated;<br>
  public DateTime LastUpdated<br>
  {<br>
    get { return _lastUpdated; }<br>
    set<br>
    {<br>
      _lastUpdated = value;<br>
      OnPropertyChanged("LastUpdated");<br>
    }<br>
  }<br>
<br>
  public event PropertyChangedEventHandler PropertyChanged;<br>
  protected void OnPropertyChanged(string propertyname)<br>
  {<br>
    if(PropertyChanged != null)<br>
      PropertyChanged(this, new PropertyChangedEventArgs(propertyname));<br>
  }<br>
<br>
  public string Error<br>
  {<br>
    get { return null; }<br>
  }<br>
<br>
  public string this[string columnName]<br>
  {<br>
    get<br>
    {<br>
      string error = null;<br>
<br>
      switch(columnName)<br>
      {<br>
        case "FirstName":<br>
          if(string.IsNullOrEmpty(_firstName))<br>
          {<br>
            error = "First Name required";<br>
          }<br>
          break;<br>
        case "LastName":<br>
          if(string.IsNullOrEmpty(_lastName))<br>
          {<br>
            error = Last Name required";<br>
          }<br>
          break;<br>
        case "Age":<br>
          if((_age &lt; 18) || (_age &gt; 85))<br>
          {<br>
            error = "Age out of range.";<br>
          }<br>
          break;<br>
      }<br>
    }<br>
    return (error);<br>
  }<br>
}<br>
</code></pre>

If we run the application you can see that it is a very simple form, it has some text elements and a save button. The functionality that we want to implement for this application, is when we click the save button we want the last updated date to reflect the date when this object was saved. Lets do that now.<br>
<br>
<ul><li>Open PersonView.XAML and change the button element<br>
<pre><code>&lt;Button Grid.Row="4" Grid.ColumnSpan="2" Content="Save" Margin="10" Command="{Binding SaveCommand}" /&gt;<br>
</code></pre></li></ul>

<ul><li>The SaveCommand does not exist in the ViewModel so lets create it. Open the PersonViewModel.<br>
<pre><code>public class PersonViewModel : ViewModelBase, IPersonViewModel<br>
{<br>
  // Here we add our DelegateCommand for the SaveCommand<br>
  public DelegateCommand SaveCommand { get; set; }<br>
<br>
  public PersonViewModel(IPersonView view)<br>
	: base(view)<br>
  {<br>
    CreatePerson();<br>
<br>
    // Here we create a instance of the SaveCommand<br>
    // The first parameter is the method that will be executed when the command is invoked<br>
    // The second parameter will be the CanExcute method<br>
    SaveCommand = new DelegateCommand(Save, CanSave);<br>
  }<br>
<br>
  private void Save()<br>
  {<br>
    Person.LastUpdated = DateTime.Now;<br>
  }<br>
<br>
  private bool CanSave()<br>
  {<br>
    return Person.Error == null; // Can save if there are no errors<br>
  }<br>
<br>
  private Person _person;<br>
  public Person Person<br>
  {<br>
    get { return _person; }<br>
    set<br>
    {<br>
      _person = value;<br>
      _person.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(_person_PropertyChanged);<br>
      OnPropertyChanged("Person");<br>
    }<br>
  }<br>
<br>
  private void _person_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)<br>
  {<br>
    SaveCommand.RaiseCanExecuteChanged();<br>
  }<br>
<br>
  private void CreatePerson()<br>
  {<br>
    Person = new Person()<br>
    {<br>
      FirstName = "Bob",<br>
      LastName = "Smith",<br>
      Age = 46<br>
    };<br>
  }<br>
}<br>
</code></pre></li></ul>


Now we can change the DelegateCommand to DelegateCommand<code>&lt;T&gt;</code>
<pre><code>public DelegateCommand&lt;object&gt; SaveCommand { get; set; }<br>
<br>
// In the constuctor<br>
SaveCommand = new DelegateCommand&lt;object&gt;(Save, CanSave);<br>
<br>
// Now we can change the methods<br>
private void Save(object value)<br>
{<br>
  Person.LastUpdated = DateTime.Now.AddYears(Convert.toInt32(value));<br>
}<br>
<br>
private bool CanSave(object value)<br>
</code></pre>

We have to add a CommandParameter to the PersonView.XAML in the button attributes<br>
<pre><code>&lt;Button Grid.Row="4" Grid.Span="2" Content="Save" Margin="10" Command="{Binding SaveCommand}" CommandParameter="2"<br>
</code></pre>

Now when we run the application and click the button we add two years to the date. We have to have the parameter of the type object because the DelegateCommand<code>&lt;T&gt;</code> can't take types that are not nullable<br>
<br>
<hr />
<h3>CompositeCommand</h3>

<ul><li>Usually global - usually globally scoped commands that exist in the infrastructure class.<br>
</li><li>Multiple child commands - they contain multiple child commands, for example multiple views that need to be independently saved where we have save all button.<br>
</li><li>Local commands are registered with command - each view would have the local command that are defined in the viewModel register with the save all compositeCommand.<br>
</li><li>When invoked, all child commands are invoked -  so when the save all command is invoked all registered child commands will also be invoked.<br>
</li><li>Supports enablement - and because compositeCommand support enablement by listening to the canExecuteChanged event of each registered child command if any call to the canExecute returns false in a child command the compositeCommand will also return false thus disabling all the invokers.</li></ul>

<hr />
<h3>Demo Creating a CompositeCommand</h3>

Now let's implement a CompositeCommand. First lets look at the changes we made to our application.<br>
<br>
<ul><li>Open the Shell<br>
<pre><code>&lt;Window x:Class="PluralsightPrismDemo.Shell"<br>
	 xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"<br>
	 xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"<br>
	 xmlns:prism="http://www.codeplex.com/prism"<br>
	 xmlns:inf="clr-namespace:Demo.Infrastructure;assembly=Demo.Infrastructure"<br>
	 Title="Shell" Height="350" Width="350"&gt;<br>
  &lt;Grid&gt;<br>
    &lt;DockPanel LastChildFill="True"&gt;<br>
      &lt;ContentControl prism:RegionManager.RegionName="{x:Static inf:RegionNames.ToolbarRegion}" DockPanel.Dock="Top" Width="Auto" Height="Auto" /&gt;<br>
      &lt;ContentControl prism:RegionManager.RegionName="{x:Static inf:RegionNames.StatusBarRegion}" DockPanel.Dock="Bottom" Width="Auto" Height="Auto" /&gt;<br>
      &lt;TabControl prism:RegionManager.RegionName="{x:Static inf:RegionNames.ContentRegion}" Width="Auto" Height="Auto" HorizontalAlignment="center" /&gt;<br>
        &lt;TabControl.ItemContainerStyle&gt;<br>
          &lt;Style TargetType="TabItem"&gt;<br>
            &lt;Setter Property="Header" Value="{Binding Content.DataContext.ViewName, RelativeSource={RelativeSource Self}}" /&gt;<br>
          &lt;/Style&gt;<br>
        &lt;/TabControl.ItemContainerStyle&gt;<br>
      &lt;/TabControl&gt;<br>
    &lt;/DockPanel&gt;<br>
  &lt;/Grid&gt;<br>
&lt;/Window&gt;<br>
</code></pre></li></ul>

<ul><li>We added a new Toolbar Module which has a single view in it that has a single button in <code>&lt;Button&gt; Save All&lt;/Button&gt;</code></li></ul>

<ul><li>We have modified the PeopleModule<br>
<pre><code>public class PeopleModule : IModule<br>
{<br>
  private readonly IRegionManager _regionManager;<br>
  private readonly IUnityContainer _container;<br>
<br>
  public PeopleModule(IUnityContainer container, IRegionManager regionManager)<br>
  {<br>
    this._container = container;<br>
    this._regionManager = regionManager;<br>
  }<br>
<br>
  public void Initialize()<br>
  {<br>
    RegisterViewsAndServices();<br>
<br>
    IRegion region = _regionManager.Regions[RegionNames.ContentRegion];<br>
<br>
    var vm = _container.Resolve&lt;IPeronsViewModel&gt;();<br>
    vm.CreatePerson("Bob", "Smith");<br>
<br>
    region.Add(vm.View);<br>
    region.Activate(vm.View);<br>
<br>
    var vm2 = _container.Resolve&lt;IPersonViewModel&gt;();<br>
    vm2.CreatePerson("Karl", "Sums");<br>
    region.Add(vm2.Vew);<br>
<br>
    var vm3 = _container.Resolve&lt;IPersonViewModel&gt;();<br>
    vm3.CreatePerson("Jeff", "Lock");<br>
    region.Add(vm3.View);<br>
  }<br>
<br>
  protected void RegisterViewsAndServices()<br>
  {<br>
    _container.RegisterType&lt;IPersonViewModel, PersonViewModel&gt;();<br>
    _container.RegisterType&lt;IPersonView, PersonView&gt;();<br>
  }<br>
}<br>
</code></pre></li></ul>

<ul><li>Now we implement the Save all function.<br>
</li><li>Create a new GlobalCommands class in our Infrastructure project<br>
<pre><code>public static class GlobalCommands<br>
{<br>
  public static CompositeCommand SaveAllCommand = new CompositeCommand();<br>
}<br>
</code></pre>
</li><li>Now open up the ToolbarView.xaml in our Toolbar Module and add a namespace <code>xmlns:inf="clr-namespace:Demo.Infrastructure;assembly=Demo.Infrastructure"</code>
</li><li>Now set the command for the button<br>
<pre><code>&lt;Button Command="{x:Static inf:GlobalCommands.SaveAllCommand}"&gt;Save All&lt;/Button&gt;<br>
</code></pre>
</li><li>Now we have to register our ViewModel for that CompositeCommand.<br>
</li><li>Open PersonViewModel in the People project and change the constructor to register the SaveCommand with the CompositeCommand<br>
<pre><code>public PersonViewModel(IPersonView view)<br>
	: base(view)<br>
{<br>
  SaveCommand = new DelegateCommand(Save, CanSave);<br>
<br>
  GlobalCommands.SaveAllCommand.RegisterCommand(SaveCommand);<br>
}<br>
</code></pre></li></ul>

<hr />
<h3>Event Aggregation</h3>

<ul><li>Loosely coupled event based communication - The basic concept of Event Aggregation is that it is a loosely coupled event based communication mechanism.<br>
</li><li>Publisher and Subscribers - it is made up of publishers and subscribers. Publisher will execute an event and a subscriber will listen for that event<br>
</li><li>Manages memory related to eventing - memory leaks are reduced because subscribers don't need a strong reference to the publishers. Therefore we don't have to manually unsubscribe our subscribers. For further information study the Event Aggregation design pattern.</li></ul>

Prism has a build in support for EventAggregator by providing a core service which can be retrieved thru the IEventAggregator<br>
<ul><li>IEventAggregator - the event aggregator is responsible for locating and building events and for keeping the  collection of events in the system. Both Publishers and Subscribers need an instance of the EventAggregator and to get that instance just request it from the container.<br>
</li><li>Multicast Pub/Sub -  It provides multicast pub/sub functionality. This means that there can be multiple publishers that raise the same event, and there can be multiple subscribers listening to that same event.<br>
</li><li>Events are typed events deriving from EventBase - Events created by the Prism library are typed events. This means we can take advantage of the compile type checking, to check for error before we run the application.<br>
</li><li>CompositePresentationEvent<br>
<br>
<T><br>
<br>
 -  this is the only class that Prism provides us with for creating events. This class maintains a list of subscribers and handles events dispatching to the subscribers.<br>
</li><li>{{<br>
<br>
<T><br>
<br>
}}} is the required Payload -  the payload is what we want to send to the subscriber when the event is published.</li></ul>

Lets look at some of the services that the EventAggregator provides.<br>
<ul><li>Publish events - we can publish event, publishers raise an event by retrieving the event from the eventAggregator and then calling the publish method.<br>
</li><li>Subscribe to events - subscribers can also register with an event using one of the subscribe method overloads available on the composite presentation event class, and there are a couple of ways to subscribe to an event.<br>
</li><li>Subscribe using a strong reference - keepSubscriberReferenceAlive<br>
</li><li>Event filtering<br>
</li><li>Unsubscribe from events</li></ul>

<hr />