Function Initialize-MassDirectoryOperation{
    param(
        [System.IO.FileInfo]$Path,
        [string]$Match = "*.*",
        [switch]$Recursve = $false,
        [System.IO.FileInfo]$Output,
        [string]$Date,
        [string]$FromFileName,
        [string]$ExactDate,
        [switch]$Iterator = $false,
		[switch]$Debugger = $false,
		[switch]$LastModified = $false,
		[string]$Exclude = ""
    );

    $r = (&{If($Recursve) {"-r"} Else {""}});
    $ffn = (&{If($FromFileName) {"--from-filename $FromFileName"} Else {""}});
    $i = (&{If($Iterator) {"--iterator"} Else {""}});
    $o = (&{If($Output) {"-o $Output"} Else {""}});
    $m = (&{If($Match) {"-m $Match"} Else {""}});
	$d = (&{If($Debugger) {"--debug"} Else {""}});
	$ed = (&{If($ExactDate) {"--exact-date $ExactDate"} Else {""}});
	$lm = (&{If($LastModified) {"--last-modified"} Else {""}});
	$e = (&{If($Exclude) {"-e $Exclude"} Else {""}});

    "MDO.CLI.exe $Path $m $r $ffn $i $o $d $ed $lm $e" | Invoke-Expression
}