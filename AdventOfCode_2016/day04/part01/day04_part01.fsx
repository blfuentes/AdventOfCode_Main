#load @"../../Modules/Utilities.fs"

open System
open System.Collections.Generic

open AdventOfCode_2016.Modules

//let path = "day04/test_input_01.txt"
let path = "day04/day04_input.txt"

let getValueOfRoom (room: string) =
    let firstPart = room.ToCharArray() |> Array.takeWhile(fun c -> c <> '[')
    let secondPart = room.ToCharArray() |> Array.skipWhile(fun c -> c <> '[') |> Array.filter(fun c -> c <> ']')
    let letters = firstPart |> Array.takeWhile(fun c -> not (['0'..'9'] 
                            |> List.contains c)) |> Array.filter(fun c -> c <> '-')
    let number = int(System.String.Concat(firstPart |> Array.skipWhile(fun c -> not (['0'..'9'] |> List.contains c))))
    let grouped = letters |> Array.countBy(fun c -> c) |> Array.sortByDescending(fun c -> snd c) |> Array.take 5
    
    let diffsCheck = grouped |> Array.groupBy(fun c -> snd c)
    // let sameCheck = grouped |> Array.groupBy(fun c -> snd c) |> Array.filter(fun c -> (fst c > 1))
    
    let secondPartCheckSum = System.String.Concat(secondPart)
    // let diffsOk = diffsCheck |> Array.map(fun c -> (fst (snd c)))
    (diffsCheck, 0)


let inputLines = Utilities.GetLinesFromFile(path)
getValueOfRoom "aaaaa-bbb-z-y-x-123[abxyz]"
