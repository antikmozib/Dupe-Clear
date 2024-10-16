#define MyAppName "Dupe Clear"
#define MyAppVersion GetVersionNumbersString("..\DupeClear.Desktop\bin\Release\net8.0-windows\publish\win-x86\Dupe Clear.exe")
#define MyAppPublisher "Antik Mozib"
#define MyAppURL "https://mozib.io/dupeclear"
#define MyAppExeName "Dupe Clear.exe"
#define MyAppOutputName "DupeClear"

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{02D4DD0F-31FD-44F3-9C83-91993CA1F391}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={autopf}\{#MyAppName}
DefaultGroupName={#MyAppName}
LicenseFile=gpl-3.0.rtf
OutputDir=output
OutputBaseFilename={#MyAppOutputName}-{#MyAppVersion}-setup
Compression=lzma
SolidCompression=yes
UsePreviousAppDir=True
UninstallDisplayName={#MyAppName}
UninstallDisplayIcon={app}\{#MyAppName}.exe
WizardStyle=modern
PrivilegesRequired=lowest
CloseApplications=yes

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: 

[Files]
Source: "{#SourcePath}\..\DupeClear.Desktop\bin\Release\net8.0-windows\publish\win-x86\*"; Excludes: "*.pdb"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{group}\{cm:UninstallProgram,{#MyAppName}}"; Filename: "{uninstallexe}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent
