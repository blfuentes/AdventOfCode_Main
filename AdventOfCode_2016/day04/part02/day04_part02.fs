﻿module day04_part02

open System
open System.Text.RegularExpressions

open AdventOfCode_Utilities

type CharCount = { Character: char; Count: int }
type Room = { Content: string; Name: string; SectorId: int; Checksum: string; Parts: CharCount array }

let getParts(input: string) =
    let pattern = @"^(.*?)-(\d+)\[(.*?)\]$"
    let regex = Regex(pattern)

    match regex.Match(input) with
    | matchResult when matchResult.Success ->
        let groups = matchResult.Groups
        let stringValueGroups = groups.[1].Value.Replace("-", "").ToCharArray() |> Array.sort |> Array.countBy (fun c -> c)
        let numberValue = groups.[2].Value
        let bracketsContent = groups.[3].Value
        { 
            Content = input;
            Name = groups.[1].Value; 
            SectorId = Int32.Parse(numberValue); 
            Checksum = bracketsContent; 
            Parts = stringValueGroups |> Array.map(fun p -> {Character = fst p; Count = snd p})
        }
    | _ ->
        raise (ArgumentException "Invalid input")

let sortByCountThenByChar(a: CharCount) (b: CharCount) =
    if a.Count = b.Count then 
        compare a.Character b.Character 
    else 
        compare b.Count a.Count

let isRealRoom (room: Room) =
    let sortedParts = (Array.sortWith sortByCountThenByChar room.Parts) |> Array.map (fun p -> p.Character) |> Array.take 5
    let isRealRoom = room.Checksum = (new String(sortedParts))
    isRealRoom

let decrypherText (input: string) (shifter: int) =
    let numberOfShifts = shifter % 26
    new String((input.ToCharArray() |> Array.map (fun c -> 
    if c = '-' then ' ' 
    else 
        let newValue = (int)c + numberOfShifts
        let newChar = (char)newValue
        if newValue > 122 then 
            (char)(96 + (numberOfShifts - (122 - (int)c)))
        else
            newChar
        )))

let decypherRoom (room: Room) =
    decrypherText room.Name room.SectorId

let execute =
    let path = "day04/day04_input.txt"
    let inputLines = Utilities.GetLinesFromFile(path)
    let rooms = inputLines |> Array.map getParts
    let realRooms = rooms |> Array.filter isRealRoom
    (realRooms |> Array.find(fun r -> (decypherRoom r).StartsWith("northpole"))).SectorId