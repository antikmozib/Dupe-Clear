#ifdef X64
  #define FilesPath "win-x64"
#else
  #define FilesPath "win-x86"
#endif

#define Title "Dupe Clear"
#define Version GetVersionNumbersString("..\DupeClear.Desktop\bin\Release\net8.0-windows\publish\" + FilesPath + "\DupeClear.Desktop.exe")
#define Publisher "Antik Mozib"
#define Url "https://mozib.io/dupeclear"
#define ExeName "DupeClear.Desktop.exe"

#ifdef X64
  #define SetupFileName "DupeClear-" + Version + "_x64-setup"
#else
  #define SetupFileName "DupeClear-" + Version + "_x86-setup"
#endif

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
LicenseFile={#SourcePath}\..\LICENSE
OutputDir=output
OutputBaseFilename={#SetupFileName}
Compression=lzma
SolidCompression=yes
UsePreviousAppDir=True
UninstallDisplayName={#Title}
UninstallDisplayIcon={app}\{#ExeName}
WizardStyle=modern
PrivilegesRequiredOverridesAllowed=commandline dialog
CloseApplications=yes

#ifdef X64
  ArchitecturesAllowed=x64compatible
  ArchitecturesInstallIn64BitMode=x64compatible
#endif

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: 

[Files]
Source: "{#SourcePath}\..\DupeClear.Desktop\bin\Release\net8.0-windows\publish\{#FilesPath}\*"; Excludes: "*.pdb"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "{#SourcePath}\..\LICENSE"; DestDir: "{app}"; Flags: 
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]
Name: "{group}\{#Title}"; Filename: "{app}\{#ExeName}"
Name: "{group}\{cm:UninstallProgram,{#Title}}"; Filename: "{uninstallexe}"
Name: "{autodesktop}\{#Title}"; Filename: "{app}\{#ExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#ExeName}"; Description: "{cm:LaunchProgram,{#StringChange(Title, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

[UninstallDelete]
Type: dirifempty; Name: "{app}"

[Code]
function GetUninstallString(): String;
var
  UninstallPath : String;
  UninstallString : String;
begin
  UninstallPath := ExpandConstant('Software\Microsoft\Windows\CurrentVersion\Uninstall\{#emit SetupSetting("AppId")}_is1');
  UninstallString := '';
  if not RegQueryStringValue(HKLM, UninstallPath, 'UninstallString', UninstallString) then
    RegQueryStringValue(HKCU, UninstallPath, 'UninstallString', UninstallString);
    
  Result := UninstallString;
end;

function IsUpgrade(): Boolean;
begin
  Result := GetUninstallString() <> '';
end;

function UninstallOldVersion(): Integer;
var
  UninstallString : String;
  ResultCode : Integer;
begin
  Result := 0;
  UninstallString := GetUninstallString();
  if UninstallString <> '' then 
  begin
    UninstallString := RemoveQuotes(UninstallString);
  
    if Exec(UninstallString, '/VERYSILENT /NORESTART /SUPPRESSMSGBOXES', '', SW_HIDE, ewWaitUntilTerminated, ResultCode) then
      Result := 3
    else
      Result := 2;
  end 
  else
    Result := 1;
end;

procedure CurStepChanged(CurStep: TSetupStep);
begin
  if CurStep = ssInstall then
    if IsUpgrade() then
      UninstallOldVersion();
end;
