using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using Microsoft.Win32;
namespace DeyafaSetupAgent;
public partial class MainWindow : Window
{
    public MainWindow() { InitializeComponent(); }
    private void Scan_Click(object sender, RoutedEventArgs e)
    {
        ScanButton.IsEnabled = false;
        try
        {
            var root = Path.GetFullPath(FolderBox.Text.Trim());
            var lines = new List<string> { $"Time: {DateTimeOffset.Now:O}", $"OS: {RuntimeInformation.OSDescription}", $"64-bit OS: {Environment.Is64BitOperatingSystem}", $"Elevated: {IsAdministrator()}", $"Package folder: {root}" };
            if (!Directory.Exists(root)) { lines.Add("ERROR: Package folder not found."); LogBox.Text = string.Join(Environment.NewLine, lines); return; }
            var files = Directory.EnumerateFiles(root, "*", SearchOption.AllDirectories).Take(2001).ToList();
            lines.Add($"Files found (maximum 2001): {files.Count}");
            foreach (var category in new[] { "sql", "crystal", "odbc", "deyafa", ".bak" })
            {
                var matches = files.Where(f => Path.GetFileName(f).Contains(category, StringComparison.OrdinalIgnoreCase)).Take(8).ToList();
                lines.Add($"{category}: {matches.Count} matching filenames shown");
                lines.AddRange(matches.Select(f => "  " + Path.GetRelativePath(root, f)));
            }
            using var sql = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Microsoft SQL Server\Instance Names\SQL");
            lines.Add("SQL instances (registry, 64-bit view): " + (sql is null ? "none detected" : string.Join(", ", sql.GetValueNames())));
            lines.Add("Scan complete. No changes made.");
            LogBox.Text = string.Join(Environment.NewLine, lines);
        }
        catch (Exception ex) { LogBox.Text = $"Scan failed: {ex.GetType().Name}: {ex.Message}"; }
        finally { ScanButton.IsEnabled = true; }
    }
    private static bool IsAdministrator()
    {
        using var identity = System.Security.Principal.WindowsIdentity.GetCurrent();
        return new System.Security.Principal.WindowsPrincipal(identity).IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator);
    }
}
