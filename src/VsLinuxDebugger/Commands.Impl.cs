﻿using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Shell;
using VsLinuxDebugger.Core;

namespace VsLinuxDebugger
{
  internal sealed partial class Commands
  {
    /// <summary>Override standard button text with.</summary>
    /// <param name="commandId">Command Id.</param>
    /// <returns>Text to display.</returns>
    public string GetMenuText(int commandId)
    {
      switch (commandId)
      {
        case CommandIds.CmdDeployAndDebug: return "Deploy and Debug";
        case CommandIds.CmdDeployOnly: return "Deploy Only";
        case CommandIds.CmdDebugOnly: return "Debug Only";
        case CommandIds.CmdShowLog: return "Show Log";
        case CommandIds.CmdShowSettings: return "Settings";
        default: return $"Unknown CommandId ({commandId})";
      }
    }

    private void InstallMenu(OleMenuCommandService cmd)
    {
      AddMenuItem(cmd, CommandIds.CmdDeployAndDebug, SetMenuTextAndVisibility, OnDeployAndDebugAsync);
      AddMenuItem(cmd, CommandIds.CmdDeployOnly, SetMenuTextAndVisibility, OnDeployOnlyAsync);
      AddMenuItem(cmd, CommandIds.CmdDebugOnly, SetMenuTextAndVisibility, OnDebugOnlyAsync);

      AddMenuItem(cmd, CommandIds.CmdShowLog, SetMenuTextAndVisibility, OnShowLog);
      AddMenuItem(cmd, CommandIds.CmdShowSettings, SetMenuTextAndVisibility, OnShowSettingsAsync);
    }

    private async Task<bool> ExecuteBuildAsync(BuildOptions buildOptions)
    {
      await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

      var options = ToUserOptions();
      var dbg = new RemoteDebugger(options);

      if (!dbg.IsProjectValid())
      {
        Console.WriteLine("No C# startup project/solution loaded.");
        return false;
      }

      if(!await dbg.BeginAsync(buildOptions))
      {
        Console.WriteLine("Failed to perform actions.");
        return false;
      }

      return true;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "VSTHRD100:Avoid async void methods", Justification = "asdf")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "VSTHRD200:Avoid async void methods", Justification = "asdf")]
    private async void OnDebugOnlyAsync(object sender, EventArgs e)
    {
      await ExecuteBuildAsync(BuildOptions.Build | BuildOptions.Debug);
    }

    private async void OnDeployAndDebugAsync(object sender, EventArgs e)
    {
      await ExecuteBuildAsync(BuildOptions.Build | BuildOptions.Deploy | BuildOptions.Debug);
    }

    private async void OnDeployOnlyAsync(object sender, EventArgs e)
    {
      await ExecuteBuildAsync(BuildOptions.Build | BuildOptions.Deploy);
    }

    private void OnShowLog(object sender, EventArgs e)
    {
      // Not implemented yet
      if (sender is OleMenuCommand cmd)
        cmd.Enabled = false;

      MessageBox("Not implemented");
    }

    private async void OnShowSettingsAsync(object sender, EventArgs e)
    {
      // Not implemented yet
      if (sender is OleMenuCommand cmd)
        cmd.Enabled = false;

      await Task.Yield();
      MessageBox("Not implemented");
    }

    private void SetMenuTextAndVisibility(object sender, EventArgs e)
    {
      ThreadHelper.ThrowIfNotOnUIThread();

      if (sender is OleMenuCommand cmd)
      {
        // TODO: Enhance by displaying IP Address
        ////var settings = SettingsManager.Instance.Load();
        //// cmd.Text = $"{GetMenuText(cmd.CommandID.ID)} ({settings.HostIp})";
        //// cmd.Enabled = _extension.IsStartupProjectAvailable();

        if (cmd.CommandID.ID == CommandIds.CmdShowLog
          || cmd.CommandID.ID == CommandIds.CmdDebugOnly
          || cmd.CommandID.ID == CommandIds.CmdShowSettings)
        {
          cmd.Enabled = false;
        }
      }
    }

    private UserOptions ToUserOptions()
    {
      return new UserOptions
      {
        HostIp = Settings.HostIp,
        HostPort = Settings.HostPort,

        LocalPlinkEnabled = Settings.LocalPlinkEnabled,
        LocalPLinkPath = Settings.LocalPLinkPath,

        RemoteDeployBasePath = Settings.RemoteDeployBasePath,
        RemoteDeployDebugPath = Settings.RemoteDeployDebugPath,
        RemoteDeployReleasePath = Settings.RemoteDeployReleasePath,
        RemoteDotNetPath = Settings.RemoteDotNetPath,
        RemoteVsDbgPath = Settings.RemoteVsDbgPath,

        UseCommandLineArgs = Settings.UseCommandLineArgs,
        UsePublish = Settings.UsePublish,

        UserPrivateKeyEnabled = Settings.UserPrivateKeyEnabled,
        UserPrivateKeyPath = Settings.UserPrivateKeyPath,
        UserName = Settings.UserName,
        UserPass = Settings.UserPass,
        UserGroupName = Settings.UserGroupName,
      };
    }
  }
}
