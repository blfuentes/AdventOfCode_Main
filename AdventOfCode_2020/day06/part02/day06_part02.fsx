open System.IO
open System.Collections.Generic
open System

#load @"../../Model/CustomDataTypes.fs"
#load @"../../Modules/Utilities.fs"

open Utilities
open CustomDataTypes

//let file = "test_input.txt"
let file = "day06_input.txt"
let path = __SOURCE_DIRECTORY__ + @"../../" + file
let inputLines = GetLinesFromFileFSI2(path) |> List.ofArray

let concatStringList (list:string list) =
   seq {
       for l in list do
           yield l.ToCharArray()
   } |> List.ofSeq

let commonElements (input: char array list) =
    let inputAsList = input |> List.map (List.ofArray)
    let inputAsSet = List.map Set.ofList inputAsList
    let elements =  Seq.reduce Set.intersect inputAsSet
    elements

let commonElements2 (input: char array list) =
    input |> List.map (List.ofArray) |> Seq.map Set.ofList |> Set.intersectMany

let testList2 = [[|'a'; 'b'|]; [|'a'; 'c'|]; [|'a'; 'd'|]]
commonElements testList2
commonElements2 testList2  

let answers = getLinesGroupBySeparator2 inputLines ""
answers |> List.map (fun x -> concatStringList x)

let result = answers |> List.map (fun x -> commonElements (concatStringList x)) |> List.map (fun x -> List.length (Set.toList x)) |> List.reduce(+)
let result2 = answers |> List.map (fun x -> commonElements2 (concatStringList x)) |> List.map (fun x -> List.length (Set.toList x)) |> List.reduce(+)
