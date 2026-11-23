using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace DeyafaSetupAgent.Diagnostics;

public sealed record DiagnosticFinding(string Code, string Severity, string Message, string SuggestedAction);
public sealed record DiagnosticReport(DateTimeOffset Timestamp, string PackageDirectory, IReadOnlyList<DiagnosticFinding> Findings);

public static class DiagnosticEngine
{
    public static DiagnosticReport Analyze(string directory)
    {
        var findings = new List<DiagnosticFinding>();
        if (string.IsNullOrWhiteSpace(directory) || !Directory.Exists(directory))
        {
            findings.Add(new("PACKAGE_FOLDER_MISSING", "Error", "Installation package directory does not exist.", "Select the approved installation folder."));
            return new(DateTimeOffset.UtcNow, directory ?? "", findings);
        }
        if (!Environment.Is64BitOperatingSystem)
            findings.Add(new("OS_32_BIT", "Warning", "32-bit Windows detected.", "Verify supported architecture before installation."));
        var files = Directory.EnumerateFiles(directory, "*", SearchOption.AllDirectories).Take(10001).ToArray();
        if (files.Length > 10000)
            findings.Add(new("PACKAGE_SCAN_LIMIT", "Warning", "More than 10,000 files found; scan was limited.", "Review folder contents."));
        foreach (var category in new[] { "sql", "crystal", "odbc", "deyafa" })
            if (!files.Any(f => Path.GetFileName(f).Contains(category, StringComparison.OrdinalIgnoreCase)))
                findings.Add(new("PACKAGE_" + category.ToUpperInvariant() + "_UNCONFIRMED", "Info", "No filename matched " + category + ".", "Inspect package manually; filenames are not proof of absence."));
        if (findings.Count == 0)
            findings.Add(new("PREFLIGHT_NO_FINDINGS", "Info", "No issues found by basic checks.", "Continue manual compatibility review; installation is not authorized."));
        return new(DateTimeOffset.UtcNow, directory, findings);
    }
    public static string ToJson(DiagnosticReport report) => JsonSerializer.Serialize(report, new JsonSerializerOptions { WriteIndented = true });
}
