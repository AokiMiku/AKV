; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "ApS KostenVerwaltung"
#define MyAppVersion "0.7.1"
#define MyAppPublisher "ApS Industries"
#define MyAppExeName "AKV.exe"

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{82653175-8278-4FBB-821C-6867DE72963C}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppVerName={#MyAppName}
AppPublisher={#MyAppPublisher}
DefaultDirName={pf}\AKV
DisableProgramGroupPage=yes
SourceDir=C:\Users\Kenji Kerman\Documents\Proggen\CSharp\AKV\Setups\SetupSource
OutputDir=C:\Users\Kenji Kerman\Documents\Proggen\CSharp\AKV\Setups\{#MyAppVersion}
OutputBaseFilename=AKV_{#MyAppVersion}
Compression=lzma
SolidCompression=yes
SetupIconFile=AKV.ico
UninstallDisplayIcon={app}\AKV.exe

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"
Name: "german"; MessagesFile: "compiler:Languages\German.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked
Name: "quicklaunchicon"; Description: "{cm:CreateQuickLaunchIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked; OnlyBelowVersion: 0,6.1

[Files]
Source: "AKV.exe"; DestDir: "{app}"; Flags: ignoreversion                          
Source: "Daten\AKV.FDB"; DestDir: "{app}\Daten"; Flags: onlyifdoesntexist
Source: "AKVCore.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "ApS.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "ApS.Firebird.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "AKV.exe.config"; DestDir: "{app}"; Flags: ignoreversion
Source: "firebird\*"; DestDir: "{app}\firebird"; Flags: ignoreversion
Source: "AKV.ico"; Flags: dontcopy
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[UninstallDelete]
Type: filesandordirs; Name:"{app}"

[Icons]
Name: "{commonprograms}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{commondesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

