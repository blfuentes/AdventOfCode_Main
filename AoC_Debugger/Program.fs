open System
open System.Text.Json
open System.Collections.Generic

let GetLinesFromFile(path: string) =
    System.IO.File.ReadAllLines(path)

let GetContentFromFile(path: string) =
    System.IO.File.ReadAllText(path)

let getGroupsOnSeparator (inputLines: 'a list) (separator: 'a) =
    let folder (a) (cur, acc) = 
        match a with
        | _ when a <> separator -> a::cur, acc
        | _ -> [], cur::acc 
    let result = List.foldBack folder (inputLines) ([], [])
    (fst result)::(snd result)

let path = "test_input.txt"
//let path = "day_input.txt"