using DirectoryScanner.Core.Services;

var tokenSource = new CancellationTokenSource();
var token = tokenSource.Token;
var directoryScanner = new Scanner();
var resultTask = directoryScanner.Scan(@"D:\Labs\SPP", 10, token);
var path = Path.GetDirectoryName(@"D:\Labs\SPP");
path = Path.GetFileName(@"D:\Labs\SPP\che.txt");
var result = await resultTask;
Console.WriteLine(result.Root.Size);