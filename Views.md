# 5. Views #
In this section we go over how Views work.
  * What is a View?
  * View Composition
  * View Discovery
  * View Injection


---

## What is a View? ##

  * Portion of the user interface -  smaller unit of the user interface which encapsulates a piece of functionality and decoupled from other parts of the User Interface.

Lets use Outlook as an example, when we first look at it you get the impression that you only have a single View to use and interact with the application. If we analyse it more closely we can identify sections of functionality that does not have any dependencies on other parts on the View:
  * Toolbar - probably its own view, has no dependencies on other parts of the application, its main function is to send messages of actions that need to be performed. Other parts of the application listen for specific messages and perform actions based on the message it receives
  * Navigation area
  * Email list Content area
  * Statusbar

<img src='http://s10.postimg.org/4o8igle9l/outlook_Views.png' width='600px' />

Even though the user sees one cohesive view it is composed of many views.

  * Can be made of multiple views (Composite view)
  * UserContol, Page, DataTemplate, etc...
  * Multiple instances - we can have tab control with number of taps in it showing the same view but they all show different data.
  * Patterns not required


---

## Demo Creating a View ##

Now we are going to create toolbarView and contentView. We begin by adding these views to the module "ModuleA".

  * Right click on the project and add user control and name it "ToolbarA", delete the grid from the XAML and add a button instead.
  * Add another user control and name it "ContentA" and add a TextBlock to the XAML grid.
  * We are not using any type of pattern to create these Views. Not in this section anyway.
  * Next we register our Views with our Container.
  * Open ModulAModule, in the Initialize method we need to register our views with our container.
  * Add a reference to Microsoft.Practices.Unity.dll
  * Add the namespace using Microsoft.Practices.unity
  * Add a private variable of IUnityContainer to the class.
  * Now create a public constructor that takes in IUnityContainer parameter and let the class variable of IUnityContainer take the value of the constructor variable.
  * Now we can register our views with the container in the Initialize method.

```
using Microsoft.Practices.Unity;

namespace ModuleA
{
  public class ModuleAModule : IModule
  {
    IUnityContainer _container;
    public ModuleAModule(IUnityContainer container)
    {
      _container = container;
    }

    public void Initialize()
    {
      _container.RegisterType<ToolbarA>();
      _container.RegisterType<ContentA>();
    }
  }
}
```

Whats happening here is when the module is created, Prism recocnizes that we are asking for IUnityContainer and provides us with one, once we have that container the Initialize mothod is going to be called and we start registering our Types with the container, once the types are registered with our container we can start using them with our module.


---

## Demo Creating a View (MVVM) ##

Now we are going to change the views in this module to use the MVVM design pattern.

  * Add a new Interface to the Demo.Infrastructure and name it IView.
  * Add another Interface to Demo.Infrastructure and name it IViewModel
  * Make sure they are both public
  * IView is going to have IViewModel instance property
  * IViewModel is going to have IView property, here the reference is to an interface not directly to the view
  * Normally we would create a base class but we are going to skip that step right now
  * Add a new interface to the ModuleA project and name it IContentAView
  * Add another interface to the ModuleA project and name it IContentViewViewModel
  * make them public
  * Add reference to the Demo.Infrastructure project and the namespace
  * Now let the IContentAView inherit the IView
  * Open the ContentA.xaml.cs code behind file and let it inherit the IContentAView and implement it
```
public Demo.Infrastructure.IViewModel ViewModel
{
  get
  {
    return (IContentAViewViewModel)DataContext;
  }
  set
  {
    DataContext = value;
  }
}
```

  * open IContentViewViewModel and let it inherit the IViewModel and implement it
  * Now lets create our ViewModel add a new class and name it ContentAViewViewModel, make sure its public and let it implement the IContentAViewViewModel
  * Now we create a constructor
```
public Demo.Infrastructure.IView View { get; set; }

public ContentAViewViewModel(IContentAView view)
{
  View = view;
  View.ViewModel = this;
}
```


So what's going to happen is when this ViewModel is resolved for our container it is going to see that we are asking for a view of Type IContentAView, when we get the view we are going to set the ViewModel to the current instance and when that happens the setter on the ViewModel property of the ContentA view will set the DataContext to that instance of the ViewModel, that is what binds the View and the ViewModel. But we have to register our types with our container.

Open ModuleAModule class and change the initialize method
```
_container.RegisterType<IContentAView, ContentA>();
_container.RegisterType<IContentAViewViewModel, ContentAViewViewModel>();
```

So whenever we ask for IContentAViewViewModel the container will resolve an instance of ContentAViewViewModel for us


---

## Demo Creating a View (MVVM) - View First ##

In the previous demo we implemented the MVVM pattern in our Prism application using a ViewModel first approach, that is where the ViewModel is responsible for instantiating the View. Now lets take a look at more common View first approach that is when the View is responsible for instantiating the ViewModel.

  * Add a new interface to Demo.Infrastructure and name it IViewModel and make it public.
  * Add a new interface to Demo.Infrastructure and name it IView and make it public
  * IView should have IViewModel property
  * But the IViewModel will not have IView property.
  * Add a new interface to ModelA Project and name it IContentAViewViewModel make it public and implement IViewModel so we need a reference to Demo.Infrastructure and a using namespace.
  * Add a new class to ModelA and name it ContentAViewViewModel, should be public and implement IContentAViewViewModel
  * Add a constructor to the class
  * Open the code behind for the ContentA view and implement the IView interface
```
public IViewModel ViewModel
{
  get
  {
    return (IViewModel)DataContext;
  }
  set
  {
    DataContext = value;
  }
}
```
  * Add a parameter of the type IContentAViewViewModel to the constructor and set the ViewModel to that parameter.

So what's going to happen is when we create an instance of this View, the view will create an instance of the IContentAViewViewModel and when that instance is created we are assigning that instance to the ViewModel property which is setting the DataContext to that instance. Thats where the binding between the ViewModel and the View is occurring

  * Lastly we need to register our types
```
_container.RegisterType<ToolbarA>();
_container.RegisterType<ContentA>();
_container.RegisterType<IContentAViewViewModel, ContentAViewViewModel>();
```

In the implementation we don't need to register our interface for the views.


---

## View Composition ##
  * Constructing of a view - simply the process of constructing a view
  * Made up of many visual elements - made up of many visual elements that are loosely coupled and are often contained within the module. As these elements are created and loaded they will be displayed in regions.
  * Displayed in Regions - which is normally hosted by the Shell and you can create and display these elements either automatically thru discovery or programmatically trug View injection
  * View Discovery
  * View Injection


---

## View Discovery ##

  * Views added automatically - With view discovery views are added to the region automatically. To enable view discovery we need to setup relationship in the region view registry between a region name and the type of view. To do this we call the RegionManager.RegisterViewWithRegion(name, type) pass in the name of the region and pass in the type.
  * RegionManager.RegisterViewWithRegion(name, type) -  typically this is done when the module initializes or when a user performs an action.
  * Region looks for view types - when the region is created it looks for all the view types that are associated with that region and automatically instantiates and displays thous views.
  * No explicit control -  Side effect of this behaviour it we don't have explicit control of when to load and display a view inside a region


---

## Demo View Discovery ##

How to implement View discovery:
  * Open ModuleAModule class and we need to get a reference to the regionManager so we add an IRegionManager parameter to the constructor
  * add a private class variable of the type IRegionManager
  * And the class variable takes the value of the constructor variable
  * now we enable view discovery for the toolbarView
```
_container.RegisterType<ToolbarA>();
_container.RegisterType<IContentAView, ContentA>();
_container.RegisterType<IContentAViewViewModel, ContentAViewViewModel>();

_manager.RegisterViewWithRegion(RegionName.ToolbarRegion, typeof(ToolbarA));
```

Now when ever the region is created the region will automatically initialize and instantiate our Toolbar item. As you can see we have very limited control on how and when the toolbar is instantiated and displayed.


---

## View Injection ##

  * View added programmatically - with the view injection views are added programmatically. This can also be done when a module initializes or as a result of user action. We can achieve view injection couple of different ways.
  * RegionManager.Region["Name"].Add(view, name) -  ["Name"] is the index of region name and pass in a view and the name of the view.
  * IRegion.Add(view, name) - Or we can get an instance of the IRegion from the RegionManager and work with the region directly.
  * Activate/Deactivate - when adding views programmatically we may need to activate and deactivate views. For example, if we have a content control as a region which already contains a view and we add a new view to it we will need to deactivate the current view before the newly injected view is shown.

  * More control - As you can see view injection gives you more control over when views are loaded and displayed, we even have the ability to remove views from the region.

  * Can't add View to Region that hasn't been created - keep in mind that we can't add a view to a region that hasn't been created, with view discovery a view is automatically created when a region is created, but with view injection we have to be more aware of what regions have been created before we try to inject our view into it.


---

## Demo View Injection ##

View injection gives us much more control on how and when views are created and displayed.

  * Open ModuleAModule class

```
public void Initialize()
{
  _container.RegionType<ToolbarA>();
  _container.RegionType<IContentAView, ContentA>();
  _container.RegionType<IContentAViewViewModel, ContentAViewViewModel>();

  _manager.RegisterViewWithRegion(RegionName.ToolbarRegion, typeof(ToolbarA));

  // Now lets create a view injection
  // Get an instance of the viewModel
  var vm = _container.Resolve<IContentAViewViewModel>();
  // now we use the viewModel to inject the view into the contentRegion
  _manager.Regions[RegionNames.ContentRegion].Add(vm.View);
  IRegion region = _manager.Regions[RegionName.ContentRegion];
  region.add (vm.View);
  // And with the instance of the region we can add View
  // and Activate the view, getting the activeViews, Behaviors, Deactivation. So we have much more control over the region and the views with in that region
}
```

  * Open IContentViewViewModel interface and add a string property named Message
  * Open ContentAViewViewModel and implement that change. Ideally we want our ViewModels to implement the INotifyPropertyChange interface and on the setter call the IPropertyChangeEvent.
  * Open the ContentA.XAML and set the text to bind to the message.
```
<TextBlock Text="{Binding Message}" FontSize="38" />
```
  * Now in the ModuleAModule we can sett the ViewModel Message

```
var vm = _container.Resolve<IContentAViewViewModel>();
vm.Message = "First View";
IRegion region = _manager.Regions[RegionNames.ContentRegion];
region.Add(vm.View);
```

  * Now lets create another View and add it to the same region
```
var vm2 = _container.Resolve<IContentAViewViewModel>();
vm2.Message = "Second View";

region.Deactivate(vm.View);
region.add(vm2.View);
```