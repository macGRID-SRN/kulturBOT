module SuperScript
open System
open System.Linq

//these are characters the OCR technology thinks it sees.
let OCRmistakenSuperChars = 
    [
        '\'';
        '"';
        '^';
        '*';
        '.';
    ]

//this is used to match 23 to 23rd, 21 to 21st and 28 to 28th
let getEnglishNumeral s = 
    match s with
    | '1' -> "st"
    | '2' -> "nd"
    | '3' -> "rd"
    | _ -> "th"

//determines if a string is a number, as in Int32.
let isNum s = 
    match Int32.TryParse(s) with
    |   (false, _) -> false
    |   (true, _) -> true

let isEnglishNumeral (s : string) i = 
    (isNum (s.Substring(i, 1))) 
    && (OCRmistakenSuperChars.Contains (s.[i+1])) 
    && (
        (OCRmistakenSuperChars.Contains (s.[i+2])) 
        || String.IsNullOrWhiteSpace (s.Substring(i+2, 1))
       )

let containsNumber (text: string) = 
    let mutable result = text
    for i in [0 .. text.Length - 2] do
        if isEnglishNumeral text i then
          result <- [result.Substring(0,i+1); getEnglishNumeral result.[i]; result.Substring(i+3);] |> String.Concat
    result

let findSuperScriptNum (text : string[]) = 
    Array.map containsNumber text