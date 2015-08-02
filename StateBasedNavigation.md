> # 7. State-Based Navigation #

Designing navigation for our application is probably one of the most important aspects of our application design. In this section we are going to go over how we implement State-Based Navigation in our Prism application.
  * State-Based Navigation - what is state-based navigation
  * Reflecting application state - we are going to implement state-based navigation to reflect application state.
  * Displaying data - We are going to see how to display data in different formats and layouts
  * user interaction - and we are going to see how we can give users the ability to perform tasks that are related to a view.


---

## What is State-Based Navigation ##

When most people think about navigation they focus on menus as a way to move from one view to another view.
  * State changes to existing controls - but in state-based navigation, navigation is accomplished by applying state changes to existing controls that exist in a view. This may include hiding controls or showing controls or applying animations to controls.
  * Doesn't replace a view - the key here is that a view is not replaced by another view. Instead the view is updated.
  * View is updated - this is done either through
    * State changes in ViewModel - for example we may have a property in our viewModel called IsBusy, and when that IsBusy property is set to true our view is updated to reflect some type of process indicator, and when the IsBusy is set to false the view is updated to reflect that no process is occurring and will hide the indicator.
    * User interaction - it can also be accomplished with user interaction, so the user may click a button or drag and drop an element, but the user performs some type of interaction which causes the view to update itself to reflect those changes.
> Easiest to implement - State-Based Navigation is probably the easiest type of navigation to implement. The majority of view updates can be accomplished with simple data binding, styles, triggers, we can even enlist the help of expression-blends behavior for animations.

**State-Base Navigation - When to use it**
  * Use it
    * Same data, different style
    * Change layout based on state
    * Perform related tasks
  * Don't use it
    * Different data
    * Different task
    * Complex state changes


---

## Demo Reflection Application State ##

In this demo we are going update our views based on changes in application state. First lets look at the application we are going to be working with.

**Our domain objects are located in the Business project**
```
public class Person : INotifyPropertyChanged, IDataErrorInfo
{
  private string _firstName;
  public string FirstName
  {
    get { return _firstName; }
    set
    {
      _firstName = value;
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
  public string Age
  {
    get { return _age; }
    set
    {
      _age = value;
      OnPropertyChanged("Age");
    }
  }

  private string _email;
  public string Email
  {
    get { return _email; }
    set
    {
      _email = value;
      OnPropertyChanged("Email");
    }
  }

  private string _imagePath;
  public string ImagePath
  {
    get { return _imagePath; }
    set
    {
      _imagePath = value;
      OnPropertyChanged("ImagePath");
    }
  }
}
```
**ModuleA project**
```
<UserControl x:Class="ModuleA.ContentAView"
	     xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
	     xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
	     xmlns:extToolkit="http://schemas.microsoft.com/winfx/2006/xaml/presentation">
  <UserControl.Resources>
    <DataTemplate x:Key="PersonItemTemplate">
      <StackPanel Margin="5">
        <TextBlock FontWeight="Bold" FontSize="18">
          <TextBlock.Text>
            <MultiBinding StringFormat="{}{0}, {1}">
              <Binding Path="LastName" />
              <Binding Path="FirstName" />
            </MultiBinding>
          </TextBlock.Text>
        </TextBlock>
      </StackPanel>
    </DataTemplate>
  </UserControl.Resources>

  <Grid Margin="25">
    <Grid.RowDefinitions>
      <RowDefinition Height="Auto" />
      <RowDefinition Height="*" />
    </Grid.RowDefinitions>

    <StackPanel Margin="10" Orientation="Horizontal">

    </StackPanel>

    <ListBox ItemsSource="{Binding People}" Grid.Row="1"
	     ItemTemplate="{StaticResource PersonItemTemplate}" >
    </ListBox>
  </Grid>
</UserControl>
```
  * Now we look at our viewModel for our View.
```
public class ContentAViewViewModel : IContentAViewViewModel, INotifyPropertyChangedEvent
{
  private readonly IPersonService _personService;

#region Properties
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
#endregion

  public ContentAViewViewModel(IPersonService personService)
  {
    _personService = personService;
    LoadPeople();
  }

#region Commands
  private void LoadPeople()
  {
    _personService.GetPeopleAsync((sender, result) =>
    {
      People = new ObservableCollection<Person>(result.Object);
    });
  }
#endregion

#region Methods
  private void LoadPeople()
  {
    _personService.GetPeopleAsync((sender, result) =>
    {
      People = new ObservableCollection<Person>(result.Object);
    });
  }
#endregion

#INotifyPropertyChanged
#endregion
}
```
  * The Service is located in our Service project PersonService.
```
public class PersonService : IPersonService
{
  private static readonly string Avatar1Uri = @"/Services.PersonService;component/Images/MC900433938.PNG";
private static readonly string Avatar2Uri = @"/Services.PersonService;compon";

  public IList<Business.Person> GetPeople()
  {
    List<Person> people = new List<Person>();

    for(int i=0; i<50; i++)
    {
      var person = new Person();
      person.FirstName = String.Format("First{0}", i);
      person.LastName = String.Format("Last{0}", i);
      person.Age = i;
      person.Email = String.Format("{0}.{1}@domain.com", person.FirstName, person.LastName);
      person.ImagePath = GetPersonImagePath(i);
      people.Add(person);
      Thread.Sleep(80); //simulate longer process
    }

    return people;
  }

  public void GetPeopleAsync(EventHandler<ServiceResult<IList<Person>>> callback)
  {
    BackgroundWorker bw = new BackgroundWorker();
    bw.DoWork += (o, e) =>
	{
	  e.Result = GetPeople();
	};
    bw.RunWorkerCompleted += (o, e) =>
	{
	  callback.Invoke(this, new ServiceReslt<IList<Person>>((IList<Person>e.Result));
	};
    bw.RunWorkerAsync();
  }
}
```
  * This service implements IPersonService, that interface is located in the Infrastructure project. That is so that any module that uses this service will not have to have a direct reference to the service project it self, it will just depend on unity to resolve the correct instance for it.

Ok now that we know how this application is put together, let talk about what we want to do to this application. What we are going to do is update the view to show the user any change in the application state, what is it doing, is it loading data? What we are going to do is show the user indication that the application is loading data.

To do that we are going to enlist the help of a toolkit called the extendedWPFToolkit. Add the namespace in !ContentAView.
```
xmlns:extToolkit="http://schemas.microsoft.com/winfx/2006/xaml/presentation/toolkit/extended"
```
  * we have to add a reference to the shell project and module project for the WPFToolkit.Extended. We need to wrap the listBox in BusyIndicator.
```
<extToolkit:BusyIndicator Grid.Row="1" BusyContent="Loading People...." IsBusy="{Binding IsBusy}">
  <ListBox ItemsSource="{Binding People}"
	   ItemTemplate="{StaticResource PersonItemTemplate}" >
  </ListBox>
</extToolkit:BusyIndicator>
```
  * Now openContentAViewViewModel class.
```
private bool _IsBusy;
public bool IsBusy
{
  get { return _IsBusy; }
  set
  {
    _IsBusy = value;
    OnPropertyChanged("IsBusy");
  }
}

private void LoadPeople()
{
  IsBusy = true;
  _ersonService.GetPeopleAsync((sender, result) =>
  {
    People = new ObservableCollection<Person>(result.Object);
    IsBusy = false;
  });
}
```
  * We can also add ToggleButton
```
<ToggleButton IsChecked="{Binding IsBusy}" Content="IsBusy" Margin="4" />
```


---

## Demo Displaying data in different layouts ##

Another common use of state-base navigation is when you want to display the same data but in a different format or layout. So in this demo we are going to take that list of people that we have and we are going to give the user the ability to view that list as rows of icons instead. And we want to provide this ability without any type viewModel interaction, we want this to be controlled with the view only. So we are going to enlist the help of triggers and styles. Now there are many ways to do this, but the approach that I'm going to show you is the most straightforward.

  * Now lets add a button to ContentAView.xaml
```
<ToggleButton x:Name="switchViewsToggleButton" Margin="4">
  <ToggleButton.Style>
    <Style TargetType="ToggleButton">
      <Style.Triggers>
        <Trigger Property="IsChecked" Value="True">
          <Setter Property="Content" Value="Show List" />
        </Trigger>
        <Trigger Property="IsChecked" Value="False">
          <Setter Property="Content" Value="Show Icons" />
        </Trigger>
      </Style.Triggers>
    </Style>
  </ToggleButton.Style>
</ToggleButton>
```

  * Now lets add an ItemTemplate to ContentAView.xaml
```
<DataTemplate x:Key="personIconTemplate">
  <StackPanel Margin="5" Width="100">
    <Image Source="{Binding ImagePath}" Height="75" Width="75">
      <Image.ToolTip>
        <TextBlock Text="{Binding Email}" FontSize="12" FontStyle="Italic" />
      </Image.ToolTip>
    </Image>
    <TextBlock FontSize="14" TextWrapping="Wrap">
      <TextBlock.Text>
        <MultiBinding StringFormat="{}{0}, {1}>
          <Binding Path="LastName" />
          <Binding Path="FirstName" />
        </MultiBinding>
      </TextBlock.Text>
    </TextBlock>
  </StackPanel>
</DataTemplate>
```

  * The last step is we need to create a style for the ListBox, which is going to use triggers to switch out the itemTemplate. So We need to remove the ListBox ItemTemplate and put in the style instead.
```
<ListBox ItemSource="{Binding People}">
  <ListBox.Style>
    <Style TargetType="ListBox">
      <Style.Triggers>
        <DataTrigger Binding="{Binding IsChecked, ElementName=swichViewsToggleButton}" Value="True">
          <Setter Property="ItemTemplate" Value="{StaticResource PeronsIconTemplate}" />
          <Setter Property="ScorllViewer.HorizontalScrollBarVisibility" Value="Diabled" />
          <Setter Property="ItemsPanel">
            <Setter.Value>
              <ItemsPanelTemplate>
                <WrapPanel />
              </ItemsPanelTemplate>
            </Setter.Value>
          </Setter>
        </DataTrigger>
        <DataTrigger Binding="{Binding IsChecked, ElementName=switchViewsToggleButton}" Value="False">
          <Setter Property="ItemTemplate" Value="{StaticResource PersonItemTemplate}" />
        </DataTrigger>
      </Style.Triggers>
    </Style>
  </ListBox.Style>
</ListBox>
```


---

## Demo User Interaction ##

What this application needs now is the ability to edit one of these people. So in order to support editing task using state-based navigation I know I'm going to show some type of module or none module editing window. I'm also going to need to provide a button in order to invoke an editing action.

  * lets open the viewModel ContentAViewViewModel
```
public DelegateCommand EditPersonCommand { get; private set; }

// And in the Constructor
public ContentAViewViewModel(IPersonService personService)
{
  _personService = personService;
  LoadPeople();
  EditPersonCommand = new DelegateCommand(EditPerson, CanEditPerson);

// Create the EditPerson and CanEditPerson methods
private void EditPerson()
{

}
private bool CanEditPerson()
{
  return true;
}
}
```
  * Now let open up the View ContentAView.xaml
```
// In the StackPanel under the last ToggleButton
<Button Command="{Binding EditPersonCommand}" Content="Edit Person" Margin="4" />
```
  * We need to know what person is selected so we add another property
```
private Person _SelectedPerson;
public Person SelectedPerson
{
  get { return _SelectedPerson; }
  set
  {
    _SelectedPerson = value;
    EditPersonCommand.RaisCanExecuteChanged();
    OnPropertyChanged("SelectedPerson");
  }
}
```
  * Now we can bind our ListBox selectedItem property to the selected person property in our viewModel.
```
<ListBox ItemSource="{Binding People}" SelectedItem="{Binding SelectedPerson}">
```
  * Lets change the CanEditPerson() method
```
private bool CanEditPerson()
{
  return SelectedPerson != null;
}
```
  * Now to implement the EditPerson we are going to use the extendedWPFToolkit and use a control in the toolkit called the childWindow place it under the stackPanel
```
<extToolkit:ChildWindow Grid.Row="1" Caption="Edit Person" IsModal="True" WindowsStartupLocation="Center" WindowsState="Open">
  <Grid DataContext="{Binding SelectedPerson}" Margin="15">
    <Grid.ColumnDefinitions>
      <ColumnDefinition Width="Auto" />
      <ColumnDefinition Width="*" />
    </Grid.ColumnDefinitions>
    <Grid.RowDefinitions>
      <RowDefinition Height="Auto" />
      <RowDefinition Height="Auto" />
      <RowDefinition Height="Auto" />
      <RowDefinition Height="Auto" />
    </Grid.RowDefinitions>

    <TextBlock Text="First Name:" />
    <TextBox Text="{Binding FirstName, UpdateSourceTrigger=PropertyChanged}" Grid.Column="1" />

    <TextBlock Text="Last Name:" Grid.Row="1" />
    <TextBox Text="{Binding LastName, UpdateSourceTrigger=PropertyChanged}" Grid.Row="1" Grid.Column="1" />

    <TextBlock Text="Age:" Grid.Row="2" />
    <TextBox Text="{Binding Age, UpdateSourceTrigger=PropertyChanged}" Grid.Row="2" Grid.Column="1" />

    <TextBlock Text="Email:" Grid.Row="3" />
    <TextBox Text="{Binding Email, UpdateSourceTrigger=PropertyChanged}" Grid.Row="3" Grid.Column="1" />
  </Grid>
```

  * Now the WindowState is either Open or Closed so we need a way to show and hide this dialog, so we create a new property in our modelView ContentAViewViewModel.
```
private WindowState _WindowState;
public WindowState WindowState
{
  get { return _WindowState; }
  set
  {
    _WindowState = value;
    OnPropertyChanged("WindowState");
  }
}

// and we change the EditPerson() method
private void EditPerson()
{
  WindowState = Microsoft.Windows.Controls.WindowState.Open;
}
```

  * Now we change the WindowState
```
WindowState="{Binding WindowState}"
```