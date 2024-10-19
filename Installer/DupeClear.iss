#define Title "Dupe Clear"
#define Version GetVersionNumbersString("..\DupeClear.Desktop\bin\Release\net8.0-windows\publish\win-x86\DupeClear.Desktop.exe")
#define Publisher "Antik Mozib"
#define Url "https://mozib.io/dupeclear"
#define ExeName "DupeClear.Desktop.exe"
#define OutputFileName "DupeClear-" + Version + "-setup"

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{02D4DD0F-31FD-44F3-9C83-91993CA1F391}
AppName={#Title}
AppVersion={#Version}
AppVerName={#Title} {#Version}
AppPublisher={#Publisher}
AppPublisherURL={#Url}
AppSupportURL={#Url}
AppUpdatesURL={#Url}
DefaultDirName={autopf}\{#Title}
DefaultGroupName={#Title}
LicenseFile=gpl-3.0.rtf
OutputDir=output
OutputBaseFilename={#OutputFileName}
Compression=lzma
SolidCompression=yes
UsePreviousAppDir=True
UninstallDisplayName={#Title}
UninstallDisplayIcon={app}\{#Title}.exe
WizardStyle=modern
PrivilegesRequiredOverridesAllowed=commandline dialog
CloseApplications=yes

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: 

[Files]
Source: "{#SourcePath}\..\DupeClear.Desktop\bin\Release\net8.0-windows\publish\win-x86\*"; Excludes: "*.pdb"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]
Name: "{group}\{#Title}"; Filename: "{app}\{#ExeName}"
Name: "{group}\{cm:UninstallProgram,{#Title}}"; Filename: "{uninstallexe}"
Name: "{autodesktop}\{#Title}"; Filename: "{app}\{#ExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#ExeName}"; Description: "{cm:LaunchProgram,{#StringChange(Title, '&', '&&')}}"; Flags: nowait postinstall skipifsilent
