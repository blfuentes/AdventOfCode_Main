﻿module day12_part02

open System.IO
open CustomDataTypes

let path = "day12_input.txt"
//let path = "test_input00.txt"
//let path = "test_input01.txt"
//let path = "test_input02.txt"

let cavePaths = 
    File.ReadLines(__SOURCE_DIRECTORY__ + @"../../" + path) |> Seq.toList

let isLowerCase(value: string) =
    let values = value.ToCharArray() |> Array.map int
    values |> Array.forall(fun v -> [97..122] |> List.contains v)

let getCaveType(value: string) =
    match value with
    | "start" -> CaveType.START
    | "end" -> CaveType.END
    | _ -> if isLowerCase value then CaveType.SMALL else CaveType.BIG 
    //| _ -> if ([97..122] |> List.contains(value.ToCharArray().[0] |> int)) then CaveType.SMALL else CaveType.BIG

let createCave(definition: string) =
    let caveParts = definition.Split('-')
    let connection = { Name = caveParts.[1]; Size = getCaveType(caveParts.[1]); Connections = [] }
    let cave = { Name = caveParts.[0]; Size = getCaveType(caveParts.[0]); Connections = [] }
    (cave, connection)

let rec generateCaves (paths: string list, listOfCaves: Cave list) =
    match paths with
    | [] -> listOfCaves
    | x::xs ->
        let tmpCaves = createCave(x)
        let oldCaveInit = listOfCaves |> List.filter(fun c -> c.Name = (fst tmpCaves).Name)
        let oldCaveEnd = listOfCaves |> List.filter(fun c -> c.Name = (snd tmpCaves).Name)
        if oldCaveInit.IsEmpty then
            if oldCaveEnd.IsEmpty then
                generateCaves (xs, listOfCaves @ 
                [{ Name = (fst tmpCaves).Name; Size = (fst tmpCaves).Size; Connections = [snd tmpCaves] };
                { Name = (snd tmpCaves).Name; Size = (snd tmpCaves).Size; Connections = [] }])                
            else
                generateCaves (xs, listOfCaves @ [{ Name = (fst tmpCaves).Name; Size = (fst tmpCaves).Size; Connections = [snd tmpCaves] }])
        else
            if oldCaveEnd.IsEmpty then
                generateCaves (xs, 
                (listOfCaves |> List.except(oldCaveInit)) @ [{ Name = (fst tmpCaves).Name; Size = (fst tmpCaves).Size; Connections = oldCaveInit.Head.Connections @ [snd tmpCaves] };
                { Name = (snd tmpCaves).Name; Size = (snd tmpCaves).Size; Connections = [] }])
            else
                generateCaves (xs, (listOfCaves |> List.except(oldCaveInit)) @ [{ Name = (fst tmpCaves).Name; Size = (fst tmpCaves).Size; Connections = oldCaveInit.Head.Connections @ [snd tmpCaves] }])

let acceptSmallCave(check: Cave, visited: Cave list) =
    let groupedCaves = visited |> List.filter(fun c -> c.Size = CaveType.SMALL) |> List.groupBy(fun c -> c.Name)
    let thisCheck = (groupedCaves |> List.filter(fun gc -> (fst gc) = check.Name))
    let numberOfTheChecks = 
        match (thisCheck |> List.length) with
        | 0 -> 0
        | _ -> (snd thisCheck.Head).Length
    (groupedCaves |> List.forall(fun gc -> (snd gc).Length < 2)) || numberOfTheChecks = 0

let rec buildPath (from: Cave, paths: Cave list, visited: Cave list) =
    let origin = paths |> List.filter(fun c -> c.Name = from.Name)
    let result = seq {
        match origin with
        | [x] -> 
            let availableConnections = x.Connections @ 
                                        (paths |> 
                                            List.filter(fun p -> p.Size <> CaveType.START && (p.Connections |> 
                                                                    List.map(fun pp -> pp.Name)) |> List.contains(x.Name)))
            let filteredConnections = availableConnections |> 
                                        List.filter(fun p -> 
                                                        match p.Size with 
                                                        | CaveType.START -> false
                                                        | CaveType.BIG -> true
                                                        | CaveType.END -> true
                                                        | CaveType.SMALL -> acceptSmallCave(p, visited))

            let newVisited = visited @ origin
            for con in filteredConnections do
                match con.Name with
                | "end" -> 
                    let finishedPath = newVisited @ [con]
                    //printfn "Path: %s" (String.Join(", ", (finishedPath |> List.map(fun p -> p.Name))))
                    finishedPath
                | _ -> yield! buildPath(con, paths, newVisited)
        | _ -> []
    }
    result

let pathIsValid(path: Cave list) = 
    let smallCaves = path |> List.filter(fun c -> c.Size = CaveType.SMALL)
    let groupedCaves = smallCaves |> List.groupBy(fun c -> c.Name)
    (groupedCaves |> List.filter(fun gc -> (snd gc).Length = 2)) |> List.length < 2

let execute =
    let listOfCaves = generateCaves(cavePaths, [])
    let origin = listOfCaves |> List.find(fun c -> c.Name = "start")
    let allPaths = buildPath(origin, listOfCaves, [])
    
    let validPaths = allPaths |> Seq.filter(fun p -> pathIsValid(p))
    validPaths |> Seq.toList |> List.length