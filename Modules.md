# 4. Modules #

This section will explain how to take the views and logic out of a large application and break them down into a smaller peaces called Modules and use the in the Prism application.
  * What is a Module?
  * Registering Modules
  * Loading Modules
  * Initializing Modules


---

## What is a Module? ##

  * Building block of a Prism application, a package that bundles up all the functionality and resources for a portion of the overall application.

Current architecture that we want to change:
  * Solution that has a Single Main Project with
    * all the views
    * all services
    * all business logic
    * and every single code that has to do with this application is in a single project
<img src='http://s17.postimg.org/brbnpypjz/app.png' width='400px'></img>

We want to identify the major functional areas of our application and start breaking them down into smaller more manageable peaces, and those peaces are called Modules<br>
<img src='http://s23.postimg.org/b910ma8ez/prism.png' width='400px' />

<ul><li>Class library/XAP - we have broken the project down into modules and the result is a class library that can be used in other applications.</li></ul>

<ul><li>Class that implements IModule - each module has a class that implements the IModule interface and that is what identifies it as a module to the Prism library, it has a single method in it called Initialize, which is responsible for initializing the module and integrating it into the Prism application.</li></ul>

<hr />
<h2>Demo Creating a Module</h2>

<ul><li>Add a WPF User Control Library into the solutions module folder, call it ModuleA and save it to the module folder in our directory structure.<br>
</li><li>delete the UserControl<br>
</li><li>add a reference Microsoft.Practices.Prism.dll<br>
</li><li>add a class and name it ModuleAModule<br>
</li><li>it has to be public and implement the IModule and the Initialize method</li></ul>

<hr />
<h2>Registering Discovering Modules</h2>

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

<hr />
<h2>Loading Modules</h2>

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
<hr />
<h2>Demo Register Load Modules in Code</h2>

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

<hr />
<h2>Demo Register Load Modules from Directory</h2>

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

<hr />
<h2>Demo Register Load Modules from XAML</h2>

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
<hr />
<h2>Demo Register Load Modules from App.config File</h2>

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
<hr />
<h2>Initializing Modules</h2>

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
The Initialize method is where we write the code that is going to Register our types, we may subscribe to services or events, we can register shared services or we can compose our views into the shell. The initialize method is where we put all our code that does all the work to get our module ready for consumptions in our Prism application.