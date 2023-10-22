#load @"../../Modules/Utilities.fs"

open System
open System.Collections.Generic

open AdventOfCode_2016.Modules

//let path = "day06/test_input_01.txt"
let path = "day06/day06_input.txt"

let input = Utilities.GetLinesFromFile path
let container = Array.create input.[0].Length List.empty

let buildContainer (value: string) (listOfChars: string list array) =
    value.ToCharArray() |> Array.iteri(fun i c -> listOfChars.[i] <- if listOfChars.[0] = [] then [(c |> string)] else (listOfChars.[i] @ [(c |> string)]))

let getErrorConnectedVersion (listOfChars: string list array) =
    listOfChars |> Array.map(fun l -> (l |> List.groupBy id) |> List.sortBy(fun (k, v) -> v.Length) |> List.head |> fst)
    
input |> Array.iter(fun s -> buildContainer s container)
let result = String.concat "" ((getErrorConnectedVersion container) |> Array.toSeq)
printfn "result: %s" result