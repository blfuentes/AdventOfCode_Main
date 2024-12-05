﻿module day05_part01

open AdventOfCode_2024.Modules
open AdventOfCode_Utilities

let parseContent(string: string array) =
    let parts = Utilities.getGroupsOnSeparator (string |> List.ofArray) ""
    let orders = parts[0] |> Array.ofList |> Array.map(fun l -> ((int)(l.Split("|")[0]), (int)(l.Split("|")[1])))
    let chekers = parts[1] |> Array.ofList  |> Array.map(fun l -> l.Split(",") |> Array.map int)
    (orders, chekers)

let orderfound (prev, next) (prev', next') =
    prev = prev' && next = next'

let inOrder (number: int) (tocheck: int array) (pages: (int * int) array) =
    tocheck
    |> Array.forall (fun c ->
        pages
        |> Array.exists (orderfound (number, c))
    )

let checkerInOrder (pairs: int array) (pages: (int * int) array) =
    pairs
    |> Array.indexed
    |> Array.tryFind (fun (index, value) ->
        let rest = Array.skip (index + 1) pairs
        not (inOrder value rest pages)
    )
    |> Option.isNone

let execute =
    let path = "day05/day05_input.txt"
    let content = LocalHelper.GetLinesFromFile path
    let (validorders, tobechecked) = parseContent content
    tobechecked
    |> Array.filter(fun check -> checkerInOrder check validorders)
    |> Array.sumBy(fun check -> check[check.Length / 2])