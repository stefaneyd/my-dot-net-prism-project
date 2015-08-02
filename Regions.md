# 3. Regions #

These are the discussion in this section.
  * What are Regions? - What are they and what role do they play in our application
  * RegionManager - how it manages the regions in our Prism application
  * RegionAdapter -  the relationship between it and RegionManager
  * Create a Custom Region -  how to create a custom region, these are needed for regions that Prism does not support


---

## What are Regions? ##
  * "Placeholder" for dynamic content that is going to be presented in the UI or in the Shell.Named location that you can define where a View will appear.

<img src='http://s16.postimg.org/z7wwjn9j9/region.png' width='400' /><br>
These regions define areas of the UI where we're gonna inject a view. Each region should be specifically named that describes the type of content that we are going to inject into that region<br>
<br>
<ul><li>No knowledge of views - they have no knowledge of views<br>
<img src='http://s12.postimg.org/4r2jlczel/region2.png' width='400' />
<br>We can redesign our Shell and it will have no impact on the region injection, no need to modify the infrastructure or module code.</li></ul>

<ul><li>Create in code or in XAML -  region is not a control, its apply to a control via the region name attach property on the region manager.</li></ul>

<ul><li>implements IRegion - Region implements the IRegion interface</li></ul>

<hr />
<h2>Region Manager</h2>
is responsible for managing the regions in the application.<br> It does it by:<br>
<ul><li>Maintains collection of regions - creates new regions for controls<br>
</li><li>Provides a RegionName attached property - used to create regions by applying the attach property to the host control, can be done through XAML or code.<br>
</li><li>Maps RegionAdapter to controls - responsible for associating region with the host control. in order to expose a UI control as a region it must have a region adapter, and each region adapter adapts a specific type of UI control.<br> <b>Prism Region Adapters</b>
<ul><li>ContentControlRegionAdapter<br>
</li><li>ItemsControlRegionAdapter<br>
</li><li>SelectorRegionAdaptor<br>
</li><li>TabControlRegionAdapter (Silverlight only)<br>
</li></ul></li><li>Provides a RegionContext attached property -  similar to dataContext in XAML, it allows you to propagate its data down the visual tree. Technik used to share data between a parent view and a child view that are hosted within a region.</li></ul>

<hr />
<h2>Demo Creating Regions</h2>

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
</li><li>Open the !ModuleAModule.cs and change the region names there to.<br>
<pre><code>using Demo.Infrastructure;<br>
<br>
_regionManager.RegisterViewWithRegion(RegionNames.ToolbarRegion, typeof(ToolbarView));<br>
</code></pre>
</li><li>Now we can change the layout of the Shell without having to change any thing else.<br>
<pre><code>DockPanel.Dock="Bottom"<br>
</code></pre>
</li><li>Now we are going add multiple item in the ToolbarRegion so we need to change from ContentControl to ItemsControl<br>
</li><li>To simulate this go to !ModuleAModule and change how we register the ToolbarRegion with the manager.<br>
<pre><code>IRegion region = _regionManager.Regions[RegionNames.ToolbarRegion];<br>
region.add(_container.Resolve&lt;ToolbarView&gt;());<br>
region.add(_container.Resolve&lt;ToolbarView&gt;());<br>
region.add(_container.Resolve&lt;ToolbarView&gt;());<br>
region.add(_container.Resolve&lt;ToolbarView&gt;());<br>
region.add(_container.Resolve&lt;ToolbarView&gt;());<br>
</code></pre></li></ul>

<hr />
<h2>Custom Region</h2>

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

<hr />
<h2>Custom Region</h2>

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
<h2>Demo Create a custom Region</h2>

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
</li><li>Register the adapter in Bootstrapper