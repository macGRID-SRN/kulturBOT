module SuperScript
open System
open System.Linq

let OCRmistakenChars = 
    [
        '\'';
        '"';
        '^';
        '*';
        '.';
    ]

let isNum s = 
    match Int32.TryParse(s) with
    |   (false, _) -> false
    |   (true, _) -> true

let isEnglishNumeral (s : string) i = 
    (isNum (s.Substring(i, 1))) && 
    (OCRmistakenChars.Contains (s.[i+1])) 
    && 
    ((OCRmistakenChars.Contains (s.[i+2])) || String.IsNullOrWhiteSpace (s.Substring(i+2, 1)))

let getEnglishNumeral s = 
    match s with
    | '1' -> "st"
    | '2' -> "nd"
    | '3' -> "rd"
    | _ -> "th"

let containsNumber (text: string) = 
    let mutable result = text
    for i in [0 .. text.Length - 2] do
        if isEnglishNumeral text i then
          result <- [result.Substring(0,i+1); getEnglishNumeral result.[i]; result.Substring(i+3);] |> String.Concat
    result
            
                

let findSuperScriptNum (text : string[]) = 
    Array.map containsNumber text