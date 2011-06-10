using System.Reflection;
using System.Runtime.InteropServices;

using DomainServices.EnvironmentConfiguration.ConfigModule;

using Hosts.Plugins.VNC;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.

[assembly: AssemblyTitle("VNC")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Polymedia")]
[assembly: AssemblyProduct("VNC")]
[assembly: AssemblyCopyright("Copyright © Polymedia 2008")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.

[assembly : ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM

[assembly : Guid("c3d93271-7e67-4225-a6f7-c9603c453f36")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:

[assembly : AssemblyVersion("1.5.80.0")]
[assembly : AssemblyFileVersion("1.5.80.0")]
[assembly : Module("VNC", typeof (VNCModule))]