module day13_part01

open System
open System.Collections.Generic

open AdventOfCode_2023.Modules
open AdventOfCode_Utilities

//let path = "day13/test_input_01.txt"
let path = "day13/day13_input.txt"

let parseGroup (line:string list) =
    let rows = if line.Length % 2 = 0 then line.Length else line.Length + 1
    let cols = if line[0].Length % 2 = 0 then line.[0].Length else line.[0].Length + 1
    let group = Array2D.create rows cols '0'
    for i in 0..line.Length - 1 do
            for j in 0..line.[0].Length - 1 do
                group.[i,j] <- line.[i].[j]
    group


let printGroup (group:char[,]) =
    for i in 0..group.GetLength(0)-1 do
        for j in 0..group.GetLength(1)-1 do
            printf "%c" group.[i,j]
        printfn ""

let findMirror (group: char[,]) =
    printGroup group
    let hMirror = 
        seq {
            let mid = group.GetLength(0) / 2 - 1
            for idx in 0..mid - 1 do
                let up = group.[mid - idx, *]
                let down = group.[mid + idx + 1, *]
                if down |> Array.forall ((=) '0') then
                    yield mid
                else
                    if up = down then
                        yield mid
                    else
                        yield -1
        } |>Seq.toList
    let vMirror =
        seq {
            let mid = group.GetLength(1) / 2 - 1
            for idx in 0..mid - 1 do
                let left = group.[*, mid - idx]
                let right = group.[*, mid + idx + 1]
                if right |> Array.forall ((=) '0') then
                    yield mid
                else
                    if left = right then
                        yield mid
                    else 
                        yield -1
        } |>Seq.toList
    (hMirror, vMirror)

let calculateMirror (mirrors: int list * int list) =
    let (hMirror, vMirror) = mirrors
    let hMirror' = 
        if (hMirror |> List.filter((=) -1)).Length > 0 then
            0
        else
            hMirror.[0] + 1
    let vMirror' = 
        if (vMirror |> List.filter((=) -1)).Length > 0 then
            0
        else
            vMirror.[0] + 1
    vMirror' + hMirror' * 100

let execute =
    //let path = "day13/test_input_01.txt"
    //let path = "day13/test_input_02.txt"
    let path = "day13/day13_input.txt"
    let lines = LocalHelper.ReadLines path |> Seq.toList
    let groups = getGroupsOnSeparator lines ""
    let maps = groups |> List.map parseGroup
    //maps |> List.iter printGroup
    maps |> List.map findMirror |> List.map calculateMirror |> List.sum