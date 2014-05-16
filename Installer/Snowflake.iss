; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "Snowflake"
#define MyAppVersion "0.2"
#define MyAppPublisher "My Company, Inc."
#define MyAppURL "http://www.example.com/"
#define MyAppExeName "Snowflake.exe"

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{B358E04F-5A31-4E92-B319-0820E48BCA93}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
;AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={pf}\{#MyAppName}
DefaultGroupName={#MyAppName}
AllowNoIcons=yes
OutputBaseFilename=setup
Compression=lzma
SolidCompression=yes

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked
Name: "quicklaunchicon"; Description: "{cm:CreateQuickLaunchIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked; OnlyBelowVersion: 0,6.1

[Files]
Source: "C:\Users\Nikko\Documents\Dev\C#\Snowflake\Snowflake\bin\Release\Snowflake.exe"; DestDir: "{app}\bin"; Flags: ignoreversion

Source: "C:\Users\Nikko\Documents\Dev\C#\Snowflake\Snowflake\bin\Media\*"; DestDir: "{app}\Media"; Flags: ignoreversion recursesubdirs createallsubdirs

Source: "C:\Users\Nikko\Documents\Dev\C#\Snowflake\Snowflake\bin\Release\cg.dll"; DestDir: "{app}\bin"; Flags: ignoreversion
Source: "C:\Users\Nikko\Documents\Dev\C#\Snowflake\Snowflake\bin\Release\Haswell.dll"; DestDir: "{app}\bin"; Flags: ignoreversion
Source: "C:\Users\Nikko\Documents\Dev\C#\Snowflake\Snowflake\bin\Release\Haswell.pdb"; DestDir: "{app}\bin"; Flags: ignoreversion
Source: "C:\Users\Nikko\Documents\Dev\C#\Snowflake\Snowflake\bin\Release\Log.exe"; DestDir: "{app}\bin"; Flags: ignoreversion
Source: "C:\Users\Nikko\Documents\Dev\C#\Snowflake\Snowflake\bin\Release\Miyagi.Backend.Mogre.dll"; DestDir: "{app}\bin"; Flags: ignoreversion
Source: "C:\Users\Nikko\Documents\Dev\C#\Snowflake\Snowflake\bin\Release\Miyagi.dll"; DestDir: "{app}\bin"; Flags: ignoreversion
Source: "C:\Users\Nikko\Documents\Dev\C#\Snowflake\Snowflake\bin\Release\Miyagi.Plugin.Input.Mois.dll"; DestDir: "{app}\bin"; Flags: ignoreversion
Source: "C:\Users\Nikko\Documents\Dev\C#\Snowflake\Snowflake\bin\Release\Miyagi.xml"; DestDir: "{app}\bin"; Flags: ignoreversion
Source: "C:\Users\Nikko\Documents\Dev\C#\Snowflake\Snowflake\bin\Release\Mogre.dll"; DestDir: "{app}\bin"; Flags: ignoreversion
Source: "C:\Users\Nikko\Documents\Dev\C#\Snowflake\Snowflake\bin\Release\MOIS.dll"; DestDir: "{app}\bin"; Flags: ignoreversion
Source: "C:\Users\Nikko\Documents\Dev\C#\Snowflake\Snowflake\bin\Release\Newtonsoft.Json.dll"; DestDir: "{app}\bin"; Flags: ignoreversion
Source: "C:\Users\Nikko\Documents\Dev\C#\Snowflake\Snowflake\bin\Release\OgreMain.dll"; DestDir: "{app}\bin"; Flags: ignoreversion
Source: "C:\Users\Nikko\Documents\Dev\C#\Snowflake\Snowflake\bin\Release\OgrePaging.dll"; DestDir: "{app}\bin"; Flags: ignoreversion
Source: "C:\Users\Nikko\Documents\Dev\C#\Snowflake\Snowflake\bin\Release\OgreRTShaderSystem.dll"; DestDir: "{app}\bin"; Flags: ignoreversion
Source: "C:\Users\Nikko\Documents\Dev\C#\Snowflake\Snowflake\bin\Release\OgreTerrain.dll"; DestDir: "{app}\bin"; Flags: ignoreversion
Source: "C:\Users\Nikko\Documents\Dev\C#\Snowflake\Snowflake\bin\Release\Plugin_BSPSceneManager.dll"; DestDir: "{app}\bin"; Flags: ignoreversion
Source: "C:\Users\Nikko\Documents\Dev\C#\Snowflake\Snowflake\bin\Release\Plugin_CgProgramManager.dll"; DestDir: "{app}\bin"; Flags: ignoreversion
Source: "C:\Users\Nikko\Documents\Dev\C#\Snowflake\Snowflake\bin\Release\Plugin_OctreeSceneManager.dll"; DestDir: "{app}\bin"; Flags: ignoreversion
Source: "C:\Users\Nikko\Documents\Dev\C#\Snowflake\Snowflake\bin\Release\Plugin_OctreeZone.dll"; DestDir: "{app}\bin"; Flags: ignoreversion
Source: "C:\Users\Nikko\Documents\Dev\C#\Snowflake\Snowflake\bin\Release\Plugin_ParticleFX.dll"; DestDir: "{app}\bin"; Flags: ignoreversion
Source: "C:\Users\Nikko\Documents\Dev\C#\Snowflake\Snowflake\bin\Release\Plugin_PCZSceneManager.dll"; DestDir: "{app}\bin"; Flags: ignoreversion
Source: "C:\Users\Nikko\Documents\Dev\C#\Snowflake\Snowflake\bin\Release\plugins.cfg"; DestDir: "{app}\bin"; Flags: ignoreversion
Source: "C:\Users\Nikko\Documents\Dev\C#\Snowflake\Snowflake\bin\Release\RenderSystem_Direct3D9.dll"; DestDir: "{app}\bin"; Flags: ignoreversion
Source: "C:\Users\Nikko\Documents\Dev\C#\Snowflake\Snowflake\bin\Release\RenderSystem_GL.dll"; DestDir: "{app}\bin"; Flags: ignoreversion
Source: "C:\Users\Nikko\Documents\Dev\C#\Snowflake\Snowflake\bin\Release\settings.cfg"; DestDir: "{app}\bin"; Flags: ignoreversion
Source: "C:\Users\Nikko\Documents\Dev\C#\Snowflake\Snowflake\bin\Release\Snowflake.pdb"; DestDir: "{app}\bin"; Flags: ignoreversion
Source: "C:\Users\Nikko\Documents\Dev\C#\Snowflake\Snowflake\bin\Release\Snowflake.vshost.exe.manifest"; DestDir: "{app}\bin"; Flags: ignoreversion

Source: "C:\Users\Nikko\Documents\Dev\C#\Snowflake\Snowflake\bin\resources.cfg"; DestDir: "{app}"; Flags: ignoreversion
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{group}\{cm:ProgramOnTheWeb,{#MyAppName}}"; Filename: "{#MyAppURL}"
Name: "{group}\{cm:UninstallProgram,{#MyAppName}}"; Filename: "{uninstallexe}"
Name: "{commondesktop}\{#MyAppName}"; Filename: "{app}\bin\{#MyAppExeName}"; Tasks: desktopicon
Name: "{userappdata}\Microsoft\Internet Explorer\Quick Launch\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: quicklaunchicon

[Run]
Filename: "{app}\bin\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

