#load @"../../Modules/Utilities.fs"

open System
open System.Collections.Generic
open System.Text.RegularExpressions

open AdventOfCode_2016.Modules

//let path = "day04/test_input_01.txt"
let path = "day04/day04_input.txt"

type Room = { Name: string; SectorId: int; Checksum: string; Parts: (char*int) array }

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
            Name = groups.[1].Value; 
            SectorId = Int32.Parse(numberValue); 
            Checksum = bracketsContent; 
            Parts = stringValueGroups
        }
    | _ ->
        raise (ArgumentException "Invalid input")


let isRealRoom (room: Room) =
    let sortedParts = room.Parts |> Array.sortByDescending (fun (c, count) -> count) |> Array.map (fun (c, count) -> c) |> Array.take 5
    printfn "Sorted parts: %s - checksum: %s" (new String(sortedParts)) room.Checksum
    let outputString = sprintf "Sorted parts: %s - checksum: %s \r\n" (new string(sortedParts)) room.Checksum

    System.IO.File.AppendAllText("./output.txt", outputString)
    room.Checksum = (new String(sortedParts))
    //Array.pairwise sortedParts|> Array.forall (fun (a, b) -> a <= b)

let inputLines = Utilities.GetLinesFromFile(path)
let rooms = inputLines |> Array.map getParts
let realRooms = rooms |> Array.filter isRealRoom
realRooms |> Array.sumBy (fun room -> room.SectorId) |> printfn "Sum of sector IDs: %d"

//getParts "aaaaa-bbb-z-y-x-123[abxyz]"
//{
//    Name = "aaaaa-bbb-z-y-x";
//    SectorId = 123;
//    Checksum = "abxyz";
//    Parts = [|
//        ('a', 5);
//        ('b', 3);
//        ('x', 1);
//        ('y', 1);
//        ('z', 1);
//    |]
//} |> isRealRoom |> printfn "Is real room: %b"

//{ 
//    Name = "not-a-real-room"
//    SectorId = 404
//    Checksum = "oarel"
//    Parts =
//        [|('a', 2); ('e', 1); ('l', 1); ('m', 1); ('n', 1); ('o', 3); ('r', 2);
//       ('t', 1)|] 
//} |> isRealRoom |> printfn "Is real room: %b"