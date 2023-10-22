open System
open System.Text.Json
open System.Collections.Generic

let path = "test_input.txt"
//let path = "day_input.txt"

let input = System.IO.File.ReadLines path |> Seq.toArray
let container = Array.create input.[0].Length [""]

let buildContainer (value: string) (listOfChars: string list array) =
    value.ToCharArray() |> Array.iteri(fun i c -> listOfChars.[i] <- (listOfChars.[i] @ [(c |> string)]))

let getErrorConnectedVersion (listOfChars: string list array) =
    listOfChars |> Array.map(fun l -> (l |> List.groupBy id) |> List.sortByDescending(fun (k, v) -> v.Length) |> List.head |> fst)
    
input |> Array.iter(fun s -> buildContainer s container)
String.concat "" ((getErrorConnectedVersion container) |> Array.toSeq) |> printfn "%A"


