module day05_part02

open AdventOfCode_2024.Modules
open AdventOfCode_Utilities

let parseContent(string: string list) =
    let parts = Utilities.getGroupsOnSeparator string ""
    let orders = parts[0] |> List.map(fun l -> ((int)(l.Split("|")[0]), (int)(l.Split("|")[1])))
    let chekers = parts[1] |> List.map(fun l -> l.Split(",") |> Array.map int |> List.ofArray)
    (orders, chekers)

let inOrder(number: int) (tocheck: int list) (pages: (int*int) list) =
    tocheck
    |> List.forall(fun c ->
        match pages |> List.tryFind(fun (f, t) -> f = number && c = t) with
        | Some(_) -> true
        | None -> false
    )

let rec checkerInOrder(pairs: int list)(pages: (int*int) list) =
    match pairs with
    | [] -> true
    | head::tail ->
        if inOrder head tail pages then
            checkerInOrder tail pages
        else
            false

let reorder(pairs: int list)(pages: (int*int) list) =
    let sort a b =
        match pages |> List.tryFind(fun (f, t) -> f = a && t =b) with
        | Some(_) -> - 1
        | None -> 1
        
    pairs
    |> List.sortWith sort

let execute =
    let path = "day05/day05_input.txt"
    let content = LocalHelper.GetLinesFromFile path |> List.ofArray
    let (orders, checkers) = parseContent content
    checkers
    |> List.filter(fun o -> not (checkerInOrder o orders))
    |> List.map(fun o -> reorder o orders)
    |> List.sumBy(fun l -> l.Item(l.Length / 2))