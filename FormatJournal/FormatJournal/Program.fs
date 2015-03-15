// Learn more about F# at http://fsharp.net
// See the 'F# Tutorial' project for more help.

open System
open System.IO

let readLines =
    "TXT.rtf"
    |> File.ReadAllLines


[<EntryPoint>]
let main argv = 
    printf "Opened File. Parsed Out."
    0 // return an integer exit code
