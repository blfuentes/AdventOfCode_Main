module day05_part02

open AdventOfCode_2024.Modules
open AdventOfCode_Utilities

let parseContent(string: string list) =
    let parts = Utilities.getGroupsOnSeparator string ""
    let orders = parts[0] |> List.map(fun l -> ((int)(l.Split("|")[0]), (int)(l.Split("|")[1])))
    let chekers = parts[1] |> List.map(fun l -> l.Split(",") |> Array.map int |> List.ofArray)
    (orders, chekers)

let orderfound (prev, next) (prev', next') =
    prev = prev' && next = next'

let inOrder (number: int) (tocheck: int list) (pages: (int * int) list) =
    tocheck
    |> List.forall (fun c ->
        pages
        |> List.exists (orderfound (number, c))
    )

let checkerInOrder (pairs: int list) (pages: (int * int) list) =
    pairs
    |> List.indexed
    |> List.tryFind (fun (index, value) ->
        let rest = List.skip (index + 1) pairs
        not (inOrder value rest pages)
    )
    |> Option.isNone

let reorder(pairs: int list)(pages: (int*int) list) =
    let sort left right =
        match pages |> List.tryFind (orderfound (left,right)) with
        | Some(_) -> - 1
        | None -> 1
        
    pairs
    |> List.sortWith sort

let execute =
    let path = "day05/day05_input.txt"
    let content = LocalHelper.GetLinesFromFile path |> List.ofArray
    let (validorders, tobechecked) = parseContent content
    tobechecked
    |> List.filter(fun check -> not (checkerInOrder check validorders))
    |> List.map(fun check -> reorder check validorders)
    |> List.sumBy(fun newsorted -> newsorted.Item(newsorted.Length / 2))