Function Perform-MassDirectoryOperation{
    param(
        [System.IO.FileInfo]$Path,
        [string]$Match = "*.*",
        [switch]$Recursve = $false,
        [System.IO.FileInfo]$Output,
        [string]$Date,
        [string]$FromFileName,
        [switch]$Iterator = $false
    );

    $r = (&{If($Recursve) {"-r"} Else {""}});
    $ffn = (&{If($FromFileName) {"--from-filename $FromFileName"} Else {""}});
    $i = (&{If($Iterator) {"--iterator"} Else {""}});
    $o = (&{If($Output) {"-o $Output"} Else {""}});
    $m = (&{If($Match) {"-m $Match"} Else {""}});

    "DateSetter.exe $Path $m $r $ffn $i $o" | Invoke-Expression
}