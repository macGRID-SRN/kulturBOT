// Learn more about F# at http://fsharp.net
// See the 'F# Tutorial' project for more help.

open System
open System.IO

let removeBadCharacters stringArr =
    0

[<EntryPoint>]
let main argv = 
    let parse file =
        try
            let toParse file =  
                file  
                |> File.ReadAllLines

            toParse file
            with
                | :? System.IO.FileNotFoundException -> printfn "File Name invalid."; [||]


    ignore(parse "TXT.rtf")
    Console.WriteLine "Opened File. Parsed Out."
    ignore(Console.ReadKey false)
    0 // return an integer exit code
