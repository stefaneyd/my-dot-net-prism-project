# 8. View-Based Navigation #

In this module we are going to learn how to implement View-Based Navigation. In Prism 4 regions have been extended to provide view-based navigation services.

  * Basic Region Navigation
  * View and ViewModel participation
  * Passing Parameters - between the view being navigated from to the view being navigated to.
  * Navigating to Existing Views
  * Confirming or Canceling Navigation
  * Navigation Journal - to implement custom Go Back or Go Forward type navigation


---

## Basic Region Navigation ##

Region navigation means that a view with in a region is replaced with another view.
  * view replaces view - this could be a new instance of a view or existing view
  * IVavigateAsyncRequestNavigate - you initiate navigation by calling the RequestNavigate method that is defined by the INavigateAsync interface but despite its name it is not async.
    * RequestNavigate
      * Region
```
IRegion region = ...;
region.RequestNavigate(New Uri("MyView", UriKind.Relative));
```
      * RegionManager
```
IRegionManager regionManager = ...;
regionManager.RequestNavigate(RegionNames.ContentRegion, new Uri("MyView", UriKind.Relative));
```
  * Based on URI's - Views are identified by URI's by default this refers to the name of the view to be navigated to.
  * Type must register as Object - we also have to register our view with the container differently than you would expect.
    * Register Types as Object
      * Standard registration
```
Container.RegisterType<HomeView>("HomeView");

Container.RegisterType<IHomeView, HomeView>("Homeview");
```
      * Navigation registration
```
Container.RegisterType<object, HomeView>("HomeView");

Container.RegisterType(typeof(object>, typeof(HomeView), "HomeView");
```
  * View or ViewModel first

Basic Region Navigation - cont.
  * Navigation Callback
```
private void Navigate(string navigatePath)
{
  RegionManager.RequestNavigate(Region.Names.ContentRegion, navigatePath, NavigateionCompleted);
}

private void NavigateionCompleted(NavigationResult result)
{
  ...
}
```


---

## Demo Basic Region Navigation ##

In this demo we have an application that we want to add View-Based Navigation to. The application has two modules ModuleA and ModuleB each module will have a button, this button will be used to navigate to the modules view and the module has a view which simply displays the name of the current view. Now lets look at the shell, there we have two regions, we have StackPanel as our toolbar region and a ContentControl as our content region where the views will be injected into


**ViewAButton**
```
<UserControl x:Class="ModuleA.ViewAButton"
	     xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
	     xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
	     xmlns:views="clr-namespace:ModuleA"
	     xmlns:infCommands="clr-namespace:Demo.Infrastructure;assebly=Demo.Infrastructure">
  <Grid Margin="5">
    <Button>Navigate to View A</Button>
  </Grid>
</UserControl>
```

**ViewA**
```
<UserControl x:Class="ModuleA.ViewA"
	     xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
	     xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
	     xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
	     xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
	     mc:Ignorable="d"
	     d:DesignHeight="300" d:DesignWidth="300">
  <Grid>
    <TextBlock Text="ViewA" FontSize="48" HorizontalAlignment="Center" VerticalAlignment="Center">ViewA</TextBlock>
  </Grid>
</UserControl>
```

**Shell**
```
<Window x:Class="Demo.Shell"
	xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
	xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
	xmlns:prism="http://www.codeplex.com/prism"
	xmlns.inf="clr-namespace:Demo.Infrastructure;assembly=Demo.Infrastrcture"
	Title="Shell">
  <DockPanel LastChldFill="True">
    <StackPanel Orientation="Horizontal" DockPanel.Dock="Top" prism:RegionManager.RegionName="{x:Static inf:RegionNames.ToolbarRegion}" Margin="5" />
    <ContentControl prism:RegionManager.RegionName="{x:Static inf:RegionNames.ContentRegion}" Margin="5" />
  </DockPanel>
</Window>
```

Now lets create a compositCommand in our infrastructure project, create a class named ApplicationCommands
```
public class ApplicationCommands
{
  public static CompositeCommand NavigateCommand = new CompositeCommand();
}
```

In this case we want the shell to be responsible for managing the navigation
```
public class ShellViewModel : viewModelBase, IShellViewModel
{
  private readonly IRegionManager _regionManager;

  public DelegateCommand<object> NavigateCommand { get; private set; }

  public ShellViewModel(IRegionManager regionManager)
  {
    _regionManager = regionManager;
    NavigateCommand = new DelegateCommand<object>(Navigate);
    ApplicationCommand.NavigateCommand.RegisterCommand(NavigateCommand);
  }

  private void Navigate(object navigatePath)
  {
    if(navigatePath != null)
      _regionManager.RequestNavigate(RegionNames.ContentRegion, navigatePath.ToString());
  }
}
```

Next we hook up our buttons to this command, so lets change the button properties in our !ViewAButton.xaml
```
<Button Command="{x:Static infCommands:ApplicationCommand.NavigateCommand}" CommandParameter="{x:Type views:ViewA}">Navigate to View A</Button>
```
And we do the same thing for ModuleB button.
We also have to change our ModuleAModule.cs RegisterTypes() method
```
Container.RegisterType<object, ViewB>(typeof(ViewB).FullName);
```

To help ease the process of navigating types for navigation we provide an extension method in the Demo.Infrastructure project.
```
public static class UnityExtensions
{
  public static void RegisterTypeForNavigation<T>(this IUnityContainer container)
  {
    container.RegisterType(typeof(Object), typeof(T), typeof(T).FullName);
  }
}
```

So we can change our RegisterTypes method in ModuleAModule class to this
```
protected override void RegisterTypes()
{
  Container.RegisterTypeForNavigation<ViewA>();
}
```

Now lets implement to let us know when the navigation process is completed. To do that we go to our ShellViewModel and change the Navigate method
```
private void Navigate(object navigatePath)
{
  if(navigatePath != null)
    _regionManager.RequestNavigate(RegionNames.ContentRegion, navigatePath.ToString(), NavigateComplete);
}

private void NavigateComplete(NavigationResult result)
{
  MesssageBox.Show(String.Format("Navigation to {0} complete.", result.Context.Uri));
}
```


---

## View and ViewModel Participation ##

View based navigation involves replacing one view with another and often is required to know when the old view is being navigated away from and the new view is being navigated to. Prism provides a way for your view and or viewModel to participate in this process and thats by implementing the INavigationAware interface.
  * INavigationAware - lets look at the definition of this interface
```
public interface INavigateionAware
{
  bool IsNavigationTarget(NavigationContext navigationContext);
  void OnNavigatedFrom(NavigationContext navigationContext);
  void OnNavigatedTo(NavigationContext navigationContext);
}
```
We see that there are three methods
  * IsNavigationTarget - which allows existing view or viewModel to determine if it can participate in the navigation request.
  * OnNavigatedFrom - which is called on the current view before the navigation takes place.
  * OnNavigatedTo - which is called after navigation is complete on the newly activated view

<img src='http://s17.postimg.org/ovez1gub3/inavigationaware.png' width='400px' />

After the new view has been navigated to, the old view is deactivated with in the region.

  * IRegionMemberLifetime - if you want to remove the view form the region completely
```
public interface IRegionMemberLifetime
{
  bool KeepAlive { get; }
}
```
If you set the KeepAlive property to false then the view is removed completely when the view is deactivated.


---

## Demo View and ViewModel Participation ##

Now that we have implemented View-Based Navigation in our application lets take it a step further. This application has added a ViewModule (!ViewAViewModel) to the views so each view has a viewModel and each viewModel is going to have a property called pageViews.
```
public class ViewAViewModel : ViewModelBase, IViewBBiewModel
{
  private int _pageViews;
  public int PageViews
  {
    get { return _pageViews; }
    set
    {
      _pageViews = value;
      OnPropertyChanged("PageViews");
    }
  }

  public ViewAViewModel()
  {

  }
}
```
What we want to do is when we navigate we want to update the count on how many times the page has been navigated to. So now we need our view and viewModels to participate in the navigate process.

We begin by implementing the INavigationAware interface to our viewModel
```
public class ViewAViewModel : ViewModelBase, IViewAViewModel, INavigationAware
{
  private int _pageViews;
  public int PageViews
  {
    get { return _pageViews; }
    set
    {
      _pageViews = value;
      OnPropertyChanged("PageViews");
    }
  }

  public ViewAViewModel()
  {

  }

  // INavigationAware implemnts these three methods
  public bool IsNavigationTarget(NavigationContext navigationContext)
  {
    return true;
  }

  public void OnNavigatedFrom(NavigationContext navigationContext)
  {

  }

  public void OnNavigatedTo(NavigationContext navigationContext)
  {
    PageViews++;
  }
}
```

Participation is not limited to viewModel we can also implement the INavigationAware on the view itself.
```
public partial class ViewA : UserControl, IView, INavigationAware
{
  public ViewA(IViewAViewModel viewModel)
  {
    InitializeComponent();
    ViewModel = viewModel;
  }

  public IViewModel ViewModel
  {
    get { return (IViewModel)DataContext; }
    set { DataContext = value; }
  }

  public bool IsNavigationTarget(NavigationContext navigationContext)
  {
    return true;
  }

  public void OnNavigatedFrom(NavigationContext navigationContext)
  {

  }

  public void OnNavigatedTo(NavigationContext navigationContext)
  {

  }
}
```
Now lets change the viewModel ViewBViewModel and implement the IRegionMemberLifetime
```
public class ViewBViewModel : ViewModelBase, IViewBViewModel, INavigationAware, IRegionMemberLifetime
{
  private int _pageViews;
  public int PageViews
  {
    get { return _pageViews; }
    set
    {
      _pageViews = value;
      OnPropertyChanged("PageViews");
    }
  }

  public ViewBViewModel()
  {

  }

  // INavigationAware implemnts these three methods
  public bool IsNavigationTarget(NavigationContext navigationContext)
  {
    return true;
  }

  public void OnNavigatedFrom(NavigationContext navigationContext)
  {

  }

  public void OnNavigatedTo(NavigationContext navigationContext)
  {
    PageViews++;
  }

  // IRegionMemberLifetime Property
  public bool KeepAlive
  {
    get { return false; }
  }
}
```
So each time we navigate from ViewB it is deactivated and deleted, so when we navigate to ViewB we create a new instance of the view.


---

## Passing Parameters ##

Sometimes it is necessary to pass parameters from the view that is being navigated from to the view that is being navigated to.
  * Append to navigation URI - this can be done by appending your parameters to the navigation URI that is passed to the request navigate method.
  * UriQuery - Prism provides the UriQuery class to help specify and retrieve parameters, for each parameter the UriQuery object maintains a list of named value pairs. The name being the name of the parameter and the value being the value of the parameter.
  * Accessed from INavigationAware methods - you gain access to these parameters from with in the INavigationAware methods.
  * NavigationContext
    * NavigationService
    * URI
    * Parameters


---

## Demo Passing Parameters ##

Lets take a look at how to pass parameters when navigating from view to view.
We still have two modules ModuleA and MOduleB, but ModuleA has some extra views.
```
<UserControl x:Class="ModuleA.ViewA"
	     xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
	     xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
	     xmlns:extToolkit="http://schemas.microsoft.com/winfx/2006/presentation/toolkit/extended">

  <UserControl.Resources>
    <DataTemplate x:Key="PersonItemTemplate">
      <StackPanel Margin="5">
        <TextBlock FontWeight="Bold" FontSize="18">
          <TextBlock.Text>
            <MultiBinding StringFormat="{}{0}, {1}>
              <Binding Path="LastName" />
              <Binding Path="FirstName" />
            </MultiBinding>
          </TextBlock.Text>
        </TextBlock>
        <TextBlock Text="{Binding Email}" FontSize="12" FontStyle="Italic" />
      </StackPanel>
    </DateTemplate>
  </UserControl.Resources>

  <Grid>
    <Grid.RowDefinitions>
      <RowDefinition Height="Auto" />
      <RowDefinition Height="*" />
    </Grid.RowDefinitions>

    <Button Command="{Binding EmailCommand}" CommandParameter="{Binding SelectedItem, Path=SelectedPerson} />
    
    <extToolkit:BusyIndicator Grid.Row="1" IsBusy="{Binding IsBusy}" BusyContent="Loading....">
      <ListBox x:Name="listOfPeople"
		Itemsource="{Binding People}"
		ItemTemplate="{StaticResource PersonItemTemplate}">
      </ListBox>
    </extToolkit:BusyIndicator>
  </Grid>
</UserControl>
```

And the EmailView
```
<Usercontrol x:Class="ModuleA.EmailView"
	     xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"	
	     xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml">
  <Grid>
    <Grid.RowDefinitions>
      <RowDefinition Height="Auto" />
      <RowDefinition Height="Auto" />
      <RowDefinition Height="*" />
      <RowDefinition Height="Auto" />
    </Grid.RowDefinitions>

  #Missing Control Layout and binding
  </Grid>
</UserControl>
```

ViewA also has a viewModel
```
public class ViewAViewModel : viewModelBase, IViewAViewModel
{
  private readonly IregionManager _regionManager;
  private readonly IPersonService _personService;

  public ViewAViewModel(IRegionManager regionManager, IPersonService personService)
  {
    _regionManager = regionManager;
    _personService = personService;
    EmailCommand = new DelegateCommand<Person>(Email);
    LoadPeople();
  }

  private ObservableCollection<Person> _People;
  public ObservableCollection<Person> People
  {
    get { return _People; }
    set
    {
      _People = value;
      OnPropertyChanged("People");
    }
  }

  public DelegateCommand<Person> EmailCommand { get; private set; }

  private void Email(Person person)
  {
    if(person != null)
    {
      var uri = new Uri(typeof(Emailview).FullName, UriKind.Relative);
      _regionManager.RequestNavigate(RegionNames.ContentRegion, uri);
    }
  }

  private void LoadPeople()
  {
    IsBusy = true;
    _personService.GetPeopleAsync((sender, result) =>
    {
      People = new ObservableCollection<Person>(result.Object);
      IsBusy = false;
    });
  }
}
```

Now what we want to do is pass the email address of the selected person to the email view. We do that by changing the Email command, and we use the uriQuary.
```
private void Email(Person person)
  {
    if(person != null)
    {
      var uriQuery = new UriQuery();
      uriQuery.Add("To", person.Email);

      var uri = new Uri(typeof(Emailview).FullName + uriQuery, UriKind.Relative);
      _regionManager.RequestNavigate(RegionNames.ContentRegion, uri);
    }
  }
```
Now we go to our EmailViewModel and change that
```
public class EmailViewViewModel : ViewModelBase, IEmailViewViewModel, INavigationAware
{
  // Contructors

  // Properties
  private string to;
  public string To
  {
    get { return _to; }
    set
    {
      _to = value;
      OnPropertyChanged("To");
    }
  }

  private string _subject;
  public string Subject
  {
    get { return _subject; }
    set
    {
      _subject = value;
      OnPropertyChanged("Subject");
    }
  }

  private string _body;
  public string Body
  {
    get { return _body; }
    set
    {
      _body = value;
      OnPropertyChanged("Body");
    }
  }

  public bool IsNavigationTarget(NavigationContext navigationContext)
  {
    return true;
  }

  public void OnNavigatedFrom(NavigationContext navigationContext)
  {

  }

  public void OnNavigatedTo(NavigateionContext navigationContext)
  {
    var toAddress = navigationContext.Parameters["To"];
    if(!string.IsNullOrWhiteSpace(toAddress))
      To = toAddress;
    else
      To = "Email not provided";
  }
}
```


---

## Navigate to Existing views ##

Sometimes it is desirable to reuse, update or activate an existing view instead of creating an instance of a new view on every navigation operation. To accomplish this task we need to know two things.
  * INavigationAware.IsNavigationTarget
  * Parameters - The parameters will be used with in the IsNavigationTarget method to help compare state or data which will help determine result of the method.
```
public bool IsNavigationTarget(NavigationContext navigationContext)
{
  string id = navigationContext.Parameters["Id"];
  return this.currentCustomer.Id.Equals(id);
}
```


---

## Demo Navigating to Existing views ##

Now we look at how to implement navigation to existing views. And we do that by changing the IsNavigationTarget method in EmailViewViewModel class.
```
public bool IsNavigationTarget(NavigationContext navigationContext)
{
  var toAddress = navigationContext.Parameters["To"];
  if(To == toAddress)
    return true;
  else
    return false;
}
```
Now we are creating a new instance of the view if we have not been there before.


---

## Confirming or Canceling Navigation ##

We need to be able to confirm or cancel navigation request, for example we may want to prompt the user to save data when a navigation operation is initiated and block the navigation until the user responds to the save dialog. And Prism supports this functionality with the use of IConfirmNavigationRequest interface.
  * IConfirmNavigationRequest - by implementing this interface on either the view and/or the viewModel you enable them to participate in the navigation process. And it provides a mechanism that interacts with the user that will allow the user to confirm or cancel the navigation request.
```
public interface IConfirmNavigationRequest : INavigationAware
{
  void ConfirmNavigationRequest(NavigationContext navigationContext, Action<bool> continuationCallback);
}
```

<img src='http://s12.postimg.org/p2oyl53al/iconfirmnavigationrequestprocess.png' width='600px' />

Example
```
public void ConfirmNavigationRequest(NavigationContext navigationContext, Action<bool> continuationCallback)
{
  //by default we allow navigation
  bool result = true;

  //prompt user for desired action
  MessageBoxResult messageResult = MessageBox.Show("Save", "Save Object?", MessageBoxButton.YesNoCancel);
  
  if(messageResult == MessageBoxResult.Yes)
    CurrentCustomer.Save(); //perform save
  else if(messageResult == MessageBoxResult.Cancel)
    result = false; //don't allow navigation

  continuationCallback(result);
}
```


---

## Demo Confirming or Canceling Navigation ##

When you implement navigation in your application it's often desirable to have the ability to prompt a user for some type of confirmation to continue navigation or to cancel navigation. And in this application we are going to implement that functionality.

Open ViewAViewModel.cs and implement the IConfirmNavigationRequest interface
```
public class ViewAViewModel : ViewModelBase, IViewAViewModel, IConfirmNavigationRequest
{
  private int _pageViews;
  public int PageViews
  {
    get { return _pageViews; }
    set
    {
      _pageViews = value;
      OnPropertyChanged("PageViews");
    }
  }

  public ViewAViewModel()
  {

  }

  public void ConfirmNavigationRequest(NavigationContext navigationContext, Action<bool> continuationCallback
  {
    bool result = true;

    if(MessageBox.Show("Do you want to navigate?", "Navigate?", MessageBoxButton.YesNo) == MessageBoxResult.No)
      result = false;

    continuationCallback(result);
  }

  public bool IsNavigationTarget(NavigationContext navigationContext)
  {
    return true;
  }

  public void OnNavigatedFrom(NavigationContext navigationContext)
  {

  }

  public void OnNavigatedTo(NavigationContext navigationContext)
  {
    PageViews++;
  }
}
```


---

## Navigation Journal ##

Prism provides a journal that allows you to programmatically navigate forward or backward with in a region it is:
  * Stack-Based
  * Works Only with Region Navigation Services - it can only be used for region navigation operations that have been managed by the region navigation services. If you use view injection or viewdiscovery the navigation journal will not be updated.
  * Accessed from NavigationService.Journal property - the navigationservice which is responsible for managing the navigation operations within a region provides access to the journal through the journal property.
  * NavigationService accessed from NavigationContext - the NavigationContext is passed as a parameter to all the methods defined in the INavigationAware interface. Most likely you will be obtaining and storing a reference to the navigationService in the OnNavigateTo method.
  * the journal provides a number methods for programmatic access to navigating forward and backwards in the region such as GoBack(),CanGoBack(), GoForward(), CanGoForward()
  * GoBack(), CanGoBack()
  * GoForward(), CanGoForward()


---

## Demo Using the Navigation Journal ##

In this demo we are going to see how to gain access to the Navigation Journal which will provide us programmatic access for navigating forward or backwards thru our regions views.

Open up EmailviewviewModel in ModuleA
```
public class EmailViewViewModel : ViewModelBase, IEmailViewViewModel, INavigationAware
{
  private IRegionNavigationJournal _journal;
  //Constructors
  public EmailViewViewModel()
  {
    CancelCommand = new DelegateCommand(Cancel);
  }

  private void Cancel()
  {
    _journal.GoBack();
  }

  //Properties
  
  public DelegateCommand CancelCommand { get; private set; }
  
  private string to;
  public string To
  {
    get { return _to; }
    set
    {
      _to = value;
      OnPropertyChanged("To");
    }
   }

  private string _subject;
  public string Subject
  {
    get { return _subject; }
    set
    {
      _subject = value;
      OnPropertyChanged("Subject");
    }
  }

  private string _body;
  public string Body
  {
    get { return _body; }
    set
    {
      _body = value;
      OnPropertyChanged("Body");
    }
  }

  public bool IsNavigationTarget(NavigationContext navigationContext)
  {

  }

  public void OnNavigateFrom(NavigationContext navigationContext)
  {

  }

  public void OnNavigatedTo(NavigationContext navigationContext)
  {
    _journal = navigationContext.NavigationService.Journal;

    var toAddress = navigationContext["To"];
    if(!string.IsNullOrWhiteSpace(toAddress))
      To = toAddress;
    else
      To = "Email not provided;
  }
}

```
Now open EmailView.xaml and find the Cancel button
```
<Button Command="{Binding CancelCommand}" Width="60">Cancel</Button>
```