﻿using System.ComponentModel;
using Microsoft.VisualStudio.Shell;

namespace Xeno.VsLinuxDebug.OptionsPages
{
  public partial class OptionsPage : DialogPage
  {
    private const string RemoteDebugger = "Remote Debugger";
    private const string Experimental = "Expermimental";

    [Category(RemoteDebugger)]
    [DisplayName("Upload to folder")]
    [Description("Folder for to transfer files to. For HOME folder, use './VLSDbg' and not '~/VLSDbg'")]
    public string RemoteDeploymentBasePath { get; set; } = $"./VSLinuxDbg"; // "VSLDebugger"

    [Category(RemoteDebugger)]
    [DisplayName(".NET Path")]
    [Description("Path to .NET on remote machine.")]
    public string RemoteDotNetPath { get; set; } = "~/.dotnet/dotnet";

    [Category(RemoteDebugger)]
    [DisplayName("Visual Studio Debugger Path")]
    [Description("Remote Machine Visual Studio Debugger Path")]
    public string RemoteVsDbgPath { get; set; } = "~/.vs-debugger/vs2022";

    [Category(Experimental)]
    [DisplayName("Display GUI")]
    [Description(
      "Display application on remote machine. This is helpful for debugging " +
      "GUI applications on remote devices.")]
    public bool DebuggerDisplayInGui { get; set; } = false;

    [Category(Experimental)]
    [DisplayName("Use Command Line Arguments")]
    [Description(
      "Apply command line arguments from Visual Studio Project Settings. " +
      "(Experimental : Project Settings -> Debugging -> Command Line Arguments)")]
    public bool UseCommandLineArgs { get; set; } = false;
  }
}
