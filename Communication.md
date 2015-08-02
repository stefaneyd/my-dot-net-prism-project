# 6. Communication #
When building a Prism application we divide the application into individual, loosely coupled modules and sometimes it is necessary to communicate between those modules. To send data, user service or correspond to an action.
In this section we go over how to communicate between modules and still maintain loose coupling.
When we need to communicate between modules there are a couple of approaches that we can take:
  * Commanding - supports delegate commands, composite commands
  * Event Aggregation -
  * Shared Services -
  * Region Context -


---

## Commanding Overview ##

The most common method of communication in a Prism is Commanding.
What is Commanding:
  * Binds a UI gesture to action - such as Button Click to action that needs to be performed.
  * Execute - each command has an execute method, this method is called when the method is invoked.
  * CanExcute - command has also a CanExecute method and this method determines whether or not the command can be executed, and the element that is bound to the command will be either enabled or disabled based on the result of the CanExcute method.
  * RoutedCommand - The most common way of creating commands is either to use the RoutedCommand or CustomCommand. RoutedCommands deliver their command messages thru UI elements in the visual tree. That means that any element outside the tree will not receive these messages. RoutedCommands require to create the handlers in the codeBehind, which is where we don't want to be placing our logic.
  * CustomCommand - This involves creating a custom class that derives from ICommand and implements the ICommand interface. Often creating custom commands involves more work. We must provide ways for command handlers to hook up and do the routing when the command is provoked. We must also decide what criteria we will use for determining when to raise the CanExcuteChangedEvent.

In a Prism application the command handler is often a ViewModel and doesn't have any association with any element in the VisualTree. Prism provides two classes that makes commanding easier and provides more functionality:
  * DelegateCommand -  which allows you to call a delegate method when the command is executed.
  * CompositeCommand - which allows us to combine multiple commands.


---

## DelegateCommand ##

  * Uses delegates - A delegate command is a command that allows you to supply methods as delegates that will be invoked when the command is invoked.
  * Doesn't require a handler - Event handler is not required in the code behind.
  * Usually local - delegates are normally locally scoped, meaning they are created within the ViewModel, and the concerns of the delegate methods are within the context of that ViewModel.
  * DelegateCommand or DelegateCommand`<T>` - Prism provides us with two delegate commands. the difference is that the Execute and CanExcute delegate methods for the delegate command will not accept a parameter for as the delegate command of `<T>` allows us to specify the type of parameter that the Execute and CanExcute methods parameter will be.


---

## Demo Creating a DelegateCommand ##

Lets take a look at how this application looks.

  * The Shell has two regions in it, StatusBarRegion and ContentRegion.
  * The Infrastructure project has a new ViewModelBase class, it implements the INotifyPropertyChanged and the IViewModel interfaces.

```
public class : ViewModelBase : IViewModel, INotifyPropertyChanged
{
  public IView View { get; set; }

  public ViewModelBase(IView view)
  {
    View = view;
    View.ViewModel = this;
  }

  public event PropertyChangedEventHandler PropertyChanged;
  protected void OnPropertyChanged(string propertyName)
  {
    if(PropertyChanged != null)
      PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
  }
}
```

It has a basic constructor which wires up the view and the Views viewModel.

  * The application has two modules
  * the StatusBar Module
  * and the People Module - it represents anything that has to do with people or a person. It has a single view in it which is a form which will be used to edit a single instance of a person class.

```
<Grid xName="LayoutRoot" Background="White">
  <Grid.ColumnDefinitions>
    <ColumnDefinition Width="Auto" />
    <ColumnDefinition />
  </Grid.ColumnDefinitions>
  <Grid.RowDefinitions>
    <RowDefinition Height="Auto" />
    <RowDefinition Height="Auto" />
    <RowDefinition Height="Auto" />
    <RowDefinition Height="Auto" />
    <RowDefinition Height="Auto" />
  </Grid.RowDefinitions>
  
  <TextBlock Text="First Name:" Margin="5" />
  <TextBox Grid.Column="1" Text="{Binding Person.FirstName, Mode=TwoWay}" Margin="5" />
  <TextBlock Grid.Row="1" Text="Last Name:" Margin="5" />
  <TextBox Grid.Row="1" Grid.Column="1" Text="{Binding Person.LastName, Mode=TwoWay}" Margin="5" />
  <TextBlock Gfid.Row="2" Text="Age:" Margin="5" />
  <TextBox Grid.Row="2" Grid.Column="1" Text="{Binding Person.Age, Mode=TwoWay}" Margin="5" />
  <TextBlock Grid.Row="3" Text="Last Updated:" Margin="5" />
  <TextBox Grid.Row="3" Grid.Column="1" Text="{Binding Person.LastUpdatedDate, Mode=TwoWay}" Margin="5" />

  <Button Grid.Row="4" Grid.ColumnSpan="2" Content="Save" Margin"10" />
</Grid>
```

  * That person class has been defined in our Business project.

```
public class Person : INotifyPropertyChanged, IDataErrorInfo
{
  private string _firstName;
  public string FirstName
  {
    get { return _firstName; }
    set
    {
      _firtsName = value;
      OnPropertyChanged("FirstName");
    }
  }

  private string _lastName;
  public string LastName
  {
    get { return _lastName; }
    set
    {
      _lastName = value;
      OnPropertyChanged("LastName");
    }
  }
  
  private int _age;
  public int Age
  {
    get { return _age; }
    set
    {
      _age = value;
      OnPropertyChanged("Age");
    }
  }

  private DateTime? _lastUpdated;
  public DateTime LastUpdated
  {
    get { return _lastUpdated; }
    set
    {
      _lastUpdated = value;
      OnPropertyChanged("LastUpdated");
    }
  }

  public event PropertyChangedEventHandler PropertyChanged;
  protected void OnPropertyChanged(string propertyname)
  {
    if(PropertyChanged != null)
      PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
  }

  public string Error
  {
    get { return null; }
  }

  public string this[string columnName]
  {
    get
    {
      string error = null;

      switch(columnName)
      {
        case "FirstName":
          if(string.IsNullOrEmpty(_firstName))
          {
            error = "First Name required";
          }
          break;
        case "LastName":
          if(string.IsNullOrEmpty(_lastName))
          {
            error = Last Name required";
          }
          break;
        case "Age":
          if((_age < 18) || (_age > 85))
          {
            error = "Age out of range.";
          }
          break;
      }
    }
    return (error);
  }
}
```

If we run the application you can see that it is a very simple form, it has some text elements and a save button. The functionality that we want to implement for this application, is when we click the save button we want the last updated date to reflect the date when this object was saved. Lets do that now.

  * Open PersonView.XAML and change the button element
```
<Button Grid.Row="4" Grid.ColumnSpan="2" Content="Save" Margin="10" Command="{Binding SaveCommand}" />
```

  * The SaveCommand does not exist in the ViewModel so lets create it. Open the PersonViewModel.
```
public class PersonViewModel : ViewModelBase, IPersonViewModel
{
  // Here we add our DelegateCommand for the SaveCommand
  public DelegateCommand SaveCommand { get; set; }

  public PersonViewModel(IPersonView view)
	: base(view)
  {
    CreatePerson();

    // Here we create a instance of the SaveCommand
    // The first parameter is the method that will be executed when the command is invoked
    // The second parameter will be the CanExcute method
    SaveCommand = new DelegateCommand(Save, CanSave);
  }

  private void Save()
  {
    Person.LastUpdated = DateTime.Now;
  }

  private bool CanSave()
  {
    return Person.Error == null; // Can save if there are no errors
  }

  private Person _person;
  public Person Person
  {
    get { return _person; }
    set
    {
      _person = value;
      _person.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(_person_PropertyChanged);
      OnPropertyChanged("Person");
    }
  }

  private void _person_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
  {
    SaveCommand.RaiseCanExecuteChanged();
  }

  private void CreatePerson()
  {
    Person = new Person()
    {
      FirstName = "Bob",
      LastName = "Smith",
      Age = 46
    };
  }
}
```


Now we can change the DelegateCommand to DelegateCommand`<T>`
```
public DelegateCommand<object> SaveCommand { get; set; }

// In the constuctor
SaveCommand = new DelegateCommand<object>(Save, CanSave);

// Now we can change the methods
private void Save(object value)
{
  Person.LastUpdated = DateTime.Now.AddYears(Convert.toInt32(value));
}

private bool CanSave(object value)
```

We have to add a CommandParameter to the PersonView.XAML in the button attributes
```
<Button Grid.Row="4" Grid.Span="2" Content="Save" Margin="10" Command="{Binding SaveCommand}" CommandParameter="2"
```

Now when we run the application and click the button we add two years to the date. We have to have the parameter of the type object because the DelegateCommand`<T>` can't take types that are not nullable


---

## CompositeCommand ##

  * Usually global - usually globally scoped commands that exist in the infrastructure class.
  * Multiple child commands - they contain multiple child commands, for example multiple views that need to be independently saved where we have save all button.
  * Local commands are registered with command - each view would have the local command that are defined in the viewModel register with the save all compositeCommand.
  * When invoked, all child commands are invoked -  so when the save all command is invoked all registered child commands will also be invoked.
  * Supports enablement - and because compositeCommand support enablement by listening to the canExecuteChanged event of each registered child command if any call to the canExecute returns false in a child command the compositeCommand will also return false thus disabling all the invokers.


---

## Demo Creating a CompositeCommand ##

Now let's implement a CompositeCommand. First lets look at the changes we made to our application.

  * Open the Shell
```
<Window x:Class="PluralsightPrismDemo.Shell"
	 xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
	 xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
	 xmlns:prism="http://www.codeplex.com/prism"
	 xmlns:inf="clr-namespace:Demo.Infrastructure;assembly=Demo.Infrastructure"
	 Title="Shell" Height="350" Width="350">
  <Grid>
    <DockPanel LastChildFill="True">
      <ContentControl prism:RegionManager.RegionName="{x:Static inf:RegionNames.ToolbarRegion}" DockPanel.Dock="Top" Width="Auto" Height="Auto" />
      <ContentControl prism:RegionManager.RegionName="{x:Static inf:RegionNames.StatusBarRegion}" DockPanel.Dock="Bottom" Width="Auto" Height="Auto" />
      <TabControl prism:RegionManager.RegionName="{x:Static inf:RegionNames.ContentRegion}" Width="Auto" Height="Auto" HorizontalAlignment="center" />
        <TabControl.ItemContainerStyle>
          <Style TargetType="TabItem">
            <Setter Property="Header" Value="{Binding Content.DataContext.ViewName, RelativeSource={RelativeSource Self}}" />
          </Style>
        </TabControl.ItemContainerStyle>
      </TabControl>
    </DockPanel>
  </Grid>
</Window>
```

  * We added a new Toolbar Module which has a single view in it that has a single button in `<Button> Save All</Button>`

  * We have modified the PeopleModule
```
public class PeopleModule : IModule
{
  private readonly IRegionManager _regionManager;
  private readonly IUnityContainer _container;

  public PeopleModule(IUnityContainer container, IRegionManager regionManager)
  {
    this._container = container;
    this._regionManager = regionManager;
  }

  public void Initialize()
  {
    RegisterViewsAndServices();

    IRegion region = _regionManager.Regions[RegionNames.ContentRegion];

    var vm = _container.Resolve<IPeronsViewModel>();
    vm.CreatePerson("Bob", "Smith");

    region.Add(vm.View);
    region.Activate(vm.View);

    var vm2 = _container.Resolve<IPersonViewModel>();
    vm2.CreatePerson("Karl", "Sums");
    region.Add(vm2.Vew);

    var vm3 = _container.Resolve<IPersonViewModel>();
    vm3.CreatePerson("Jeff", "Lock");
    region.Add(vm3.View);
  }

  protected void RegisterViewsAndServices()
  {
    _container.RegisterType<IPersonViewModel, PersonViewModel>();
    _container.RegisterType<IPersonView, PersonView>();
  }
}
```

  * Now we implement the Save all function.
  * Create a new GlobalCommands class in our Infrastructure project
```
public static class GlobalCommands
{
  public static CompositeCommand SaveAllCommand = new CompositeCommand();
}
```
  * Now open up the ToolbarView.xaml in our Toolbar Module and add a namespace `xmlns:inf="clr-namespace:Demo.Infrastructure;assembly=Demo.Infrastructure"`
  * Now set the command for the button
```
<Button Command="{x:Static inf:GlobalCommands.SaveAllCommand}">Save All</Button>
```
  * Now we have to register our ViewModel for that CompositeCommand.
  * Open PersonViewModel in the People project and change the constructor to register the SaveCommand with the CompositeCommand
```
public PersonViewModel(IPersonView view)
	: base(view)
{
  SaveCommand = new DelegateCommand(Save, CanSave);

  GlobalCommands.SaveAllCommand.RegisterCommand(SaveCommand);
}
```


---

## Event Aggregation ##

  * Loosely coupled event based communication - The basic concept of Event Aggregation is that it is a loosely coupled event based communication mechanism.
  * Publisher and Subscribers - it is made up of publishers and subscribers. Publisher will execute an event and a subscriber will listen for that event
  * Manages memory related to eventing - memory leaks are reduced because subscribers don't need a strong reference to the publishers. Therefore we don't have to manually unsubscribe our subscribers. For further information study the Event Aggregation design pattern.

Prism has a build in support for EventAggregator by providing a core service which can be retrieved thru the IEventAggregator
  * IEventAggregator - the event aggregator is responsible for locating and building events and for keeping the  collection of events in the system. Both Publishers and Subscribers need an instance of the EventAggregator and to get that instance just request it from the container.
  * Multicast Pub/Sub -  It provides multicast pub/sub functionality. This means that there can be multiple publishers that raise the same event, and there can be multiple subscribers listening to that same event.
  * Events are typed events deriving from EventBase - Events created by the Prism library are typed events. This means we can take advantage of the compile type checking, to check for error before we run the application.
  * CompositePresentationEvent`<T>` -  this is the only class that Prism provides us with for creating events. This class maintains a list of subscribers and handles events dispatching to the subscribers.
  * `<T>` is the required Payload -  the payload is what we want to send to the subscriber when the event is published.

Lets look at some of the services that the EventAggregator provides.
  * Publish events - we can publish event, publishers raise an event by retrieving the event from the eventAggregator and then calling the publish method.
  * Subscribe to events - subscribers can also register with an event using one of the subscribe method overloads available on the composite presentation event class, and there are a couple of ways to subscribe to an event.
  * Subscribe using a strong reference - keepSubscriberReferenceAlive
  * Event filtering
  * Unsubscribe from events


---

## Demo Using IEventAggregator ##

We are going to change the application so that whenever a person is saved we are going send that persons name to the status bar, to update the UI so the user can see what action just occurred. To do this we are going to use the Event Aggregator.

  * Lets begin by adding a new class to our Infrastructure project. Name it Events, delete the default class and create this one.
```
public class PersonUpdatedEvent : CompositePresentationEvent<string> { }
```
The payload is going to be a string, we're simply going to pass the persons name. Now that we have declared the event we need to publish the event so we need to open up our viewModel the PersonViewModel and add an IEventAggregator Property or a member.
```
IEventAggregator _eventAggregator;
```
We also have to pass IEventAggregator thru the constructor.
```
public PersonViewModel(IPersonView view, IEventAggregator eventaggregator)
	: base(view)
{
  _eventAggregator = eventaggregator;
  Savecommand = new DelegateCommand(Save, CanSave);

  GlobalCommands.SaveAllCommand.registerCommand(SaveCommand);
}
```
Now what needs to happen is when a person is saved we need to send a message with the person name.
```
private void Save()
{
  Person.LastUpdated = Datetime.Now;
  _eventAggregator.getEvent<PersonUpdatedEvent>().Publish(String.Format("{0}, {1}", Person.LastName, Person.FirstName));
}
```
Now that we have published the event we need a subscriber to that event. So we go to our StatusBar module project and open the viewModel StatusBarViewModel.
```
public class StatusBarViewModel : ViewModelBase, IStatusBarViewModel
{
  IEventAggregator _eventAggregator;
  
  public StatusBarviewModel(IStatusBarview view, IEventAggregator eventAggregator)
	: base(view)
  {
    _eventAggregator = eventAggregator;
    _eventAggregator.GetEvent<PersonUpdatedEvent>().Subscribe(PersonUpdated);
  }

  private string _message = "Ready";
  public string Message
  {
    get { return _message; }
    set
    {
      _message = value;
      OnPropertyChanged("Message");
    }
  }

  private void PersonUpdated(string obj)
  {
    Message = String.Format("{0} was updated.", obj);
  }
}
```


---

## Shared Services ##

  * Custom service - is a custom class that provides functionality to other modules in a loosely coupled way. This is normally done thru an interface and is often a singleton. these services normally exist in a separate module and when these modules load we can register these services with the Service Locator.
  * Register with a Service Locator
  * Common Interface - when we register these services we use a common interface. This allows other modules to use our service without requiring a static reference to the service module.
  * Concrete implementation doesn't have to be shared - nice side effect of using a common interface is that our concrete implementations don't have to be shared. Our common interface can exist in our infrastructure class while the concrete implementation will exist in the service module, and registering our service as a shared service is as easy as specifying a ContainerControlledLifetimeManager when we register our types.
  * ContainerControlledLifetimeManager


---

## Demo Creating a Shared Service ##

Let look at how we would implement shared services

  * Open PersonViewModel in our People project and take a look at the Save() function
  * Lets create a shared service that will handle all the saving of our person objects
  * Create a Service module project
```
public class ServicesModule : IModule
{
  IUnityContainer _container;

  public ServicesModule(IUnityContainer container)
  {
    _container = container;
  }

  public void Initialize()
  {
  }
}
```
  * Lets add an interface to our Infrastructure project and name it IPersonRepository.
```
public interface IPersonRepository
{
  int SavePerson(Person person);
}
```
  * Now Create a class in our Service project named PersonRepository
```
public class PersonRepository : IPersonRepository
{
  int count = 0;

  public int SavePerson(Business.Person person)
  {
    count++;
    person.LastUpdated = DateTime.Now;
    return count;
  }
}
```
  * The next step is to register this with our container as a shared service.
```
public class ServicesModule: IModule
{
  IUnityContainer _container;

  public ServicesModule(IUnityContainer container)
  {
    _container = container;
  }

  public void Initialize()
  {
    // To register this as a singleton we use new ContainerControlledLifetimeManager()
    // This tells the container giv me the same instance every time
    _container.RegisterType<IPersonRepository, PersonRepository>(new ContainerControlledLifetimeManager());
  }
}
```
  * next step is to go to our viewModel PersonViewModel and replace the Save() method with our repository.
```
IPersonRepository _repository;

public PersonViewModel(IPersonView view, IPersonRepository repository)
	: base(view)
{
  _repository = repository;
  SaveCommand = new DelegateCommand(Save, CanSave);
}

private void Save()
{
  int count = _repository.SavePerson(Person);
  MessageBox.Show(count.toString());
}
```


---

## Region Context ##

There may be some scenarios where we might want to share contextual information between a view that is hosting a region and a view that is inside that region.
  * hare an object between the region host and views inside the region - which is a technique that can be used to share context between a parent view and child views that are hosted in a region.
  * Expose in XAML - we can expose this object either in XAML or
  * Expose in Code - or code
  * Only supports DependencyObjects - Prism only supports consuming the region context from a view inside a region only if that view is a DependencyObject. If we are using dataTemplates that are not DependencyObject we may need to create a custom region behavior to forward to the region context in order to view your objects.
  * Don't use DataContext - the DataContext is commonly used to bind the viewModel to a view. so this means that your dataContext is now storing the views entire viewModel


---

## Demo Using the RegionContext ##


  * Lets look at how we use the region context, our People module is a little different than what we have seen in the past.
  * We have a PeopleView
```
<UserControl x:Class="Demo.People.PeopleView"
	     xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
	     xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
	     xmlns:prism="http://www.codeplex.com/prism">
  <Grid x:Name="LayoutRoot" Background="White" Margin="10">
    <Grid.RowDefinitions>
      <RowDefinition Height="100" />
      <RowDefinition Height="Auto" />
    </Grid.RowDefinitions>

    <ListBox x:Name="_listOfPeople" Itemsource="{Binding People}" />
    <ContentControl Grid.row="1" Margin="10" prism:RegionManager.RegionName="PersonDetailsRegion" />
  </Grid>
</UserControl>
```
  * And then we have the PersonDetailsView
```
<UserControl x:Class="PluralsightPrismDemo.People.PersonDetailsView"
	     xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
	     xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml">
  <Grid x:Name="LayoutRoot" Background="White">
    <Grid.ColumnDefinitions>
      <ColumnDefinition Whidth="Auto" />
      <ColumnDefinition />
    </Grid.ColumnDefintions>
    <Grid.RowDefinitions>
      <RowDefinition Height="Auto" />
      <RowDefinition Height="Auto" />
      <RowDefinition Height="Auto" />
      <RowDefinition Height="Auto" />
    </Grid.RowDefinitions>
    </Grid.ColumnDefinitions>
    
    <TextBlock Text="First Name:" Margion="5" />
    <TextBox Grid.Column="1" Margin="5" Text="{Binding SelectedPerson.FirstName, Mode=TwoWay}" />

    <TextBlock Grid.Row="1" Text="Last Name:" Margin="5" />
    <TextBox Grid.Row="1" Grid.Column="1" Margin="5" Text="{Binding SelectedPerson.FirstName, Mode=TwoWay}" />

    <TextBlock Grid.Row="2" Text="Age:" Margin="5" />
    <TextBox Grid.Row="2" Grid.Column="1" Margin="5" Text="{Binding SelectedPerson.Age, Mode=TwoWay}" />

    <TextBlock Grid.Row="3" Text="Last Updated:" Margin="5" />
    <TextBox Grid.Row="3" Grid.Column="1" Margin="5" Text="{Binding SelectedPerson.LastUpdate, Mode=TwoWay}" />
  </Grid>
</UserControl>
```
  * This detail View is being resolved a little differently from the people view, this is using view first approach. The view is going to resolve the viewModel.
```
public partial class PersonDetailsView : UserControl, IPersonDetailsView
{
  public PersonDetailsView(IPersonDetailsViewModel viewModel)
  {
    InitializeComponent();
    
    ViewModel = viewModel;
    ViewModel.View = this;
  }

  public IViewModel ViewModel
  {
    get { return (IViewModel)DataContext; }
    set { Datacontext = value; }
  }
}
```
  * Now lets change the PeopleView ContentControl
```
<ContentControl Grid.Row="1" Margin="10"
		prism:RegionManager.RegionName="PersonDetailsRegion"
		prism:RegionManager.RegionContext="{Binding SelectedItem, ElementName=_listOfPeople}" />
```
  * Now lets change the PersonDetailsView.xaml.cs
```
public PersonDetailsView(IPersonDetailsViewModel viewModel)
{
  InitializeComponent();
  
  ViewModel = viewModel;
  ViewModel.View = this;

  RegionContext.GetObservableContext(this).PropertyChanged += (s, e) =>
	{
	  var context = (ObservableObject<object>)s;
	  var selectedPerson = (Person)context.Value;
	  (ViewModel as IPersonDetailsViewModel).SelectedPerson = selectedPerson;
	};
}
```

  * This is how the PersonDetailsViewModel class looks like
```
public class PersonDetailsViewModel : ViewModelBase, IPersonDetailsViewModel
{
  private Person _SelectedPerson;
  public Person SelectedPerson
  {
    get { return _SelectedPerson; }
    set
    {
      _SelectedPerson = value;
      OnPropertyChanged("SelectedPerson");
    }
  }
}
```
  * And this is how the interface looks like
```
public interface IPersonDetailsViewModel : IViewModel
{
  Person SelectedPerson { get; set; }
}
```