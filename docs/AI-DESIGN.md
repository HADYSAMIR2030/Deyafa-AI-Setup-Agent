# AI diagnostics design (planned, not connected)

V1 remains offline and read-only. No model/API subscription required.

## Future optional online mode
Windows Agent -> Simple Touch backend -> model API. Never embed an API key in the EXE.
Send only redacted diagnostic codes and approved log excerpts. Exclude credentials, activation codes, customer data, database backups and personal information.
Model returns structured suggestions, never executable commands.
Only signed, versioned and allowlisted remediation actions may be offered; any write requires technician approval.
Add explicit opt-in, audit logs, quotas and rate limits before enabling online mode.

## Release gates
1. Windows build succeeds.
2. Test preflight on disposable Windows 10 x64 VM.
3. Validate actual Deyafa installation media and checksums.
4. Test Check/Apply/Verify implementations in isolated VM.
5. Require approval before any LIVE customer installation.
