# Deyafa Setup Agent — V1 source scaffold

Windows 10 x64 WPF preflight scanner. This is NOT a completed installer or AI agent. No customer changes.

## Build on Windows
From src/DeyafaSetupAgent:
```powershell
dotnet restore
dotnet build -c Release
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -o ../../publish
```

## Current behavior
- Scans selected installation folder for candidate files.
- Reports OS, architecture, admin privileges and registered SQL instances.
- Read-only. Does not install SQL, Crystal, ODBC or Deyafa.
- Does not modify customer data, services, printers or activation.

Next: inspect real installation media, confirm compatibility, implement approved Check/Apply/Verify steps, test on disposable Windows 10 VMs.
SQL Server 2008 is unsupported by Microsoft and must be evaluated before deployment.
