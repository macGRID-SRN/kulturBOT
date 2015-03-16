// Learn more about F# at http://fsharp.net
// See the 'F# Tutorial' project for more help.
module Program

open System
open System.IO
open System.Text
open SuperScript

//characters which should not be allowed
let banishedChars = 
    [|
        '%';
        ';';
        '[';
        ']';
        '^';
    |]

let replacePhrases = 
    [
        ("M^'", "Mc");
        ("M'^", "Mc");
    ]

//option to replace case.
let matchCase = false;

let removePhrase (text : string) = 
    let mutable result = text
    for i in replacePhrases do
        result <- result.Replace(fst i, snd i)
    result

let removePhrases (text : string[]) = 
    Array.map removePhrase text

//removes all the unwanted characters
let removeBadCharFromString (text : string) =
    text.Split(banishedChars, StringSplitOptions.RemoveEmptyEntries) |> String.Concat

//wrapper for string[]
let removeBadCharacters (textArr : string[]) =
    Array.map (fun (n : string) -> removeBadCharFromString n) textArr

[<EntryPoint>]
let main argv = 
    let parse file out =
        try
            let toParse file =  
                file  
                |> File.ReadAllLines
                |> findSuperScriptNum
                |> removePhrases
                |> removeBadCharacters

            File.WriteAllLines (out, toParse file)
            true
        with
            | :? System.IO.FileNotFoundException -> printfn "File name invalid or file not found!"; false //returns empty string array.

    let result = parse argv.[0] argv.[1]

    let stringOutput = 
        if result 
        then "Opened File. Parsed Out." 
        else "Something Failed."

    Console.WriteLine stringOutput
    ignore(Console.ReadKey false)
    0 // return an integer exit code
